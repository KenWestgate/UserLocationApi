param (
    [Parameter(Mandatory=$false)]$azureAcrName = "techtest20200824",
    [Switch]$AzureAcr,
    [Switch]$AWSEcr
)

function BuildDockerImage {
    param (
        [Parameter(Mandatory=$true)]$imageName,
        [Parameter(Mandatory=$true)]$imageTag
    )
    Write-Host "Build $($imageName):$($imageTag)"
    docker build -t "$($imageName):$($imageTag)" .
}

function PushToAzureAcr {
    param (
        [Parameter(Mandatory=$true)]$imageName,
        [Parameter(Mandatory=$true)]$imageTag
    )

    $imageNameAndTag = "$($imageName):$($imageTag)"
    $azureAcrFullName = "$($azureAcrName).azurecr.io"
    $azureImageNameAndTag = "$($azureAcrFullName)/$($imageName):$($imageTag)"
    
    Write-Host "pushing to Azure ACR $($azureAcrName)"

    az login
    az acr login --name $azureAcrName

    docker tag $imageNameAndTag $azureImageNameAndTag
    docker push $azureImageNameAndTag
}

function PushToAwsEcr {
    param (
        [Parameter(Mandatory=$true)]$imageName,
        [Parameter(Mandatory=$true)]$imageTag
    )

    $imageNameAndTag = "$($imageName):$($imageTag)"

    aws configure

    $awsAccountId = Read-Host -Prompt "Enter your AWS account id"
    $awsRegion = Read-Host -Prompt "Enter AWS Region (eg. eu-west-2)"

    aws ecr get-login-password --region eu-west-2 | docker login --username AWS --password-stdin "$($awsAccountId).dkr.ecr.$($awsRegion).amazonaws.com"

    $awsImageNameAndTag = "$($awsAccountId).dkr.ecr.$($awsRegion).amazonaws.com/$($imageName):$($imageTag)"

    Write-Host "Tag AWS image $($awsImageNameAndTag)"

    docker tag $imageNameAndTag $awsImageNameAndTag

    docker push $awsImageNameAndTag
}

#####################################################################################
$imageTag = Get-Date -Format "yyyyMMddHHmmss"
$imageName = "userlocation-api"

BuildDockerImage -imageName $imageName -imageTag $imageTag

if (!$?) {
    Write-Host "Image was not built"
    Exit
}

if ($true -eq $AzureAcr) {
    PushToAzureAcr -imageName $imageName -imageTag $imageTag
}

if ($true -eq $AWSEcr) {
    PushToAwsEcr -imageName $imageName -imageTag $imageTag
}