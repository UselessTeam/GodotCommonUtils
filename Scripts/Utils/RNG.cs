using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Utils {
    public static class RNG {
        static int currentSeed = new Random().Next();
        static Random rand = new Random(currentSeed);

        public static Random Get { get { return rand; } }
        public static int Seed {
            get { return currentSeed; }
            set {
                currentSeed = value;
                rand = new Random(value);
                GD.Print("Change seed to ", value);
            }
        }

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