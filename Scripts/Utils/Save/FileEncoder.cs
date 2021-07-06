using System;
using System.Linq;
using System.Reflection;
using Godot;

namespace Utils {
    public class FileEncoder {
        public static Version CurrentVersion = new Version("0.0.0");

        public static Func<Version, bool> IsSaveCompatible = (Version version) => { return version == CurrentVersion; };

        const string defaultSavePath = "user://savegame.save";

        static readonly byte[] encodeKey = new byte[8] { 123, 111, 251, 227, 134, 180, 214, 252 };

        public static void Write (string data, string path = defaultSavePath) {
            using (var file = new File()) {
                file.Open(path, File.ModeFlags.Write);
                file.StoreLine(CurrentVersion.ToString());
                file.StoreString(data);
            }
        }

        public static string Read (string path = defaultSavePath) {
            using (var file = new File()) {
                file.Open(path, File.ModeFlags.Read);
                Version foundVersion = new Version(file.GetLine());
                if (!IsSaveCompatible(foundVersion)) {
                    throw new WrongVersionException(foundVersion.ToString());
                };
                string data = file.GetAsText();
                return data.Split("\n", 2).Skip(1).FirstOrDefault();
            }
        }

        public static void Delete (string path = defaultSavePath) {
            var dir = new Directory();
            dir.Remove(path);
        }

        public static bool SaveExists (string path = defaultSavePath) {
            var file = new File();
            return file.FileExists(path);
        }
        public static bool SaveIsCompatible (string path = defaultSavePath) {
            using (var file = new File()) {
                if (!file.FileExists(path)) return false;

                file.Open(path, File.ModeFlags.Read);
                Version foundVersion = new Version(file.GetLine());
                return IsSaveCompatible(foundVersion);
            }
        }
    }

    public static class StringExtensions {
        public static string ToPath (this string name) {
            return $"user://{name}.save";
        }
    }

    public class WrongVersionException : Exception {
        public string FoundVersion;

        public WrongVersionException (string FoundVersion) {
            this.FoundVersion = FoundVersion;
        }

        public string GetMessage () {
            return $"The last save was made with version {FoundVersion} of the game,\nwhich is not compatible with current version {FileEncoder.CurrentVersion} of the game\nYou will have to start a new game";
        }
    }
}