using System;
using System.Collections.Generic;
using Godot;
using Utils;

namespace Utils {
    public class TextMemory : ISaveable {
        [Save] private List<Resource> savedResources = new List<Resource>();

        public const char RESOURCE_CHAR = '@';
        public const char STRING_CHAR = '%';
        public const string BBCODE = "[rem][url={1}]{0}[/url][/rem]";

        public struct MetaTag {
            public char group;
            public string key;
            public string Box (string s) {
                return string.Format(BBCODE, s, this);
            }

            public override string ToString () {
                return group + key.ToString();
            }
        }

        public static MetaTag Parse (string s) {
            return new MetaTag() {
                group = s[0],
                key = s.Substring(1)
            };
        }

        public Resource Remember (MetaTag metaTag) {
            return savedResources[Encoding.DecodeInt(metaTag.key)];
        }

        public T Remember<T> (MetaTag metaTag) where T : Resource {
            return (T) Remember(metaTag);
        }

        public MetaTag Tag (Resource resource, char groupOverride = RESOURCE_CHAR) {
            int index = savedResources.IndexOf(resource);
            if (index == -1) {
                index = savedResources.Count;
                savedResources.Add(resource);
            }
            return new MetaTag {
                group = groupOverride,
                key = Encoding.EncodeNumeric(index),
            };
        }

        public MetaTag Tag (string value, char groupOverride = STRING_CHAR) {
            return new MetaTag {
                group = groupOverride,
                key = value,
            };
        }
    }
}