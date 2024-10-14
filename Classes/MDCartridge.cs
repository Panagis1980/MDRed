using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace MDRed.Classes
{
    public class MDCartridge
    {
        public string HDNAME { get; set; }  // Microdrive cartridge name (10 characters, blank padded)
        public List<Sector> Sectors { get; set; } // = new List<Sector>(254);
        public byte WriteProtection { get; set; }
        public List<MDFile> FileList { get; set; }

        public int DataSize = 0;
        private bool SuppressChkError = false;

        public MDCartridge(string hDNAME)
        {
            HDNAME = hDNAME;
            Sectors = new List<Sector>();
            byte[] dBlock = new byte[512];
            for (int i = 0; i < 512; i++)
            {
                dBlock[i] = 0xfc;
            }

            for (byte i = 254; i > 0; i--)
            {
                Sector sector = new Sector(1, i, HDNAME, 0, 0, "", dBlock);
                Sectors.Add(sector);
            }

            WriteProtection = 0x00;
            FileList = new List<MDFile>(0);
        }

        public MDCartridge()
        {

        }

        public void MDCartridgeSave(string filePath)
        {
            byte[] buffer = new byte[137923];
            int sectCount = 0;
            foreach (Sector sect in this.Sectors)
            {

                byte[] tempBuffer = new byte[543];
                tempBuffer[0] = sect.HDFLAG;
                tempBuffer[1] = sect.HDNUMB;
                sect.Unused.CopyTo(tempBuffer, 2);
                Encoding.ASCII.GetBytes(sect.HDNAME).CopyTo(tempBuffer, 4);
                tempBuffer[14] = sect.HDCHK;
                tempBuffer[15] = sect.RECFLG;
                tempBuffer[16] = sect.RECNUM;

                //RECLEN is maintained in Big Endian (swapped bytes, Least Significant Byte first)
                sect.RECLEN.CopyTo(tempBuffer, 17);

                Encoding.ASCII.GetBytes(sect.RECNAM).CopyTo(tempBuffer, 19);
                tempBuffer[29] = sect.DESCHK;
                sect.DataBlock.CopyTo(tempBuffer, 30);
                tempBuffer[542] = sect.DCHK;

                tempBuffer.CopyTo(buffer, sectCount * 543);
                sectCount++;
            }
            buffer[137922] = this.WriteProtection;

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        // Write some data to the file
                        bw.Write(buffer); // an integer
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        public void MDCartridgeOpen(string filePath)
        {
            MDCartridge tempCart = new MDCartridge("Empty");

            if (!File.Exists(filePath))
            {
                throw new Exception("File does not exist!");
            }
            byte[] buffer = new byte[137923];

            try
            {
                buffer = File.ReadAllBytes(filePath);
                if (buffer.Length != 137923)
                {
                    MessageBox.Show("File is not a correct MDR file!");
                    return;
                }

            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

            tempCart.WriteProtection = buffer[137922];
            byte[] block = new byte[543];
            int i = 0;

            foreach (Sector sect in tempCart.Sectors)
            {
                Array.Clear(sect.DataBlock);
                sect.DataBlock = new byte[512];

                Array.Copy(buffer, i * 543, block, 0, 543);

                // HDFLAG Value 1, to indicate header block 
                sect.HDFLAG = block[0];
                // HDNUMB  sector number (values 254 down to 1)
                sect.HDNUMB = block[1];

                // not used (and of undetermined value)
                Array.Copy(block, 2, sect.Unused, 0, 2);

                // HDNAME   microdrive cartridge name (blank padded)
                byte[] hdname = new byte[10];
                Array.Copy(block, 4, hdname, 0, 10);
                sect.HDNAME = System.Text.Encoding.Default.GetString(hdname);

                // make it also the MD name not only in sector
                if (i == 0) this.HDNAME = sect.HDNAME;

                // HDCHK    header checksum (of first 14 bytes)
                sect.HDCHK = block[14];
                if (sect.HDCHK != sect.CalculateHeaderChecksum() && !SuppressChkError)
                {
                    DialogResult dr = MessageBox.Show("File is not passing Header checksum test!\nDo you want to suppress Checksum warnings? ","Question",MessageBoxButtons.YesNoCancel);
                    if (dr == DialogResult.Yes)
                    {
                        SuppressChkError = true;
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        // Do nothing
                    }
                    
                }

                //RECFLG - bit 0: always 0 to indicate record block
                //      -bit 1: set for the EOF block
                //      - bit 2: reset for a PRINT file
                //      - bits 3 - 7: not used(value 0)
                sect.RECFLG = block[15];

                // RECNUM data block sequence number(value starts at 0)
                sect.RECNUM = block[16];

                // RECLEN   data block length (<=512, LSB first)
                sect.RECLEN = new byte[2];
                Array.Copy(block, 17, sect.RECLEN, 0, 2);

                //RECNAM   filename (blank padded)
                byte[] blkname = new byte[10];
                Array.Copy(block, 19, blkname, 0, 10);
                sect.RECNAM = System.Text.Encoding.Default.GetString(blkname);

                // DESCHK record descriptor checksum(of previous 14 bytes)
                sect.DESCHK = block[29];
                byte chk = sect.CalculateDescriptorChecksum();
                if (sect.DESCHK != sect.CalculateDescriptorChecksum() && !SuppressChkError)
                {
                    DialogResult dr = MessageBox.Show("File is not passing Descriptor checksum test!\nDo you want to suppress Checksum warnings? ", "Question", MessageBoxButtons.YesNoCancel);
                    if (dr == DialogResult.Yes)
                    {
                        SuppressChkError = true;
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        // Do nothing
                    }
                }

                // DATABLOCK
                Array.Copy(block, 30, sect.DataBlock, 0, 512);

                // DCHK data block checksum (of all 512 bytes of data block, even when not all bytes are used)
                sect.DCHK = block[542];
                if (sect.DCHK != sect.CalculateDataBlockChecksum() && !SuppressChkError)
                {
                    DialogResult dr = MessageBox.Show("File is not passing DataBlock checksum test!\nDo you want to suppress Checksum warnings? ", "Question", MessageBoxButtons.YesNoCancel);
                    if (dr == DialogResult.Yes)
                    {
                        SuppressChkError = true;
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        // Do nothing
                    }
                }

                i++;
            }

            tempCart.PopulateFileList();
            this.FileList = tempCart.FileList;
            this.Sectors = tempCart.Sectors;
            this.HDNAME = tempCart.HDNAME;
            this.WriteProtection = tempCart.WriteProtection;
        }

        public void PopulateFileList()
        {
            FileList = new List<MDFile>();
            DataSize = 0;
            for (int i = 0; i < 254; i++)
            {
                if ((Sectors[i].RECFLG & (1 << 1)) == 2) // it is an EOF used record block
                {
                    MDFile mdFile = new MDFile(Sectors[i].RECNAM);
                    mdFile.SectorList = new int[(int)Sectors[i].RECNUM + 1];
                    mdFile.SectorList[((int)Sectors[i].RECNUM)] = i;
                    mdFile.FileSize = (((int)Sectors[i].RECNUM) * 512) + BitConverter.ToUInt16(Sectors[i].RECLEN);
                    FileList.Add(mdFile);
                    this.DataSize += mdFile.FileSize;
                }
            }

            for (int k = 0; k < FileList.Count; k++)
            {
                MDFile mdFile = FileList[k];
                for (int i = 0; i < 254; i++)
                {
                    if (Sectors[i].RECNAM == mdFile.FileName && Sectors[i].RECNUM != mdFile.SectorList[mdFile.SectorList.Length - 1])
                    {
                        mdFile.SectorList[((int)Sectors[i].RECNUM)] = i;
                        //if (mdFile.SectorList[((int)Sectors[i].RECNUM)] == mdFile.SectorList.Length - 1) break;
                    }
                }
            }


        }

        public void MDFileExport(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            foreach (MDFile mdFile in FileList)
            {
                if (mdFile.FileName.Equals(fileName.PadRight(10)))
                {
                    MDFileExport(mdFile, filePath);
                }
            }
        }

        public void MDFileExport(MDFile mdFile, string filePath)
        {

            byte[] buffer = new byte[mdFile.FileSize];

            for (int i = 0; i < mdFile.SectorList.Length; i++)
            {
                Sector sect = new Sector();
                sect = Sectors[mdFile.SectorList[i]];
                //MessageBox.Show(BitConverter.ToUInt16(sect.RECLEN).ToString());
                System.Buffer.BlockCopy(sect.DataBlock, 0, buffer, i * 512, BitConverter.ToUInt16(sect.RECLEN));
            }

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        // Write some data to the file
                        bw.Write(buffer); // an integer
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private int GetAvailableSectors()
        {
            //An empty record block has a zero in bit 1 of RECFLG and also RECLEN = 0.
            int freeSectors = 0;
            foreach (Sector sect in Sectors)
            {
                byte[] tempArray = sect.RECLEN;
                //Array.Reverse(tempArray);
                if (BitConverter.ToUInt16(tempArray) == 0 && (sect.RECFLG & (1 << 1)) == 0) {
                    freeSectors++;
                }
            }
            return freeSectors;
        }

        public void MDCartridgeAddFile(string filePath)
        {
            byte[] fileBytes = new byte[130048]; //File cannot be larger than the maximum available space of MD
            // during File read it will bomb and stop the process
            try
            {
                fileBytes = File.ReadAllBytes(filePath);
                this.DataSize += fileBytes.Length;
                if ((double)(fileBytes.Length / 512) <= (double)GetAvailableSectors()) //if there are avaialbe sectors to fit the file
                {
                    int sectorsNeeded = fileBytes.Length % 512 > 0 ? (fileBytes.Length / 512) + 1 : (fileBytes.Length / 512);
                    int i = 1;
                    while (i <= sectorsNeeded) {
                        foreach (Sector sect in Sectors)
                        {
                            if (BitConverter.ToUInt16(sect.RECLEN) == 0 && (sect.RECFLG & (1 << 1)) == 0) //and sector is free
                            {
                                sect.RECFLG = (byte)(i == sectorsNeeded ? 2 : 0);
                                sect.DataBlock = new byte[512];

                                Array.Copy(fileBytes, (i - 1) * 512, sect.DataBlock, 0,
                                    i == sectorsNeeded ? fileBytes.Length % 512 : 512);

                                //RECLEN is maintained in Big Endian (swapped bytes, Least Significant Byte first)

                                sect.RECLEN = BitConverter.GetBytes((ushort)(i == sectorsNeeded ? fileBytes.Length % 512 : 512));

                                if (Path.GetFileName(filePath).Length <= 10)
                                {
                                    sect.RECNAM = Path.GetFileName(filePath).PadRight(10);
                                }
                                sect.RECNUM = (byte)(i - 1);
                                sect.DESCHK = sect.CalculateDescriptorChecksum();
                                sect.DCHK = sect.CalculateDataBlockChecksum();
                                i++;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("File to big for this Microdrive!");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            PopulateFileList();
        }

        public void MDCartridgeDeleteFile(int fileIndex)
        {
            if (fileIndex >= FileList.Count)
            {
                MessageBox.Show("Error. File index does not exist.");
            }
            DialogResult dr = MessageBox.Show("You are about to delete file " + FileList[fileIndex].FileName, "Question", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                // Do your task
                for (int i = 0; i < FileList[fileIndex].SectorList.Length; i++)
                {
                    byte[] dBlock = new byte[512];
                    for (int k = 0; k < 512; k++)
                    {
                        dBlock[k] = 0xfc;
                    }             

                    byte hdnumb = Sectors[FileList[fileIndex].SectorList[i]].HDNUMB;
                    Sectors[FileList[fileIndex].SectorList[i]] = new Sector(1, hdnumb, HDNAME, 0, 0, "", dBlock);
                }
                FileList.RemoveAt(fileIndex);
            }
            else
            {
                // Exit
            }
        }

        public void MDCartridgeRenameFile(int fileIndex, string newFilename)
        {
            if (fileIndex >= FileList.Count)
            {
                MessageBox.Show("Error. File index does not exist.");
            }
            if (newFilename.Length >= 10)
            {
                MessageBox.Show("Error. Invalid filename size");
            }

            DialogResult dr = MessageBox.Show("You are about to rename file " + FileList[fileIndex].FileName, "Question", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                // Do your task
                for (int i = 0; i < FileList[fileIndex].SectorList.Length; i++)
                {
                    Sectors[i].RECNAM = newFilename.PadRight(10);
                }
                FileList[fileIndex].FileName = newFilename.PadRight(10);
            }
            else
            {
                // Exit
            }
        }

    }
}
