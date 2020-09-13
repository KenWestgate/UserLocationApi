resource "auth0_client" "userlocation_client" {
  name = "userlocation_client - Client Grant (Managed by Terraform)"
}

resource "auth0_resource_server" "userlocation_resource_server" {
  name       = "userlocation_server Server - Client Grant (Managed by Terraform)"
  identifier = local.auth0_identifier
  #   scopes {
  #     value       = "create:foo"
  #     description = "Create foos"
  #   }

  #   scopes {
  #     value       = "create:bar"
  #     description = "Create bars"
  #   }
}

resource "auth0_client_grant" "userlocation_client_grant" {
  client_id = auth0_client.userlocation_client.id
  audience  = auth0_resource_server.userlocation_resource_server.identifier
  scope     = ["create:client_grants"]
}


output "auth0_audience" {
  value = auth0_client_grant.userlocation_client_grant.audience
}

output "auth0_client_id" {
  value = auth0_client.userlocation_client.client_id
}

output "auth0_client_secret" {
  value = auth0_client.userlocation_client.client_secret
}