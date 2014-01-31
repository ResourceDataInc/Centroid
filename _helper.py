class config:
    def __init__(self, config, env, root='root'):
        self.config = config
        self.environment = env
        self.root = root

    def __getattr__(self, attrib):
        return self[attrib]

    # config['key']
    def __getitem__(self, key):
        key = _get_actual_key(key, self.config)
        value = _get_value(key, self.config)
        if type(value) is dict:
            return config(value, self.environment)
        return value

    # to string
    def __str__(self):
        return ''

# case insensitive hashtable helpers
def _get_normalised_key(unnormalisedKey):
    return unnormalisedKey.replace('_','').lower()

def _get_value(key, hashtable):
    return hashtable[_get_actual_key(key, hashtable)]

def _get_actual_key(key, hashtable):
    result = [ k for k in hashtable.keys() if _get_normalised_key(key) == _get_normalised_key(k) ]
    if len(result) > 0:
        if len(result) > 1:
            raise Exception('Too many matching keys in collection.')
        return result[0]
    raise Exception('Key not found in collection.')
