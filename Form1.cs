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
            GameSetup.ExtractSSXTrickyPS2("I:\\PS2\\SSX\\SSX Tricky\\SSX Tricky (NTSC).iso");
        }
    }
}
