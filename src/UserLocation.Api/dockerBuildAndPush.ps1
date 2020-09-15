$imageTag = Get-Date -Format "yyyyMMddHHmmss"
$imageName = "userlocation-api"

$namespace = Read-Host -Prompt "Docker hub namespace"
$imageFullTag = "docker.io/$($namespace)/$($imageName):$($imageTag)"

Write-Host "Build $($imageFullTag)"
docker build -t $imageFullTag .

Write-Host "pushing image to docker hub: $($imageFullTag)"

docker push $imageFullTag