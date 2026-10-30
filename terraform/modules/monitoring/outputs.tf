output "sns_topic_arn" {
  description = "The ARN of the SNS alert topic"
  value       = length(aws_sns_topic.alerts) > 0 ? aws_sns_topic.alerts[0].arn : null
}
