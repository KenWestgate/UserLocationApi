. ./login.ps1

Get-TerraformToken
Get-Auth0Secrets

Push-Location ./terraform

terraform init
terraform plan `
    -out userlocation.tfplan

Pop-Location