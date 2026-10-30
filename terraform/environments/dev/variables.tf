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
  default     = "dev"
}

variable "vpc_cidr" {
  description = "VPC CIDR"
  type        = string
  default     = "10.0.0.0/16"
}

variable "availability_zones" {
  description = "List of AZs"
  type        = list(string)
  default     = ["us-east-1a", "us-east-1b"]
}

variable "public_subnet_cidrs" {
  description = "Public subnet CIDRs"
  type        = list(string)
  default     = ["10.0.1.0/24", "10.0.2.0/24"]
}

variable "private_subnet_cidrs" {
  description = "Private subnet CIDRs"
  type        = list(string)
  default     = ["10.0.11.0/24", "10.0.12.0/24"]
}

variable "enable_nat_gateway" {
  description = "Enable NAT Gateway"
  type        = bool
  default     = true
}

variable "container_image" {
  description = "Specific container image tag to run (leave empty to default to ECR latest)"
  type        = string
  default     = ""
}

variable "smtp_host" {
  description = "SMTP Host address"
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
  default     = "no-reply@dev.sumitkumar.dev"
}

variable "smtp_from_name" {
  description = "Sender display name"
  type        = string
  default     = "SumitKumar Dev Portfolio"
}

variable "smtp_contact_inbox_address" {
  description = "Inbox email address for incoming contact messages"
  type        = string
  default     = "contact@dev.sumitkumar.dev"
}

variable "alert_email" {
  description = "Email to receive CloudWatch alarms"
  type        = string
  default     = ""
}
