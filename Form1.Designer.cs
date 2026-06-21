namespace SSXModManagerWinForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button1 = new Button();
            button2 = new Button();
            ApplyModsButton = new Button();
            hScrollBar1 = new HScrollBar();
            ConsoleSelection = new ToolStripComboBox();
            GameSelection = new ToolStripComboBox();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripButton4 = new ToolStripButton();
            toolStrip1 = new ToolStrip();
            toolStripButton1 = new ToolStripButton();
            ModListCheck = new CheckedListBox();
            tabPage2 = new TabPage();
            tabPage1 = new TabPage();
            ModPicture = new PictureBox();
            DescriptionLabel = new Label();
            AuthorLabel = new Label();
            ModNameLabel = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            tabControl1 = new TabControl();
            tabPage3 = new TabPage();
            toolStrip1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ModPicture).BeginInit();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button1.Location = new Point(197, 572);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "\\/";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button2.Location = new Point(6, 572);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 3;
            button2.Text = "/\\";
            button2.UseVisualStyleBackColor = true;
            // 
            // ApplyModsButton
            // 
            ApplyModsButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ApplyModsButton.Location = new Point(1095, 572);
            ApplyModsButton.Name = "ApplyModsButton";
            ApplyModsButton.Size = new Size(135, 23);
            ApplyModsButton.TabIndex = 4;
            ApplyModsButton.Text = "Apply Mods";
            ApplyModsButton.UseVisualStyleBackColor = true;
            ApplyModsButton.Click += ApplyModsButton_Click;
            // 
            // hScrollBar1
            // 
            hScrollBar1.Location = new Point(0, 0);
            hScrollBar1.Name = "hScrollBar1";
            hScrollBar1.Size = new Size(80, 17);
            hScrollBar1.TabIndex = 0;
            // 
            // ConsoleSelection
            // 
            ConsoleSelection.Alignment = ToolStripItemAlignment.Right;
            ConsoleSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            ConsoleSelection.Items.AddRange(new object[] { "PS2" });
            ConsoleSelection.Name = "ConsoleSelection";
            ConsoleSelection.Size = new Size(121, 25);
            // 
            // GameSelection
            // 
            GameSelection.Alignment = ToolStripItemAlignment.Right;
            GameSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            GameSelection.Name = "GameSelection";
            GameSelection.Size = new Size(121, 25);
            GameSelection.SelectedIndexChanged += GameSelection_SelectedIndexChanged;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Alignment = ToolStripItemAlignment.Right;
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // toolStripButton4
            // 
            toolStripButton4.Alignment = ToolStripItemAlignment.Right;
            toolStripButton4.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButton4.Image = (Image)resources.GetObject("toolStripButton4.Image");
            toolStripButton4.ImageTransparentColor = Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new Size(84, 22);
            toolStripButton4.Text = "Launch Game";
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { ConsoleSelection, GameSelection, toolStripSeparator1, toolStripButton4, toolStripButton1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1268, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(67, 22);
            toolStripButton1.Text = "Add Game";
            toolStripButton1.Click += toolStripButton1_Click;
            // 
            // ModListCheck
            // 
            ModListCheck.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            ModListCheck.FormattingEnabled = true;
            ModListCheck.Location = new Point(6, 6);
            ModListCheck.Name = "ModListCheck";
            ModListCheck.Size = new Size(266, 562);
            ModListCheck.TabIndex = 1;
            ModListCheck.SelectedIndexChanged += ModListCheck_SelectedIndexChanged;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1236, 601);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Download Mods";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(ModPicture);
            tabPage1.Controls.Add(DescriptionLabel);
            tabPage1.Controls.Add(AuthorLabel);
            tabPage1.Controls.Add(ModNameLabel);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(ApplyModsButton);
            tabPage1.Controls.Add(ModListCheck);
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(button2);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1236, 601);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Mod List";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // ModPicture
            // 
            ModPicture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ModPicture.BackColor = Color.Black;
            ModPicture.Location = new Point(683, 6);
            ModPicture.Name = "ModPicture";
            ModPicture.Size = new Size(547, 547);
            ModPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            ModPicture.TabIndex = 5;
            ModPicture.TabStop = false;
            // 
            // DescriptionLabel
            // 
            DescriptionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            DescriptionLabel.Location = new Point(279, 94);
            DescriptionLabel.Name = "DescriptionLabel";
            DescriptionLabel.Size = new Size(398, 107);
            DescriptionLabel.TabIndex = 11;
            DescriptionLabel.Text = "None";
            // 
            // AuthorLabel
            // 
            AuthorLabel.AutoSize = true;
            AuthorLabel.Location = new Point(278, 59);
            AuthorLabel.Name = "AuthorLabel";
            AuthorLabel.Size = new Size(36, 15);
            AuthorLabel.TabIndex = 10;
            AuthorLabel.Text = "None";
            // 
            // ModNameLabel
            // 
            ModNameLabel.AutoSize = true;
            ModNameLabel.Location = new Point(279, 21);
            ModNameLabel.Name = "ModNameLabel";
            ModNameLabel.Size = new Size(36, 15);
            ModNameLabel.TabIndex = 9;
            ModNameLabel.Text = "None";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(278, 79);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 8;
            label3.Text = "Description";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(278, 44);
            label2.Name = "label2";
            label2.Size = new Size(44, 15);
            label2.TabIndex = 7;
            label2.Text = "Author";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(278, 6);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 6;
            label1.Text = "Name";
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(12, 28);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1244, 629);
            tabControl1.TabIndex = 5;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1236, 601);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Settings";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1268, 669);
            Controls.Add(tabControl1);
            Controls.Add(toolStrip1);
            Controls.Add(hScrollBar1);
            Name = "Form1";
            Text = "Mod Manager";
            Load += Form1_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ModPicture).EndInit();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private Button button2;
        private Button ApplyModsButton;
        private HScrollBar hScrollBar1;
        private ToolStripComboBox ConsoleSelection;
        private ToolStripComboBox GameSelection;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripButton4;
        private ToolStrip toolStrip1;
        private CheckedListBox ModListCheck;
        private TabPage tabPage2;
        private TabPage tabPage1;
        private TabControl tabControl1;
        private TabPage tabPage3;
        private Label label3;
        private Label label2;
        private Label label1;
        private PictureBox ModPicture;
        private Label DescriptionLabel;
        private Label AuthorLabel;
        private Label ModNameLabel;
        private ToolStripButton toolStripButton1;
    }
}
