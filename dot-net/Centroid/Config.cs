using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Centroid
{
    public class Config : DynamicObject
    {
        public dynamic RawConfig { get; private set; }

        public Config(string json)
        {
            RawConfig = JObject.Parse(json);
        }

        protected Config(dynamic config)
        {
            RawConfig = config;
        }

        public dynamic WithEnvironment(string environment)
        {
            var envConfig = RawConfig[environment];
            var allConfig = RawConfig.All;

            foreach (var cfg in allConfig)
            {
                envConfig[cfg.Name] = cfg.Value;
            }

            return new Config(envConfig);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            try
            {
                var container = GetValue(binder.Name);

                if (container is JContainer)
                {
                    result = new Config(container);
                }
                else
                {
                    result = container.Value;
                }

                return true;
            }
            catch
            {
                result = null;
            }

            return false;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(string))
            {
                result = ToString();
                return true;
            }

            try
            {
                result = Convert.ChangeType(RawConfig, binder.Type);
                return true;
            }
            catch (InvalidCastException)
            {
                return base.TryConvert(binder, out result);
            }
        }

        public override string ToString()
        {
            return RawConfig.ToString();
        }

        private static string NormaliseKey(string key)
        {
            return key.Replace("_", String.Empty).ToLower();
        }

        private dynamic GetValue(string key)
        {
            key = GetActualKey(key);
            return RawConfig[key];
        }

        private string GetActualKey(string key)
        {
            var properties = (IEnumerable<dynamic>) RawConfig.Properties();
            var keys = properties.Select(property => property.Name);
            return keys.Single(m => NormaliseKey(m) == NormaliseKey(key));
        }
    }
}
