# User Location API
## Overview
A RESTful API for storage and retrieval of user location data. Latitude and Longitude should be supplied as decimal values rather than traditional notation. eg: some coordinates for Oxford are
Latitude: 51.752022,
Longitude: -1.257726

## Development
Open the src/UserLocation.Api/UserLocation.Api.sln solution file using Visual Studio 2019.

Add the following json to the user secrets file:
`{
    "Auth0": {
        "Domain": "{value may be supplied on request}",
        "ApiIdentifier": "{value may be supplied on request}"
    },
    "ConnectionStrings": {
        "RedisCache": "{value may be supplied on request}"
    }
}`

**NOTE** The two POST methods need authentication and can not currently be used from the Swagger UI.

## Test Data
The TestData.json file in the resources folder has data for use in testing.

## Build and deploy
### Docker image
pre-requisites:
- docker hub account
Building and pushing the image
- Change to the src/UserLocation.Api folder
- run dockerBuildAndPush.ps1, enter your docker namespace.
- take a note of the image name for use in the terraform plan/apply

### Infrastructure
To create or update the infrastructure you need:
- an auth0 account set up for terraform to access: https://auth0.com/blog/use-terraform-to-manage-your-auth0-configuration/#Create-an-Auth0-client-using-HashiCorp-Terraform
- a Microsoft Azure account to access both DevOps and the Portal
  - note that terraform creates a basic redis cache that will cost a small amount to maintain. Remember to run Terraform destroy when the application is no longer needed.
- a terraform account for the state file

Change to the terraform folder and run plan.ps1. This will prompt for credentials the first time it's run and also for the docker image (eg, docker.io/kenwestgate/userlocation-api:20200913143342)

Once the plan is generated, run apply.ps1 to create the infrastructure. The redis cache can take up to twenty minutes to provision.

The URL created for the API is output to the console.

When you no longer need the API, run ./destroy.ps1 to remove the infrastructure.

## Caveats
This is not intended for use in real-world navigation or any other application. The code is currently only in alpha test. There are some known limitations.