namespace SSXModManagerWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConsoleSelection.SelectedIndex = 0;
            CheckAddedGames();
            GenerateMissingInfo();
            if(GameList.Count!=0)
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
        }
    }
}
