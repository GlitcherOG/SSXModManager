using CommunityToolkit.HighPerformance;
using SSX_Library;
using SSX_Library.EATextureLibrary;
using SSXLibrary.FileHandlers;
using System.IO.Compression;

namespace SSXModManagerWinForm.ModSystem
{
    public class ModZipFolder
    {
        public ModInfo modInfo = new ModInfo();
        public string ModPath = "";
        public bool Zip = false;
        public Image image;
        public ModMakingInstructions modInstructions = new ModMakingInstructions();

        public void LoadMod(string ZipPath)
        {
            modInfo = new ModInfo();
            modInstructions = new ModMakingInstructions();
            image = null;

            ModPath = ZipPath;

            if (image != null)
            {
                image.Dispose();
            }

            if (ModPath.ToLower().Contains(".zip"))
            {
                Zip = true;
                using (ZipArchive archive = ZipFile.OpenRead(ModPath))
                {
                    ZipArchiveEntry entry = archive.GetEntry(GetZipPath(archive, "ModInfo.json"));

                    using (Stream stream = entry.Open())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string fileContents = reader.ReadToEnd();

                            modInfo = ModInfo.LoadJsonText(fileContents);
                        }
                    }

                    entry = archive.GetEntry(GetZipPath(archive, "ModInstructions.txt"));

                    if (entry != null)
                    {
                        using (Stream stream = entry.Open())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                string fileContents = reader.ReadToEnd();

                                modInstructions.LoadText(fileContents);
                            }
                        }
                    }

                    entry = archive.GetEntry(GetZipPath(archive, "Icon.png"));

                    if (entry != null)
                    {
                        image = Image.FromStream(entry.Open());
                    }
                }
            }
            else
            {
                modInfo = ModInfo.LoadJsonPath(ModPath+ "\\ModInfo.json");
                if (File.Exists(ModPath + "\\Icon.png"))
                {
                    image = Image.FromFile(ModPath + "\\Icon.png");
                }
                else
                {
                    image = new Bitmap(1, 1);
                }
                modInstructions.Load(ModPath + "\\ModInstructions.txt");
            }

            if(modInfo.ModPackVersion>3)
            {
                throw new Exception("New Mod Pack Version Detected. Please Update Tool");
            }
        }

        public static string GetZipPath(ZipArchive archive, string FileName)
        {
            for (int i = 0; i < archive.Entries.Count; i++)
            {
                if (archive.Entries[i].FullName.ToLower()==FileName.ToLower())
                {
                    return archive.Entries[i].FullName;
                }
            }

            return "null";
        }

//Copy
//Delete
//Big Extract
//BigF Make
//BigC0FB Make
//Config Insert
        public void ApplyPS2Mod(string GamePath, string ZipExtractPath, string PCSX2TexturePath, string PCSX2CheatPath, string PCSX2PerGameSettingsPath)
        {
            bool Valid = false;
            if (modInstructions.Instructions.Count != 0)
            {
                string TempModPath = ModPath;

                if (Zip)
                {
                    TempModPath = ZipExtractPath;
                    //Extract Zip
                    using (ZipArchive archive = ZipFile.OpenRead(ModPath))
                    {
                        archive.ExtractToDirectory(ZipExtractPath, true);
                    }
                }

                //Check Textures
                ApplyTextures(TempModPath + "\\PCSX2 Textures", PCSX2TexturePath);

                //Check Cheats
                ApplyCheatsPS2(TempModPath + "\\PCSX2Patch.txt", PCSX2CheatPath);

                var Instructions = modInstructions.Instructions;
                for (int i = 0; i < Instructions.Count; i++)
                {
                    //Load Source and Output
                    string Source = Instructions[i].Source;
                    string Output = Instructions[i].Ouput;

                    if (Source.StartsWith("game\\"))
                    {
                        Source = Source.Replace("game\\", GamePath + "//");
                    }

                    if (Source.StartsWith("mod\\"))
                    {
                        Source = Source.Replace("mod\\", TempModPath + "\\");
                    }

                    if (Output.StartsWith("game\\"))
                    {
                        Output = Output.Replace("game\\", GamePath + "//");
                    }

                    if (Output.StartsWith("mod\\"))
                    {
                        MessageBox.Show("Error Effecting Mod Folder");
                        return;
                    }

                    if (Output != "")
                    {
                        Output = Path.GetFullPath(Output);
                    }

                    Source = Path.GetFullPath(Source);

                    //Check Source Is Valid
                    if (File.Exists(Source))
                    {
                        Valid = true;
                    }
                    else if (Directory.Exists(Source))
                    {
                        Valid = true;
                    }

                    if (!Valid)
                    {
                        break;
                    }

                    //Working
                    if (Instructions[i].Type == "copy")
                    {
                        if (File.Exists(Source))
                        {
                            if (File.Exists(Output))
                            {
                                File.Delete(Output);
                            }
                            File.Copy(Source, Output);
                        }
                        else if (Directory.Exists(Source))
                        {
                            CopyDirectory(Source, Output, true);
                        }
                    }
                    //Working
                    else if (Instructions[i].Type == "delete")
                    {
                        if (File.Exists(Source))
                        {
                            File.Delete(Source);
                        }
                        else if (Directory.Exists(Source))
                        {
                            Directory.Delete(Source, true);
                        }
                    }
                    //Working
                    else if (Instructions[i].Type == "big extract")
                    {
                        if (File.Exists(Source))
                        {
                            BIG.Extract(Source, Output);
                        }
                    }
                    else if (Instructions[i].Type == "bigf make")
                    {
                        if (Directory.Exists(Source))
                        {
                            BIG.Create(BigType.BIGF, Source, Output, false);
                        }
                    }
                    //Working
                    else if (Instructions[i].Type == "bigc0fb make")
                    {
                        if (Directory.Exists(Source))
                        {
                            BIG.Create(BigType.C0FB, Source, Output, false);
                        }
                    }
                    else if (Instructions[i].Type == "big4 make")
                    {
                        if (Directory.Exists(Source))
                        {
                            BIG.Create(BigType.BIG4, Source, Output, false);
                        }
                    }
                    else if (Instructions[i].Type == "big insert")
                    {
                        var Type = BIG.GetBigType(Output);

                        var Members = BIG.GetMembersInfo(Output);
                        bool Slash = false;

                        if (Members[0].Path.Contains("\\"))
                        {
                            Slash = true;
                        }

                        string ExtractPath = Output.ToLower().Replace(".big", "") + "\\";

                        BIG.Extract(Output, ExtractPath);

                        if (File.Exists(Source))
                        {
                            string FileName = Path.GetFileName(Source);

                            if (File.Exists(ExtractPath + Source))
                            {
                                File.Delete(ExtractPath + Source);
                            }
                            File.Copy(Source, ExtractPath+ Source);
                        }
                        else if (Directory.Exists(Source))
                        {
                            CopyDirectory(Source, ExtractPath, true);
                        }

                        BIG.Create(Type, ExtractPath, Output,false, Slash);

                        Directory.Delete(ExtractPath, true);
                    }
                    else if (Instructions[i].Type=="loc insert")
                    {
                        LOC Loc = new LOC();
                        Loc.Load(Output);

                        var AllText = File.ReadAllText(Source);

                        var SplitLines = AllText.Split("###");

                        for (int j = 0; j < (SplitLines.Length-1)/2; j++)
                        {
                            Loc.SetTextByID(int.Parse(SplitLines[j*2+1]), SplitLines[j * 2 + 2].TrimStart("\r\n".ToCharArray()));
                        }

                        Loc.Save(Output);
                    }
                    else if (Instructions[i].Type == "ssh insert")
                    {
                        OldShapeHandler shapeHandler = new OldShapeHandler();

                        string[] ARGS = Instructions[i].Ouput.Split("-");

                        shapeHandler.LoadShape(ARGS[0]);

                        var Shape = shapeHandler.ShapeImages[int.Parse(ARGS[1])];
                        Shape.Image = (SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>)SixLabors.ImageSharp.Image.Load(Source);
                        shapeHandler.ShapeImages[int.Parse(ARGS[1])] = Shape;

                        shapeHandler.SaveShape(ARGS[0]);

                    }
                    else if(Instructions[i].Type == "ssx3 music insert")
                    {
                        MusicINFHandler SourceINF = new MusicINFHandler();
                        MusicINFHandler OutputINF = new MusicINFHandler();

                        SourceINF.LoadMusFile(Source);
                        OutputINF.LoadMusFile(Output);

                        int LastSong = 0;

                        for (int j = 0; j < OutputINF.musFileSongs.Count; j++)
                        {
                            if (OutputINF.musFileSongs[j].ID.TrimEnd()== "[charsel]")
                            {
                                LastSong = j;
                            }
                        }

                        OutputINF.musFileSongs.InsertRange(LastSong, SourceINF.musFileSongs);

                        OutputINF.SaveMusFile(Output);
                    }
                    else if (Instructions[i].Type == "ssx3 playlist insert")
                    {
                        string[] Lines = File.ReadAllLines(Source);

                        string[] OutputLines = File.ReadAllLines(Output);

                        var SourceLines = Lines.ToList();

                        var ListLines = OutputLines.ToList();

                        ListLines.AddRange(SourceLines);

                        ListLines.RemoveAll(string.IsNullOrWhiteSpace);

                        File.WriteAllLines(Output, ListLines.ToArray());
                    }
                }

                if(Zip)
                {
                    Directory.Delete(TempModPath, true);
                }
            }
            else
            {
                Console.WriteLine("No Instructions Starting Base Copy");
                Valid = true;
                if (Zip)
                {
                    //Fallback Copy Game Files Over
                    using (ZipArchive archive = ZipFile.OpenRead(ModPath))
                    {
                        archive.ExtractToDirectory(GamePath, true);

                        ApplyTextures(GamePath + "\\PCSX2 Textures", PCSX2TexturePath);

                        ApplyCheatsPS2(GamePath + "\\PCSX2Patch.txt", PCSX2CheatPath);

                        File.Delete(GamePath + "\\Icon.png");
                        File.Delete(GamePath + "\\ModInfo.json");
                        File.Delete(GamePath + "\\ModInstructions.txt");
                    }
                }
                else
                {
                    CopyDirectory(ModPath, GamePath, true);

                    ApplyTextures(GamePath + "\\PCSX2 Textures", PCSX2TexturePath);

                    ApplyCheatsPS2(GamePath + "\\PCSX2Patch.txt", PCSX2CheatPath);

                    File.Delete(GamePath + "\\Icon.png");
                    File.Delete(GamePath + "\\ModInfo.json");
                    File.Delete(GamePath + "\\ModInstructions.txt");
                }
            }

            if (Valid)
            {
                if (File.Exists(GamePath + "\\ModList.txt"))
                {
                    var String = File.ReadAllText(GamePath + "\\ModList.txt");
                    String += "\n" + modInfo.Name + "-" + modInfo.Version;
                    File.WriteAllText(GamePath + "\\ModList.txt", String);
                }
                else
                {
                    File.WriteAllText(GamePath + "\\ModList.txt", modInfo.Name + "-" + modInfo.Version);
                }
            }
            else
            {
                MessageBox.Show("Instructions Source Path Invalid. Are you using the correct game?");
            }
        }

        public void ApplyCheatsPS2(string CheatPath, string PCSX2CheatPath)
        {
            if (File.Exists(CheatPath))
            {
                string LoadCheats = File.ReadAllText(CheatPath);

                File.AppendAllText(PCSX2CheatPath, LoadCheats + "\n");
            }
        }

        public void ApplyTextures(string ModTextures, string PCSX2TexturePath)
        {
            if (Directory.Exists(ModTextures))
            {
                CopyDirectory(ModTextures, PCSX2TexturePath, true);
            }
        }

        void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                if(File.Exists(targetFilePath))
                {
                    File.Delete(targetFilePath);
                }
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
    }
}
