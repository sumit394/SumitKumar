variable "aws_region" {
  description = "AWS deployment region"
  type        = string
  default     = "us-east-1"
}

variable "project_name" {
  description = "Project name identifier"
  type        = string
  default     = "sumitkumar"
}

variable "environment" {
  description = "Deployment environment name"
  type        = string
  default     = "prod"
}

variable "vpc_cidr" {
  description = "VPC CIDR"
  type        = string
  default     = "10.0.0.0/16"
}

variable "availability_zones" {
  description = "List of AZs"
  type        = list(string)
  default     = ["us-east-1a", "us-east-1b", "us-east-1c"]
}

variable "public_subnet_cidrs" {
  description = "Public subnet CIDRs"
  type        = list(string)
  default     = ["10.0.1.0/24", "10.0.2.0/24", "10.0.3.0/24"]
}

variable "private_subnet_cidrs" {
  description = "Private subnet CIDRs"
  type        = list(string)
  default     = ["10.0.11.0/24", "10.0.12.0/24", "10.0.13.0/24"]
}

variable "certificate_arn" {
  description = "ACM Certificate ARN for ALB (in region of ALB)"
  type        = string
  default     = ""
}

variable "cloudfront_certificate_arn" {
  description = "ACM Certificate ARN for CloudFront (must be in us-east-1)"
  type        = string
  default     = ""
}

variable "domain_aliases" {
  description = "Custom domains for CloudFront (e.g. ['sumitkumar.dev', 'www.sumitkumar.dev'])"
  type        = list(string)
  default     = []
}

variable "enable_deletion_protection" {
  description = "Enable ALB deletion protection"
  type        = bool
  default     = true
}

variable "container_image" {
  description = "Specific container image tag to run (leave empty to default to ECR latest)"
  type        = string
  default     = ""
}

variable "ecs_cpu" {
  description = "ECS Task CPU"
  type        = number
  default     = 512
}

variable "ecs_memory" {
  description = "ECS Task Memory in MB"
  type        = number
  default     = 1024
}

variable "ecs_desired_count" {
  description = "Desired number of ECS tasks"
  type        = number
  default     = 2
}

variable "ecs_min_capacity" {
  description = "Auto scaling min tasks"
  type        = number
  default     = 2
}

variable "ecs_max_capacity" {
  description = "Auto scaling max tasks"
  type        = number
  default     = 6
}

variable "smtp_host" {
  description = "SMTP Host address (e.g., AWS SES)"
  type        = string
  default     = "email-smtp.us-east-1.amazonaws.com"
}

variable "smtp_port" {
  description = "SMTP Port"
  type        = number
  default     = 587
}

variable "smtp_user" {
  description = "SMTP Username"
  type        = string
  default     = ""
}

variable "smtp_password" {
  description = "SMTP Password"
  type        = string
  sensitive   = true
  default     = ""
}

variable "smtp_from_address" {
  description = "Sender email address"
  type        = string
  default     = "no-reply@sumitkumar.dev"
}

variable "smtp_from_name" {
  description = "Sender display name"
  type        = string
  default     = "SumitKumar Portfolio"
}

variable "smtp_contact_inbox_address" {
  description = "Inbox email address for incoming contact messages"
  type        = string
  default     = "owner@sumitkumar.dev"
}

variable "alert_email" {
  description = "Email to receive CloudWatch alarms"
  type        = string
  default     = ""
}
