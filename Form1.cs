using SSXModManagerWinForm.ModSystem;

namespace SSXModManagerWinForm
{
    public partial class Form1 : Form
    {
        ModList ModList = new ModList();
        ModZipFolder ModZipFolder = new ModZipFolder();
        public Form1()
        {
            InitializeComponent();
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
            string Backup = AppDomain.CurrentDomain.BaseDirectory + "\\Backup\\";
            string GamesFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Game\\";
            string ModsFolders = AppDomain.CurrentDomain.BaseDirectory + "\\Mods\\";
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
    }
}
