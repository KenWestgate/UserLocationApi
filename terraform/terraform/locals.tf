locals {
  location         = "uksouth"
  resource_name    = "userlocation-${random_string.suffix.result}"
  auth0_identifier = "${local.resource_name}"
  docker_image     = "kenwestgate/userlocation-api:latest"
}