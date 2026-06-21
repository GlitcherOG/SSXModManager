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
        public string ModZipPath = "";
        public Image image;
        public ModMakingInstructions modInstructions = new ModMakingInstructions();

        public void LoadMod(string ZipPath)
        {
            modInfo = new ModInfo();
            modInstructions = new ModMakingInstructions();
            image = null;

            ModZipPath = ZipPath;

            if (image != null)
            {
                image.Dispose();
            }

            using (ZipArchive archive = ZipFile.OpenRead(ModZipPath))
            {
                ZipArchiveEntry entry = archive.GetEntry("ModInfo.json");

                using (Stream stream = entry.Open())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string fileContents = reader.ReadToEnd();

                        modInfo = ModInfo.LoadJsonText(fileContents);
                    }
                }

                //modInstructions.Load(ModZipPath + "\\ModInstructions.txt");

                entry = archive.GetEntry("Icon.png");

                if (entry!=null)
                {
                    image = Image.FromStream(entry.Open());
                }
            }
        }

//Copy
//Delete
//Big Extract
//BigF Make
//BigC0FB Make
//Config Insert
        public void ApplyMod(string GamePath)
        {
            if(modInstructions.Instructions.Count != 0)
            {
                var Instructions = modInstructions.Instructions;
                bool Valid = false;
                for (int i = 0; i < Instructions.Count(); i++)
                {
                    //Load Source and Output
                    string Source = Instructions[i].Source;
                    string Output = Instructions[i].Ouput;

                    if (Source.StartsWith("Game\\"))
                    {
                        Source = Source.Replace("Game\\", GamePath + "//");
                    }

                    if (Source.StartsWith("Mod\\"))
                    {
                        Source = Source.Replace("Mod\\", ModZipPath + "//Temp//");
                    }

                    if (Output.StartsWith("Game\\"))
                    {
                        Output = Output.Replace("Game\\", GamePath + "//");
                    }

                    if (Output.StartsWith("Mod\\"))
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

                    if(!Valid)
                    {
                        break;
                    }

                    //Working
                    if (Instructions[i].Type == "Copy")
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
                    else if (Instructions[i].Type == "Delete")
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
                    else if (Instructions[i].Type == "Big Extract")
                    {
                        if (File.Exists(Source))
                        {
                            BIG.Extract(Source, Output);
                        }
                    }
                    else if (Instructions[i].Type == "BigF Make")
                    {
                        if (Directory.Exists(Source))
                        {
                            BIG.Create(BigType.BIGF, Source, Output, false);
                        }
                    }
                    //Working
                    else if (Instructions[i].Type == "BigC0FB Make")
                    {
                        if (Directory.Exists(Source))
                        {
                            BIG.Create(BigType.C0FB, Source, Output, false);
                        }
                    }
                    else if (Instructions[i].Type == "Big4 Make")
                    {
                        if (Directory.Exists(Source))
                        {
                            BIG.Create(BigType.BIG4, Source, Output, false);
                        }
                    }
                }

                if (Valid)
                {
                    if(File.Exists(Application.StartupPath + "//ModList.txt"))
                    {
                        var String = File.ReadAllText(Application.StartupPath + "//ModList.txt");
                        String += "\n" + modInfo.Name + " (" + modInfo.Version + ")";
                        File.WriteAllText(Application.StartupPath + "//ModList.txt", String);
                    }
                    else
                    {
                        File.WriteAllText(Application.StartupPath + "//ModList.txt", modInfo.Name + " (" + modInfo.Version + ")");
                    }
                    MessageBox.Show("Mod Applied");
                }
                else
                {
                    MessageBox.Show("Instructions Source Path Invalid. Are you using the correct game?");
                }
            }
            else
            {
                //Fallback Copy Game Files Over
                using (ZipArchive archive = ZipFile.OpenRead(ModZipPath))
                {
                    archive.ExtractToDirectory(GamePath, true);

                    File.Delete(GamePath + "\\Icon.png");
                    File.Delete(GamePath + "\\ModInfo.json");
                    File.Delete(GamePath + "\\ModInstructions.txt");
                }
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
