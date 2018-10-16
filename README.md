# DevConnector Sample Application

This application is a full stack Node.js/React application based on the tutorial code from Brad Traversy's excellent "MERN Stack Front to Back: Full Stack React, Redux, and Node.js" course. I am using it to demonstrate and explore various options for deploying and hosting of Node.js applications in Microsoft Azure.

# Project Structure

The project directory structure is as follows:

```
/react-client - React Web User Interface
/rest-api-node - Rest API using Node.js & Express
```

# Components

## React Web Application
This application is a React application build initially using `create-react-app`.
The application directory structure is as follows:

```
/react-client
	/build
	/node_modules
	/public
	/src
		/actions
		/components
		/reducers
		/utils
		/validation
```

### Building the application

- `npm run build`- to build the application
- `npm run docker:build` - to build the Docker image

## Node REST API Component

This project is an implementation of the REST API using Node.js and Express Web Server.
The application directory structure is as follows:

/rest-api-node
	/models
	/node_modules
	/routes
	/validation

The API routes are defined in:

- `/routes/api/posts.js`: Posts service
- `/routes/api/profile.js`: Profile service
- `/routes/api/users.js`: User account service

**Posts:**

- `GET /api/posts` - Get all posts
- `GET /api/post/{id}` - Get a single post matching the specified id
- `POST /api/posts` - Create a new post (authenticated)
- `DELETE /api/posts/{id}` - Delete post (authenticated)
- `POST api/posts/{id}/like` - Like to a post (authenticated)
- `POST api/posts/{id}/unlike` - Unlike a post
- `POST api/posts/{id}/comments` - Add a comment to a post (authenticated)
- `DELETE api/posts/{id}/comments/{comment_id}` - Delete a comment from a post

**Profile:**

- `GET api/profile/all` - Get all user profiles
- `GET /api/profile/` - Get the current user's profile (authenticated)
- `GET api/profile/handle/{handle}` - Get a user's profile using their handle
- `GET api/profile/user/{user_id}` - Get a user's profile using their id
- `POST api/profile` - Create or update the current user's profile (authenticated)
- `DELETE api/profile` - Delete a user & their profile
- `POST api/profile/experience` - Add experience to the current user's user profile (authenticated)
- `POST api/profile/education` - Add education to the current user's user profile (authenticated)
- `DELETE api/profile/experience/{experience_id}` - Delete experience from a user profile (authenticated)
- `DELETE api/profile/education/{education_id}` - Delete education from a user profile (authenticated)

**Users:**

- `POST api/users/register` - Register a user
- `POST api/users/login` - Login the user and return a JWT Token
- `GET api/users/current` - Returns the current user (authenticated)

**Diag:**

- `GET /api/diag/info` - Get system information for the Rest Api server

### Swagger/OpenAPI Definition

There is a [Swagger definition file for the API](rest-api-node/swagger.json) and Swagger UI is also available, just use `/api-docs` as the URL, e.g. **http://localhost:5000/api-docs/**

### Building the application

- `npm run build`- to build the application
- `npm run docker:build` - to build the Docker image


### Deploying the application
#### Docker

`docker compose up`

# Code of Conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

# License

Copyright (c) John Duckmanton. All rights reserved.

Licensed under the MIT License.
