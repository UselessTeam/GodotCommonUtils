using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Utils {
    public static class RNG {
        public static Random rng { get; } = new Random();

        public static T Random<T> (this T[] list) {
            return list[rng.Next(0, list.Length)];
        }
        public static T Random<T> (this IEnumerable<T> list) {
            return list.ElementAt(rng.Next(0, list.Count()));
        }

        public static T PopRandom<T> (this List<T> list) {
            T r = list.Random();
            list.Remove(r);
            return r;
        }

        public static IEnumerable<T> RandomOrder<T> (this IEnumerable<T> list) {
            List<T> elements = new List<T>(list);
            while (elements.Count > 0) {
                yield return elements.PopRandom();
            }
        }
    }
}