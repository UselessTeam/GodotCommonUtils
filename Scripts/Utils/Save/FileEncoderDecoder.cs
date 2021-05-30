using System;
using System.Linq;
using System.Reflection;
using Godot;

namespace Utils {
    public class FileEncoder {
        static readonly string savePath = "user://savegame.save";

        static readonly byte[] encodeKey = new byte[8] { 123, 111, 251, 227, 134, 180, 214, 252 };

        public static void Write (string data) {
            var file = new File();
            file.Open(savePath, File.ModeFlags.Write);
            file.StoreString(data);
            file.Close();
        }

        public static string Read () {
            var file = new File();
            file.Open(savePath, File.ModeFlags.Read);
            string data = file.GetAsText();
            file.Close();
            return data;
        }

        public static bool SaveExists () {
            return new File().FileExists(savePath);
        }
    }

}