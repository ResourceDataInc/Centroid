using System;
using System.Collections;
using NUnit.Framework;
using System.Linq;

namespace Centroid.Test
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void dev_environment_config_is_not_null()
        {
            var config = Centroid.Config.FromFile("../../../../config.json").GetEnvironmentConfig("Dev");
            Assert.NotNull(config);
        }
    }
}
