# EC.RestfulToken

## Overview
The EC.RestfulToken project was built to simulate a normal system integration between a consumer [Worker Service](#EC.RestfulToken.Client.WorkerService) and a secured vendor [RESTful API](#EC.RestfulToken.Server.Api).  These types of integrations are very common in both professional and personal environments.  Though the means of authentation could be different based on the vendor's api specs this project should give a good understanding of how to do this kind of communication and persist the security token(s) until they expire as not to flood new token requests.

## EC.RestfulToken.Server.Database
The database project is to support the Api project by providing user and user secret lookups.  this project will allow you to create your own copy of the database consisting of just 3 tables.

### Database Project details
- dbo
  - Data
    - **Seed.sql** 
      > This file contains the seed data for the database. It is used to populate the database with initial data when it is created. Executed via the **EC.RestfulToken.Server.Database.publish**.
  - Tables
	- **Domain.sql**
	  > This file contains the schema for the Domain table. It is used to store information about the domains that are used in the application. Domain being a grouping of users, like a customers company.
	- **User.sql**
	  > This file contains the schema for the User table. It is used to store information about the users that are used in the application. The user is a person that is using the application.
	- **UserSecret.sql** 
	  > This file contains the schema for the UserSecret table. It is used to store information about the secrets that are used in the application. The secret is a token that is used to authenticate the user.
- **EC.RestfulToken.Server.Database.publish.sql**
  > This file is the publish profile for the defined database and is used to spin up the database locally for the **EC.RestfulToken.Server.Api** project to run.  This publish profile targets the **(localdb)\MSSQLLocalDB** which is installed with Visual Studio.  This is a local database that is used for development and testing purposes.  It is not intended to be used in production.  The publish profile can be modified to target a different database if needed.  The publish profile can be used to create the database and populate it with initial data.

### Database Creation
1. Open the **EC.RestfulToken.Server.Database** project in Visual Studio.
1. Open the **EC.RestfulToken.Server.Database.publish.sql** file.
1. If you have a **(localdb)\MSSQLLocalDB** instance installed, you can click **Publish**.
> if you do not have a **(localdb)\MSSQLLocalDB** instance installed, you can modify the publish profile to target a different database.  If you do change the target you will also need to update the appsettings.json file in the **EC.RestfulToken.Server.Api** project to point to the new database.


## EC.RestfulToken.Server.Api
The API project is a simple RESTful API that is used to authenticate users with JWT tokens and provide a simple data endpoint that is secured by those JWT tokens.  This API does implement a Swagger page to show a simple implementation using auth setup. 

### API Project details
- **Controllers**
  - **TokenController.cs**
	> This controller is used to authenticate users and generate JWT tokens. It contains the following endpoints:
	- **POST /Token/\{tenantId\}**
	  >This endpoint is used to authenticate a user and generate a JWT token. The user must provide a username and password in the request body. If the authentication is successful, the server will return a JWT token that can be used to access secured endpoints.
  - **TestContentController.cs**
	> This controller is used to provide a simple data endpoint that is secured by JWT tokens. It contains the following endpoints:
	- **GET /TestContent**
	  >This endpoint is used to retrieve data that the user has access to. The user must provide a valid JWT token in the request header. If the token is valid, the server will return the data.
- **EC.RestfulToken.Server.Api.http**
  > This file is used to test the API endpoints using the **REST Client** embedded in **Visual Studio v17.8+**.


## EC.RestfulToken.Client.WorkerService
The Worker Service project is a simple service that runs in a basic loop on a interval defined in the appsettings.json file.  The service shows how to authenticate with the API and retrieve a JWT token, how to handle token expiration and refresh the token when needed.  It also shows how to use that token to access a secured endpoint for data retrieval.  Normally in such a service the data would be processed in some way, either into a database or other medium for current or future use... but that is all up to you.

### Worker Service Project details
- **Program.cs**
  >This file contains the main entry point for the worker service. It is used to configure and start the worker service.
- **Worker.cs**
  >This is the actual worker class that runs in the background. It calls to the **RestfulTokenServerService.cs** which contains the logic for authenticating with the API, retrieving a JWT token, and accessing secured endpoints with that token. It also contains the logic for handling token expiration and refreshing the token when needed.
- **appsettings.json**
  >This file contains the configuration for the worker service. It includes the following settings:
  - **IntervalSeconds**
    >The interval in seconds between each request to the API.  
  - **RestfulTokenServer**
    >Section for the API settings.  This section contains the following settings:
      - **Endpoint** 
        >The base URL of the API endpoints used.
      - **TenantId**
    	>The TenantID used to authenticate. This is a unique identifier for the user group.
      - **ClientId**
    	>The ClientID used to authenticate. This is a unique identifier for the user.
      - **ClientSecret**
    	>The ClientSecret used to authenticate. This is a unique secret for the user.
  
  ##