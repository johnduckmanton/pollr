# Pollr Sample Application

This application is a full stack ASP.NET Core/Angular application. I am using it to demonstrate and explore various options for deploying and hosting of web applications in Microsoft Azure.

# Project Structure

The project directory structure is as follows:
```
/Pollr.UI - Angular Web User Interface for voting
/Pollr.AdminUI - Angular Administration Web User Interface
/Pollr.Api - ASP.NET Core Web Api
```
# Components

## Angular Web User Interface
This application is an Angular application built initially using the Angular CLI and hosted in an ASP.NET Core Web Application. This application is designed primarily to display on mobule devices

### Building the application

- `npm run build`- to build the application
- `npm run docker:build` - to build the Docker image

## Angular Administration Web User Interface
This application is an Angular application built initially using the Angular CLI and hosted in an ASP.NET Core Web Application.
The purpose of this application is to provide an administrative user interface for creating and maintaining poll definitions and for viewing poll results.

### Building the application

- `npm run build`- to build the application
- `npm run docker:build` - to build the Docker image

## ASP.NET Core REST API Component

This project is an implementation of the REST API using ASP.NET Core 2.1 Web Api.

### Swagger/OpenAPI Definition

There is a Swagger definition file for the API and Swagger UI is also available, just use `/swagger` as the URL, e.g. **http://localhost:5000/swagger/**

### Building the application

- `npm run build`- to build the application
- `npm run docker:build` - to build the Docker image

### Deploying the application
#### Azure DevOps Status

Project  | Build Status | Release Status |
 ------------ | :-----------: | :-----------: |
Pollr.Api    | [![Build status](https://dev.azure.com/johnduckmanton/pollr/_apis/build/status/pollr-api.build)](https://dev.azure.com/johnduckmanton/pollr/_build/latest?definitionId=7) | ![Release status](https://vsrm.dev.azure.com/johnduckmanton/_apis/public/Release/badge/09f8c2ac-eb3c-4f28-b7ee-7512a43283e6/2/2)
Pollr.UI       |[![Build status](https://dev.azure.com/johnduckmanton/pollr/_apis/build/status/pollr-UI.build)](https://dev.azure.com/johnduckmanton/pollr/_build/latest?definitionId=9) | ![Release status](https://vsrm.dev.azure.com/johnduckmanton/_apis/public/Release/badge/09f8c2ac-eb3c-4f28-b7ee-7512a43283e6/4/4)
Pollr.AdminUI  |[![Build status](https://dev.azure.com/johnduckmanton/pollr/_apis/build/status/pollr-adminUI.build)](https://dev.azure.com/johnduckmanton/pollr/_build/latest?definitionId=8) | ![Release status](https://vsrm.dev.azure.com/johnduckmanton/_apis/public/Release/badge/09f8c2ac-eb3c-4f28-b7ee-7512a43283e6/3/3)





# License

Copyright (c) John Duckmanton. All rights reserved.
Licensed under the MIT License.
