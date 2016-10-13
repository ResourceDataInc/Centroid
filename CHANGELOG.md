# 1.2.0 - 2016/10/12

## CSharp

### Features

* Support to access key values via index syntax (e.g. config.database[connection] where 'connection' is a code side variable) - [#71](https://github.com/ResourceDataInc/Centroid/pull/71)

## Build

* Update build dependencies  - [#70](https://github.com/ResourceDataInc/Centroid/pull/70)

# 1.1.0 - 2014/08/22

## CSharp

### Features

* Add `environment` property to config created with `ForEnvironment` - [#63](https://github.com/ResourceDataInc/Centroid/pull/63)
* Deep merge support - [#56](https://github.com/ResourceDataInc/Centroid/pull/56)
* Add `ContainsKey` method - [#50](https://github.com/ResourceDataInc/Centroid/pull/50)

### Fixes

* Fix issue opening project in Xamarin Studio - [#49](https://github.com/ResourceDataInc/Centroid/pull/49)
* Fix issue with lowercase `all` environment not being picked up - [#54](https://github.com/ResourceDataInc/Centroid/pull/54)
* Fix issue with enumerating over config values causing values to become non-Centroidy - [#60](https://github.com/ResourceDataInc/Centroid/pull/60)

## Python

### Features

* Add `environment` property to config created with `for_environment` - [#63](https://github.com/ResourceDataInc/Centroid/pull/63)
* Deep merge support - [#56](https://github.com/ResourceDataInc/Centroid/pull/56)
* Add `__contains__` support - [#50](https://github.com/ResourceDataInc/Centroid/pull/50)

### Fixes

* Fix issue with enumerating over config values causing values to become non-Centroidy - [#61](https://github.com/ResourceDataInc/Centroid/pull/61)

## Ruby

### Features

* Add `environment` property to config created with `for_environment` - [#63](https://github.com/ResourceDataInc/Centroid/pull/63)
* Deep merge support - [#56](https://github.com/ResourceDataInc/Centroid/pull/56)
* Add `has_key?` method - [#50](https://github.com/ResourceDataInc/Centroid/pull/50)

### Fixes

* Fix issue with enumerating over config values causing values to become non-Centroidy - [#61](https://github.com/ResourceDataInc/Centroid/pull/61)
* Fix issue with enumerating arrays causing values to become non-Centroidy - [#53](https://github.com/ResourceDataInc/Centroid/pull/53)

# 1.0.0 - 2014/02/20

Initial public release.
