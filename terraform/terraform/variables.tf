resource "random_string" "suffix" {
  length  = 8
  lower   = true
  upper   = false
  special = false
}

variable "auth0_client_id" {
  type = string
}

variable "auth0_domain" {
  type = string
}

variable "auth0_client_secret" {
  type = string
}

variable "docker_image" {
  type = string
}