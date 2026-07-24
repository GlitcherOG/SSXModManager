using Microsoft.VisualBasic;
using SSXModManagerWinForm.ModSystem;
using SSXModManagerWinForm.Utilities;
using System.Diagnostics;

namespace SSXModManagerWinForm
{
    public partial class Form1 : Form
    {
        ModList ModList = new ModList();
        ModZipFolder ModZipFolder = new ModZipFolder();
        string Backup = AppDomain.CurrentDomain.BaseDirectory + "Backup\\";
        string GamesFolders = AppDomain.CurrentDomain.BaseDirectory + "Game\\";
        string ModsFolders = AppDomain.CurrentDomain.BaseDirectory + "Mods\\";
        public List<GameInfo> gameInfos = new List<GameInfo>();
        Settings AppSettings = new Settings();
        string SettingsPath = "";

        public Form1()
        {
            InitializeComponent();

            SettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SSXModManager\\Settings.json";
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SSXModManager"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SSXModManager");
            }
            AppSettings = Settings.LoadJsonPath(SettingsPath);
            PCSX2PathTextBox.Text = AppSettings.PCSX2Path;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConsoleSelection.SelectedIndex = 0;
            GenerateMissingInfo();
            CheckAddedGames();
            if (gameInfos.Count != 0)
            {
                GameSelection.SelectedIndex = 0;
            }
        }

        private void GenerateMissingInfo()
        {
            if (!Directory.Exists(Backup))
            {
                Directory.CreateDirectory(Backup);
            }
            if (!Directory.Exists(GamesFolders))
            {
                Directory.CreateDirectory(GamesFolders);
            }
            if (!Directory.Exists(ModsFolders))
            {
                Directory.CreateDirectory(ModsFolders);
            }
        }

        int SelectedGame = 0;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Disk File (*.iso, *.bin)|*.iso;*.bin|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ConsoleWindow.GenerateConsole();
                if (openFileDialog.FileName.Contains(".iso"))
                {
                    try
                    {
                        GameSetup.CheckDisk(openFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else if (openFileDialog.FileName.Contains(".bin"))
                {
                    try
                    {
                        PS2CDReading.Extract(openFileDialog.FileName, AppDomain.CurrentDomain.BaseDirectory +"\\TempDisk.iso");
                        GameSetup.CheckDisk(AppDomain.CurrentDomain.BaseDirectory + "\\TempDisk.iso");
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\TempDisk.iso");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    try
                    {
                        GameSetup.CheckElf(openFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                CheckAddedGames();
                ConsoleWindow.CloseConsole();
                if (gameInfos.Count != 0)
                {
                    GameSelection.SelectedIndex = 0;
                }
            }
        }

        public void CheckAddedGames()
        {
            if (!Directory.Exists(GamesFolders))
            {
                return;
            }
            var FullList = Directory.GetDirectories(GamesFolders);
            gameInfos = new List<GameInfo>();
            GameSelection.Items.Clear();
            for (int i = 0; i < FullList.Length; i++)
            {
                var TempInfo = GameInfo.LoadJsonPath(FullList[i] + "\\GameInfo.json");
                if (TempInfo.Game != "")
                {
                    TempInfo.GameDirectory = GamesFolders + Path.GetFileName(FullList[i]);
                    TempInfo.BackupDirectory = Backup + Path.GetFileName(FullList[i]);
                    TempInfo.ModDirectory = ModsFolders + Path.GetFileName(FullList[i]);
                    gameInfos.Add(TempInfo);
                    GameSelection.Items.Add(TempInfo.Game);
                }
            }
        }

        private void GameSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedGame = GameSelection.SelectedIndex;

            ModList.LoadModFolder(gameInfos[SelectedGame].ModDirectory);

            ModList.CheckModList(gameInfos[SelectedGame].GameDirectory + "\\ModList.txt");

            ModListCheck.Items.Clear();

            for (int i = 0; i < ModList.modItems.Count; i++)
            {
                if (!ModList.modItems[i].Folder)
                {
                    ModListCheck.Items.Add(ModList.modItems[i].Name, ModList.modItems[i].Enabled);
                }
                else
                {
                    ModListCheck.Items.Add("*" + ModList.modItems[i].Name, ModList.modItems[i].Enabled);
                }
            }
        }

        private void ModListCheck_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ModListCheck.SelectedIndex != -1)
            {
                try
                {
                    ModZipFolder.LoadMod(ModList.modItems[ModListCheck.SelectedIndex].Path);

                    ModNameLabel.Text = ModZipFolder.modInfo.Name;
                    AuthorLabel.Text = ModZipFolder.modInfo.Author;
                    DescriptionLabel.Text = ModZipFolder.modInfo.Description;
                    VersionText.Text = ModZipFolder.modInfo.Version;

                    ModPicture.Image = ModZipFolder.image;
                }
                catch
                {
                    ModNameLabel.Text = "Error";
                    AuthorLabel.Text = "None";
                    DescriptionLabel.Text = "Unknown Error Loading Mod";
                    VersionText.Text = "None";

                    ModPicture.Image = null;
                }
            }
            else
            {
                ModNameLabel.Text = "None";
                AuthorLabel.Text = "None";
                DescriptionLabel.Text = "None";
                VersionText.Text = "None";

                ModPicture.Image = null;
            }
        }

        private void ApplyModsButton_Click(object sender, EventArgs e)
        {
            if (GameSelection.SelectedIndex != -1)
            {
                ConsoleWindow.GenerateConsole();
                GameSetup.RestoreBackup(gameInfos[SelectedGame].BackupDirectory, gameInfos[SelectedGame].GameDirectory);
                ModList.ApplyMods(gameInfos[SelectedGame].GameDirectory, AppSettings.PCSX2DataPath);

                ConsoleWindow.CloseConsole();
                MessageBox.Show("Mods Applied");
            }
        }

        private void PCSX2PathTextBox_TextChanged(object sender, EventArgs e)
        {
            AppSettings.PCSX2Path = PCSX2PathTextBox.Text;

            //Check for portable.ini
            string DirectoryFolder = Path.GetDirectoryName(AppSettings.PCSX2Path);

            if (File.Exists(DirectoryFolder + "\\portable.ini"))
            {
                AppSettings.PCSX2DataPath = DirectoryFolder;
            }
            else
            {
                if (Directory.Exists(DirectoryFolder))
                {
                    AppSettings.PCSX2DataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PCSX2";
                }
            }

            PCSX2DataPathTextBox.Text = AppSettings.PCSX2DataPath;

            AppSettings.CreateJson(SettingsPath);
        }

        private void LaunchGameButton_Click(object sender, EventArgs e)
        {
            if (GameSelection.SelectedIndex != -1)
            {
                if (File.Exists(AppSettings.PCSX2Path))
                {
                    //Apply Any Per Game Fixes
                    string CRC = CRCCalculator.CalculateCRC32(gameInfos[SelectedGame].GameDirectory + "\\" + gameInfos[SelectedGame].Elf);
                    if (!File.Exists(AppSettings.PCSX2DataPath + "\\gamesettings\\" + CRC + ".ini"))
                    {
                        PerGameSettings.GenerateStandardSettings(AppSettings.PCSX2DataPath + "\\gamesettings\\" + CRC + ".ini");
                    }

                    Process.Start(AppSettings.PCSX2Path, "-elf \"" + gameInfos[SelectedGame].GameDirectory + "\\" + gameInfos[SelectedGame].Elf + "\"");
                }
                else
                {
                    MessageBox.Show("Missing Emulator");
                }
            }
            else
            {
                MessageBox.Show("Missing Game");
            }
        }

        private void ModListCheck_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ModListCheck.SelectedIndex != -1)
            {
                var ModelItem = ModList.modItems[ModListCheck.SelectedIndex];
                ModelItem.Enabled = e.CurrentValue != CheckState.Checked;
                ModList.modItems[ModListCheck.SelectedIndex] = ModelItem;
            }
        }

        private void MoveModUp_Click(object sender, EventArgs e)
        {
            if (ModListCheck.SelectedIndex > 0)
            {
                int SelectedID = ModListCheck.SelectedIndex;

                ModList.modItems.Insert(ModListCheck.SelectedIndex - 1, ModList.modItems[ModListCheck.SelectedIndex]);
                ModList.modItems.RemoveAt(ModListCheck.SelectedIndex + 1);

                ModListCheck.Items.Insert(ModListCheck.SelectedIndex - 1, ModListCheck.Items[ModListCheck.SelectedIndex]);
                ModListCheck.Items.RemoveAt(ModListCheck.SelectedIndex);

                ModListCheck.SelectedIndex = SelectedID - 1;
                ModListCheck.SetItemChecked(ModListCheck.SelectedIndex, ModList.modItems[ModListCheck.SelectedIndex].Enabled);
            }
        }

        private void MoveModDown_Click(object sender, EventArgs e)
        {
            if (ModListCheck.SelectedIndex != -1 && ModListCheck.SelectedIndex < ModList.modItems.Count - 1)
            {
                int SelectedID = ModListCheck.SelectedIndex;

                ModList.modItems.Insert(ModListCheck.SelectedIndex + 2, ModList.modItems[ModListCheck.SelectedIndex]);
                ModList.modItems.RemoveAt(ModListCheck.SelectedIndex);

                ModListCheck.Items.Insert(ModListCheck.SelectedIndex + 2, ModListCheck.Items[ModListCheck.SelectedIndex]);
                ModListCheck.Items.RemoveAt(ModListCheck.SelectedIndex);

                ModListCheck.SelectedIndex = SelectedID + 1;
                ModListCheck.SetItemChecked(ModListCheck.SelectedIndex, ModList.modItems[ModListCheck.SelectedIndex].Enabled);
            }
        }

        private void PCSX2DataPathTextBox_TextChanged(object sender, EventArgs e)
        {
            AppSettings.PCSX2DataPath = PCSX2DataPathTextBox.Text;
        }

        private void PCSX2exepathload_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PCSX2 EXE (*.exe)|*.exe|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                AppSettings.PCSX2Path = openFileDialog.FileName;
                PCSX2PathTextBox.Text = AppSettings.PCSX2Path;
            }
        }

        private void RefreshModsButton_Click(object sender, EventArgs e)
        {
            if(GameSelection.SelectedIndex!=-1)
            {
                ModList.LoadModFolder(gameInfos[SelectedGame].ModDirectory);

                ModList.CheckModList(gameInfos[SelectedGame].GameDirectory + "\\ModList.txt");

                ModListCheck.Items.Clear();

                for (int i = 0; i < ModList.modItems.Count; i++)
                {
                    if (!ModList.modItems[i].Folder)
                    {
                        ModListCheck.Items.Add(ModList.modItems[i].Name, ModList.modItems[i].Enabled);
                    }
                    else
                    {
                        ModListCheck.Items.Add("*" + ModList.modItems[i].Name, ModList.modItems[i].Enabled);
                    }
                }
            }
        }
    }
}