import unittest
import json
from centroid import Config

class ConfigTest(unittest.TestCase):

    @property
    def _json_config(self):
        return '{"theEnvironment": {"theKey": "TheValue"}}'

    @property
    def _json_config_with_array(self):
        return '{"theArray": [{"theKey": "Value1"}, {"theKey": "Value2"}]}'

    @property
    def _shared_file_path(self):
        return 'config.json'

    def test_create_from_string(self):
        config = Config(self._json_config)
        self.assertEqual(config.the_environment.the_key, "TheValue")

    def test_create_from_file(self):
        config = Config.from_file(self._shared_file_path)
        self.assertEqual(config.dev.database.server, "sqldev01.centroid.local")

    def test_raises_if_key_not_found(self):
        config = Config(self._json_config)
        with self.assertRaisesRegexp(KeyError, "does_not_exist"):
            config = config.does_not_exist

    def test_raises_if_duplicate_normalized_keys_exist(self):
        json = '{ "someKey": "value", "some_key": "value" }'
        with self.assertRaisesRegexp(KeyError, "duplicate.+someKey.+some_key"):
            Config(json)

    def test_readable_using_snake_case_property(self):
        config = Config(self._json_config)
        self.assertEqual(config.the_environment.the_key, "TheValue")

    def test_environment_property_is_included(self):
        config = Config(self._json_config)
        environment_config = config.for_environment("theEnvironment")
        self.assertEqual(environment_config.environment, "theEnvironment")

    def test_environment_specific_config_is_included(self):
        config = Config(self._json_config)
        environment_config = config.for_environment("theEnvironment")
        self.assertEqual(environment_config.the_key, "TheValue")

    def test_shared_config_is_included(self):
        config = Config.from_file(self._shared_file_path)
        config = config.for_environment("Dev")
        self.assertEqual(config.ci.repo, "https://github.com/ResourceDataInc/Centroid")

    def test_to_string_returns_json(self):
        json = self._json_config;
        config = Config(json)
        self.assertEqual(str(config), json)

    def test_iterating_raw_config(self):
        config = Config.from_file(self._shared_file_path)
        keyCount = 0;
        for key in config.raw_config:
            keyCount += 1
        self.assertEqual(keyCount, 4)

    def test_modifying_raw_config(self):
        config = Config(self._json_config)
        config.raw_config["theEnvironment"]["theKey"] = "NotTheValue"
        self.assertEqual(config.the_environment.the_key, "NotTheValue")

    def test_environment_specific_config_overrides_all(self):
        config = Config('{"Prod": {"Shared": "production!"}, "All": {"Shared": "none"}}')
        config = config.for_environment("Prod")
        self.assertEqual(config.shared, "production!")

    def test_indexing_json_array(self):
        config = Config(self._json_config_with_array)
        self.assertEqual(config.the_array[0].the_key, "Value1")
        self.assertEqual(config.the_array[1].the_key, "Value2")

    def test_enumerating_json_array(self):
        config = Config(self._json_config_with_array)
        itemCount = 0
        for item in config.the_array:
            itemCount += 1
        self.assertEqual(itemCount, 2)

    def test_enumerating_json_object(self):
        config = Config(self._json_config)
        itemCount = 0
        for item in config:
            itemCount += 1
        self.assertEqual(itemCount, 1)

    def test_enumerated_json_object_values_are_still_shiny(self):
      json = """
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
        }"""

      config = Config(json)
      for k, v in config.connections:
        self.assertEqual(v.password, "secret")

    def test_all_environment_is_not_case_sensitive(self):
        config = Config('{"Prod": {"Shared": "production!"}, "All": {"Shared": "none", "AllOnly": "works"}}')
        config = config.for_environment("Prod")
        self.assertEqual(config.all_only, "works")

        config = Config('{"Prod": {"Shared": "production!"}, "all": {"Shared": "none", "AllOnly": "works"}}')
        config = config.for_environment("Prod")
        self.assertEqual(config.all_only, "works")

    def test_supports_deep_merge(self):
        config = Config('{"Prod": {"Database": {"Server": "prod-sql"}}, "All": {"Database": {"MigrationsPath": "path/to/migrations"}}}')
        config = config.for_environment("Prod")
        self.assertEqual(config.database.server, "prod-sql")
        self.assertEqual(config.database.migrations_path, "path/to/migrations")

    def test_supports_merge_override(self):
        json = """
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
        }"""

        config = Config(json)
        config = config.for_environment("Dev")
        self.assertEqual(config.Connection.Server, "dev-server")
        self.assertEqual(config.Connection.database, "dev_database")
        self.assertEqual(config.Connection.instance, "")
        self.assertEqual(config.Connection.user, "default-user")
        self.assertEqual(config.Connection.password, "default-password")
        self.assertEqual(config.Connection.version, "")
        self.assertEqual(config.Connection.SdeConnectionFile, "DEV:sde(file)")

    def test_has_key(self):
        config = Config(self._json_config)
        self.assertTrue("the_environment" in config)
        self.assertTrue("does_not_exist" not in config)

    def test_key_as_index(self):
        config = Config(self._json_config)
        my_string = "thekey"
        self.assertEqual(config.the_environment[my_string], "TheValue")

