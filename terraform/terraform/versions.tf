terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 2.25"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 2.3.0"
    }
    auth0 = {
      source  = "terraform-providers/auth0"
      version = "~> 0.14.0"
    }
  }
}