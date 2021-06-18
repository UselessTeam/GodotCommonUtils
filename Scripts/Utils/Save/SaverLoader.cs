using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace Utils {
    public class Saver {
        List<Resource> resources = new List<Resource>();
        List<Godot.Collections.Dictionary> encoded_resources = new List<Godot.Collections.Dictionary>();

        public static string Save (Resource resource) {
            Saver saver = new Saver();
            saver.ToKey(resource);
            return saver.Flush();
        }
        public static string Save (ISaveable saveable) {
            Saver saver = new Saver();
            var object_data = saver.CreateAndSerialize(saveable);
            return JSON.Print(object_data);
        }
        public static string Save (object _object) {
            var saver = new Saver();
            object serialization = saver.Serialize(_object);
            if (serialization == null) return null;
            if (serialization is string) return JSON.Print(new Godot.Collections.Dictionary() { { "type", _object.GetType().FullName }, { "value", serialization } });
            if (serialization is int) return JSON.Print(saver.encoded_resources[(int) serialization]);
            if (serialization is Godot.Collections.Dictionary) return JSON.Print((Godot.Collections.Dictionary) serialization);
            if (serialization is Godot.Collections.Array) { GD.PrintErr("Godot array doesn't work"); return null; }
            return null;
        }

        public string Flush () {
            if (encoded_resources.Count > 0) {
                return JSON.Print(encoded_resources);
            }
            return "";
        }
        public int ToKey (Resource resource) {
            int index = resources.IndexOf(resource);
            if (index == -1) {
                index = resources.Count;
                resources.Add(resource);
                encoded_resources.Add(null);
                encoded_resources[index] = CreateAndSerialize(resource);
            }
            return index;
        }

        private object Serialize (object obj) {
            if (obj == null) {
                return null;
            }
            if (obj is Resource resource) {
                return ToKey(resource);
            }
            if (obj is ISaveable saveable) {
                return CreateAndSerialize(saveable);
            }
            if (obj is string s) {
                return s;
            }
            if (obj is Enum e) {
                return Encoding.EncodeNumeric(Convert.ToInt32(e));
            }
            if (obj.GetType().IsNumeric()) {
                return Encoding.EncodeNumeric(Convert.ToInt64(obj));
            }
            if (obj is Color color) {
                return Encoding.EncodeNumeric(color.ToRgba64());
            }
            if (obj is IEnumerable<object> obj_enumerable) {
                return obj_enumerable.Select(obj => Serialize(obj)).ToGodotArray();
            }
            if (obj is IEnumerable enumerable) {
                return enumerable.Cast<object>().Select(obj => Serialize(obj)).ToGodotArray();
            }
            return obj;
        }

        private Godot.Collections.Dictionary CreateAndSerialize (object obj) {
            Type type = obj.GetType();
            var data = new Godot.Collections.Dictionary();
            data["type"] = type.FullName;
            foreach (FieldInfo field in type.GetSaveableFields()) {
                data[field.Name] = Serialize(field.GetValue(obj));
            }
            return data;
        }
    }

    public class Loader {
        List<Godot.Collections.Dictionary> encoded_resources = new List<Godot.Collections.Dictionary>();
        Dictionary<int, Resource> objects = new Dictionary<int, Resource>();

        public static Loader LoadThatCrashes (string value) { // Kunchan's function that needs repair
            Loader instance = new Loader();
            instance.encoded_resources = ((Godot.Collections.Array) JSON.Parse(value).Result).Cast<object>().Select(obj => (Godot.Collections.Dictionary) obj).ToList();
            return instance;
        }

        public static object Load (string data) { // Hedi's function
            var dict = (Godot.Collections.Dictionary) JSON.Parse(data).Result;
            Type type = Type.GetType((string) dict["type"]);

            if (type.GetInterfaces().Contains(typeof(ISaveable)) || typeof(Resource).IsAssignableFrom(type)) {
                return new Loader().CreateAndDeserialize(type, dict);
            } else {
                return new Loader().Deserialize(type, dict["value"]);
            }
        }
        public static List<object> LoadMany (string datas) {
            var list = new List<object>();
            foreach (string data in datas.Split("\n"))
                list.Add(Load(data));
            return list;
        }

        public Resource FromKey (int key) {
            if (!objects.ContainsKey(key)) {
                Godot.Collections.Dictionary data = encoded_resources[key];
                Type type = Type.GetType((string) data["type"]);
                objects[key] = (Godot.Resource) CreateAndDeserialize(type, data);
            }
            return objects[key];
        }

        private object Deserialize (Type type, object data) {
            if (data == null) {
                return null;
            }
            if (typeof(Resource).IsAssignableFrom(type)) {
                return FromKey(Convert.ToInt32((float) data));
            }
            if (typeof(ISaveable).IsAssignableFrom(type)) {
                return CreateAndDeserialize(type, (Godot.Collections.Dictionary) data);
            }
            if (type == typeof(string)) {
                return (string) data;
            }
            if (type.IsEnum) {
                return Enum.ToObject(type, Deserialize(Enum.GetUnderlyingType(type), data));
            }
            if (type.IsNumeric()) {
                return Convert.ChangeType(Encoding.DecodeNumeric((string) data), type);
            }
            if (type == typeof(Color)) {
                return new Color(Encoding.DecodeNumeric((string) data));
            }
            if (typeof(IEnumerable).IsAssignableFrom(type)) {
                Type elementType = type.GetElementType() ?? type.GenericTypeArguments[0];
                var raw_enumerable = ((IEnumerable) data).Cast<object>().Select(v => Deserialize(elementType, v)).ToList();
                var value = typeof(Enumerable)
                    .GetMethod("Cast")
                    .MakeGenericMethod(elementType)
                    .Invoke(
                        null,
                        new object[] { raw_enumerable });
                if (typeof(Array).IsAssignableFrom(type)) {
                    return typeof(Enumerable)
                        .GetMethod("ToArray")
                        .MakeGenericMethod(elementType)
                        .Invoke(null, new object[] { value });
                }
                return Activator.CreateInstance(type, value);
            }
            try {
                return Convert.ChangeType(data, type);
            } catch {
                try {
                    return Activator.CreateInstance(type, data);
                } catch {
                    throw new FormatException();
                }
            }

        }

        private object CreateAndDeserialize (Type type, Godot.Collections.Dictionary data) {
            object obj = Activator.CreateInstance(type);
            foreach (FieldInfo field in type.GetSaveableFields()) {
                var value = Deserialize(field.FieldType, data[field.Name]);
                if (value == null) {
                    continue;
                }
                try {
                    field.SetValue(obj, value);
                } catch (Exception e) {
                    GD.PrintErr($"{type}.{field.Name} = {value} failed");
                    throw e;
                }
            }
            return obj;
        }
    }
}