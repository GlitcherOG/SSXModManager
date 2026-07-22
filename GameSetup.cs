using DiscUtils.Iso9660;
using SSX_Library;
using SSXLibrary.FileHandlers;
using SSXModManagerWinForm.Internal.Utilities;
using SSXModManagerWinForm.ModSystem;
using System.IO.Hashing;

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

            //if (crc == "085653F4")
            //{
            //    ExtractSSXOGPS2(LoadPath);
            //}
            if (crc == "8E7CFF62")
            {
                ExtractSSXTrickyPS2(LoadPath);
            }
            else if (crc == "8FFF00D")
            {
                ExtractSSX3PS2(LoadPath);
            }
            else
            {
                throw new Exception("Currently Unsupported Iso");
            }
            //if (crc == "0F27ED9B")
            //{
            //    ExtractSSXOnTourPS2(LoadPath);
            //}
        }

        //SSX OG US
        public static void ExtractSSXOGPS2(string LoadPath)
        {
            string Backup = AppDomain.CurrentDomain.BaseDirectory + "\\Backup\\";
            string BackupGameFolder = Backup + "\\SSX OG\\";
            string GamesFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Game\\";
            string GameFolder = GamesFolders + "\\SSX OG\\";
            string ModsFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Mods\\";
            string ModFolder = ModsFolders + "\\SSX OG\\";

            if (!Directory.Exists(BackupGameFolder))
            {
                Directory.CreateDirectory(BackupGameFolder);
            }
            if (!Directory.Exists(GamesFolders))
            {
                Directory.CreateDirectory(GamesFolders);
            }

            //Extract to Backup
            ExtractDisk(LoadPath, BackupGameFolder);

            //Write HostSF
            Console.WriteLine("Patching Elf File");
            string ElfPath = BackupGameFolder + "\\SLUS_203.26";
            using (Stream stream = File.Open(ElfPath, FileMode.Open))
            {

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
            Console.WriteLine("SSX Tricky PS2 Detected");
            Console.WriteLine("Generating Folders");
            string Backup = AppDomain.CurrentDomain.BaseDirectory + "\\Backup\\";
            string BackupGameFolder = Backup + "\\SSX Tricky\\";
            string GamesFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Game\\";
            string GameFolder = GamesFolders + "\\SSX Tricky\\";
            string ModsFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Mods\\";
            string ModFolder = ModsFolders + "\\SSX Tricky\\";

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
            Console.WriteLine("Starting Disk Extracting");
            ExtractDisk(LoadPath, BackupGameFolder);

            Console.WriteLine("Deleting Padding Files");
            if (File.Exists(BackupGameFolder + "\\PAD0.000"))
            {
                File.Delete(BackupGameFolder + "\\PAD0.000");
            }
            if (File.Exists(BackupGameFolder + "\\PAD1.000"))
            {
                File.Delete(BackupGameFolder + "\\PAD1.000");
            }

            //Write HostSF
            // The new REAL library version introduced here onwards
            // doesn't hardcode the length of the host0 string, and trying to hardcode
            // the length results in crashing.
            // So we admit defeat and just give it what it wants, to a point.

            //File offset is -FF000 from original SSX-ElfLdr code
            Console.WriteLine("Patching Elf File");
            string ElfPath = BackupGameFolder + "\\SLUS_203.26";
            using (Stream stream = File.Open(ElfPath, FileMode.Open))
            {
                stream.Position = 0x00387468 - 0xFF000;
                StreamUtil.WriteString(stream, "host0:", 8);

                stream.Position = 0x003b9130 - 0xFF000;
                StreamUtil.WriteString(stream, "host:", 8);

                stream.Position = 0x00387258 - 0xFF000;
                StreamUtil.WriteString(stream, "host:data/modules/ioprp224.img", 40);
                stream.Position = 0x003872b0 - 0xFF000;
                StreamUtil.WriteString(stream, "host:data/modules/sio2man.irx", 40);
                stream.Position = 0x003872f0 - 0xFF000;
                StreamUtil.WriteString(stream, "host:data/modules/padman.irx", 40);
                stream.Position = 0x00387330 - 0xFF000;
                StreamUtil.WriteString(stream, "host:data/modules/libsd.irx", 40);
                stream.Position = 0x00387370 - 0xFF000;
                StreamUtil.WriteString(stream, "host:data/modules/snddrv.irx", 40);
                stream.Position = 0x003873b0 - 0xFF000;
                StreamUtil.WriteString(stream, "host:data/modules/mcman.irx", 40);
                stream.Position = 0x003873f0 - 0xFF000;
                StreamUtil.WriteString(stream, "host:data/modules/mcserv.irx", 40);

                // BIGless worlds
                // You'll need bigfile's bigextract to extract the world archives,
                // since they're c0fb BIG archives.

                // It seems they got a little mad at the mound of paths and made paths composed
                // via sprintf(), so this is actually quite a bit easier to do than OG.
                stream.Position = 0x003a7bb8 - 0xFF000;
                StreamUtil.WriteString(stream, "data/models/%s%s", 24);

                // NOP world BIG file mounts, both for hardcoded SSXFE and the world's mounting
                stream.Position = 0x001862dc - 0xFF000;

                for (int i = 0; i < 4; i++)
                    StreamUtil.WriteInt32(stream, 0);

                stream.Position = 0x00263e1c - 0xFF000;
                StreamUtil.WriteInt32(stream, 0);
            }

            //Extract Levels
            Console.WriteLine("Extracting Level Files");
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

            Console.WriteLine("Deleting Redundent files");
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

            GameInfo gameInfo = new GameInfo();
            gameInfo.Game = "SSX Tricky";
            gameInfo.Version = "1";
            gameInfo.Console = "PS2";
            gameInfo.Elf = "SLUS_203.26";
            gameInfo.CreateJson(BackupGameFolder + "\\GameInfo.json");

            //Extract to Active Game Folder
            Console.WriteLine("Copying Game to Active Folder");
            CopyFilesRecursively(BackupGameFolder, GameFolder);
        }

        //SSX 3 US
        public static void ExtractSSX3PS2(string LoadPath)
        {
            Console.WriteLine("SSX 3 PS2 Detected");
            Console.WriteLine("Generating Folders and Clearing Old Files");
            string Backup = AppDomain.CurrentDomain.BaseDirectory + "\\Backup\\";
            string BackupGameFolder = Backup + "\\SSX 3\\";
            string GamesFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Game\\";
            string GameFolder = GamesFolders + "\\SSX 3\\";
            string ModsFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Mods\\";
            string ModFolder = ModsFolders + "\\SSX 3\\";

            if (!Directory.Exists(BackupGameFolder))
            {
                Directory.CreateDirectory(BackupGameFolder);
            }
            else
            {
                Directory.Delete(BackupGameFolder, true);
                Directory.CreateDirectory(BackupGameFolder);
            }
            if (!Directory.Exists(GameFolder))
            {
                Directory.CreateDirectory(GameFolder);
            }
            else
            {
                Directory.Delete(GameFolder, true);
                Directory.CreateDirectory(GameFolder);
            }
            if (!Directory.Exists(ModFolder))
            {
                Directory.CreateDirectory(ModFolder);
            }

            //Extract to Backup
            Console.WriteLine("Starting Disk Extracting");
            ExtractDisk(LoadPath, BackupGameFolder);

            Console.WriteLine("Deleting Padding Files");
            if (File.Exists(BackupGameFolder + "\\PAD0.000"))
            {
                File.Delete(BackupGameFolder + "\\PAD0.000");
            }
            if (File.Exists(BackupGameFolder + "\\PAD1.000"))
            {
                File.Delete(BackupGameFolder + "\\PAD1.000");
            }

            //HostSF
            Console.WriteLine("Patching Elf File");
            string ElfPath = BackupGameFolder + "\\SLUS_207.72";
            using (Stream stream = File.Open(ElfPath, FileMode.Open))
            {
                stream.Position = 0x004a3ed8 - 0xFF000;
                StreamUtil.WriteString(stream, "host0:", 8);
                stream.Position = 0x0048d9c8 - 0xFF000;
                StreamUtil.WriteString(stream, "host:", 8);

                stream.Position = 0x004a3ea0 - 0xFF000;
                StreamUtil.WriteString(stream, "", 8);

                stream.Position = 0x00495828 - 0xFF000;
                StreamUtil.WriteString(stream, "host:", 8);

                //Character Icon Paths
                stream.Position = 0x35B418;
                StreamUtil.WriteString(stream, "data/char/mobytxp/moby_icons.ssh", 40);
                StreamUtil.WriteString(stream, "data/char/kaoritxp/kaori_icons.ssh", 40);
                StreamUtil.WriteString(stream, "data/char/allegtxp/arielle_icons.ssh", 48);
                StreamUtil.WriteString(stream, "data/char/mactxp/mac_icons.ssh", 40);
                StreamUtil.WriteString(stream, "data/char/zoetxp/zoe_icons.ssh", 40);
                StreamUtil.WriteString(stream, "data/char/grifftxp/grommet_icons.ssh", 48);
                StreamUtil.WriteString(stream, "data/char/elisetxp/elise_icons.ssh", 40);
                StreamUtil.WriteString(stream, "data/char/natetxp/rocco_icons.ssh", 40);
                StreamUtil.WriteString(stream, "data/char/psymotxp/psymon_icons.ssh", 40);
                StreamUtil.WriteString(stream, "data/char/viggotxp/deiter_icons.ssh", 40);

                //Music Paths
                stream.Position = 0x384420;
                StreamUtil.WriteString(stream, "audio/music/", 16);
                StreamUtil.WriteString(stream, "audio/music/", 16);
            }

            //Correct Bolt File
            Console.WriteLine("Patching Bolt File");
            var TempboltPS2 = new BoltPS2Handler();
            TempboltPS2.load(BackupGameFolder+ "DATA\\CHAR\\BOLTPS2.DAT");
            for (int i = 0; i < TempboltPS2.characters.Count; i++)
            {
                var TempCharacter = TempboltPS2.characters[i];
                for (int a = 0; a < TempCharacter.entries.Count; a++)
                {
                    var TempEntries = TempCharacter.entries[a];

                    if (TempEntries.ModelPath != null)
                    {
                        if (TempEntries.ModelPath.ToLower().Contains(".big|"))
                        {
                            TempEntries.ModelPath = TempEntries.ModelPath.ToLower().Replace(".big|", "/");
                        }
                    }

                    if (TempEntries.TexturePath != null)
                    {
                        if (TempEntries.TexturePath.ToLower().Contains(".big|"))
                        {
                            TempEntries.TexturePath = TempEntries.TexturePath.ToLower().Replace(".big|", "/");
                        }
                    }
                    TempCharacter.entries[a] = TempEntries;
                }
                TempboltPS2.characters[i] = TempCharacter;
            }
            TempboltPS2.Save(BackupGameFolder + "DATA\\CHAR\\BOLTPS2.DAT");
            //Extract Character Textures
            Console.WriteLine("Extracting Character Textures and Models");
            BIG.Extract(BackupGameFolder + "DATA\\CHAR\\ALLEGTXP.BIG", BackupGameFolder + "DATA\\CHAR\\ALLEGTXP");
            BIG.Extract(BackupGameFolder + "DATA\\CHAR\\ELISETXP.BIG", BackupGameFolder + "DATA\\CHAR\\ELISETXP");
            BIG.Extract(BackupGameFolder + "DATA\\CHAR\\GRIFFTXP.BIG", BackupGameFolder + "DATA\\CHAR\\GRIFFTXP");
            BIG.Extract(BackupGameFolder + "DATA\\CHAR\\KAORITXP.BIG", BackupGameFolder + "DATA\\CHAR\\KAORITXP");
            BIG.Extract(BackupGameFolder + "DATA\\CHAR\\MACTXP.BIG", BackupGameFolder + "DATA\\CHAR\\MACTXP");
            BIG.Extract(BackupGameFolder + "DATA\\CHAR\\MOBYTXP.BIG", BackupGameFolder + "DATA\\CHAR\\MOBYTXP");
            BIG.Extract(BackupGameFolder + "DATA\\CHAR\\NATETXP.BIG", BackupGameFolder + "DATA\\CHAR\\NATETXP");
            BIG.Extract(BackupGameFolder + "DATA\\CHAR\\OTHERTXP.BIG", BackupGameFolder + "DATA\\CHAR\\OTHERTXP");
            BIG.Extract(BackupGameFolder + "DATA\\CHAR\\PSYMOTXP.BIG", BackupGameFolder + "DATA\\CHAR\\PSYMOTXP");
            BIG.Extract(BackupGameFolder + "DATA\\CHAR\\VIGGOTXP.BIG", BackupGameFolder + "DATA\\CHAR\\VIGGOTXP");
            BIG.Extract(BackupGameFolder + "DATA\\CHAR\\ZOETXP.BIG", BackupGameFolder + "DATA\\CHAR\\ZOETXP");
            //Extract Character Models
            BIG.Extract(BackupGameFolder + "DATA\\CHAR\\MDLPS2.BIG", BackupGameFolder + "DATA\\CHAR\\MDLPS2");

            File.Delete(BackupGameFolder + "DATA\\CHAR\\ALLEGTXP.BIG");
            File.Delete(BackupGameFolder + "DATA\\CHAR\\ELISETXP.BIG");
            File.Delete(BackupGameFolder + "DATA\\CHAR\\GRIFFTXP.BIG");
            File.Delete(BackupGameFolder + "DATA\\CHAR\\KAORITXP.BIG");
            File.Delete(BackupGameFolder + "DATA\\CHAR\\MACTXP.BIG");
            File.Delete(BackupGameFolder + "DATA\\CHAR\\MOBYTXP.BIG");
            File.Delete(BackupGameFolder + "DATA\\CHAR\\NATETXP.BIG");
            File.Delete(BackupGameFolder + "DATA\\CHAR\\OTHERTXP.BIG");
            File.Delete(BackupGameFolder + "DATA\\CHAR\\PSYMOTXP.BIG");
            File.Delete(BackupGameFolder + "DATA\\CHAR\\VIGGOTXP.BIG");
            File.Delete(BackupGameFolder + "DATA\\CHAR\\ZOETXP.BIG");
            File.Delete(BackupGameFolder + "DATA\\CHAR\\MDLPS2.BIG");

            //Extract Music Files
            Console.WriteLine("Extracting Music");
            if (!Directory.Exists(BackupGameFolder + "DATA\\AUDIO\\MUSIC"))
            {
                Directory.CreateDirectory(BackupGameFolder + "DATA\\AUDIO\\MUSIC");
            }
            BIG.Extract(BackupGameFolder + "DATA\\AUDIO\\MUSIC.BIG", BackupGameFolder + "DATA\\AUDIO\\MUSIC");
            BIG.Extract(BackupGameFolder + "DATA\\AUDIO\\MUSIC2.BIG", BackupGameFolder + "DATA\\AUDIO\\MUSIC");

            var Files = Directory.GetFiles(BackupGameFolder + "DATA\\AUDIO\\MUSIC", "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < Files.Length; i++)
            {
                File.Move(Files[i], BackupGameFolder + "DATA\\AUDIO\\MUSIC\\" + Path.GetFileName(Files[i]));
            }

            Directory.Delete(BackupGameFolder + "DATA\\AUDIO\\MUSIC\\DATA", true);

            //Fix Music Inf
            var MusicText = File.ReadAllText(BackupGameFolder + "DATA\\CONFIG\\MUSIC.INF");
            MusicText = MusicText.Replace("BASEPATH = \"|data\\audio\\\"", "BASEPATH = \"\"");
            File.WriteAllText(BackupGameFolder + "DATA\\CONFIG\\MUSIC.INF", MusicText);

            File.Delete(BackupGameFolder + "DATA\\AUDIO\\MUSIC.BIG");
            File.Delete(BackupGameFolder + "DATA\\AUDIO\\MUSIC2.BIG");

            //Extract Audio Files
            Console.WriteLine("Extracting Audio");
            BIG.Extract(BackupGameFolder + "DATA\\AUDIO\\AUDIO.BIG", BackupGameFolder);

            var BanksText = File.ReadAllText(BackupGameFolder + "DATA\\CONFIG\\BANKS.INF");
            BanksText = BanksText.Replace("BASEPATH = \"audio/audio.big|data\\audio\\\"", "BASEPATH = \"audio/\"");
            File.WriteAllText(BackupGameFolder + "DATA\\CONFIG\\BANKS.INF", BanksText);

            var CROWDText = File.ReadAllText(BackupGameFolder + "DATA\\CONFIG\\CROWD.INF");
            CROWDText = CROWDText.Replace("|data\\audio\\", "");
            File.WriteAllText(BackupGameFolder + "DATA\\CONFIG\\CROWD.INF", CROWDText);

            GameInfo gameInfo = new GameInfo();
            gameInfo.Game = "SSX 3";
            gameInfo.Version = "1";
            gameInfo.Console = "PS2";
            gameInfo.Elf = "SLUS_207.72";
            gameInfo.CreateJson(BackupGameFolder + "\\GameInfo.json");

            //Extract to Active Game Folder
            Console.WriteLine("Copying Game to Active Folder");
            CopyFilesRecursively(BackupGameFolder, GameFolder);
        }

        //SSX On Tour US
        public static void ExtractSSXOnTourPS2(string LoadPath)
        {
            string Backup = AppDomain.CurrentDomain.BaseDirectory + "\\Backup\\";
            string BackupGameFolder = Backup + "\\SSX On Tour\\";
            string GamesFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Game\\";
            string GameFolder = GamesFolders + "\\SSX On Tour\\";
            string ModsFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Mods\\";
            string ModFolder = ModsFolders + "\\SSX On Tour\\";

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
            ExtractDisk(LoadPath, BackupGameFolder);

            //Extract Character Models

            //Extract to Active Game Folder
            CopyFilesRecursively(BackupGameFolder, GameFolder);
        }

        //SSX 2012
        public static void ExtractDisk(string LoadPath, string ExtractFolder)
        {
            using (FileStream isoStream = File.Open(LoadPath, FileMode.Open))
            {
                CDReader cd = new CDReader(isoStream, true);
                string[] Files = cd.GetFiles("", "*.*", SearchOption.AllDirectories);

                for (int i = 0; i < Files.Length; i++)
                {
                    Console.WriteLine("Extracting File " + Files[i].Replace(";1", ""));
                    string directory = Path.GetDirectoryName(ExtractFolder + Files[i]);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    SaveFile(cd.OpenFile(Files[i], FileMode.Open), ExtractFolder + Files[i].Replace(";1", ""));
                }
            }
        }

        public static void RestoreBackup(string BackupFolder, string GameFolder)
        {
            //Basic Restore
            //Should swap with a system that checks for extra files and deletes them
            //Then checks hashs for the files and if any are different restores those files
            //Directory.Delete(GameFolder, true);
            Console.WriteLine("Restoring Backup Files... This may take some time...");
            SyncFolder(BackupFolder, GameFolder);

            //CopyFilesRecursively(BackupFolder, GameFolder);
        }

        public static void SyncFolder(string sourceFolder, string destinationFolder)
        {
            Directory.CreateDirectory(destinationFolder);

            foreach (string sourceFile in Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourceFolder, sourceFile);
                string destFile = Path.Combine(destinationFolder, relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);

                if (!File.Exists(destFile))
                {
                    File.Copy(sourceFile, destFile);
                    Console.WriteLine($"Copied: {relativePath}");
                    continue;
                }

                FileInfo srcInfo = new(sourceFile);
                FileInfo dstInfo = new(destFile);

                // Different size? Definitely different.
                if (srcInfo.Length != dstInfo.Length)
                {
                    File.Copy(sourceFile, destFile, true);
                    Console.WriteLine($"Replaced (size): {relativePath}");
                    continue;
                }

                if(srcInfo.LastWriteTimeUtc != dstInfo.LastWriteTimeUtc)
                {
                    File.Copy(sourceFile, destFile, true);
                    Console.WriteLine($"Replaced (time): {relativePath}");
                    continue;
                }
            }
        }

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
                Console.WriteLine("Creating Directory " + dirPath.Replace(sourcePath, ""));
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                Console.WriteLine("Copying File " + newPath.Replace(sourcePath, ""));
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }
}
