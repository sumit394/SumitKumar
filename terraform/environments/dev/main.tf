terraform {
  required_version = ">= 1.5.0"
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }

  # Example S3 backend configuration (uncomment & configure for remote state)
  # backend "s3" {
  #   bucket         = "sumitkumar-terraform-state-dev"
  #   key            = "dev/terraform.tfstate"
  #   region         = "us-east-1"
  #   dynamodb_table = "sumitkumar-terraform-locks-dev"
  #   encrypt        = true
  # }
}

provider "aws" {
  region = var.aws_region

  default_tags {
    tags = {
      Project     = var.project_name
      Environment = var.environment
      ManagedBy   = "Terraform"
    }
  }
}

# 1. VPC Module
module "vpc" {
  source = "../../modules/vpc"

  project_name         = var.project_name
  environment          = var.environment
  vpc_cidr             = var.vpc_cidr
  availability_zones   = var.availability_zones
  public_subnet_cidrs  = var.public_subnet_cidrs
  private_subnet_cidrs = var.private_subnet_cidrs
  enable_nat_gateway   = var.enable_nat_gateway
  single_nat_gateway   = true
}

# 2. ECR Module
module "ecr" {
  source = "../../modules/ecr"

  project_name    = var.project_name
  environment     = var.environment
  max_image_count = 5
}

# 3. ALB Module
module "alb" {
  source = "../../modules/alb"

  project_name      = var.project_name
  environment       = var.environment
  vpc_id            = module.vpc.vpc_id
  public_subnet_ids = module.vpc.public_subnet_ids
  container_port    = 80
  health_check_path = "/"
}

# 4. Secrets Module
module "secrets" {
  source = "../../modules/secrets"

  project_name               = var.project_name
  environment                = var.environment
  smtp_host                  = var.smtp_host
  smtp_port                  = var.smtp_port
  smtp_user                  = var.smtp_user
  smtp_password              = var.smtp_password
  smtp_from_address          = var.smtp_from_address
  smtp_from_name             = var.smtp_from_name
  smtp_contact_inbox_address = var.smtp_contact_inbox_address
  recovery_window_in_days    = 0
}

# 5. EFS Module (For SQLite databases persistence)
module "efs" {
  source = "../../modules/efs"

  project_name          = var.project_name
  environment           = var.environment
  vpc_id                = module.vpc.vpc_id
  private_subnet_ids    = module.vpc.private_subnet_ids
  ecs_security_group_id = module.ecs.security_group_id
}

# 6. ECS Module
module "ecs" {
  source = "../../modules/ecs"

  project_name               = var.project_name
  environment                = var.environment
  aws_region                 = var.aws_region
  vpc_id                     = module.vpc.vpc_id
  private_subnet_ids         = module.vpc.private_subnet_ids
  alb_security_group_id      = module.alb.security_group_id
  target_group_arn           = module.alb.target_group_arn
  container_image            = var.container_image != "" ? var.container_image : "${module.ecr.repository_url}:latest"
  container_port             = 80
  cpu                        = 256
  memory                     = 512
  desired_count              = 1
  min_capacity               = 1
  max_capacity               = 2
  efs_file_system_id         = module.efs.file_system_id
  efs_access_point_id        = module.efs.access_point_id
  secret_arn                 = module.secrets.secret_arn
  aspnetcore_environment     = "Development"
  log_retention_in_days      = 14
  enable_container_insights  = false
}

# 7. Monitoring Module
module "monitoring" {
  source = "../../modules/monitoring"

  project_name            = var.project_name
  environment             = var.environment
  ecs_cluster_name        = module.ecs.cluster_name
  ecs_service_name        = module.ecs.service_name
  alb_arn_suffix          = module.alb.alb_arn_suffix
  target_group_arn_suffix = module.alb.target_group_arn_suffix
  alert_email             = var.alert_email
}
