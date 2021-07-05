using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Utils {
    public static class RNG {
        //Management
        static int currentSeed = new Random().Next();
        static Random rand = new Random(currentSeed);

        public static int Seed {
            get { return currentSeed; }
            private set {
                currentSeed = value;
                rand = new Random(value);
            }
        }

        // public static int? NextSeed { get; private set; } = null;
        // public static int SetSeedForNextCycle (int? seed = null) {
        //     NextSeed = seed ?? rand.Next();
        //     return (int) NextSeed;
        // }
        public static void StartCycle (int? seed = null) {
            Seed = seed /* ?? NextSeed */ ?? rand.Next();
            // if (!seed.HasValue) NextSeed = null;
        }
        public static void ResetCycle () {
            Seed = Seed;
        }

        //Utils
        public static Random Get { get { return rand; } }

        public static T Random<T> (this T[] list) {
            return list[Get.Next(0, list.Length)];
        }
        public static T Random<T> (this IEnumerable<T> list) {
            return list.ElementAt(Get.Next(0, list.Count()));
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