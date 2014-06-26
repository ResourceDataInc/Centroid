# Centroid - Python

This document includes information specific to Python in Centroid. Refer to the [Centroid document] (../README.md) for general information, including information the JSON configuration file. 

## Installation

The Centroid Python package is hosted at [PyPi](https://pypi.python.org/pypi/centroid). 

Install Centroid using pip with `pip install centroid` or by downloading Centroid, unpacking it, and using `python setup.py install`.

## Config Class with Python

In Python, the `centroid.Config` class exposes the following: 

+ Static `from_file` method
+ Constructor
+ `for_environment` instance method
+ `__contains__` instance method

> *Note:* The examples given in the following sections are based on the JSON configuration file examples in the [Centroid doucment] (../README.md#examples). 

### from_file Method

Using the static `Config.from_file(filename)` method, you can create an instance of `Config` from a JSON file, as in the example below. 

```py
# from_file.py
config = Config.from_file("config.json")
server = config.database.server_address # => "my-server.local"
```

### Config Constructor

To load a string instead of a file, create an instance of `Config` by passing a JSON string to the `Config` constructor, as in the following example.

```py
# from_string.py
json = '{ "database": { "serverAddress": "my-server.local" } }'
config = Config(json)
server = config.database.server_address # => "my-server.local"
```

### for_environment Instance Method

In the `Config` instance, you can use the `for_environment` instance method to retrieve the configuration values for an environment. 

If you specify an environment in `for_environment`, Centroid will merge the requested environment's configuration values with the values in *all*. Refer to [Examples in the Centroid document] (../README.md#examples) for information on creating an environment-based JSON configuration file. 

With the following example, Centroid will merge the configuration for *prod* with the configuration for *all*; the result is then available from a new instance of `Config`.

```py
# for_enviroment.py
config = Config.from_file("config.json").for_environment("prod")
server = config.some_resource.server # => "resource-prod.local"
solution_path = config.keys.ssh # => "path/to/id_rsa.pub"
```

### \_\_contains\_\_ Instance Method

In a `Config` instance, the `__contains__` method allows for determining if a key exists, by using the `in` and `not in` operators. This method uses the same case and underscore rules as is used for value lookups.

```py
json = '{ "database": { "serverAddress": "my-server.local" } }'
config = Config(json)
"Database" in config # => True
"does_not_exist" in config # => False
```
