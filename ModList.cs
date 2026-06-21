using Newtonsoft.Json;
using SSXModManagerWinForm.ModSystem;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace SSXModManagerWinForm
{
    internal class ModList
    {
        public List<ModItem> modItems = new List<ModItem>();

        public void LoadModFolder(string Folder)
        {
            string[] Mods = Directory.GetFiles(Folder, "*.zip", SearchOption.TopDirectoryOnly);

            modItems = new List<ModItem>();


            for (int i = 0; i < Mods.Length; i++)
            {
                using (ZipArchive archive = ZipFile.OpenRead(Mods[i]))
                {
                    // 2. Find the specific file entry
                    ZipArchiveEntry entry = archive.GetEntry("ModInfo.json");

                    if (entry != null)
                    {
                        // 3. Open the entry stream and read its contents
                        using (Stream stream = entry.Open())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string fileContents = reader.ReadToEnd();

                            ModItem modItem = new ModItem();

                            modItem.Path = Mods[i];

                            modItem.modInfo = ModInfo.LoadJsonText(fileContents);

                            modItem.Name = modItem.modInfo.Name;

                            modItem.Enabled = true;

                            modItems.Add(modItem);
                        }
                    }
                    else
                    {
                        Console.WriteLine("File not found inside the ZIP archive.");
                    }
                }

            }

        }

        public void ApplyMods(string GameFolder)
        {
            for (int i = 0; i < modItems.Count; i++)
            {
                if (modItems[i].Enabled)
                {
                    ModZipFolder modZipFolder = new ModZipFolder();

                    modZipFolder.LoadMod(modItems[i].Path);

                    modZipFolder.ApplyMod(GameFolder);
                }
            }
        }

        public struct ModItem
        {
            public string Name;
            public string Path;
            [JsonIgnore]
            public ModInfo modInfo;
            public bool Enabled;
        }
    }
}
