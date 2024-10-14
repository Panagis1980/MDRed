using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDRed.Classes
{
    public class Sector
    {
           
        // Header Block
        public byte HDFLAG { get; set; }    // Header block flag (Value 1, indicates header block)
        public byte HDNUMB { get; set; }    // Sector number (254 down to 1)
        public byte[] Unused { get; set; }  // 2 unused bytes
        public string HDNAME { get; set; }  // Microdrive cartridge name (10 characters, blank padded)
        public byte HDCHK { get; set; }     // Header checksum (checksum of first 14 bytes)

        // Record Block Descriptor
        public byte RECFLG { get; set; }    // Record flag - bit 0: always 0 to indicate record block
                                            // - bit 1: set for the EOF block
                                            // - bit 2: reset for a PRINT file
                                            // - bits 3-7: not used(value 0)
        public byte RECNUM { get; set; }    // Data block sequence number
        public byte[] RECLEN { get; set; }  // Data block length (ushort integer is always <= 512, LSByte first)
        public string RECNAM { get; set; }  // Filename (10 characters, blank padded)
        public byte DESCHK { get; set; }    // Descriptor checksum (checksum of previous 14 bytes)

        // Data Block
        public byte[] DataBlock { get; set; }  // 512-byte data block
        public byte DCHK { get; set; }       // Data block checksum (checksum of all 512 bytes)

        public Sector(byte hDFLAG, byte hDNUMB, string hDNAME, byte rECFLG, byte rECNUM, string rECNAM, byte[] dataBlock)
        {
            HDFLAG = hDFLAG;
            HDNUMB = hDNUMB;
            Unused = new byte[2] { 0, 0 };
            HDNAME = hDNAME;
            HDCHK = CalculateHeaderChecksum();

            RECFLG = rECFLG;
            RECNUM = rECNUM;
            DataBlock = dataBlock;
            
            RECLEN = new byte[2] { 0, 0 };
            RECNAM = rECNAM;
            DESCHK = CalculateDescriptorChecksum();
            DCHK = CalculateDataBlockChecksum();
        }


        // Constructor to initialize sector properties
        public Sector()
        {

        }

        // Calculate checksum for the first 14 bytes of the header block
        public byte CalculateHeaderChecksum()
        {
            byte[] sum = new byte[14];
            sum[0]=HDFLAG;
            sum[1]=HDNUMB;
            sum[2] = Unused[0];
            sum[3] = Unused[1];
            Encoding.ASCII.GetBytes(HDNAME).CopyTo(sum, 4);

            int check = 0;

            for (int i = 0; i < sum.Length; i++)
            {
                check += sum[i];
            }
            return (byte)(check % 255);

        }

        // Calculate checksum for the record descriptor (first 14 bytes)
        public byte CalculateDescriptorChecksum()
        {
            byte[] sum = new byte[14];
            
            sum[0] = RECFLG;
            sum[1] = RECNUM;
            RECLEN.CopyTo(sum, 2);           
            Encoding.ASCII.GetBytes(RECNAM).CopyTo(sum, 4);

            int check = 0;

            for (int i = 0; i < sum.Length; i++)
            {
                check += sum[i];
            }
            return (byte)(check % 255);
        }

        // Calculate checksum for the 512-byte data block
        public byte CalculateDataBlockChecksum()
        {
            int sum = 0;
            foreach (byte b in DataBlock)
            {
                sum += b;
            }
            return (byte)(sum % 255);
        }
    }
    /*
       MDR Format

      ZX Microdrive cartridge file format. The following information is adapted from documentation supplied with Carlo Delhez
      ' Spectrum emulator (Spectator - this emulator is no longer maintained by the author) for the Sinclair QL. 
      It can also be found in the 'Spectrum Microdrive Book' by Dr. Ian Logan (co-author of the 'Complete Spectrum ROM Disassembly', 
      and author of the Microdrive software.)

      A cartridge file contains 254 'sectors' of 543 bytes each, and a final byte flag which is non-zero is the cartridge 
      is write protected, so the total length is 137923 bytes. On the cartridge tape, after a GAP of some time the ZX Interface 
      I writes 10 zeros and 2 FF bytes (the preamble), and then a fifteen byte header-block-with-checksum. After another GAP, 
      it writes a preamble again, with a 15-byte record-descriptor-with-checksum (which has a structure very much like the header block), 
      immediately followed by the data block of 512 bytes, and a final checksum of those 512 bytes. 
      The preamble is used by the ZX Interface I hardware to synchronise, and is not explicitly used by the software. 
      The preamble is not saved to the microdrive file:

          Offset Length Name    Contents
          ------------------------------
            0      1   HDFLAG   Value 1, to indicate header block  *See note.
            1      1   HDNUMB   sector number (values 254 down to 1)
            2      2            not used (and of undetermined value)
            4     10   HDNAME   microdrive cartridge name (blank padded)
           14      1   HDCHK    header checksum (of first 14 bytes)

           15      1   RECFLG   - bit 0: always 0 to indicate record block
                                - bit 1: set for the EOF block
                                - bit 2: reset for a PRINT file
                                - bits 3-7: not used (value 0)

           16      1   RECNUM   data block sequence number (value starts at 0)
           17      2   RECLEN   data block length (<=512, LSB first)
           19     10   RECNAM   filename (blank padded)
           29      1   DESCHK   record descriptor checksum (of previous 14 bytes)
           30    512            data block
          542      1   DCHK     data block checksum (of all 512 bytes of data
                                block, even when not all bytes are used)
          ---------
          254 times
      (Actually, this information is 'transparent' to the emulator. All it does is store 2 times 
      254 blocks in the .mdr file as it is OUTed, alternatingly of length 15 and 528 bytes.
      The emulator does check checksums, see below; the other fields are dealt with by the emulated Interface I software.)
      A used record block is either an EOF block (bit 1 of RECFLG is 1) or contains 512 bytes of data 
      (RECLEN=512, i.e. bit 1 of MSB is 1). An empty record block has a zero in bit 1 of RECFLG and also RECLEN=0.
      An unusable block (as determined by the FORMAT command) is an EOF block with RECLEN=0.
      The three checksums are calculated by adding all the bytes together modulo 255; this will never produce a checksum of 255.
      Possibly, this is the value that is read by the ZX Interface I if there's no or bad data on the tape.
      In normal operation, all first-fifteen-byte blocks of each header or record block will have the right checksum.
      If the checksum is incorrect, the block will be treated as a GAP. For instance,
      if you type OUT 239,0 on a normal Spectrum with ZX Interface I, the microdrive motor starts running and the 
      cartridge will be erased completely in 7 seconds. CAT 1 will respond with 'microdrive not ready'.
      Warajevo uses basically the same format, but ignores the 'read-only' final byte (it obtains this information 
      from the file attributes), and also the files do not have to contain all 254 sectors.
      Note: This is not strictly correct; it is not set to 1 - only bit 0 is set which would give the value 1 if the 
      location previously held 0 or 1. Also, please be aware that if you format if you FORMAT cartridges after the NEW command, 
      then the channel is created in an area where the bytes have been set to zero. If you FORMAT cartridges 
      with a BASIC program in memory then the channel is created where the BASIC was and so these bits and bytes show through.
       */
}
