# Centroid - Python

## Installation

The Centroid Python package is hosted at [PyPi](https://pypi.python.org/pypi/centroid). 

You can install Centroid using pip with `pip install centroid`, or you can download Centroid, unpack, and `python setup.py install` it.

## Usage

Begin by declaring your application's configuration values in JSON. The following example stores the database's server address.

```json
{
    "database": {
        "serverAddress": "my-server.local"
    }
}
```

Python applications can consume this JSON using Centroid's Python API.

### Python API

The Python API is available through the `centroid.Config` class, an instance of which is used to consume the JSON. 

The `Config` class exposes the static `from_file` method, a constructor, and the instance method `for_environment`.

#### from_file(filename)

You can create an instance of `Config` from a JSON file using the static `Config.from_file(filename)` method.

In the example below, note the `serverAddress` configuration value is retrieved using the snake_case `server_address` property even though the value is specified as camelCase in the JSON.

```py
# from_file.py
config = Config.from_file("config.json")
server = config.database.server_address # => "my-server.local"
```

#### Config(json)

Alternatively you can create an instance of `Config` by passing a JSON string to the `Config` constructor.

```py
# from_string.py
json = '{ "database": { "serverAddress": "my-server.local" } }'
config = Config(json)
server = config.database.server_address # => "my-server.local"
```

#### for_enviroment(environment)

Typically, real-world applications have different configuration values depending on the environment. For example, you might have *dev* and *prod* environments that use different servers, user accounts, etc. However, applications usually have other configuration values that are the same across all environments. Centroid makes it easy to retrieve all of the configuration values you need for a specific environment.

In environment-based configuration, the top-level objects in the JSON represent the various environments. Place the configuration values that are the same in all environments within the *all* environment. 

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

Then, in the `Config` instance, use the instance method `for_environment` to retrieve the environment-based configuration. Centroid merges the requested environment's configuration values with the *all* environment configuration values. In the following example, the configuration for `prod` is merged with the configuration from `all`; the result is then available from a new instance of `Config`.

```py
# for_enviroment.py
config = Config.from_file("config.json").for_environment("prod")
server = config.some_resource.server # => "resource-prod.local"
solution_path = config.keys.ssh # => "path/to/id_rsa.pub"
```

## Contributing

See the [contributing section of the main README](../README.md#contributing).
