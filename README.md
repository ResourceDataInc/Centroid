# Centroid

[![Build Status](https://travis-ci.org/ResourceDataInc/Centroid.png?branch=master)](https://travis-ci.org/ResourceDataInc/Centroid)

Often, multiple related projects will need access to the same configuration values, which can be problematic if the projects use different technologies. Typically, configuration values are then stored in multiple places, making managing these values difficult. 

Centroid lets you take control of your configurations by making it easy to work with a central configuration file that is accessible by various languages using a single API. Currently, Centroid supports Python, .NET, and Ruby.

## Key Features of Centroid

+ Unified API supports multiple languages
+ Convenient method for retrieving all configuration values for a specific environment: environment-specific values are merged with values that apply to all environments 
+ Ability to use native-language conventions to look up a key (underscores and case are ignored)
+ Ability to load the configuration file as a file or a string

## Using Centroid

To use Centroid, you must first install the package for each desired language. Refer to the [language-specific documents](#language_specific) for installation instructions.

Once a Centroid package is installed, complete the following two items. [Examples](#examples) of both items are available later in this document.

1. Declare your application's configuration values in JSON. 
1. Create an instance of the Config class to use the API. Refer to the [language-specific documents](#language_specific) for details specific to each language.

## Using Native-language Conventions
Because Centroid ignores underscores and case, you can use native-language conventions to look up a key. 

For example, your JSON configuration file could use camelCase while your Config class uses snake_case, PascalCase, or camelCase as appropriate for the language.

## <a name="examples"></a>Examples

### JSON Configuration File

The following example demonstrates storing a database's server address configuration value.
```json
{
    "database": {
        "serverAddress": "my-server.local"
    }
}
```
However, applications typically have different configuration values for each environment. For example, *dev* and *prod* environments use different servers and user accounts. But applications usually also have configuration values that are the same across all environments. 

To create an environment-based configuration, make the top-level objects in the JSON represent the various environments, as in the example below. List the configuration values specific to an environment under the appropriate top-level object. 

Also create an *all* top-level object. List the configuration values that are the same across all environments within this *all* environment.


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
> *Note:*	Refer to the [language-specific documents](#language_specific) for information on using the environment instance method to retrieve environment-based configurations.

### API (Config Class)
> *Note:*	These Config class examples assume a JSON configuration file with the following: 

```json
{
    "database": {
        "serverAddress": "my-server.local"
    }
}
```
The following Python example demonstrates loading a file (`config.json`) and retrieving the database’s server address using a snake_case property (`database.server_address`). 

```py
# my_app.py
config = Config.from_file("config.json")
server = config.database.server_address # => "my-server.local"
```
The following C# example demonstrates loading the JSON as a string and then retrieving the database's server address using a PascalCase property (`Database.ServerAddress`).

```cs
// MyApp.cs
var json = @"{ ""database"": { ""serverAddress"": ""my-server.local"" } }";
dynamic config = new Config(json);
var server = config.Database.ServerAddress; // => "my-server.local"
```
Refer to the [language-specific documents](#language_specific) for additional details about using the API, including examples of using the environment instance method.

## <a name="language_specific"></a>Language-specific Documents
Each language has a separate document with information specific to that language. 

* [.NET](dot-net/README.md)
* [Python](python/README.md)
* [Ruby](ruby/README.md)

## Contributing

See [Contributing](CONTRIBUTING.md).

## Credits

Thanks to all [contributors](https://github.com/ResourceDataInc/Centroid/graphs/contributors)!

## License

Centroid is licensed under the [MIT License](LICENSE.txt).
