# Centroid

[![Build Status](https://travis-ci.org/ResourceDataInc/Centroid.png?branch=master)](https://travis-ci.org/ResourceDataInc/Centroid)

A centralized paradigm of configuration management

**Centroid:** *(of a finite set) the point whose coordinates are the mean values of the coordinates of the points of the set*

## About

In our experience, multiple projects related to one another will eventually need access to the same configuration values. This can be problematic when the projects are built upon different technologies. Our solution is to extract the configuration values to a shared JSON file and provide a means for various languages to access the values.

Centroid is a tool for loading configuration values declared in JSON and accessing those values using object properties.

## Usage

Start by declaring your application's configuration values in JSON. The following example stores the database's server address.

```json
{
    "database": {
        "serverAddress": "my-server.local"
    }
}
```

Applications can consume this JSON using Centroid's API.

### API

Centroid's API is pretty simple. Centroid can load the JSON as a file or a string, and then your application has access to the configuration values through object properties. Applications can choose to retrieve the entire set of configuration values or only the values appropriate for a specific environment.

Centroid also includes niceties that make interacting with the configuration values a bit cleaner. For example, the process to look up a key in the JSON ignores underscores and is case insensitive. This feature allows you to store the configuration value in camelCase while accessing it in snake_case, PascalCase, or camelCase, giving the option to consume the configuration values using native-language conventions.

The API is currently available in Python and .NET.

#### Python

Below is a Python example that loads a `config.json` file containing the JSON above and then retrieves the database's server address using a snake_case property `database.server_address`.

```py
# my_app.py
config = Config.from_file("config.json")
server = config.database.server_address # => "my-server.local"
```

See the [Python](python/README.md) API documentation for information on how to install and use the Python API.

#### .NET

Below is a C# example that loads the JSON as a string and then retrieves the database's server address using a PascalCase property `Database.ServerAddress`.

```cs
// MyApp.cs
var json = @"{ ""database"": { ""serverAddress"": ""my-server.local"" } }";
dynamic config = new Config(json);
var server = config.Database.ServerAddress; // => "my-server.local"
```

See the [.NET](dot-net/README.md) API documentation for information on how to install and use the .NET API.

#### Environments

The API also includes a convenience method for retrieving configuration values for a specific environment. The environment-specific configuration values are merged with the configuration values that apply to all environments, giving your application all of the appropriate configuration values for the specified environment.

See the [Python](python/README.md) or [.NET](dot-net/README.md) API documentation for more details.

## Contributing

If you'd like to report a bug or contribute a fix or feature, that's great!

To file a bug report or request a feature, open a new [GitHub Issue](https://github.com/ResourceDataInc/Centroid/issues/new).

To contribute code/documentation changes, complete the following steps:

1. [Fork](https://github.com/ResourceDataInc/Centroid/fork) the repository.
1. Create a feature branch and commit your changes.
1. Submit a pull request.

## Credits

Thanks to all [contributors](https://github.com/ResourceDataInc/Centroid/graphs/contributors)!

## License

Centroid is licensed under the [MIT License](LICENSE.txt).
