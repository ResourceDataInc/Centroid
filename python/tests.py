import unittest
import json
from centroid import Config

class ConfigTest(unittest.TestCase):

    @property
    def _json_config(self):
        return '{"Environment": {"TheKey": "TheValue"}}'

    @property
    def _shared_file_path(self):
        return 'config.json'

    def test_create_from_string(self):
        config = Config(self._json_config)
        self.assertEqual(config.Environment.TheKey, "TheValue")

    def test_create_from_file(self):
        config = Config.from_file(self._shared_file_path)
        self.assertEqual(config.dev.database.server, "sqldev01.centroid.local")

    def test_raises_if_key_not_found(self):
        config = Config(self._json_config)
        with self.assertRaises(Exception):
            config = config.does_not_exist

    def test_readable_using_snake_case_property(self):
        config = Config(self._json_config)
        self.assertEqual(config.environment.the_key, "TheValue")

    def test_environment_specific_config_is_included(self):
        config = Config(self._json_config)
        environment_config = config.for_environment("Environment")
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
        config.raw_config["Environment"]["TheKey"] = "NotTheValue"
        self.assertEqual(config.environment.the_key, "NotTheValue")

    def test_environment_specific_config_overrides_all(self):
        config = Config('{"Prod": {"Shared": "production!"}, "All": {"Shared": "none"}}')
        config = config.for_environment("Prod")
        self.assertEqual(config.shared, "production!")
