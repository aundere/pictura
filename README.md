# Pictura API

Pictura is a API for storing and retrieving images. It provides endpoints for creating images,
retrieving images by tags, and deleting images. The API is built using ASP.NET Core and Entity Framework Core,
and it uses SQLite as the database.

## Running

To run Pictura, clone the repository:

```bash
git clone https://github.com/aundere/pictura.git
```

### Running with Dotnet CLI

To run Pictura using the .NET CLI, you need to have the .NET SDK installed on your machine. You can download it from the [.NET website](https://dotnet.microsoft.com/download).

Publish the project using the following command:

```bash
dotnet publish -c Release -o publish
```

Then, the `publish` directory will be created with the published files. You can run the API using the following command:

```bash
dotnet Pictura.Api.dll
```

### Running with Docker

Build the Docker image using the following command:

```bash
docker build -t pictura-api ./src/Pictura.Api
```

Then, run the Docker container using the following command:

```bash
docker run -d -p 80:80 pictura-api
```

### Running with Docker Compose

Just run the following command to build and run the Docker container:

```bash
docker compose up --build
```

## Usage

To use the Pictura API, you can send HTTP requests to the endpoints. The API supports the following operations:

- **Upload an image**: Send a POST request to `/images` with the JSON body containing the `url` and `tags` fields.
- **Retrieve all images**: Send a GET request to `/images` with comma-separated image tags as a query parameter
  `tags`, `offset` and `limit` for pagination.
- **Retrieve a random image**: Send a GET request to `/images/random` with optional query parameter `tags` with 
  comma-separated image tags.
- **Delete an image**: Send a DELETE request to `/images/{id}` where `{id}` is the ID of the image to delete.

### Authentication

The Pictura API also supports authentication using a custom header `X-Api-Key`. You can set the API key in the `appsettings.json`:

```json
{
    "ApiAuth": {
        "SecretKey": "your api key here",
        "AuthEndpoints": "All"
    }
}
```

The `AuthEndpoints` can be set to `All` to require authentication for all endpoints, `Modifying` to require
authentication only for modifying endpoints (uploading and deleting images), or `None` to disable authentication.

### Database Configuration

The Pictura API uses SQLite as the database. You can change the database provider and connection string in the `appsettings.json`:

```json
{
    "Database": {
        "Type": "Sqlite",
        "ConnectionString": "Data Source=pictura.db"
    }
}
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
