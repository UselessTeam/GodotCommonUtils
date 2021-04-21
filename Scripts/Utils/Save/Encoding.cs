using System;
using System.Collections.Generic;
using Godot;

namespace Utils {
    public static class Encoding {
        public static Godot.Collections.Array<T> ToGodotArray<T>(this IEnumerable<T> enumerable) {
            return new Godot.Collections.Array<T>(enumerable);
        }

        public static bool IsNumeric(this Type type) {
            if (!type.IsValueType) {
                return false;
            }
            switch (Type.GetTypeCode(type)) {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return true;
                default:
                    return false;
            }
        }

        public static string EncodeNumeric(long i) {
            return Convert.ToBase64String(BitConverter.GetBytes(i)).TrimEnd('A', '=');
        }

        public static long DecodeNumeric(string s) {
            s = s + "AAAAAAAAAAA=".Substring(s.Length);
            return BitConverter.ToInt64(Convert.FromBase64String(s), 0);
        }

        public static int DecodeInt(string s) {
            s = s + "AAAAAA==".Substring(s.Length);
            return BitConverter.ToInt32(Convert.FromBase64String(s), 0);
        }
    }
}