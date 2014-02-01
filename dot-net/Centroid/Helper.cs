using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace CentroidConfig
{
    public class Config : DynamicObject
    {
        public dynamic config;
        public string environment;
        public string root;

        public Config(dynamic config, string env, string root = "root")
        {
            this.config = config;
            this.environment = env;
            this.root = root;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            try
            {
                result = this._getValue(binder.Name);
                if (result != null)
                {
                    result = new Config(result, this.environment, binder.Name);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public override string ToString()
        {
            return config.ToString();
        }

        private string _normaliseKey(string key)
        {
            return key.Replace("_", String.Empty).ToLower();
        }

        private dynamic _getValue(string key)
        {
            key = _getActualKey(key);
            return config[key];
        }

        private string _getActualKey(string key)
        {
            var properties = this.config.Properties() as IEnumerable<dynamic>;
            var keys = properties.Select(property => property.Name);
            return keys
                .Single(m => _normaliseKey(m) == _normaliseKey(key));
        }
    }
}

