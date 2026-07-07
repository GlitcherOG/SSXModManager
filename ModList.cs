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
            modItems = new List<ModItem>();

            string[] Mods = Directory.GetFiles(Folder, "*.zip", SearchOption.TopDirectoryOnly);

            string[] ModFolders = Directory.GetDirectories(Folder, "*", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < ModFolders.Length; i++)
            {
                if (File.Exists(ModFolders[i] +"\\"+ "ModInfo.json"))
                {
                    ModItem modItem = new ModItem();

                    modItem.Path = ModFolders[i];

                    modItem.modInfo = ModInfo.LoadJsonPath(ModFolders[i] + "\\" + "ModInfo.json");

                    modItem.Name = "*"+ modItem.modInfo.Name;

                    modItem.Enabled = true;

                    modItems.Add(modItem);
                }

            }


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
            File.Delete(GameFolder + "\\ModList.txt");
            while (File.Exists(GameFolder + "\\ModList.txt"))
            {

            }

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
