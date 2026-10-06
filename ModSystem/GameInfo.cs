using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSXModManagerWinForm.ModSystem
{
    public class GameInfo
    {
        public string Game = "";
        public string Version = "";
        public string GameVersion = "";
        public string Console = "";
        public string Elf = "";
        [JsonIgnore]
        public string GameDirectory = "";
        [JsonIgnore]
        public string ModDirectory = "";
        [JsonIgnore]
        public string BackupDirectory = "";

        public void CreateJson(string path, bool Inline = false)
        {
            var TempFormating = Formatting.None;
            if (Inline)
            {
                TempFormating = Formatting.Indented;
            }

            var serializer = JsonConvert.SerializeObject(this, TempFormating);
            File.WriteAllText(path, serializer);
        }

        public static GameInfo LoadJsonPath(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<GameInfo>(stream);
                return container;
            }
            else
            {
                return new GameInfo();
            }
        }
    }
}
