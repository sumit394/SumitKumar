output "file_system_id" {
  description = "The ID of the EFS file system"
  value       = aws_efs_file_system.data.id
}

output "file_system_arn" {
  description = "The ARN of the EFS file system"
  value       = aws_efs_file_system.data.arn
}

output "access_point_id" {
  description = "The ID of the EFS Access Point"
  value       = aws_efs_access_point.app_data.id
}

output "security_group_id" {
  description = "Security group ID of the EFS"
  value       = aws_security_group.efs.id
}
