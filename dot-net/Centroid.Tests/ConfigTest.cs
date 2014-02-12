using NUnit.Framework;
using System.IO;

namespace Centroid.Tests
{
    [TestFixture]
    public class ConfigTest
    {
        private readonly string sharedFilePath;

        private const string JsonConfig = @"
            {
                ""Dev"": {
                    ""Database"": {
                        ""Server"": ""the-dev-database""
                    }
                },
                ""Prod"": {
                    ""Database"": {
                        ""Server"": ""the-prod-database""
                    }
                },
                ""All"": {
                    ""SharedNest"": {
                        ""SharedKey"": ""shared value""
                    }
                }
            }
        ";

        public ConfigTest()
        {
            sharedFilePath = Path.Combine("..", "..", "..", "..", "config.json");
        }

        [Test]
        public void environment_property_shows_correct_environment()
        {
            var config = new Config(JsonConfig).WithEnvironment("Prod");
            Assert.AreEqual("Prod", config.Environment);
        }

        [Test]
        public void environment_specific_config_is_included_correctly()
        {
            var config = new Config(JsonConfig).WithEnvironment("Dev");
            Assert.AreEqual("the-dev-database", config.Database.Server);
        }

        [Test]
        public void shared_config_is_included_correctly()
        {
            var config = new Config(JsonConfig).WithEnvironment("Prod");
            Assert.AreEqual("shared value", config.SharedNest.SharedKey);
        }

        [Test]
        public void pascal_case_can_be_used_with_snake_cased_json()
        {
            dynamic config = new Config(@"{ ""snake_key"": ""some value"" }");
            Assert.AreEqual("some value", config.SnakeKey);
        }

        [Test]
        public void can_load_config_from_file()
        {
            Assert.DoesNotThrow(() =>
                {
                    dynamic config = Config.FromFile(sharedFilePath);
                    Assert.NotNull(config.All);
                });
        }
    }
}
