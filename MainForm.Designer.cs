namespace MDRed
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            contentToolStripMenuItem = new ToolStripMenuItem();
            addFileToolStripMenuItem = new ToolStripMenuItem();
            deleteFileToolStripMenuItem = new ToolStripMenuItem();
            exportFileToolStripMenuItem = new ToolStripMenuItem();
            editFileContentsToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            panel1 = new Panel();
            label1 = new Label();
            label2 = new Label();
            comboBox1 = new ComboBox();
            dataGridView1 = new DataGridView();
            textBox1 = new TextBox();
            label3 = new Label();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, contentToolStripMenuItem, aboutToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1051, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, toolStripMenuItem2, saveToolStripMenuItem, saveAsToolStripMenuItem, exitToolStripMenuItem });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(37, 20);
            toolStripMenuItem1.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(180, 22);
            openToolStripMenuItem.Text = "New";
            openToolStripMenuItem.Click += newItemToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(180, 22);
            toolStripMenuItem2.Text = "Open...";
            toolStripMenuItem2.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(180, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(180, 22);
            saveAsToolStripMenuItem.Text = "Save As...";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(180, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // contentToolStripMenuItem
            // 
            contentToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addFileToolStripMenuItem, deleteFileToolStripMenuItem, exportFileToolStripMenuItem, editFileContentsToolStripMenuItem });
            contentToolStripMenuItem.Name = "contentToolStripMenuItem";
            contentToolStripMenuItem.Size = new Size(62, 20);
            contentToolStripMenuItem.Text = "Content";
            // 
            // addFileToolStripMenuItem
            // 
            addFileToolStripMenuItem.Name = "addFileToolStripMenuItem";
            addFileToolStripMenuItem.Size = new Size(164, 22);
            addFileToolStripMenuItem.Text = "Add File...";
            addFileToolStripMenuItem.Click += addFileToolStripMenuItem_Click;
            // 
            // deleteFileToolStripMenuItem
            // 
            deleteFileToolStripMenuItem.Name = "deleteFileToolStripMenuItem";
            deleteFileToolStripMenuItem.Size = new Size(164, 22);
            deleteFileToolStripMenuItem.Text = "Delete File";
            deleteFileToolStripMenuItem.Click += deleteFileToolStripMenuItem_Click;
            // 
            // exportFileToolStripMenuItem
            // 
            exportFileToolStripMenuItem.Name = "exportFileToolStripMenuItem";
            exportFileToolStripMenuItem.Size = new Size(164, 22);
            exportFileToolStripMenuItem.Text = "Export File...";
            // 
            // editFileContentsToolStripMenuItem
            // 
            editFileContentsToolStripMenuItem.Name = "editFileContentsToolStripMenuItem";
            editFileContentsToolStripMenuItem.Size = new Size(164, 22);
            editFileContentsToolStripMenuItem.Text = "Edit File contents";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(52, 20);
            aboutToolStripMenuItem.Text = "About";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Dock = DockStyle.Right;
            panel1.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 161);
            panel1.Location = new Point(613, 24);
            panel1.Name = "panel1";
            panel1.Size = new Size(438, 411);
            panel1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 32);
            label1.Name = "label1";
            label1.Size = new Size(104, 15);
            label1.TabIndex = 4;
            label1.Text = "Cartridge file list - ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(256, 32);
            label2.Name = "label2";
            label2.Size = new Size(76, 15);
            label2.TabIndex = 6;
            label2.Text = "File Contents";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "HEX", "ASCII" });
            comboBox1.Location = new Point(486, 21);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 50);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(238, 373);
            dataGridView1.TabIndex = 3;
            dataGridView1.MouseClick += dataGridView1_MouseClick;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(256, 50);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(351, 373);
            textBox1.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(111, 32);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 7;
            label3.Text = "label3";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1051, 435);
            Controls.Add(label3);
            Controls.Add(comboBox1);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Controls.Add(panel1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "MDRed Microdrive Editor";
            Load += MainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem contentToolStripMenuItem;
        private ToolStripMenuItem addFileToolStripMenuItem;
        private ToolStripMenuItem deleteFileToolStripMenuItem;
        private ToolStripMenuItem exportFileToolStripMenuItem;
        private ToolStripMenuItem editFileContentsToolStripMenuItem;
        private Panel panel1;
        private Label label1;
        private Label label2;
        private ComboBox comboBox1;
        private DataGridView dataGridView1 = new DataGridView();
        private TextBox textBox1;
        private Label label3;
    }
}
