import unittest
from centroid import Config

class ConfigTest(unittest.TestCase):

	def test_environment_property_shows_correct_environment(self):
		config = Config().environment("Prod")
		self.assertEqual(config.environment, "Prod")

	def test_environment_specific_config_is_included_correctly(self):
		config = Config().environment("Dev")
		self.assertEqual(config.database.server, "sqldev01.centroid.local")

	def test_shared_config_is_included_correctly(self):
		config = Config().environment("Dev")
		self.assertEqual(config.ci.repo, "https://github.com/ResourceDataInc/Centroid")
