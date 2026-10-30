# AWS Infrastructure & Deployment for SumitKumar Solution (Terraform)

This directory contains modular, production-grade Infrastructure as Code (IaC) written in Terraform to provision, secure, and operate the **SumitKumar** Blazor Web App & API solution on Amazon Web Services (AWS).

---

## 1. Architectural Overview

```
                      [ Users / Internet ]
                               │
                               ▼
                   [ Amazon CloudFront CDN ] (Edge Caching / SSL)
                               │
                               ▼
               [ Application Load Balancer (ALB) ] (Public Subnets)
                               │
                   ┌───────────┴───────────┐
                   ▼                       ▼
      [ ECS Fargate Service ]     [ ECS Fargate Service ] (Private Subnets)
                   │                       │
                   └───────────┬───────────┘
                               ▼
               [ Amazon EFS (Encrypted NFS) ] (Persisted /app/Data SQLite DBs)
                               │
            ┌──────────────────┴──────────────────┐
            ▼                                     ▼
 [ AWS Secrets Manager ]               [ Amazon CloudWatch ]
 (SMTP Credentials & Config)           (Logs & Metric Alarms)
```

### Key Components & Design Choices

| Component | AWS Resource | Purpose |
|---|---|---|
| **Networking** | AWS VPC, Public & Private Subnets, IGW, NAT Gateway | Isolates compute workloads into private subnets while routing inbound traffic through public ALB. |
| **Container Registry** | Amazon ECR | Private Docker container registry with vulnerability scanning and lifecycle expiration policies. |
| **Compute** | Amazon ECS (AWS Fargate) | Serverless container execution (.NET 10 Blazor Server/WASM host) without EC2 management overhead. |
| **Persistence** | Amazon EFS + Access Points | Persistent NFS volume mounted at `/app/Data` inside container tasks to preserve SQLite databases (`app.db`, `blog.db`) across task redeployments. |
| **Load Balancing** | Application Load Balancer (ALB) | Distributes traffic, performs health checks, and terminates TLS/SSL. |
| **CDN & Caching** | Amazon CloudFront | Caches static assets (`/_framework/*`, `/img/*`, `/css/*`) at AWS edge locations, reducing latency and backend load. |
| **Secrets & Config** | AWS Secrets Manager | Securely stores SMTP credentials and injects them directly as environment variables into ECS tasks. |
| **Monitoring** | Amazon CloudWatch & SNS | Real-time container logs, CPU/Memory utilization alarms, 5XX error detection, and email alerts. |

---

## 2. Directory Structure

```
terraform/
├── environments/
│   ├── dev/
│   │   ├── main.tf                    # Development environment instantiation
│   │   ├── variables.tf               # Dev variables
│   │   ├── outputs.tf                 # Dev outputs
│   │   └── terraform.tfvars.example   # Example Dev values
│   └── prod/
│       ├── main.tf                    # Production environment instantiation
│       ├── variables.tf               # Prod variables
│       ├── outputs.tf                 # Prod outputs
│       └── terraform.tfvars.example   # Example Prod values
└── modules/
    ├── vpc/                           # VPC, Subnets, IGW, NAT Gateway, Route Tables
    ├── ecr/                           # ECR repository, lifecycle policy
    ├── efs/                           # EFS file system, access point, mount targets & SG
    ├── alb/                           # ALB, target group, HTTP/HTTPS listeners & SG
    ├── ecs/                           # ECS cluster, Fargate task def, IAM roles, service, autoscaling
    ├── secrets/                       # Secrets Manager for SMTP/App credentials
    ├── cloudfront/                    # CloudFront CDN distribution with custom behaviors
    └── monitoring/                    # CloudWatch metric alarms & SNS alert topics
```

---

## 3. Prerequisites

1. **AWS CLI** configured (`aws configure`) with appropriate administrator/infrastructure permissions.
2. **Terraform** >= 1.5.0 installed.
3. **Docker** installed for building and pushing the container image.

---

## 4. Step-by-Step Deployment Guide

### Step 4.1: Build and Push Docker Image to ECR

1. Initialize and provision the ECR repository first (or apply Terraform with a placeholder):
   ```bash
   cd terraform/environments/dev
   terraform init
   terraform apply -target=module.ecr
   ```
2. Authenticate Docker with AWS ECR:
   ```bash
   aws ecr get-login-password --region <your-region> | docker login --username AWS --password-stdin <aws_account_id>.dkr.ecr.<your-region>.amazonaws.com
   ```
3. Build the container from the project root:
   ```bash
   cd ../../../
   docker build -t sumitkumar-dev-web:latest -f Dockerfile .
   ```
4. Tag and push the image to ECR:
   ```bash
   docker tag sumitkumar-dev-web:latest <ecr_repository_url>:latest
   docker push <ecr_repository_url>:latest
   ```

### Step 4.2: Configure Environment Variables

1. Copy the example variable file:
   ```bash
   cd terraform/environments/dev
   cp terraform.tfvars.example terraform.tfvars
   ```
2. Edit `terraform.tfvars` with your AWS region, SMTP credentials, and alert email.

### Step 4.3: Deploy Complete Infrastructure

```bash
terraform plan -out=tfplan
terraform apply tfplan
```

---

## 5. Connecting Custom Domain & SSL (Production)

1. Request or import an SSL certificate in **AWS Certificate Manager (ACM)**:
   - For ALB: Certificate in the same region as the ALB (e.g. `us-east-1`).
   - For CloudFront: Certificate must be created in `us-east-1`.
2. Update `terraform/environments/prod/terraform.tfvars` with the certificate ARNs and domain aliases (`sumitkumar.dev`, `www.sumitkumar.dev`).
3. Point your DNS CNAME / Route 53 Alias record to the `cloudfront_domain_name` output.

---

## 6. Teardown / Cleanup

To avoid unnecessary AWS charges:
```bash
cd terraform/environments/dev
terraform destroy
```
