using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;

namespace FourDepack_cs
{
    public class FourDots_PackedApplication
    {
        public ModuleDefMD TargetApplicationModuleMD;
        public Assembly TargetApplicationASM;
        public string TargetApplicationPath;
        public ProjectFile TargetApplicationPrj;
        public List<AppResources> TargetApplicationResources = new List<AppResources>();

        public List<string> ResourceTargets = new List<string>() {"LockedDocument", "project", "4dotsAudioBackgroundMusic", "zipexe", ".jpg"};

        public FourDots_PackedApplication(string path)
        {
            TargetApplicationPath = path;
            InitializeTargetApplication();
            ExtractResources();
            InitializeTargetApplicationProjectFile();
            TryDecryptImages("433424234234-93435849839453");
        }

        private void InitializeTargetApplication()
        {
            TargetApplicationASM = Assembly.LoadFile(TargetApplicationPath);
            TargetApplicationModuleMD = ModuleDefMD.Load(TargetApplicationPath);
        }

        private void InitializeTargetApplicationProjectFile()
        {
            foreach (AppResources resource in TargetApplicationResources)
            {
                if (resource.Filename.Contains("project.xml")) { TargetApplicationPrj = new ProjectFile(Encoding.Default.GetString(resource.RawData)); return; }
                if (resource.Filename.Contains("project.zsp")) { TargetApplicationPrj = new ProjectFile(Encoding.Default.GetString(resource.RawData)); TargetApplicationPrj.isZSPProjectFile = true; return; }
            }
        }

        private void ExtractResources()
        {
            foreach (string ResourceName in TargetApplicationASM.GetManifestResourceNames())
            {
                foreach (string ResourceTargetNames in ResourceTargets)
                {
                    if (ResourceName.Contains(ResourceTargetNames))
                    {
                        using (BinaryReader br = new BinaryReader(TargetApplicationASM.GetManifestResourceStream(ResourceName)))
                        {
                            MemoryStream ms = new MemoryStream();
                            MemoryStream resourceStream = new MemoryStream();
                            TargetApplicationASM.GetManifestResourceStream(ResourceName).CopyTo(resourceStream);
                            while (true)
                            {
                                byte[] buffer = new byte[0x8000];
                                int num = br.Read(buffer, 0, 0x8000); // Reads in chunks of 32768 bytes
                                if (num <= 0) { break; }              // if 0, don't write data & exit loop
                                ms.Write(buffer, 0, num);
                            }
                            TargetApplicationResources.Add(new AppResources(resourceStream.ToArray(), ResourceName));
                        }
                    }
                }
            }
        }

        public void TryDecryptImages(string decryptionKey)
        {
            foreach (AppResources resource in TargetApplicationResources)
            {
                int fileExtensionDotIndex = resource.Filename.LastIndexOf('.');
                string resourceFileExtension = resource.Filename.ToLower().Substring(fileExtensionDotIndex);
                switch (resourceFileExtension)
                {
                    case "png":
                    case "bmp":
                    case "gif":
                    case "jpg":
                    case "jpeg":
                    case "tiff":
                        if (TargetApplicationPrj.EncryptImages) { resource.RawData = Security.CryptoHelper.DecryptBytes(resource.RawData, decryptionKey); }
                        else { return; }
                        break;
                    default:
                        continue;
                }
            }
        }
    }
}
