using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;

namespace Centroid.Tests
{
    [TestFixture]
    public class ConfigTest
    {
        const string JsonConfig = @"{""theEnvironment"": {""theKey"": ""TheValue""}}";

        const string JsonConfigWithArray = @"{""theArray"": [{""theKey"": ""Value1""}, {""theKey"": ""Value2""}]}";

        readonly string sharedFilePath;

        public ConfigTest()
        {
            sharedFilePath = Path.Combine("..", "..", "..", "..", "config.json");
        }

        [Test]
        public void test_create_from_string()
        {
            dynamic config = new Config(JsonConfig);
            Assert.That(config.TheEnvironment.TheKey, Is.EqualTo("TheValue"));
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
            var exception = Assert.Throws(Is.InstanceOf<RuntimeBinderException>(), () => Console.Write(config.DoesNotExist));
            Assert.That(exception.Message, Is.StringContaining("DoesNotExist"));
        }

        [Test]
        public void test_raises_if_duplicate_normalized_keys_exist()
        {
            const string json = @"{ ""someKey"": ""value"", ""some_key"": ""value"" }";
            var exception = Assert.Throws<InvalidOperationException>(() => new Config(json));
            Assert.That(exception.Message, Is.StringContaining("duplicate"));
            Assert.That(exception.Message, Is.StringContaining("someKey"));
            Assert.That(exception.Message, Is.StringContaining("some_key"));
        }

        [Test]
        public void test_readable_using_snake_case_property()
        {
            dynamic config = new Config(JsonConfig);
            Assert.That(config.the_environment.the_key, Is.EqualTo("TheValue"));
        }

        [Test]
        public void test_environment_property_is_included()
        {
            var config = new Config(JsonConfig);
            dynamic environmentConfig = config.ForEnvironment("theEnvironment");
            Assert.That(environmentConfig.environment, Is.EqualTo("theEnvironment"));
        }

        [Test]
        public void test_environment_specific_config_is_included()
        {
            var config = new Config(JsonConfig);
            dynamic environmentConfig = config.ForEnvironment("theEnvironment");
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
            config.RawConfig["theEnvironment"]["theKey"] = "NotTheValue";
            Assert.That(config.TheEnvironment.TheKey, Is.EqualTo("NotTheValue"));
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
            Assert.That(config.TheArray[0].TheKey, Is.EqualTo("Value1"));
            Assert.That(config.TheArray[1].TheKey, Is.EqualTo("Value2"));
        }

        [Test]
        public void test_enumerating_json_array()
        {
            dynamic config = new Config(JsonConfigWithArray);
            var itemCount = 0;
            foreach (var item in config.TheArray)
            {
                Assert.That(item.TheKey, Is.EqualTo(config.TheArray[itemCount].TheKey));
                itemCount++;
            }
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

        [Test]
        public void test_enumerated_json_object_values_are_still_shiny()
        {
            const string json = @"
                {
                    ""connections"": {
                        ""firstConnection"": {
                            ""user"": ""firstUser"",
                            ""password"":""secret""
                        },
                        ""secondConnection"": {
                            ""user"": ""secondUser"",
                            ""password"":""secret""
                        },
                    }
                }";
            dynamic config = new Config(json);
            foreach (var kvp in config.Connections)
            {
                Assert.That(kvp.Value.Password, Is.EqualTo("secret"));
            }
        }

        [Test]
        public void test_all_environment_is_not_case_sensitive()
        {
            var upperCaseAllConfig = new Config(@"{""Prod"": {""Shared"": ""production!""}, ""All"": {""Shared"": ""none"", ""AllOnly"": ""works""}}");
            var upperCaseAllEnvironmentConfig = upperCaseAllConfig.ForEnvironment("Prod");
            Assert.That(upperCaseAllEnvironmentConfig.AllOnly, Is.EqualTo("works"));

            var lowerCaseAllConfig = new Config(@"{""Prod"": {""Shared"": ""production!""}, ""all"": {""Shared"": ""none"", ""AllOnly"": ""works""}}");
            var lowerCaseAllEnvironmentConfig = lowerCaseAllConfig.ForEnvironment("Prod");
            Assert.That(lowerCaseAllEnvironmentConfig.AllOnly, Is.EqualTo("works"));
        }

        [Test]
        public void test_supports_deep_merge()
        {
            const string json = @"
                {
                    ""Dev"": {
                        ""Database"": {
                            ""Server"": ""the-dev-database""
                        }
                    },
                    ""All"": {
                        ""Database"": {
                            ""MigrationsPath"": ""path/to/migrations""
                        }
                    }
                }";

            dynamic config = new Config(json).ForEnvironment("Dev");
            Assert.That(config.Database.Server, Is.EqualTo("the-dev-database"));
            Assert.That(config.Database.MigrationsPath, Is.EqualTo("path/to/migrations"));
        }

        [Test]
        public void test_supports_merge_override()
        {
            const string json = @"
                {
                    ""Dev"": {
                        ""Connection"": {
                            ""server"": ""dev-server"",
                            ""database"": ""dev_database"",
                            ""SdeConnectionFile"": ""DEV:sde(file)""
                        }
                    },
                    ""All"": {
                        ""Connection"": {
                            ""server"": """",
                            ""database"": """",
                            ""instance"": """",
                            ""user"": ""default-user"",
                            ""password"": ""default-password"",
                            ""version"": """",
                            ""SdeConnectionFile"": """"
                        }
                    }
                }";

            dynamic config = new Config(json).ForEnvironment("Dev");
            Assert.That(config.Connection.Server, Is.EqualTo("dev-server"));
            Assert.That(config.Connection.database, Is.EqualTo("dev_database"));
            Assert.That(config.Connection.instance, Is.EqualTo(""));
            Assert.That(config.Connection.user, Is.EqualTo("default-user"));
            Assert.That(config.Connection.password, Is.EqualTo("default-password"));
            Assert.That(config.Connection.version, Is.EqualTo(""));
            Assert.That(config.Connection.SdeConnectionFile, Is.EqualTo("DEV:sde(file)"));
        }

        [Test]
        public void test_contains_key()
        {
            dynamic config = new Config(JsonConfig);
            Assert.That(config.ContainsKey("theEnvironment"), Is.True);
            Assert.That(config.ContainsKey("DoesNotExist"), Is.False);
        }

        [Test]
        public void test_key_as_index()
        {
            dynamic config = new Config(JsonConfig);
            var myString = "thekey";
            Assert.That(config.theEnvironment[myString], Is.EqualTo("TheValue"));
        }

        [Test]
        public void test_key_as_index2()
        {
            const string json = @"
            {
              ""Prod"": {
                ""task"": {
                  ""task1"": {
                    ""param1"": ""val11"",
                    ""param2"": ""val12"",
                    ""param3"": ""val13""
                  },
                  ""task2"": {
                    ""param1"": ""val21"",
                    ""param2"": ""val22""
                  },
                  ""task3"": {
                    ""param1"": ""val31"",
                    ""param2"": ""val32"",
                    ""param3"": ""val33"",
                    ""param4"": ""val34""
                  }
                }
              },
              ""All"": {
                ""CI"": {
                  ""Server"": ""centroid-ci01.centroid.local"",
                  ""Repo"": ""https://github.com/ResourceDataInc/Centroid""
                }
              }
            }";

            dynamic config = new Config(json).ForEnvironment("Prod"); ;
            for (var ii = 1; ii <= 3; ii++)
            {
                var index = "task" + ii;
                Assert.That(config.task[index].param1, Is.EqualTo("val" + ii + "1"));
            }
        }
    }
}
