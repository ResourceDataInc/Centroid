# Centroid - Python

## Installation

**TODO**

## Usage

**TODO** more words

### Simple

```py
json = '{ "database": { "server": "my-server.local" } }'
config = Config(json)
server = config.database.server # => "my-server.local"
```

### Case agnostic

```py
json = '{ "ThirdParty": { "RestEndpoint": "http://localhost:4567" } }'
config = Config(json)
server = config.third_party.rest_endpoint # => "http://localhost:4567"
```

### From file

*config.json*
```json
{
    "database": {
        "server": "my-server.local"
    }
}
```

*my_file.py*
```py
config = Config.from_file("config.json")
server = config.database.server # => "my-server.local"
```

### Environments

*config.json*
```json
{
    "dev": {
        "some_resource": {
            "server": "resource-dev.local"
        }
    },
    "prod": {
        "some_resource": {
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

*my_file.py*
```py
config = Config.from_file("config.json").for_environment("prod")
server = config.some_resource.server # => "resource-prod.local"
solution_path = config.keys.ssh # => "path/to/id_rsa.pub"
```

## Contributing

See the [contributing section of the main README](../README.md#contributing)
