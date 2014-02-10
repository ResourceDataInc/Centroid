class Config:
    def __init__(self, fileName = 'config.json', AllConfig_key = 'All'):
        self.fileName = fileName
        self.AllConfig_key = 'All'

    def environment(self, env):
        import json
        import _helper

        with open(self.fileName) as json_file:
            str_json = json_file.read()
            raw_json = json.loads(str_json)

            if not env in raw_json.keys():
                raise Exception('Key not found in configuration.')

            env_json = raw_json[env]

            if self.AllConfig_key in raw_json.keys():
                all_json = raw_json[self.AllConfig_key]
                env_json.update(all_json)

            return _helper.config(env_json, env)

