import unittest
import json
from centroid import Config

class ConfigTest(unittest.TestCase):

    def test_create_from_string(self):
        config = Config(_mock_config())
        self.assertEqual(config.Prod.RealDeal, "whatever")

    def test_can_create_from_file(self):
        config = Config.from_file('config.json')
        self.assertEqual(config.dev.database.server, "sqldev01.centroid.local")

    def test_create_from_custom_action(self):
        config = Config.from_action(_mock_config)
        self.assertEqual(config.Prod.RealDeal, "whatever")

    def test_raises_if_key_not_found(self):
        config = Config(_mock_config())
        with self.assertRaises(Exception):
            config = config.does_not_exist

    def test_readable_using_snake_case_property(self):
        config = Config(_mock_config())
        self.assertEqual(config.prod.real_deal, "whatever")

    def test_environment_specific_config_is_included(self):
        config = Config(_mock_config())
        config = config.environment('Prod')
        self.assertEqual(config.real_deal, "whatever")

    def test_shared_config_is_included(self):
        config = Config.from_file('config.json')
        config = config.environment("Dev")
        self.assertEqual(config.ci.repo, "https://github.com/ResourceDataInc/Centroid")

    def test_environment_property(self):
        config = Config.from_file('config.json')
        config = config.environment("Prod")
        self.assertEqual(config.environment, "Prod")

    def test_to_string(self):
        json = _mock_config();
        config = Config(json)
        self.assertEqual(str(config), json)

def _mock_config():
    return '{"Prod": {"RealDeal": "whatever"}}'
