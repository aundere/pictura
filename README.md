# Pictura API

Pictura is a API for storing and retrieving images. It provides endpoints for creating images,
retrieving images by tags, and deleting images. The API is built using ASP.NET Core and Entity Framework Core,
and it uses SQLite as the database.

## Installation

To install Pictura, clone the repository, install the dependencies, and publish the project:

```bash
git clone https://github.com/aundere/pictura.git && cd pictura && dotnet publish -c Release -o publish
```

The project will be published to the `publish` directory. You can then run the API using the following command:

```bash
dotnet publish/pictura.dll
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
