# W37L Social Media API

## Overview
The W37L API is a crucial part of a decentralized social media platform built on a Domain-Driven Design (DDD) framework using CQRS principles. It supports a variety of user and content interaction functionalities that allow for a robust social networking experience. This backend is designed to work in conjunction with the W37L React Client, which handles the frontend interactions and authentication processes.

## Prerequisites
To run this API, ensure you have the following:
- .NET Core SDK (version specified in your project)
- Suitable IDE for .NET development (e.g., Visual Studio, Visual Studio Code)
- Connectivity to a Firebase project (for user authentication and data storage)
- Access to a configuration file (`appSettings.json`) not included in the repository for security reasons.

## Configuration
Before running the API, you must set up the `appSettings.json` file in the `WebApi` project with the following structure:

```json
{
  "FirebaseConfig": {
    "apiKey": "<your_firebase_api_key>",
    "authDomain": "<your_firebase_auth_domain>",
    "projectId": "<your_firebase_project_id>",
    "storageBucket": "<your_firebase_storage_bucket>",
    "messagingSenderId": "<your_firebase_messaging_sender_id>",
    "appId": "<your_firebase_app_id>",
    "measurementId": "<your_firebase_measurement_id>"
  },
  "BackendConfig": {
    "BaseUrl": "http://db.w37l.com:3000/api"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
## Running the API
To run the API locally:
1. Open your command line or terminal.
2. Navigate to the root directory of the API.
3. Execute `dotnet restore` to install dependencies.
4. Run `dotnet run` to start the server.

The API will be available by default at `http://localhost:5000` unless configured otherwise in the project properties.

## API Endpoints
The API provides endpoints divided into queries and actions according to CQRS principles:

### User Endpoints
- **Get User by Username**: `GET /user/username/{Username}`
- **Get User by UID**: `GET /user/uid/{UserId}`
- **Get User Relations**: `GET /user/{userId}/relations`
- **Get User Following**: `GET /user/{userId}/following`
- **Get User Followers**: `GET /user/{userId}/followers`
- **Get All Users**: `GET /users`
- **Create User**: `POST /user/create`
- **Update User**: `PUT /user/update`
- **Update Avatar**: `PATCH /user/update/avatar`
- **Update Banner**: `PATCH /user/update/banner`

### Post Endpoints
- **Get All Posts**: `GET /posts`
- **Get Posts by User Comments**: `GET /posts/user/comments/{userId}`
- **Get Post by ID**: `GET /posts/{PostId}`
- **Get Posts by User ID**: `GET /posts/user/id/{UserId}`
- **Get Posts by Username**: `GET /posts/user/username/{Username}`
- **Create Post**: `POST /post/create`
- **Comment on a Post**: `POST /post/{ParentPostId}/comment`

### Comment Endpoints
- **Get Comments by Post**: `GET /comments/post/{postId}`
- **Get Comments by User**: `GET /comments/user/{userId}`
- **Get Comment by ID**: `GET /comments/{commentId}`
- **Get Comments Count by Post**: `GET /comments/post/count/{postId}`

### Interaction Endpoints
- **Block User**: `POST /interaction/block/{userToBlockId}`
- **Follow User**: `POST /interaction/follow/{userToFollowId}`
- **Highlight Post**: `POST /interaction/highlight/{postId}`
- **Like Content**: `POST /interaction/like/{contentId}`
- **Mute User**: `POST /interaction/mute/{userToMuteId}`
- **Report User**: `POST /interaction/report/{userId}`
- **Retweet Post**: `POST /interaction/retweet/{postId}`
- **Unblock User**: `POST /interaction/unblock/{userToUnblockId}`
- **Unfollow User**: `POST /interaction/unfollow/{userToUnfollowId}`
- **Unhighlight Post**: `POST /interaction/unhighlight/{postId}`
- **Unlike Content**: `POST /interaction/unlike/{contentId}`
- **Unmute User**: `POST /interaction/unmute/{userToUnmuteId}`
- **Unreport User**: `POST /interaction/unreport/{userId}`
- **Unretweet Post**: `POST /interaction/unretweet/{postId}`

### Authentication
All API endpoints require JWT authentication provided by the frontend application. The API uses the `[Authorize]` attribute to secure endpoints. You must connect this API with the frontend to successfully make authenticated requests.

## Integration with Frontend
This API is designed to work seamlessly with the [W37L React Client](https://github.com/W37L/W37L-React-Client). Ensure that the client is configured to communicate with this API using the provided `BackendConfig.BaseUrl` in `appSettings.json`.

## Testing
To test the API, you should run it locally. Ensure that your local setup is correctly configured with the necessary credentials and environment variables as described in the Configuration section. Once the API is running, you can use tools like Postman or cURL to make requests to `http://localhost:5000`. Ensure that your requests include the JWT in the Authorization header, which you will need to obtain from the corresponding authentication service or by using the frontend application to log in and retrieve the token.


## Repository Structure
This repository is part of a larger project. Other related repositories can be found within the same GitHub organization, providing frontend and additional backend services. Following the established repository style will maintain consistency across the project.

