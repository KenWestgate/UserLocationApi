. ./login.ps1

Get-TerraformToken
Get-Auth0Secrets
Get-DockerImage

Push-Location ./terraform

terraform init
terraform plan `
    -out userlocation.tfplan

Pop-Location