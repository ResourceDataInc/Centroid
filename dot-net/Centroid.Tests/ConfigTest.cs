﻿using NUnit.Framework;

namespace Centroid.Tests
{
    [TestFixture]
    public class ConfigTest
    {
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
    }
}