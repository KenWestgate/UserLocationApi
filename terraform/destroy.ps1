Push-Location ./terraform

terraform plan -destroy -out userlocation.destroy.tfplan

Write-Host "This will destroy all the resources created with terraform"
Write-Host "There is no way to undo this action."
$confirmation = Read-Host -Prompt "If you are you sure you wish to contine, enter yes"

if ($confirmation -eq "yes") {
    terraform apply userlocation.destroy.tfplan
}

Pop-Location