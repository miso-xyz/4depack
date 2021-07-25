using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FourDepack_cs;

namespace _4depack_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            FourDepack_cs.FourDots_PackedApplication app = new FourDots_PackedApplication(args[0]);
            if (app.TargetApplicationPrj.AskForPassword) { Console.WriteLine(app.TargetApplicationPrj.Password); }
            Directory.CreateDirectory("4depack\\" + Path.GetFileName(args[0]));
            string[] keys = FourDepack_cs.Security.CryptoHelper.FindDecryptionKeys(app);
            foreach (AppResources resource in app.TargetApplicationResources)
            {
                byte[] tempData = resource.RawData;
                foreach (string key in keys)
                {
                    try { tempData = FourDepack_cs.Security.CryptoHelper.DecryptBytes(tempData, key); }
                    catch { continue; }
                }
                File.WriteAllBytes("4depack\\" + Path.GetFileName(args[0]) + "\\" + resource.Filename, tempData);
            }
            Console.ReadKey();
        }
    }
}
