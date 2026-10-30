variable "project_name" {
  description = "Name of the project"
  type        = string
}

variable "environment" {
  description = "Deployment environment (e.g., dev, prod)"
  type        = string
}

variable "smtp_host" {
  description = "SMTP host"
  type        = string
  default     = ""
}

variable "smtp_port" {
  description = "SMTP port"
  type        = number
  default     = 587
}

variable "smtp_user" {
  description = "SMTP username"
  type        = string
  default     = ""
}

variable "smtp_password" {
  description = "SMTP password"
  type        = string
  sensitive   = true
  default     = ""
}

variable "smtp_from_address" {
  description = "SMTP From Address"
  type        = string
  default     = "no-reply@sumitkumar.dev"
}

variable "smtp_from_name" {
  description = "SMTP From Name"
  type        = string
  default     = "SumitKumar Portfolio"
}

variable "smtp_contact_inbox_address" {
  description = "Contact Inbox Address"
  type        = string
  default     = "owner@sumitkumar.dev"
}

variable "recovery_window_in_days" {
  description = "Number of days that AWS Secrets Manager waits before deleting a secret"
  type        = number
  default     = 0
}

variable "tags" {
  description = "Additional tags for resources"
  type        = map(string)
  default     = {}
}
