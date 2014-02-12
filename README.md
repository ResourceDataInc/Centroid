# Centroid

A centralized paradigm to configuration management.

**Centroid:** *(of a finite set) the point whose coordinates are the mean values of the coordinates of the points of the set*.

## About

In our experience, multiple projects related to one another will eventually need access to the same configuration. This can be problematic when the projects are built upon different technologies. Our solution is to extract the configuration to a shared JSON file, and provide means to access the values in various languages.

We also add some niceties to make interacting with the configuration a bit more clean. For example, the process to lookup a key in the config will ignore underscores and is case insensitive. This allows for storing the config in PascalCase, while accessing it in snake_case, and vice versa.

## Installation and Usage

* [.NET](dot-net/README.md)
* [Python](python/README.md)

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
