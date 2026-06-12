using DiscUtils.Iso9660;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSXModManagerWinForm
{
    public class GameSetup
    {
        //SSX OG US
        public static void ExtractSSXOGPS2(string LoadPath)
        {
            string Backup = AppDomain.CurrentDomain.BaseDirectory + "\\Backup\\";
            string BackupGameFolder = Backup + "\\SSXOG\\";
            string GamesFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Game\\";
            string GameFolder = GamesFolders + "\\SSXOG\\";

            if (!Directory.Exists(Backup))
            {
                Directory.CreateDirectory(Backup);
            }
            if (!Directory.Exists(BackupGameFolder))
            {
                Directory.CreateDirectory(BackupGameFolder);
            }
            if (!Directory.Exists(GamesFolders))
            {
                Directory.CreateDirectory(GamesFolders);
            }
            if (!Directory.Exists(GameFolder))
            {
                Directory.CreateDirectory(GameFolder);
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


            //Copy to Active Game Folder

        }

        //SSX Tricky US
        public static void ExtractSSXTrickyPS2(string LoadPath)
        {
            string Backup = AppDomain.CurrentDomain.BaseDirectory + "\\Backup\\";
            string BackupGameFolder = Backup + "\\SSXTricky\\";
            string GamesFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Game\\";
            string GameFolder = GamesFolders + "\\SSXTricky\\";

            if (!Directory.Exists(Backup))
            {
                Directory.CreateDirectory(Backup);
            }
            if (!Directory.Exists(BackupGameFolder))
            {
                Directory.CreateDirectory(BackupGameFolder);
            }
            if (!Directory.Exists(GamesFolders))
            {
                Directory.CreateDirectory(GamesFolders);
            }
            if (!Directory.Exists(GameFolder))
            {
                Directory.CreateDirectory(GameFolder);
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
            //Extract to Active Game Folder
        }

        //SSX 3 US
        public void ExtractSSX3PS2()
        {
            //Extract to Backup
            //Extract Character Models
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
    }
}
