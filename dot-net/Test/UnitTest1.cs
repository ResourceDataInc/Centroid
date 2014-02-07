using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;
using System.Linq;

namespace Centroid.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        private const string ConfigPath = "../../../../config.json";

        [Test]
        public void dev_environment_config_is_not_null()
        {
            var config = Centroid.Config.FromFile(ConfigPath).GetEnvironmentConfig("Dev");
            Assert.NotNull(config);
        }

        [Test]
        public void test_environment_config_is_not_null()
        {
            var config = Config.FromFile(ConfigPath).GetEnvironmentConfig("Test");
            Assert.NotNull(config);
        }

        [Test]
        public void dev_environment_config_has_database_server()
        {
            var config = Config.FromFile(ConfigPath).GetEnvironmentConfig("Dev");
            var server = config.Database.Server as string;
            Assert.NotNull(server);
            Debug.Write(server);
            Assert.Greater(server.Length, 0);
        }

        [Test]
        public void environment_property_shows_correct_environment()
        {
            const string env = "Dev";
            var config = Config.FromFile(ConfigPath).GetEnvironmentConfig(env);
            var envOut = config.Environment;
            var dictD = (object) config;
            Assert.AreEqual(env, envOut);
        }
    }
}
