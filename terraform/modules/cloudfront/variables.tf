variable "project_name" {
  description = "Name of the project"
  type        = string
}

variable "environment" {
  description = "Deployment environment (e.g., dev, prod)"
  type        = string
}

variable "alb_dns_name" {
  description = "DNS name of the Application Load Balancer"
  type        = string
}

variable "origin_protocol_policy" {
  description = "Origin protocol policy (http-only, match-viewer, https-only)"
  type        = string
  default     = "http-only"
}

variable "price_class" {
  description = "CloudFront price class"
  type        = string
  default     = "PriceClass_100"
}

variable "domain_aliases" {
  description = "List of custom domain aliases (e.g. ['sumitkumar.dev', 'www.sumitkumar.dev'])"
  type        = list(string)
  default     = []
}

variable "acm_certificate_arn" {
  description = "ACM Certificate ARN in us-east-1 for CloudFront custom domain"
  type        = string
  default     = ""
}

variable "tags" {
  description = "Additional tags for resources"
  type        = map(string)
  default     = {}
}
