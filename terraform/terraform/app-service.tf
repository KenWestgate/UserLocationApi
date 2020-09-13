resource "azurerm_redis_cache" "userlocation" {
  name                = local.resource_name
  location            = local.location
  resource_group_name = azurerm_resource_group.userlocation.name
  capacity            = 0
  family              = "C"
  sku_name            = "Basic"
  enable_non_ssl_port = false
  minimum_tls_version = 1.2

  redis_configuration {
    maxfragmentationmemory_reserved = 0
    maxmemory_delta                 = 0
    maxmemory_policy                = "volatile-lru"
    maxmemory_reserved              = 0
  }

  #    timeouts {}
}

resource "azurerm_app_service_plan" "userlocation" {
  name                = local.resource_name
  location            = local.location
  resource_group_name = azurerm_resource_group.userlocation.name
  kind                = "Linux"
  reserved            = true

  sku {
    tier = "Free"
    size = "F1"
  }
}

resource "azurerm_app_service" "userlocation" {
  name                = local.resource_name
  location            = local.location
  resource_group_name = azurerm_resource_group.userlocation.name
  app_service_plan_id = azurerm_app_service_plan.userlocation.id

  app_settings = {
    "Auth0__ApiIdentifier"          = local.auth0_identifier
    "Auth0__Domain"                 = var.auth0_domain
    "ConnectionStrings__RedisCache" = azurerm_redis_cache.userlocation.primary_connection_string
    # "DOCKER_REGISTRY_SERVER_PASSWORD"     = azurerm_container_registry.userlocation.admin_password
    "DOCKER_REGISTRY_SERVER_URL" = "https://docker.io"
    # "DOCKER_REGISTRY_SERVER_USERNAME"     = azurerm_container_registry.userlocation.admin_username
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
  }
  enabled = true

  site_config {
    app_command_line = "dotnet TechnicalTest.Api.dll"
    default_documents = [
      "index.html"
    ]
    dotnet_framework_version  = "v4.0"
    linux_fx_version          = "DOCKER|${var.docker_image}"
    managed_pipeline_mode     = "Integrated"
    min_tls_version           = "1.2"
    use_32_bit_worker_process = true # needed for Free tier?
  }
}

output "userlocation_api_url" {
  value = "https://${azurerm_app_service.userlocation.default_site_hostname}"
}