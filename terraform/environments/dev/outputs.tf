output "alb_dns_name" {
  description = "ALB DNS endpoint to access the application"
  value       = module.alb.alb_dns_name
}

output "ecr_repository_url" {
  description = "ECR Docker image repository URL"
  value       = module.ecr.repository_url
}

output "ecs_cluster_name" {
  description = "ECS cluster name"
  value       = module.ecs.cluster_name
}

output "ecs_service_name" {
  description = "ECS service name"
  value       = module.ecs.service_name
}

output "secrets_manager_arn" {
  description = "Secrets Manager ARN"
  value       = module.secrets.secret_arn
}

output "efs_file_system_id" {
  description = "EFS file system ID"
  value       = module.efs.file_system_id
}
