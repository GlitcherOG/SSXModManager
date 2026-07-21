using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSXModManagerWinForm.ModSystem
{
    public class ModInfo
    {
        [JsonIgnore]
        public static int MainModPackVersion = 3;
        public int ModPackVersion;
        public string Name;
        public string Author;
        public int Game;
        public string Description;
        public string Version;
        public string DownloadLink;
        public string[] Tags;

        public void CreateListJson(string path, List<ModInfo> modInfos, bool Inline = false)
        {
            var TempFormating = Formatting.None;
            if (Inline)
            {
                TempFormating = Formatting.Indented;
            }

            var serializer = JsonConvert.SerializeObject(modInfos, TempFormating);
            File.WriteAllText(path, serializer);
        }

        public static List<ModInfo> LoadListJson(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<List<ModInfo>>(stream);
                return container;
            }
            else
            {
                return new List<ModInfo>();
            }
        }

        public void CreateJson(string path, ModInfo modInfos, bool Inline = false)
        {
            var TempFormating = Formatting.None;
            if (Inline)
            {
                TempFormating = Formatting.Indented;
            }

            var serializer = JsonConvert.SerializeObject(modInfos, TempFormating);
            File.WriteAllText(path, serializer);
        }

        public static ModInfo LoadJsonPath(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<ModInfo>(stream);
                return container;
            }
            else
            {
                return new ModInfo();
            }
        }

        public static ModInfo LoadJsonText(string Text)
        {
            var container = JsonConvert.DeserializeObject<ModInfo>(Text);

            if (container != null)
            {
                return container;
            }
            else
            {
                return new ModInfo();
            }
        }
    }
}
