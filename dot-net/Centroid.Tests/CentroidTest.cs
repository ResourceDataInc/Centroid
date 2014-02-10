using NUnit.Framework;

namespace Centroid.Tests
{
    [TestFixture]
    public class CentroidTest
    {
        private const string ConfigJson = @"
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

        [Test]
        public void environment_property_shows_correct_environment()
        {
            var config = new Config(ConfigJson).GetEnvironmentConfig("Prod");
            Assert.AreEqual("Prod", config.Environment);
        }

        [Test]
        public void environment_specific_config_is_included_correctly()
        {
            var config = new Config(ConfigJson).GetEnvironmentConfig("Dev");
            Assert.AreEqual("the-dev-database", config.Database.Server);
        }

        [Test]
        public void shared_config_is_included_correctly()
        {
            var config = new Config(ConfigJson).GetEnvironmentConfig("Prod");
            Assert.AreEqual("shared value", config.SharedNest.SharedKey);
        }
    }
}
