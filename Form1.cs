using Microsoft.VisualBasic;
using SSXModManagerWinForm.ModSystem;
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
            CheckAddedGames();
            GenerateMissingInfo();
            if (GameList.Count != 0)
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

        List<string> GameList = new List<string>();
        int SelectedGame = 0;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Iso File (*.iso)|*.iso|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                GameSetup.CheckDisk(openFileDialog.FileName);
                CheckAddedGames();
            }
        }

        public void CheckAddedGames()
        {
            string GamesFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Game\\";

            if (!Directory.Exists(GamesFolders))
            {
                return;
            }
            var FullList = Directory.GetDirectories(GamesFolders);

            for (int i = 0; i < FullList.Length; i++)
            {
                GameList.Add(Path.GetFileName(FullList[i]));
                GameSelection.Items.Add(GameList[i]);
            }
        }

        private void GameSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedGame = GameSelection.SelectedIndex;

            ModList.LoadModFolder(AppDomain.CurrentDomain.BaseDirectory + "\\Mods\\SSX Tricky");

            ModListCheck.Items.Clear();

            for (int i = 0; i < ModList.modItems.Count; i++)
            {
                ModListCheck.Items.Add(ModList.modItems[i].Name, ModList.modItems[i].Enabled);
            }
        }

        private void ModListCheck_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ModListCheck.SelectedIndex != -1)
            {
                ModZipFolder.LoadMod(ModList.modItems[ModListCheck.SelectedIndex].Path);

                ModNameLabel.Text = ModZipFolder.modInfo.Name;
                AuthorLabel.Text = ModZipFolder.modInfo.Author;
                DescriptionLabel.Text = ModZipFolder.modInfo.Description;

                ModPicture.Image = ModZipFolder.image;
            }
            else
            {
                ModNameLabel.Text = "None";
                AuthorLabel.Text = "None";
                DescriptionLabel.Text = "None";

                ModPicture.Image = null;
            }
        }

        private void ApplyModsButton_Click(object sender, EventArgs e)
        {
            GameSetup.RestoreBackup("SSX Tricky");
            ModList.ApplyMods(AppDomain.CurrentDomain.BaseDirectory + "\\Game\\SSX Tricky");
            MessageBox.Show("Mods Applied");
        }

        private void PCSX2PathTextBox_TextChanged(object sender, EventArgs e)
        {
            AppSettings.PCSX2Path = PCSX2PathTextBox.Text;

            //Check for portable.ini
            string Directory = Path.GetDirectoryName(AppSettings.PCSX2Path);

            if(File.Exists(Directory + "\\portable.ini"))
            {
                AppSettings.PCSX2DataPath = Directory;
            }
            else
            {
                AppSettings.PCSX2DataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PCSX2";
            }

            AppSettings.CreateJson(SettingsPath);
        }

        private void LaunchGameButton_Click(object sender, EventArgs e)
        {
            Process.Start(AppSettings.PCSX2Path, "-elf \"G:\\Visual Studio Projects\\SSXModManagerWinForm\\bin\\Debug\\net10.0-windows\\Game\\SSX Tricky\\SLUS_203.26\"");
            //Process.Start(PCSX2Path, "-gameargs \"DebugMenu\" -- I:\\PS2\\SSX\\SSX Tricky\\SSX Tricky (NTSC).iso");
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

                ModListCheck.SelectedIndex = SelectedID-1;
                ModListCheck.SetItemChecked(ModListCheck.SelectedIndex, ModList.modItems[ModListCheck.SelectedIndex].Enabled);
            }
        }

        private void MoveModDown_Click(object sender, EventArgs e)
        {
            if (ModListCheck.SelectedIndex != -1 && ModListCheck.SelectedIndex < ModList.modItems.Count-1)
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
    }
}