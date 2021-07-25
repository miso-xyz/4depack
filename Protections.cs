using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.IO;

namespace FourDepack_cs.Security
{
    class Protections
    {
        private FourDepack_cs.FourDots_PackedApplication TargetApplicationGlobalInfo;

        public Protections(string path) { TargetApplicationGlobalInfo = new FourDepack_cs.FourDots_PackedApplication(path); }

        public bool isCRCValid() // To pass the checksum, the variable 'list' should contain 3 '|', if it doesn't the CRC isn't valid.
        {
            using (FileStream fileStream = new FileStream(TargetApplicationGlobalInfo.TargetApplicationPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int charCount = 0;
                int cycleCount = 0;
                List<byte> list = new List<byte>();
                while (true)
                {
                    cycleCount++;
                    fileStream.Seek(-cycleCount, SeekOrigin.End);
                    int streamByte = fileStream.ReadByte();
                    char c = (char)streamByte;
                    if (c == '|')
                    {
                        charCount++;
                        if (charCount == 3) { break; }
                    }
                    else { charCount = 0; }
                    list.Insert(0, Convert.ToByte(streamByte)); // adds junk character, this instruction invalidates the CRC.
                }
                if (list.Count == 2) { return true; } else { return false; }
            }
        }

        public string PackerName // Packer name can be retrieved via namespaces, its very inaccurate yet nothing more can be done.
        {
            get
            {
                foreach (TypeDef t_ in TargetApplicationGlobalInfo.TargetApplicationModuleMD.Types)
                {
                    switch (t_.Namespace)
                    {
                        case "ConvertExcelToEXE4dots":
                            return "Excel To EXE Converter (Can be based on it)";
                        case "ConvertWordToEXE4dots":
                            return "Word To EXE Converter";
                        case "ConvertPowerpointToEXE4dots":
                            return "Powerpoint To EXE Converter";
                        case "PDFToEXEConverter":
                            return "PDF To EXE Converter";
                        case "ZIPSelfExtractor":
                            return "ZIP Self Extractor Maker";
                        case "LockedDocument":
                            return "EXE (Document) Locker";
                    }
                }
                return null;
            }
        }
    }
}