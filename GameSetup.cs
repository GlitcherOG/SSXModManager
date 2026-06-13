using DiscUtils.Iso9660;
using SSX_Library;
using SSXLibrary.FileHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSXModManagerWinForm
{
    public class GameSetup
    {
        public static void CheckDisk(string LoadPath)
        {
            string crc = "";

            using (FileStream isoStream = File.Open(LoadPath, FileMode.Open))
            {
                CDReader cd = new CDReader(isoStream, true);
                string[] Files = cd.GetFiles("", "*.*", SearchOption.TopDirectoryOnly);

                int SystemCNF = Files.IndexOf("\\SYSTEM.CNF;1");

                var CDFile = cd.OpenFile(Files[SystemCNF], FileMode.Open);

                using var reader = new StreamReader(CDFile);
                CDFile.Position = 0; // rewind first
                string text = reader.ReadToEnd();

                string[] Lines = text.Split("\r\n");

                string Path = Lines[0].Replace("BOOT2=cdrom0:","");

                int ELFPath = Files.IndexOf(Path);

                CDFile = cd.OpenFile(Files[ELFPath], FileMode.Open);

                crc = CRCCalculator.CalculateCRC32(CDFile);
            }

            if (crc == "8E7CFF62")
            {
                ExtractSSXTrickyPS2(LoadPath);
            }
        }


        //SSX OG US
        public static void ExtractSSXOGPS2(string LoadPath)
        {
            string Backup = AppDomain.CurrentDomain.BaseDirectory + "\\Backup\\";
            string BackupGameFolder = Backup + "\\SSX OG\\";
            string GamesFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Game\\";
            string GameFolder = GamesFolders + "\\SSX OG\\";
            string ModsFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Mods\\";
            string ModFolder = GamesFolders + "\\SSX OG\\";

            if (!Directory.Exists(BackupGameFolder))
            {
                Directory.CreateDirectory(BackupGameFolder);
            }
            if (!Directory.Exists(GamesFolders))
            {
                Directory.CreateDirectory(GamesFolders);
            }

            //Extract to Backup
            using (FileStream isoStream = File.Open(LoadPath, FileMode.Open))
            {
                CDReader cd = new CDReader(isoStream, true);
                string[] Files = cd.GetFiles("", "*.*", SearchOption.AllDirectories);

                for (int i = 0; i < Files.Length; i++)
                {
                    string directory = Path.GetDirectoryName(BackupGameFolder + Files[i]);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    SaveFile(cd.OpenFile(Files[i], FileMode.Open), BackupGameFolder + Files[i].Replace(";1", ""));
                }
            }

            //Extract Levels
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\ALOHA.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\ELYSIUM.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\MEGAPLEX.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\MERQUERY.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\MESA.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\PIPE.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\SNOW.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\UNTRACK.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\WARMUP.BIG", BackupGameFolder);

            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\ALOHA.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\ELYSIUM.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\MEGAPLEX.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\MERQUERY.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\MESA.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\PIPE.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\SNOW.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\UNTRACK.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\WARMUP.BIG");

            //Copy to Active Game Folder
            CopyFilesRecursively(BackupGameFolder, GameFolder);
        }

        //SSX Tricky US
        public static void ExtractSSXTrickyPS2(string LoadPath)
        {
            string Backup = AppDomain.CurrentDomain.BaseDirectory + "\\Backup\\";
            string BackupGameFolder = Backup + "\\SSX Tricky\\";
            string GamesFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Game\\";
            string GameFolder = GamesFolders + "\\SSX Tricky\\";
            string ModsFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Mods\\";
            string ModFolder = GamesFolders + "\\SSX Tricky\\";

            if (!Directory.Exists(BackupGameFolder))
            {
                Directory.CreateDirectory(BackupGameFolder);
            }
            if (!Directory.Exists(GameFolder))
            {
                Directory.CreateDirectory(GameFolder);
            }
            if (!Directory.Exists(ModFolder))
            {
                Directory.CreateDirectory(ModFolder);
            }

            //Extract to Backup
            using (FileStream isoStream = File.Open(LoadPath, FileMode.Open))
            {
                CDReader cd = new CDReader(isoStream, true);
                string[] Files = cd.GetFiles("", "*.*", SearchOption.AllDirectories);

                for (int i = 0; i < Files.Length; i++)
                {
                    string directory = Path.GetDirectoryName(BackupGameFolder + Files[i]);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    SaveFile(cd.OpenFile(Files[i], FileMode.Open), BackupGameFolder + Files[i].Replace(";1", ""));
                }
            }

            //Extract Levels
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\ALASKA.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\ALOHA.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\ELYSIUM.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\GARI.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\MEGAPLE.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\MERQUER.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\MESA.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\PIPE.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\SNOW.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\SSXFE.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\TRICK.BIG", BackupGameFolder);
            BIG.Extract(BackupGameFolder + "\\DATA\\MODELS\\UNTRACK.BIG", BackupGameFolder);

            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\ALASKA.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\ALOHA.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\ELYSIUM.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\GARI.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\MEGAPLE.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\MERQUER.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\MESA.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\PIPE.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\SNOW.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\SSXFE.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\TRICK.BIG");
            File.Delete(BackupGameFolder + "\\DATA\\MODELS\\UNTRACK.BIG");

            //Extract to Active Game Folder
            CopyFilesRecursively(BackupGameFolder, GameFolder);
        }

        //SSX 3 US
        public void ExtractSSX3PS2()
        {
            //Extract to Backup
            //Extract Character Models

            //var TempboltPS2 = new BoltPS2Handler();
            //TempboltPS2.load(openFileDialog.FileName);
            //for (int i = 0; i < TempboltPS2.characters.Count; i++)
            //{
            //    var TempCharacter = TempboltPS2.characters[i];
            //    for (int a = 0; a < TempCharacter.entries.Count; a++)
            //    {
            //        var TempEntries = TempCharacter.entries[a];

            //        if (TempEntries.ModelPath != null)
            //        {
            //            if (TempEntries.ModelPath.ToLower().Contains(".big|"))
            //            {
            //                TempEntries.ModelPath = TempEntries.ModelPath.ToLower().Replace(".big|", "/");
            //            }
            //        }

            //        if (TempEntries.TexturePath != null)
            //        {
            //            if (TempEntries.TexturePath.ToLower().Contains(".big|"))
            //            {
            //                TempEntries.TexturePath = TempEntries.TexturePath.ToLower().Replace(".big|", "/");
            //            }
            //        }
            //        TempCharacter.entries[a] = TempEntries;
            //    }
            //    TempboltPS2.characters[i] = TempCharacter;
            //}

            //TempboltPS2.Save(openFileDialog.FileName);

            //Extract to Active Game Folder
        }

        //SSX On Tour US

        //SSX Blur US

        //SSX 2012


        public static void SaveFile(Stream Input, string FilePath)
        {
            MemoryStream memoryStream = new MemoryStream();

            Input.CopyTo(memoryStream, (int)Input.Length);

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            var file = File.Create(FilePath);
            memoryStream.Position = 0;
            memoryStream.CopyTo(file);
            memoryStream.Dispose();
            file.Close();

        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }
}
