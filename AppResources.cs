using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourDepack_cs
{
    public class AppResources
    {
        public AppResources(byte[] data, string filename)
        {
            RawData = data;
            Filename = filename;
        }

        public byte[] RawData { get; set; }
        public string Filename { get; set; }
    }
}
