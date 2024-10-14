using MDRed.Classes;
using System.Data;
using System.Text;

namespace MDRed
{
    public partial class MainForm : Form
    {
        private const int GridSize = 16;
        private const int TotalBlocks = 254;
        private MDCartridge cartridge;
        private string FilePath = "";
        private DataTable files = new DataTable();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cartridge = new MDCartridge("Empty".PadRight(10));
            //cartridge.MDCartridgeOpen("Test.mdr");

            DataGridReDraw();
            panel1.Paint += new PaintEventHandler(DrawGrid);
            comboBox1.SelectedIndex = 0;

            //foreach (MDFile file in cartridge.FileList)
            //{
            //    dataGridView1.Rows.Add(file);
            //}
        }

        private void DataGridReDraw()
        {
            files = new DataTable();
            files.Columns.Add("Filename", typeof(string));
            files.Columns.Add("Size", typeof(int));

            for (int i = 0; i < cartridge.FileList.Count; i++)
            {
                MDFile file = new MDFile();
                file = cartridge.FileList[i];
                files.Rows.Add(file.FileName, file.FileSize);
            }

            dataGridView1.DataSource = files;
            dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            panel1.Refresh();
            dataGridView1.Refresh();
        }

        private void DrawGrid(object sender, PaintEventArgs e)
        {

            byte[] blockArray = new byte[TotalBlocks];
            Graphics g = e.Graphics;
            int cellSize = 25;

            for (int i = 0; i < 254; i++)
            {
                Sector tempSect = new Sector();
                tempSect = cartridge.Sectors[i];
                if (BitConverter.ToUInt16(tempSect.RECLEN) == 0 && (tempSect.RECFLG & (1 << 1)) == 0)
                {
                    blockArray[tempSect.HDNUMB - 1] = 0;
                }
                else
                {
                    blockArray[tempSect.HDNUMB - 1] = 1;
                }
            }

            for (int i = 0; i < TotalBlocks; i++)
            {
                int row = i / GridSize;
                int col = i % GridSize;

                // Calculate the rectangle for each cell
                Rectangle cellRect = new Rectangle(col * cellSize, row * cellSize, cellSize, cellSize);

                // If blockArray[i] is set (non-zero), fill the cell with red
                if (blockArray[i] != 0)
                {
                    g.FillRectangle(Brushes.Red, cellRect);
                }
                else
                {
                    g.FillRectangle(Brushes.White, cellRect);
                }

                // Draw the cell border
                g.DrawRectangle(Pens.Black, cellRect);

                // Draw the block number in the center of the cell
                string blockNumber = (i + 1).ToString();
                SizeF stringSize = g.MeasureString(blockNumber, this.Font);
                g.DrawString(blockNumber, this.Font, Brushes.Black,
                             col * cellSize + (cellSize - stringSize.Width) / 2,
                             row * cellSize + (cellSize - stringSize.Height) / 2);
            }
        }

        private MDFile GetSelectedFile()
        {
            int selectedRow = dataGridView1.CurrentCell.RowIndex;
            //MessageBox.Show(selectedRow.ToString());
            MDFile workingFile = new MDFile();
            workingFile = cartridge.FileList[selectedRow];
            return workingFile;
        }

        private string ByteArrayToASCIIString(byte[] ba)
        {
            return System.Text.Encoding.ASCII.GetString(ba);
        }

        private byte[] HexStringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            int count = 0;
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
                if (count % 1 == 0) hex.Append(" ");
                if (count % 16 == 0) hex.Append("\n");
                count++;
            }
            return hex.ToString();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (cartridge.FileList.Count > 0)
            {
                //MessageBox.Show(selectedRow.ToString());
                MDFile workingFile = GetSelectedFile();
                byte[] fileContents = new byte[workingFile.FileSize];
                int offset = 0;
                for (int i = 0; i < workingFile.SectorList.Length; i++)
                {
                    Sector sect = new Sector();
                    sect = cartridge.Sectors[workingFile.SectorList[i]];
                    Array.Copy(sect.DataBlock, 0, fileContents, i == 0 ? 0 : offset, BitConverter.ToUInt16(sect.RECLEN));
                    offset += BitConverter.ToUInt16(sect.RECLEN);
                }

                textBox1.Text = comboBox1.Text.Equals("HEX") ? ByteArrayToHexString(fileContents) : ByteArrayToASCIIString(fileContents);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                int selectedRow = dataGridView1.CurrentCell.RowIndex;
                //MessageBox.Show(selectedRow.ToString());
                MDFile workingFile = new MDFile();
                workingFile = cartridge.FileList[selectedRow];
                byte[] fileContents = new byte[workingFile.FileSize];
                int offset = 0;
                for (int i = 0; i < workingFile.SectorList.Length; i++)
                {
                    Sector sect = new Sector();
                    sect = cartridge.Sectors[workingFile.SectorList[i]];
                    Array.Copy(sect.DataBlock, 0, fileContents, i == 0 ? 0 : offset, BitConverter.ToUInt16(sect.RECLEN));
                    offset += BitConverter.ToUInt16(sect.RECLEN);
                }
                textBox1.Text = comboBox1.Text.Equals("HEX") ? ByteArrayToHexString(fileContents) : ByteArrayToASCIIString(fileContents);
            }

        }

        private void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRow = dataGridView1.CurrentCell.RowIndex;
            this.cartridge.MDCartridgeDeleteFile(selectedRow);
            DataGridReDraw();
        }

        private void newItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cartridge = new MDCartridge("Empty".PadRight(10));
            DataGridReDraw();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Declare the File Dialog
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Microdrive files (*.mdr)|*.mdr";
            string path = "";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path = ofd.FileName;
            }

            cartridge.MDCartridgeOpen(path);
            FilePath = ofd.FileName;
            label3.Text = cartridge.HDNAME;
            DataGridReDraw();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FilePath.Equals(""))
            {
                //Declare the File Dialog
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Microdrive files (*.mdr)|*.mdr";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    FilePath = sfd.FileName;
                }
            }
            cartridge.MDCartridgeSave(FilePath);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Declare the File Dialog
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Microdrive files (*.mdr)|*.mdr";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FilePath = sfd.FileName;
            }

            cartridge.MDCartridgeSave(FilePath);
            label3.Text = cartridge.HDNAME;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Declare the File Dialog
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = "Microdrive files (*.mdr)|*.mdr";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                cartridge.MDCartridgeAddFile(ofd.FileName);
                DataGridReDraw();
            }
        }

    }
}
