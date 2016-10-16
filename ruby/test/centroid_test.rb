require_relative "../lib/centroid"
require "test/unit"
require "json"

class ConfigTests < Test::Unit::TestCase
  def json_config
    '{"theEnvironment":{"theKey":"TheValue"}}'
  end

  def json_config_with_array
    '{"theArray":[{"theKey": "Value1"},{"theKey": "Value2"}]}'
  end

  def shared_file_path
    'config.json'
  end

  def test_create_from_string
    config = Centroid::Config.new(json_config)
    assert_equal(config.the_environment.the_key, "TheValue")
  end

  def test_create_from_file
    config = Centroid::Config.from_file(shared_file_path)
    assert_equal(config.dev.database.server, "sqldev01.centroid.local")
  end

  def test_raises_if_key_not_found
    config = Centroid::Config.new(json_config)

    exception = assert_raise KeyError do
      config = config.does_not_exist
    end

    assert exception.message =~ /does not contain/i, "Message should indicate key does not exist"
    assert exception.message =~ /does_not_exist/, "Message should contain missing key"
  end

  def test_raises_if_duplicate_normalized_keys_exist
    json = '{ "someKey": "value", "some_key": "value" }'

    exception = assert_raise KeyError do
      Centroid::Config.new(json)
    end

    assert exception.message =~ /duplicate/i, "Message should indicate duplicate key"
    assert exception.message =~ /someKey/, "Message should contain duplicate key"
    assert exception.message =~ /some_key/, "Message should contain duplicate key"
  end

  def test_readable_using_snake_case_property
    config = Centroid::Config.new(json_config)
    assert_equal(config.the_environment.the_key, "TheValue")
  end

  def test_environment_property_is_included
    config = Centroid::Config.new(json_config)
    environment_config = config.for_environment("theEnvironment")
    assert_equal(environment_config.environment, "theEnvironment")
  end

  def test_environment_specific_config_is_included
    config = Centroid::Config.new(json_config)
    environment_config = config.for_environment("theEnvironment")
    assert_equal(environment_config.the_key, "TheValue")
  end

  def test_shared_config_is_included
    config = Centroid::Config.from_file(shared_file_path)
    config = config.for_environment("Dev")
    assert_equal(config.ci.repo, "https://github.com/ResourceDataInc/Centroid")
  end

  def test_to_string_returns_json
    config = Centroid::Config.new(json_config)
    assert_equal(config.to_s, json_config)
  end

  def test_iterating_raw_config
    config = Centroid::Config.from_file(shared_file_path)
    keyCount = 0
    config.raw_config.each { |k| keyCount += 1 }
    assert_equal(keyCount, 4)
  end

  def test_modifying_raw_config
    config = Centroid::Config.new(json_config)
    config.raw_config["theEnvironment"]["theKey"] = "NotTheValue"
    assert_equal(config.the_environment.the_key, "NotTheValue")
  end

  def test_environment_specific_config_overrides_all
    config = Centroid::Config.new('{"Prod": {"Shared": "production!"}, "All": {"Shared": "none"}}')
    config = config.for_environment("Prod")
    assert_equal(config.shared, "production!")
  end

  def test_environment_specific_config_has_environment_property
    config = Centroid::Config.new('{"Prod": {"Shared": "production!"}, "All": {"Shared": "none"}}')
    config = config.for_environment("Prod")
    assert_equal(config.environment, "Prod")
  end

  def test_environment_specific_config_no_environment_property_if_has_environment_config_json
    config = Centroid::Config.new('{"Prod": {"Environment": "production!"}, "All": {"Shared": "none"}}')
    config = config.for_environment("Prod")
    assert_equal(config.environment, "production!")
  end

  def test_indexing_json_array
    config = Centroid::Config.new(json_config_with_array)
    assert_equal(config.the_array[0].the_key, "Value1")
    assert_equal(config.the_array[1].the_key, "Value2")
  end

  def test_enumerating_json_array
    config = Centroid::Config.new(json_config_with_array)
    itemCount = 0
    config.the_array.each do |item|
      assert_equal(item.the_key, config.the_array[itemCount].the_key)
      itemCount += 1
    end
    assert_equal(itemCount, 2)
  end

  def test_enumerating_json_object
    config = Centroid::Config.new(json_config)
    itemCount = 0
    config.each do |item|
      itemCount += 1
    end
    assert_equal(itemCount, 1)
  end

  def test_enumerated_json_object_values_are_still_shiny
      json = '
        {
          "connections": {
            "firstConnection": {
              "user": "firstUser",
              "password":"secret"
            },
            "secondConnection": {
              "user": "secondUser",
              "password":"secret"
            }
          }
        }'

      config = Centroid::Config.new(json)
      config.connections.each do |k, v|
        assert_equal(v.password, "secret")
      end
  end

  def test_all_environment_is_not_case_sensitive
    config = Centroid::Config.new('{"Prod": {"Shared": "production!"}, "All": {"Shared": "none", "AllOnly": "works"}}')
    config = config.for_environment("Prod")
    assert_equal(config.all_only, "works")

    config = Centroid::Config.new('{"Prod": {"Shared": "production!"}, "all": {"Shared": "none", "AllOnly": "works"}}')
    config = config.for_environment("Prod")
    assert_equal(config.all_only, "works")
  end

  def test_supports_deep_merge
    config = Centroid::Config.new('{"Prod": {"Database": {"Server": "prod-sql"}}, "All": {"Database": {"MigrationsPath": "path/to/migrations"}}}')
    config = config.for_environment("Prod")
    assert_equal(config.database.server, "prod-sql")
    assert_equal(config.database.migrations_path, "path/to/migrations")
  end

  def test_supports_merge_override()
    json = '
    {
      "Dev": {
        "Connection": {
          "server": "dev-server",
          "database": "dev_database",
          "SdeConnectionFile": "DEV:sde(file)"
        }
      },
      "All": {
        "Connection": {
          "server": "",
          "database": "",
          "instance": "",
          "user": "default-user",
          "password": "default-password",
          "version": "",
          "SdeConnectionFile": ""
        }
      }
    }'

    config = Centroid::Config.new(json)
    config = config.for_environment("Dev");
    assert_equal(config.Connection.Server, "dev-server");
    assert_equal(config.Connection.database, "dev_database");
    assert_equal(config.Connection.instance, "");
    assert_equal(config.Connection.user, "default-user");
    assert_equal(config.Connection.password, "default-password");
    assert_equal(config.Connection.version, "");
    assert_equal(config.Connection.SdeConnectionFile, "DEV:sde(file)");
  end

  def test_has_key
    config = Centroid::Config.new(json_config)
    assert(config.has_key?("the_environment"))
    assert(!config.has_key?("does_not_exist"))
  end

  def test_key_as_index
    config = Centroid::Config.new(json_config)
    my_string = "thekey"
    assert_equal(config.the_environment[my_string], "TheValue")
  end

  def test_respond_to
    config = Centroid::Config.new(json_config)
    assert(config.respond_to?(:the_environment))
    assert(!config.respond_to?(:does_not_exist))
  end

end
