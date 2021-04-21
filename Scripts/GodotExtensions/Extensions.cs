using System;
using System.Collections.Generic;
using Godot;

namespace Utils {
    public static class Extensions {
        public static Godot.Collections.Array ArrayFrom (params object[] objects) {
            Godot.Collections.Array result = new Godot.Collections.Array();
            foreach (object obj in objects) {
                result.Add(obj);
            }
            return result;
        }
        public static Godot.Collections.Array InArray (this object obj) {
            return ArrayFrom(obj);
        }
        public static void QueueFreeChildren (this Node node) {
            foreach (Node child in node.GetChildren()) {
                child.QueueFree();
            }
        }

        public static V GetOrDefault<K, V> (this IDictionary<K, V> dictionary, K key, V defaultValue) {
            V value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static bool EqualsOrFalse<K, V> (this IDictionary<K, V> dictionary, K key, V defaultValue) {
            V value;
            return dictionary.TryGetValue(key, out value) && value.Equals(defaultValue);
        }
    }
}