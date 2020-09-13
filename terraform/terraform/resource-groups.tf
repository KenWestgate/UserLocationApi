resource "azurerm_resource_group" "userlocation" {
  name     = local.resource_name
  location = local.location
}