require_relative "../lib/centroid"
require "test/unit"
require "json"

class ConfigTests < Test::Unit::TestCase
  def json_config
    '{"Environment":{"TheKey":"TheValue"}}'
  end

  def shared_file_path
    'config.json'
  end

  def test_create_from_string
    config = Centroid::Config.new(json_config)
    assert_equal(config.environment.the_key, "TheValue")
  end

  def test_create_from_file
    config = Centroid::Config.from_file(shared_file_path)
    assert_equal(config.dev.database.server, "sqldev01.centroid.local")
  end

  def test_raises_if_key_not_found
    config = Centroid::Config.new(json_config)
    assert_raise Exception do
      config = config.does_not_exist
    end
  end

  def test_readable_using_snake_case_property
    config = Centroid::Config.new(json_config)
    assert_equal(config.environment.the_key, "TheValue")
  end

  def test_environment_specific_config_is_included
    config = Centroid::Config.new(json_config)
    environment_config = config.for_environment("Environment")
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
    config.raw_config["Environment"]["TheKey"] = "NotTheValue"
    assert_equal(config.environment.the_key, "NotTheValue")
  end

  def test_environment_specific_config_overrides_all
    config = Centroid::Config.new('{"Prod": {"Shared": "production!"}, "All": {"Shared": "none"}}')
    config = config.for_environment("Prod")
    assert_equal(config.shared, "production!")
  end
end