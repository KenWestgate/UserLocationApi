# resource "azurerm_container_registry" "userlocation" {
#   name                = "userlocation${local.location}"
#   resource_group_name = azurerm_resource_group.userlocation.name
#   location            = local.location
#   sku                 = "Basic"
#   admin_enabled       = true
# }