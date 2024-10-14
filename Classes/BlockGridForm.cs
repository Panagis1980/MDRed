namespace MDRed.Classes
{
    internal class BlockGridForm : Form
    {
        private const int GridSize = 16;
        private const int TotalBlocks = 254;
        private byte[] blockArray = new byte[TotalBlocks];

        public BlockGridForm()
        {
            this.Text = "Block Grid";
            this.Size = new Size(400, 400);
            //this.Paint += new PaintEventHandler(DrawGrid);
            //
            //// Example: Set some bytes in the array for testing
            //blockArray[0] = 1;  // First block will be red
            //blockArray[10] = 1; // 11th block will be red
            //blockArray[253] = 1; // Last block will be red
        }

        public BlockGridForm(MDCartridge cartridge)
        {
            this.Text = "Block Grid";
            this.Size = new Size(498, 520);
            this.Paint += new PaintEventHandler(DrawGrid);

            // Example: Set some bytes in the array for testing

            foreach (Sector sect in cartridge.Sectors)
            {
                Sector tempSect = new Sector();
                tempSect = sect;
                if (BitConverter.ToUInt16(tempSect.RECLEN) == 0 && (tempSect.RECFLG & (1 << 1)) == 0)
                {
                    blockArray[tempSect.HDNUMB-1] = 0;
                }
                else
                {
                    blockArray[tempSect.HDNUMB-1] = 1;
                }
            }

            //blockArray[0] = 1;  // First block will be red
            //blockArray[10] = 1; // 11th block will be red
            //blockArray[253] = 1; // Last block will be red
        }

        private void DrawGrid(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int cellSize = 30;

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
                string blockNumber = (i+1).ToString();
                SizeF stringSize = g.MeasureString(blockNumber, this.Font);
                g.DrawString(blockNumber, this.Font, Brushes.Black,
                             col * cellSize + (cellSize - stringSize.Width) / 2,
                             row * cellSize + (cellSize - stringSize.Height) / 2);
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // BlockGridForm
            // 
            ClientSize = new Size(561, 526);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MaximumSize = new Size(577, 565);
            MinimumSize = new Size(577, 565);
            Name = "BlockGridForm";
            Text = "Cartridge Block Map";
            Load += BlockGridForm_Load;
            ResumeLayout(false);
        }

        private void BlockGridForm_Load(object sender, EventArgs e)
        {

        }
    }
}


