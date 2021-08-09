using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;

namespace FourDepack
{
    public class FourDots_PackedApplication
    {
        public ModuleDefMD PackedApplication_ModuleMD;
        public Assembly PackedApplication_ASM;
        public string PackedApplication_FilePath;
        public XMLProject PackedApplication_XMLProject;
        public ZSPProject PackedApplication_ZSPProject;
        public List<AppResources> PackedApplication_Resources = new List<AppResources>();

        public List<string> ResourceTargets = new List<string>() {"LockedDocument", "project", "4dotsAudioBackgroundMusic", "zipexe", ".jpg"};

        public FourDots_PackedApplication(string path)
        {
            PackedApplication_FilePath = path;
            InitializePackedApplication();
            ExtractResources();
            InitializePackedApplicationProjectFiles();
            TryDecryptImages("433424234234-93435849839453");
        }

        public bool UsesZSPProject()
        {
            foreach (AppResources resource in PackedApplication_Resources)
            {
                if (resource.Filename.Contains("project.zsp")) { return true; }
            }
            return false;
        }

        private void InitializePackedApplication()
        {
            PackedApplication_ASM = Assembly.LoadFile(PackedApplication_FilePath);
            PackedApplication_ModuleMD = ModuleDefMD.Load(PackedApplication_FilePath);
        }

        private void InitializePackedApplicationProjectFiles()
        {
            foreach (AppResources resource in PackedApplication_Resources)
            {
                if (resource.Filename.Contains("project.xml")) { PackedApplication_XMLProject = new XMLProject(Encoding.Default.GetString(resource.RawData), PackedApplication_FilePath); }
                if (resource.Filename.Contains("project.zsp")) { PackedApplication_ZSPProject = new ZSPProject(Encoding.Default.GetString(resource.RawData)); }
            }
        }

        private void ExtractResources()
        {
            foreach (string ResourceName in PackedApplication_ASM.GetManifestResourceNames())
            {
                foreach (string ResourceTargetNames in ResourceTargets)
                {
                    if (ResourceName.Contains(ResourceTargetNames))
                    {
                        using (BinaryReader br = new BinaryReader(PackedApplication_ASM.GetManifestResourceStream(ResourceName)))
                        {
                            MemoryStream ms = new MemoryStream();
                            MemoryStream resourceStream = new MemoryStream();
                            PackedApplication_ASM.GetManifestResourceStream(ResourceName).CopyTo(resourceStream);
                            while (true)
                            {
                                byte[] buffer = new byte[0x8000];
                                int num = br.Read(buffer, 0, 0x8000); // Reads in chunks of 32768 bytes
                                if (num <= 0) { break; }              // if 0, don't write data & exit loop
                                ms.Write(buffer, 0, num);
                            }
                            PackedApplication_Resources.Add(new AppResources(resourceStream.ToArray(), ResourceName));
                        }
                    }
                }
            }
        }

        public void TryDecryptImages(string decryptionKey)
        {
            if (UsesZSPProject()) { return; }
            foreach (AppResources resource in PackedApplication_Resources)
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
                        if (PackedApplication_XMLProject.Properties.EncryptImages) { resource.RawData = Security.CryptoHelper.DecryptBytes(resource.RawData, decryptionKey); }
                        else { return; }
                        break;
                    default:
                        continue;
                }
            }
        }
    }
}
