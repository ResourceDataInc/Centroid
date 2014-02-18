# Centroid - Ruby

## Installation

The Centroid Ruby package is hosted at [rubygems.org](https://rubygems.org/gems/centroid).

You can install Centroid using gem with `gem install centroid`, or you can download, unpack, then `bundle install` and `rake install` from the `/ruby` directory.

## Usage

Begin by declaring your application's configuration values in JSON. The following example stores the database's server address.

```json
{
    "database": {
        "serverAddress": "my-server.local"
    }
}
```

With Centroid's API, Ruby applications can then use the configuration values in this JSON.

### Ruby API

The Ruby API is available through the `Centroid::Config` class, an instance of which is used to consume the JSON.

The `Config` class exposes the `from_file` class method, an initializer, and the instance method `for_environment`.

#### from_file(filename)

You can create an instance of `Config` from a JSON file using the `Centroid::Config.from_file(filename)` class method.

In the example below, note the `serverAddress` configuration value is retrieved using the snake_case `server_address` method even though the value is specified as camelCase in the JSON.

```rb
# from_file.rb
config = Centroid::Config.from_file("config.json")
server = config.database.server_address # => "my-server.local"
```

### Config(json)

Alternatively, you can create an instance of `Config` by passing a JSON string to the `Config` initializer.

```rb
# from_string.rb
json = '{ "database": { "serverAddress": "my-server.local" } }'
config = Centroid::Config(json)
server = config.database.server_address # => "my-server.local"
```

### for_enviroment(environment)

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

Then, in the `Config` instance, use the instance method `for_environment` to retrieve the environment-based configuration. Centroid merges the requested environment's configuration values with the *all* environment configuration values.

In the following example, the configuration for `prod` is merged with the configuration from `all`; the result is then available from a new instance of `Config`.

```rb
# for_enviroment.rb
config = Centroid::Config.from_file("config.json").for_environment("prod")
server = config.some_resource.server # => "resource-prod.local"
solution_path = config.keys.ssh # => "path/to/id_rsa.pub"
```

## Contributing

See the [contributing section of the main README](../README.md#contributing)
