using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDRed.Classes
{
    public class MDFile
    {
        public string FileName = "";
        public int FileSize = 0;
        public int[] SectorList;
        //protected bool ChecksumPass = true;

        public MDFile()
        {
        }

        public MDFile(string fileName)
        {
            FileName = fileName;
        }
    }
}
