require "json"

module Centroid
  class Config
    attr_reader :raw_config

    def initialize(config)
      if config.is_a?(Hash)
        @raw_config = config
      else
        @raw_config = JSON.parse(config)
        validate_unique_keys!
      end
    end

    def method_missing(attrib, *)
      self[attrib]
    end

    def respond_to_missing?(method_name, include_all)
      has_key?(method_name)
    end

    def [](key)
      value = find_value(key, raw_config)

      raise KeyError.new("Centroid::Config instance does not contain key: #{key}") if value.nil?

      if value.is_a?(Hash)
        Config.new(value)
      else
        value
      end
    end

    def has_key?(key)
      !!actual_key(key, raw_config)
    end

    alias_method :include?, :has_key?

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
        Config.new(deep_merge(all_json, env_json))
      end
    end

    def self.from_file(filename)
      str_json = File.read(filename)
      self.new(str_json)
    end

    private

    def normalize_key(unnormalised_key)
      unnormalised_key.to_s.gsub("_", "").downcase
    end

    def find_value(key, hashtable)
      hashtable[actual_key(key, hashtable)]
    end

    def actual_key(key, hashtable)
      hashtable.keys.find { |k| normalize_key(key) == normalize_key(k) }
    end

    def validate_unique_keys!
      normalized_keys = raw_config.keys.map { |k| { key: k, normalized_key: normalize_key(k) } }
      dups = normalized_keys.group_by { |nk| nk[:normalized_key] }.select { |_, g| g.size > 1 }

      return if dups.empty?

      keys = dups.values.flat_map { |d| d.map { |e| e[:key] } }
      raise KeyError, "Centroid::Config instance contains duplicate keys: #{keys.join(', ')}"
    end

    def deep_merge(left, right)
      return right if not right.is_a?(Hash)

      right.each_pair do |k, rv|
        lv = left[k]
        left[k] = if lv.is_a?(Hash) && rv.is_a?(Hash)
          deep_merge(lv, rv)
        else
          rv
        end
      end

      left
    end
  end
end
