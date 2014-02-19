using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centroid
{
    public class Config : DynamicObject, IEnumerable
    {
        public Config(string json)
        {
            RawConfig = JObject.Parse(json);
            ValidateUniqueKeys();
        }

        public Config(dynamic config)
        {
            RawConfig = config;
        }

        public static Config FromFile(string fileName)
        {
            string json = System.IO.File.ReadAllText(fileName);
            return new Config(json);
        }

        public dynamic RawConfig { get; set; }

        public object this[int index]
        {
            get { return GetValue(index); }
            set { RawConfig[index] = value; }
        }

        public dynamic ForEnvironment(string environment)
        {
            var envConfig = RawConfig[environment];
            var allConfig = RawConfig.All;

            if (allConfig == null)
            {
                return new Config(envConfig);
            }

            foreach (var cfg in envConfig)
            {
                allConfig[cfg.Name] = cfg.Value;
            }
            return new Config(allConfig);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            try
            {
                result = GetValue(binder.Name);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof (string))
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
            return RawConfig.ToString(Formatting.None);
        }

        public IEnumerator GetEnumerator()
        {
            return RawConfig.GetEnumerator();
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var container = (JObject) RawConfig;
            return container.Properties().Select(p => p.Name);
        }

        static string NormaliseKey(string key)
        {
            return key.Replace("_", String.Empty).ToLower();
        }

        static dynamic GetValueFromContainer(dynamic container)
        {
            if (container is JContainer)
            {
                return new Config(container);
            }
            return container.Value;
        }

        dynamic GetValue(int index)
        {
            var container = RawConfig[index];
            return GetValueFromContainer(container);
        }

        dynamic GetValue(string key)
        {
            var actualKey = GetActualKey(key);
            var container = RawConfig[actualKey];
            return GetValueFromContainer(container);
        }

        string GetActualKey(string key)
        {
            return GetDynamicMemberNames().Single(m => NormaliseKey(m) == NormaliseKey(key));
        }

        void ValidateUniqueKeys()
        {
            var normalizedKeys = GetDynamicMemberNames().Select(p => new { Key = p, NormalizedKey = NormaliseKey(p) });
            var duplicates = normalizedKeys.GroupBy(nk => nk.NormalizedKey).Where(g => g.Count() > 1).ToArray();

            if (!duplicates.Any()) return;

            var keys = duplicates.SelectMany(d => d.Select(x => x.Key));
            throw new InvalidOperationException("Centroid.Config instance contains duplicate keys: " + string.Join(", ", keys));
        }
    }
}