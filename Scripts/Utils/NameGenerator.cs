using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Utils {
    public static class NameGenerator {

        private static readonly string[] CONSONANTS = {
        "b", "c", "d", "f", "g", "h", "k", "l", "m", "n", "p", "r", "s", "t", "v", "w", "x", "z"
    };
        private static readonly string[] PREFIX_MIDDLE = Merge(CONSONANTS, new string[] {
        "r", "t", "n", "s", "l", "c", "j",
        "ch", "tr", "qu",
    });

        private static readonly string[] PREFIX = Merge(PREFIX_MIDDLE, new string[] {
        "gw", "tw"
    });

        private static readonly string[] MIDDLE = Merge(PREFIX_MIDDLE, new string[] {
        "lr", "lv", "nn", "n"
    });

        private static readonly string[] SUFFIX = Merge(CONSONANTS, new string[] {
        "r", "t", "n", "s", "l", "ck", "ne", "z", ""
    });

        private static readonly string[] VOWEL = {
        "a", "e", "i", "o", "u", "a", "e", "i", "o", "u", "a", "e", "i", "o", "u",
        "a", "e", "i",
        "y", "ae", "aa", "ea", "ie", "ao"
    };

        public static string RandomName () {
            while (true) {
                string s = RandomUncheckedString();
                if (s.Length >= 3 && s.Length <= 12) {
                    s = char.ToUpper(s[0]) + s.Substring(1);
                    return s;
                }
            }
        }

        private static string RandomUncheckedString () {
            float x = (float) RNG.rng.NextDouble();
            if (x < 0.10) {
                return PREFIX.Random() + VOWEL.Random();
            }
            if (x < 0.20) {
                return VOWEL.Random() + SUFFIX.Random();
            }
            if (x < 0.35) {
                return PREFIX.Random() + VOWEL.Random() + SUFFIX.Random();
            }
            if (x < 0.55) {
                return PREFIX.Random() + VOWEL.Random() + MIDDLE.Random() + VOWEL.Random();
            }
            if (x < 0.75) {
                return VOWEL.Random() + MIDDLE.Random() + VOWEL.Random() + SUFFIX.Random();
            }
            if (x < 0.85) {
                return PREFIX.Random() + VOWEL.Random() + MIDDLE.Random() + VOWEL.Random() + SUFFIX.Random();
            }
            if (x < 0.90) {
                return PREFIX.Random() + VOWEL.Random() + MIDDLE.Random() + VOWEL.Random() + MIDDLE.Random() + VOWEL.Random();
            }
            if (x < 0.95) {
                return VOWEL.Random() + MIDDLE.Random() + VOWEL.Random() + MIDDLE.Random() + VOWEL.Random() + SUFFIX.Random();
            }
            return PREFIX.Random() + VOWEL.Random() + MIDDLE.Random() + VOWEL.Random() + MIDDLE.Random() + VOWEL.Random() + SUFFIX.Random();
        }

        private static string[] Merge (params string[][] lists) {
            IEnumerable<string> l = lists[0];
            foreach (string[] other in lists.Skip(1)) {
                l = l.Concat(other);
            }
            return l.ToArray();
        }
    }
}