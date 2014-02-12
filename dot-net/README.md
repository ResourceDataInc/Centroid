# Centroid - .NET

## Installation

**TODO**

## Usage

**TODO** more words

### Simple

```cs
var json = @"{ ""Database"": { ""Server"": ""my-server.local"" } }";
dynamic config = new Config(json);
var server = config.Database.Server; // => "my-server.local"
```

### Case agnostic

```cs
var json = @"{ ""third_party"": { ""rest_endpoint"": ""http://localhost:4567"" } }";
dynamic config = new Config(json);
var server = config.ThirdParty.RestEndpoint; // => "http://localhost:4567"
```

### From file

*config.json*
```json
{
    "Database": {
        "Server": "my-server.local"
    }
}
```

*MyFile.cs*
```cs
var config = Config.FromFile("config.json");
var server = config.Database.Server; // => "my-server.local"
```

### Environments

*config.json*
```json
{
    "Dev": {
        "Database": {
            "Server": "sql-dev.local"
        }
    },
    "Prod": {
        "Database": {
            "Server": "sql-prod.local"
        }
    },
    "All": {
        "Solutions": {
            "Main": "path/to/Main.sln"
        }
    }
}
```

*MyFile.cs*
```cs
var config = Config.FromFile("config.json").ForEnvironment("Prod");
var server = config.Database.Server; // => "sql-prod.local"
var solutionPath = config.Solutions.Main; // => "path/to/Main.sln";
```

## Contributing

See the [contributing section of the main README](../README.md#contributing)
