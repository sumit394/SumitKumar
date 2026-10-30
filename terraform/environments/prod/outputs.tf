output "cloudfront_domain_name" {
  description = "CloudFront distribution domain name"
  value       = module.cloudfront.distribution_domain_name
}

output "cloudfront_distribution_id" {
  description = "CloudFront distribution ID"
  value       = module.cloudfront.distribution_id
}

output "alb_dns_name" {
  description = "ALB DNS endpoint"
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

output "efs_file_system_id" {
  description = "EFS file system ID"
  value       = module.efs.file_system_id
}

output "secrets_manager_arn" {
  description = "Secrets Manager ARN"
  value       = module.secrets.secret_arn
}
