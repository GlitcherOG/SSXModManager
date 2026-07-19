using SSX_Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SSXModManagerWinForm.ModList;

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

                    //modInstructions.Load(ModZipPath + "\\ModInstructions.txt");

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
                image = Image.FromFile(ModPath + "\\Icon.png");
                modInstructions.Load(ModPath + "\\ModInstructions.txt");
            }

            if(modInfo.ModPackVersion>2)
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
        public void ApplyMod(string GamePath, string ZipExtractPath)
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

                        BIG.Create(Type, ExtractPath, Output, false);

                        Directory.Delete(ExtractPath, true);
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

                        File.Delete(GamePath + "\\Icon.png");
                        File.Delete(GamePath + "\\ModInfo.json");
                        File.Delete(GamePath + "\\ModInstructions.txt");
                    }
                }
                else
                {
                    CopyDirectory(ModPath, GamePath, true);
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
