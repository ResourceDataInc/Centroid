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

    def each
      return enum_for :each unless block_given?

      raw_config.each do |key, value|
        if value.is_a?(Hash)
          yield key, Config.new(value)
        else
          yield key, value
        end
      end
    end

    def [](key)
      raise KeyError.new("Centroid::Config instance does not contain key: #{key}") unless has_key?(key)

      value = find_value(key)

      if value.is_a?(Hash)
        Config.new(value)
      elsif value.is_a?(Array)
        value.map { |e| e.is_a?(Hash) ? Config.new(e) : e }
      else
        value
      end
    end

    def has_key?(key)
      !!actual_key(key)
    end

    alias_method :include?, :has_key?

    def to_s
      JSON.dump(raw_config)
    end

    def for_environment(env)
      env_json = raw_config[env]
      all_key = actual_key("all")
      if all_key.nil?
        config = Config.new(env_json)
      else
        all_json = raw_config[all_key]
        config = Config.new(deep_merge(all_json, env_json))
      end

      if !config.has_key?('environment')
        config.raw_config['environment'] = env
      end

      config
    end

    def self.from_file(filename)
      str_json = File.read(filename)
      self.new(str_json)
    end

    private

    def normalize_key(unnormalised_key)
      unnormalised_key.to_s.gsub("_", "").downcase
    end

    def find_value(key)
      raw_config[actual_key(key)]
    end

    def actual_key(key)
      raw_config.keys.find { |k| normalize_key(key) == normalize_key(k) }
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
