# User Location API
## Overview
A RESTful API for storage and retrieval of user location data. Latitude and Longitude should be supplied as decimal values rather than traditional notation. eg: some coordinates for Oxford are
Latitude: 51.752022,
Longitude: -1.257726

## Development
Open the src/TechnicalTest/TechnicalTest.sln solution file using Visual Studio 2019.

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

## Caveats
This is not intended for use in real-world navigation or any other application. The code is currently only in alpha test. There are some known limitations.