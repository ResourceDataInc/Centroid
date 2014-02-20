# Centroid - Ruby

This readme file includes information specific to Ruby in Centroid. Refer to the [Centroid Readme file] (../README.md) for general information, including information the JSON configuration file. 

## Installation

The Centroid Ruby package is hosted at [rubygems.org](https://rubygems.org/gems/centroid). Install Centroid using gem with `gem install centroid` or clone the repo and then `gem build centroid.gemspec` and `gem install -l centroid` from the `ruby/` directory.

## Centroid::Config Class with Ruby

In Ruby, the `Centroid::Config` class exposes the following: 

+ Static `from_file` class method
+ Initializer
+ `for_environment` instance method

### from_file Class Method

Using the static `Centroid::Config.from_file(filename)` class method, you can create an instance of `Config` from a JSON file, as in the example below. 

```rb
# from_file.rb
config = Centroid::Config.from_file("config.json")
server = config.database.server_address # => "my-server.local"
```

### Config Initializer

To load a string instead of a file, create an instance of `Config` by passing a JSON string to the `Config` initializer, as in the following example.

```rb
# from_string.rb
json = '{ "database": { "serverAddress": "my-server.local" } }'
config = Centroid::Config(json)
server = config.database.server_address # => "my-server.local"
```

### for_enviroment Instance Method

In the `Config` instance, you can use the `for_environment` instance method to retrieve the configuration values for an environment. 

If you specify an environment in `for_environment`, Centroid will merge the requested environment's configuration values with the values in *all*. Refer to [Examples in the Centroid Readme file] (../README.md#examples) for information on creating an environment-based JSON configuration file. 

With the following example, Centroid will merge the configuration for *prod* with the configuration for *all*; the result is then available from a new instance of `Config`.

```rb
# for_enviroment.rb
config = Centroid::Config.from_file("config.json").for_environment("prod")
server = config.some_resource.server # => "resource-prod.local"
solution_path = config.keys.ssh # => "path/to/id_rsa.pub"
```
