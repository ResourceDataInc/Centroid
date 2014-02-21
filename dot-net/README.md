# Centroid - .NET

This document includes information specific to .NET in Centroid. Refer to the [general Centroid document] (../README.md) for general information, including information the JSON configuration file. 

## Installation

The Centroid .NET package is hosted at [Nuget](http://www.nuget.org/packages/Centroid/). Install Centroid using Package Manager Console with `Install-Package Centroid`.

## Config Class with .NET

In .NET, the `Centroid.Config` class exposes the following:

+ Static `FromFile` method
+ Constructor
+ `ForEnvironment` instance method

> *Note:* The examples given in the following sections are based on the JSON configuration file examples in the [primary Centroid document] (../README.md#examples). 

### FromFile Method

Using the static `Config.FromFile(filename)` method, you can create an instance of `Config` from a JSON file, as in the example below. 

```cs
// FromFile.cs
dynamic config = Config.FromFile("config.json");
var server = config.Database.ServerAddress; // => "my-server.local"
```

### Config Constructor

To load a string instead of a file, create an instance of `Config` by passing a JSON string to the `Config` constructor, as in the following example.

```cs
// FromString.cs
var json = @"{ ""Database"": { ""Server"": ""my-server.local"" } }";
dynamic config = new Config(json);
var server = config.Database.ServerAddress; // => "my-server.local"
```

### ForEnviroment Instance Method

In the `Config` instance, you can use the `ForEnvironment` instance method to retrieve the configuration values for an environment. 

If you specify an environment in `ForEnvironment`, Centroid will merge the requested environment's configuration values with the values in *all*. Refer to [Examples in the primary Centroid document] (../README.md#examples) for information on creating an environment-based JSON configuration file. 

With the following example, Centroid will merge the configuration for *prod* with the configuration for *all*; the result is then available from a new instance of `Config`.

```cs
// ForEnvironment.cs
var config = Config.FromFile("config.json").ForEnvironment("Prod");
var server = config.Database.Server; // => "sql-prod.local"
var solutionPath = config.Solutions.Main; // => "path/to/Main.sln";
```

