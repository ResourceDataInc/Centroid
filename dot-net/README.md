# Centroid - .NET

## Installation

The Centroid .NET package is hosted at [Nuget](http://www.nuget.org/packages/Centroid/).

You can install Centroid using Package Manager Console with `Install-Package Centroid`.

## Usage

Begin by declaring your application's configuration values in JSON. The following example stores the database's server address.

```json
{
    "database": {
        "serverAddress": "my-server.local"
    }
}
```

.NET applications can consume this JSON configuration using Centroid's .NET API.

### .NET API

The .NET API is available through the `Centroid.Config` class, an instance of which is used to consume the JSON. 

The `Config` class exposes the static `FromFile` method, a constructor, and the instance method `ForEnvironment`.

#### FromFile(filename)

You can create an instance of `Config` from a JSON file using the static `Config.FromFile(filename)` method.

In the example below, note the `serverAddress` configuration value is retrieved using the PascalCase `ServerAddress` property even though the value is specified as camelCase in the JSON.

```cs
// FromFile.cs
dynamic config = Config.FromFile("config.json");
var server = config.Database.ServerAddress; // => "my-server.local"
```

#### Config(json)

Alternatively, you can create an instance of `Config` by passing a JSON string to the `Config` constructor.

```cs
// FromString.cs
var json = @"{ ""Database"": { ""Server"": ""my-server.local"" } }";
dynamic config = new Config(json);
var server = config.Database.ServerAddress; // => "my-server.local"
```

#### ForEnviroment(environment)

Typically, real-world applications have different configuration values depending on the environment. For example, you might have *dev* and *prod* environments that use different servers, user accounts, etc. However, applications usually have other configuration values that are the same across all environments. Centroid makes it easy to retrieve all the configuration values you need for a specific environment.

In environment-based configuration, the top-level objects in the JSON represent the various environments. Place the configuration values that are the same across all environments within the *all* environment. 

```json
{
    "dev": {
        "someResource": {
            "server": "resource-dev.local"
        }
    },
    "prod": {
        "someResource": {
            "server": "resource-prod.local"
        }
    },
    "all": {
        "keys": {
            "ssh": "path/to/id_rsa.pub"
        }
    }
}
```

Then, in the `Config` instance, use the instance method `ForEnvironment` to retrieve the environment-based configuration. Centroid merges the requested environment's configuration values with the *all* environment configuration values. 

In the following example, the configuration for `prod` is merged with the configuration from `all`; the result is then available from a new instance of `Config`.

```cs
// ForEnvironment.cs
var config = Config.FromFile("config.json").ForEnvironment("Prod");
var server = config.Database.Server; // => "sql-prod.local"
var solutionPath = config.Solutions.Main; // => "path/to/Main.sln";
```

## Contributing

See the [Contributing section of the main README](../README.md#contributing).
