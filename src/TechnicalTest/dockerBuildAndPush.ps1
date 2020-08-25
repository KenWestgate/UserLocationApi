$imageTag = Get-Date -Format "yyyyMMddHHmmss"
$imageName = "technical-test"
$acrName = "techtest20200824"
$acrFullName = "techtest20200824.azurecr.io"
$imageFullTag = "$($acrFullName)/$($imageName):$($imageTag)"

Write-Host "Build $($imageFullTag)"
docker build -t $imageFullTag .

Write-Host "pushing to ACR"
az login
az acr login --name $acrName

docker push $imageFullTag