using Newtonsoft.Json;
using SSXModManagerWinForm.ModSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSXModManagerWinForm
{
    public class Settings
    {
        public string PCSX2Path;
        public string PCSX2DataPath;

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

        public static Settings LoadJsonPath(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<Settings>(stream);
                return container;
            }
            else
            {
                return new Settings();
            }
        }
    }
}
