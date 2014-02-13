import json

class Config:
    def __init__(self, config):
        if type(config) is dict or type(config) is list:
            self.raw_config = config
        else:
            self.raw_config = json.loads(config)

    def __getattr__(self, attrib):
        return self[attrib]

    # config['key']
    def __getitem__(self, key):
        value = _get_value(key, self.raw_config)
        if type(value) is dict or type(value) is list:
            return Config(value)
        return value

    def __iter__(self):
        return self.raw_config.__iter__()

    # to string
    def __str__(self):
        return str(json.dumps(self.raw_config))

    def for_environment(self, env):
        env_json = self.raw_config[env]

        actual_key = _get_actual_key('all', self.raw_config)
        if actual_key is None:
            return Config(env_json)

        all_json = _get_value(actual_key, self.raw_config)
        all_json.update(env_json);

        return Config(all_json)

    @staticmethod
    def from_file(filename):
        with open(filename) as json_file:
            str_json = json_file.read()
            return Config(str_json)

# case insensitive hashtable helpers
def _get_normalised_key(unnormalisedKey):
    return unnormalisedKey.replace('_','').lower()

def _get_value(key, hashtable):
    if type(key) is int:
        return hashtable[key]

    actual_key = _get_actual_key(key, hashtable)
    if actual_key is None:
        raise Exception('Key not found in collection.')
    return hashtable[actual_key]

def _get_actual_key(key, hashtable):
    result = [ k for k in hashtable.keys() if _get_normalised_key(key) == _get_normalised_key(k) ]
    if len(result) > 0:
        if len(result) > 1:
            raise Exception('Too many matching keys in collection.')
        return result[0]
    return None
