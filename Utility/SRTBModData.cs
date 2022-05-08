using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpinCore.Utility; 

/// <summary>
/// A container for miscellaneous info that is saved to a chart file
/// </summary>
[Serializable, JsonObject(MemberSerialization.OptIn)]
public class SRTBModData {
    public class StringPair {
        public string Key { get; set; }
        
        public string Value { get; set; }

        public StringPair(string key, string value) {
            Key = key;
            Value = value;
        }
    }
    
    [JsonProperty] public List<StringPair> Pairs { get; } = new();

    private Dictionary<string, object> dictionary;

    /// <summary>
    /// True if there is no info
    /// </summary>
    public bool Empty => Pairs == null || Pairs.Count == 0;

    /// <summary>
    /// Sets a value in this container
    /// </summary>
    /// <param name="key">The key to set the value of</param>
    /// <param name="value">The value to assign</param>
    public void SetValue(string key, object value) {
        dictionary ??= new Dictionary<string, object>();
        dictionary[key] = value;

        string str = JsonConvert.SerializeObject(value);

        foreach (var pair in Pairs) {
            if (pair.Key != key)
                continue;

            pair.Value = str;

            return;
        }
        
        Pairs.Add(new StringPair(key, str));
    }

    /// <summary>
    /// Attempts to get a serialized object stored within this container
    /// </summary>
    /// <param name="key">The key for the object to get</param>
    /// <param name="value">The acquired object</param>
    /// <typeparam name="T">The type of the object to find</typeparam>
    /// <returns>True if the object was found</returns>
    public bool TryGetValue<T>(string key, out T value) {
        if (Empty) {
            value = default;

            return false;
        }
        
        dictionary ??= new Dictionary<string, object>();
        
        if (dictionary.TryGetValue(key, out object obj)) {
            if (obj is T newValue) {
                value = newValue;

                return true;
            }

            value = default;

            return false;
        }

        foreach (var pair in Pairs) {
            if (pair.Key != key)
                continue;
            
            value = JsonConvert.DeserializeObject<T>(pair.Value);

            if (value == null)
                return false;
            
            dictionary.Add(key, value);

            return true;
        }

        value = default;

        return false;
    }

    /// <summary>
    /// Attempts to get the raw string value for an object stored within this container
    /// </summary>
    /// <param name="key">The key for the string to get</param>
    /// <param name="value">The acquired string</param>
    /// <returns>True if the string was found</returns>
    public bool TryGetValueRaw(string key, out string value) {
        foreach (var pair in Pairs) {
            if (pair.Key != key)
                continue;

            value = pair.Value;

            return true;
        }

        value = null;

        return false;
    }
}