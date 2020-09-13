function Get-TerraformToken() {
    if (-Not (Test-Path -Path $env:APPDATA/terraform.d/credentials.tfrc.json)) {
        terraform login
    }
}

function Get-Auth0Secrets() {
    $auth0_secrets_file = "$($env:APPDATA)/auth0.json"
    if (Test-Path -Path $auth0_secrets_file) {
        $auth0_variables = Get-Content -Path $auth0_secrets_file | ConvertFrom-Json -Depth 100
        $env:TF_VAR_auth0_domain = $auth0_variables.auth0_domain
        $env:TF_VAR_auth0_client_id = $auth0_variables.auth0_client_id
        $env:TF_VAR_auth0_client_secret = $auth0_variables.auth0_client_secret
    }
    if ([string]::IsNullOrWhiteSpace($env:TF_VAR_auth0_domain)) {
        $auth0_domain = Read-Host -Prompt "Auth0 Domain"
        $env:TF_VAR_auth0_domain = $auth0_domain
    }
    if ([string]::IsNullOrWhiteSpace($env:TF_VAR_auth0_client_id)) {
        $auth0_client_id = Read-Host -Prompt "Auth0 Client Id"
        $env:TF_VAR_auth0_client_id = $auth0_client_id
    }
    if ([string]::IsNullOrWhiteSpace($env:TF_VAR_auth0_client_secret)) {
        $auth0_client_secret = Read-Host -Prompt "Auth0 Client Secret"
        $env:TF_VAR_auth0_client_secret = $auth0_client_secret
    }
    $auth0_variables = @{
        auth0_domain = $env:TF_VAR_auth0_domain
        auth0_client_id = $env:TF_VAR_auth0_client_id
        auth0_client_secret = $env:TF_VAR_auth0_client_secret
    }
    $auth0_variables_json = $auth0_variables | ConvertTo-Json -Depth 100
    Set-Content -Path $auth0_secrets_file -Value $auth0_variables_json
}

function Get-DockerImage() {
    if ([string]::IsNullOrWhiteSpace($env:TF_VAR_docker_image)) {
        $env:TF_VAR_docker_image = Read-Host -Prompt "Docker image"
    }
}