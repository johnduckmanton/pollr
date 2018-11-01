# Pollr Sample Application

This application is a full stack ASP.NET Core/Angular application. I am using it to demonstrate and explore various options for deploying and hosting of web applications in Microsoft Azure.

# Project Structure

The project directory structure is as follows:

```
/Pollr.UI		- Angular Web User Interface
/Pollr.AdminUI	- Angular Administration Web User Interface
/Pollr.Api		- ASP.NET Core Web Api

```

# Components

## Angular Web User Interface
This application is an Angular application build initially using the Angular CLI and hosted in an ASP.NET Core Web Application.

### Building the application

- `npm run build`- to build the application
- `npm run docker:build` - to build the Docker image

## ASP.NET Core REST API Component

This project is an implementation of the REST API using ASP.NET Core Web Api.
The application directory structure is as follows:

**Diag:**

- `GET /api/diag/info` - Get system information for the Rest Api server

### Swagger/OpenAPI Definition

There is a [Swagger definition file for the API] and Swagger UI is also available, just use `/api-docs` as the URL, e.g. **http://localhost:5000/api-docs/**

### Building the application

- `npm run build`- to build the application
- `npm run docker:build` - to build the Docker image


### Deploying the application
#### Docker

`docker compose up`

  "MongoConnection": {
    "ConnectionString": <Connection string>
    "Database": "pollr"
  },
  "SignalR": {
    "UseAzureSignalRManagedHub": false,
    "Azure:SignalR:ConnectionString": <Connection string>
  }

# Code of Conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

# License

Copyright (c) John Duckmanton. All rights reserved.
Licensed under the MIT License.
