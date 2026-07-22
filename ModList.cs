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

                    if (modItem.modInfo.ModPackVersion <= ModInfo.MainModPackVersion)
                    {
                        modItem.Name = modItem.modInfo.Name;

                        modItem.Version = modItem.modInfo.Version;

                        modItem.Folder = true;

                        modItem.Enabled = false;

                        modItems.Add(modItem);
                    }
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

                            if (modItem.modInfo.ModPackVersion <= ModInfo.MainModPackVersion)
                            {
                                modItem.Name = modItem.modInfo.Name;

                                modItem.Version = modItem.modInfo.Version;

                                modItem.Folder = false;

                                modItem.Enabled = false;

                                modItems.Add(modItem);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("File not found inside the ZIP archive.");
                    }
                }

            }
        }

        public void CheckModList(string ModListFile)
        {
            List<ModItem> NewModItems = new List<ModItem>();

            if (File.Exists(ModListFile))
            {
                var ModListNames = File.ReadAllLines(ModListFile);

                for (int i = 0; i < ModListNames.Length; i++)
                {
                    for (int j = 0; j < modItems.Count; j++)
                    {
                        if (modItems[j].Name + "-" + modItems[j].Version == ModListNames[i])
                        {
                            var TempItem = modItems[j];
                            TempItem.Enabled = true;
                            NewModItems.Add(TempItem);
                            modItems.RemoveAt(j);
                            j--;
                        }
                    }
                }

                for (int i = 0; i < modItems.Count; i++)
                {
                    var TempItem = modItems[i];
                    TempItem.Enabled = false;
                    NewModItems.Add(TempItem);
                }

                modItems = NewModItems;
            }
        }

        public void ApplyMods(string GameFolder, string PCSX2DataPath)
        {
            Console.WriteLine("Starting Applying Mods");

            GameInfo gameInfo = GameInfo.LoadJsonPath(GameFolder + "\\GameInfo.json");
            string TextureFolder = PCSX2DataPath + "\\textures\\" + gameInfo.Elf.Split('.')[0] + "\\replacements\\";
            string CheatsIniFile = PCSX2DataPath + "\\cheats\\" + CRCCalculator.CalculateCRC32(GameFolder + "\\" + gameInfo.Elf) + ".pnach";
            string PerGameFolder = PCSX2DataPath + "\\gamesettings\\" + CRCCalculator.CalculateCRC32(GameFolder + "\\" + gameInfo.Elf) + ".ini";

            //Add Texture Replacement Folder
            if (Directory.Exists(TextureFolder))
            {
                Directory.Delete(TextureFolder, true);
            }

            //Add Cheats Folder and clear ini
            if(File.Exists(CheatsIniFile))
            {
                File.Delete(CheatsIniFile);
            }
            File.CreateText(CheatsIniFile).Close();

            //Clear Mods List
            if (File.Exists(GameFolder + "\\ModList.txt"))
            {
                File.Delete(GameFolder + "\\ModList.txt");
            }


            Directory.CreateDirectory(TextureFolder);
            if(!File.Exists(PerGameFolder))
            {
                PerGameSettings.GenerateStandardSettings(PerGameFolder);
            }
            else
            {
                PerGameSettings.ClearCheats(PerGameFolder);
            }

            for (int i = 0; i < modItems.Count; i++)
            {
                if (modItems[i].Enabled)
                {
                    try
                    {
                        ModZipFolder modZipFolder = new ModZipFolder();
                        modZipFolder.LoadMod(modItems[i].Path);
                        Console.WriteLine("Applying " + modItems[i].Name);

                        modZipFolder.ApplyPS2Mod(GameFolder, AppDomain.CurrentDomain.BaseDirectory + "\\Temp", TextureFolder, CheatsIniFile, PerGameFolder);
                    }
                    catch (Exception ex) 
                    { 
                        Console.WriteLine(ex.ToString());
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

            //Apply Cheats
            var FileLines = File.ReadAllLines(CheatsIniFile);
            for (int i = 0; i < FileLines.Length; i++)
            {
                if (FileLines[i].StartsWith("["))
                {
                    PerGameSettings.AddCheats(PerGameFolder, FileLines[i].Replace("[","").Replace("]",""));
                }
            }
        }

        public struct ModItem
        {
            public string Name;
            public string Path;
            public string Version;
            public bool Folder;
            [JsonIgnore]
            public ModInfo modInfo;
            public bool Enabled;
        }
    }
}
