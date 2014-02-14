using System;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Centroid.Tests
{
    [TestFixture]
    public class ConfigTest
    {
        const string JsonConfig = @"{""Environment"": {""TheKey"": ""TheValue""}}";

        const string JsonConfigWithArray = @"{""Array"": [{""Key"": ""Value1""}, {""Key"": ""Value2""}]}";

        readonly string sharedFilePath;

        public ConfigTest()
        {
            sharedFilePath = Path.Combine("..", "..", "..", "..", "config.json");
        }

        [Test]
        public void test_create_from_string()
        {
            dynamic config = new Config(JsonConfig);
            Assert.That(config.Environment.TheKey, Is.EqualTo("TheValue"));
        }

        [Test]
        public void test_create_from_file()
        {
            dynamic config = Config.FromFile(sharedFilePath);
            Assert.That(config.Dev.Database.Server, Is.EqualTo("sqldev01.centroid.local"));
        }

        [Test]
        public void test_raises_if_key_not_found()
        {
            dynamic config = new Config(JsonConfig);
            Assert.Throws(Is.InstanceOf<Exception>(), delegate { var doesNotExist = config.DoesNotExist; });
        }

        [Test]
        public void test_readable_using_snake_case_property()
        {
            dynamic config = new Config(JsonConfig);
            Assert.That(config.environment.the_key, Is.EqualTo("TheValue"));
        }

        [Test]
        public void test_environment_specific_config_is_included()
        {
            var config = new Config(JsonConfig);
            dynamic environmentConfig = config.ForEnvironment("Environment");
            Assert.That(environmentConfig.TheKey, Is.EqualTo("TheValue"));
        }

        [Test]
        public void test_shared_config_is_included()
        {
            var config = Config.FromFile(sharedFilePath);
            dynamic environmentConfig = config.ForEnvironment("Dev");
            Assert.That(environmentConfig.CI.Repo, Is.EqualTo(@"https://github.com/ResourceDataInc/Centroid"));
        }

        [Test]
        public void test_to_string_returns_json()
        {
            var config = new Config(JsonConfig);
            var configMinusWhitespace = Regex.Replace(JsonConfig, @"\s+", "");
            Assert.That(config.ToString(), Is.EqualTo(configMinusWhitespace));
        }

        [Test]
        public void test_iterating_raw_config()
        {
            dynamic config = Config.FromFile(sharedFilePath);
            var keyCount = 0;
            foreach (var key in config.RawConfig)
            {
                keyCount++;
            }
            Assert.That(keyCount, Is.EqualTo(4));
        }

        [Test]
        public void test_modifying_raw_config()
        {
            dynamic config = new Config(JsonConfig);
            config.RawConfig["Environment"]["TheKey"] = "NotTheValue";
            Assert.That(config.Environment.TheKey, Is.EqualTo("NotTheValue"));
        }

        [Test]
        public void test_environment_specific_config_overrides_all()
        {
            var config = new Config(@"{""Prod"": {""Shared"": ""production!""}, ""All"": {""Shared"": ""none""}}");
            dynamic environmentConfig = config.ForEnvironment("Prod");
            Assert.That(environmentConfig.Shared, Is.EqualTo("production!"));
        }

        [Test]
        public void test_indexing_json_array()
        {
            dynamic config = new Config(JsonConfigWithArray);
            Assert.That(config.Array[0].Key, Is.EqualTo("Value1"));
            Assert.That(config.Array[1].Key, Is.EqualTo("Value2"));
        }

        [Test]
        public void test_enumerating_json_array()
        {
            dynamic config = new Config(JsonConfigWithArray);
            var itemCount = 0;
            foreach (var item in config.Array)
            {
                itemCount++;
            }
            Assert.That(itemCount, Is.EqualTo(2));
        }

        [Test]
        public void test_enumerating_json_object()
        {
            dynamic config = new Config(JsonConfig);
            var itemCount = 0;
            foreach (var item in config)
            {
                itemCount++;
            }
            Assert.That(itemCount, Is.EqualTo(1));
        }
    }
}