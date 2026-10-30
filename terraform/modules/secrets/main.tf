terraform {
  required_version = ">= 1.5.0"
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

resource "aws_secretsmanager_secret" "app_secrets" {
  name                    = "${var.project_name}/${var.environment}/app-secrets"
  recovery_window_in_days = var.recovery_window_in_days

  tags = merge(var.tags, {
    Name = "${var.project_name}-${var.environment}-secrets"
  })
}

resource "aws_secretsmanager_secret_version" "app_secrets" {
  secret_id = aws_secretsmanager_secret.app_secrets.id
  secret_string = jsonencode({
    SMTP_HOST                  = var.smtp_host
    SMTP_PORT                  = tostring(var.smtp_port)
    SMTP_USER                  = var.smtp_user
    SMTP_PASSWORD              = var.smtp_password
    SMTP_FROM_ADDRESS          = var.smtp_from_address
    SMTP_FROM_NAME             = var.smtp_from_name
    SMTP_CONTACT_INBOX_ADDRESS = var.smtp_contact_inbox_address
  })
}
