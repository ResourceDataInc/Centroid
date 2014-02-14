# Centroid

[![Build Status](https://travis-ci.org/ResourceDataInc/Centroid.png?branch=master)](https://travis-ci.org/ResourceDataInc/Centroid)

A centralized paradigm to configuration management.

**Centroid:** *(of a finite set) the point whose coordinates are the mean values of the coordinates of the points of the set*.

## About

In our experience, multiple projects related to one another will eventually need access to the same configuration. This can be problematic when the projects are built upon different technologies. Our solution is to extract the configuration to a shared JSON file, and provide means to access the values from various languages.

Centroid is library for loading configuration values declared in JSON, and accessing those configuration values using object properties.

## Usage

You start by declaring your application's configuration values in JSON. This example is storing the database's server address.

```json
{
    "database": {
        "serverAddress": "my-server.local"
    }
}
```

Applications can consume this JSON configuration using Centroid's API.

### API

Centroid's API is pretty simple. Centroid can load JSON configuration from a file or as a string, and then your application has access to the configuration values through object properties. Applications can choose retrieve the entire set of configuration or only the configuration appropriate for a specific enviroment.

Centroid also includes niceties that make interacting with the configuration a bit more clean. For example, the process to lookup a key in the config will ignore underscores and is case insensitive. This allows for storing the config in camelCase, while accessing it in camelCase or snake_case or PascalCase, therefore giving different languages the option to consume configuration using native language conventions.

The API is currently available in python and .NET.

#### Python

Here's a python example that loads a `config.json` file containing the JSON declared above and then retrieves the database's server address using a snake_case property `database.server_address`.

```py
# my_app.py
config = Config.from_file("config.json")
server = config.database.server_address # => "my-server.local"
```

See the [python](python/README.md) API documention for information on how to install and use the python API.

#### .NET

Here's a C# example that loads the JSON as a string and then retrieves the the database's server address using a PascalCase property `Database.ServerAddress`.

```cs
// MyApp.cs
var json = @"{ ""database"": { ""serverAddress"": ""my-server.local"" } }";
dynamic config = new Config(json);
var server = config.Database.ServerAddress; // => "my-server.local"
```

See the [.NET](dot-net/README.md) API documentation for information on how to install and use the .NET API.

#### Environments

The API also includes a convenience method for retrieving configuration values for a specific environment. The environment specific configuration is merged with the configuration that applies to all environents, giving your application all the appropriate configuration for the specified environment.

See the [python](python/README.md) and/or [.NET](dot-net/README.md) API documention for more details.

## Contributing

If you'd like to report a bug or contribute a fix or feature, that's great!

To file a bug report or request a feature, open a new [GitHub Issue](https://github.com/ResourceDataInc/Centroid/issues/new).

To contribute code/documentation changes:

1. [Fork](https://github.com/ResourceDataInc/Centroid/fork) the repository
1. Create a feature branch and commit your changes
1. Submit a pull request

## Credits

Thanks to all [contributors](https://github.com/ResourceDataInc/Centroid/graphs/contributors)!

## License

Centroid is licensed under the [MIT License](LICENSE.txt)
