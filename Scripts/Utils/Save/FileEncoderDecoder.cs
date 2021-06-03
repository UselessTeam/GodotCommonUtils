using System;
using System.Linq;
using System.Reflection;
using Godot;

namespace Utils {
    public class FileEncoder {
        public static string Version = "0.0.0";

        static readonly string savePath = "user://savegame.save";

        static readonly byte[] encodeKey = new byte[8] { 123, 111, 251, 227, 134, 180, 214, 252 };

        public static void Write (string data) {
            var file = new File();
            file.Open(savePath, File.ModeFlags.Write);
            file.StoreLine(Version);
            file.StoreString(data);
            file.Close();
        }

        public static string Read () {
            var file = new File();
            file.Open(savePath, File.ModeFlags.Read);
            string foundVersion = file.GetLine();
            if (foundVersion != Version) {
                throw new WrongVersionException(foundVersion);
            };
            string data = file.GetLine();
            file.Close();
            return data;
        }

        public static void Delete () {
            var dir = new Directory();
            dir.Remove(savePath);
        }

        public static bool SaveExists () {
            return new File().FileExists(savePath);
        }
    }

    public class WrongVersionException : Exception {
        public string FoundVersion;

        public WrongVersionException (string FoundVersion) {
            this.FoundVersion = FoundVersion;
        }

        public string GetMessage () {
            return $"The last save was made with version {FoundVersion} of the game,\nwhich is not compatible with current version {FileEncoder.Version} of the game\nYou will have to start a new game";
        }
    }
}