require "json"

module Centroid
  class Config
    attr_reader :raw_config

    def initialize(config)
      if config.is_a?(Hash)
        @raw_config = config
      else
        @raw_config = JSON.parse(config)
      end
    end

    def method_missing(attrib, *)
      self[attrib]
    end

    def [](key)
      value = find_value(key, raw_config)

      raise Exception.new("Key not found in collection.") if value.nil?

      if value.is_a?(Hash)
        Config.new(value)
      else
        value
      end
    end

    def to_s
      JSON.dump(raw_config)
    end

    def for_environment(env)
      env_json = raw_config[env]
      all_key = actual_key("all", raw_config)
      if all_key.nil?
        Config.new(env_json)
      else
        all_json = raw_config[all_key]
        Config.new(all_json.merge(env_json))
      end
    end

    def self.from_file(filename)
      str_json = File.read(filename)
      self.new(str_json)
    end

    def normalised_key(unnormalised_key)
      unnormalised_key.to_s.gsub("_", "").downcase
    end

    def find_value(key, hashtable)
      hashtable[actual_key(key, hashtable)]
    end

    def actual_key(key, hashtable)
      hashtable.keys.find { |k| normalised_key(key) == normalised_key(k) }
    end
  end
end