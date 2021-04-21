using System;
using System.Collections.Generic;

namespace Utils {
    public sealed class ComparableFunc<T> : IComparer<T> {
        private Func<T, T, int> comparer;

        public ComparableFunc(Func<T, T, int> comparer) {
            this.comparer = comparer;
        }
        public static ComparableFunc<T> FromSelect<V>(Func<T, V> select) where V : IComparable {
            return new ComparableFunc<T>((a, b) => select(a).CompareTo(select(b)));
        }
        public int Compare(T a, T b) {
            return comparer(a, b);
        }
    }
}