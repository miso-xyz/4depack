using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Globalization;
using FourDepack_cs.Security;
using System.IO;
using System.Diagnostics;

namespace FourDepack_cs
{
    public class ProjectFile
    {
        public ProjectFile(string RawProjectFile)
        {
            XMLSource.LoadXml(RawProjectFile);
            InitializeSourceFile();
        }

        /// <summary>
        ///  Re-reads the current source file
        /// </summary>
        public void Refresh() { InitializeSourceFile(); }

        public XmlDocument XMLSource = new XmlDocument();

        public bool isZSPProjectFile = false;
        public decimal StayOnSlide { get; set; }
        public decimal EffectDuration { get; set; }
        public int ImageTransition { get; set; }
        public string BackgroundMusic { get; set; }
        public int BackgroundMusicVolume { get; set; }
        public bool LoopBackgroundMusic { get; set; }
        public SlideshowSizeMode SizeMode { get; set; }
        public int ImageAlign { get; set; }
        public Color BackgroundColor { get; set; }
        public bool RotateEXIF { get; set; }
        public bool LoopSlideshow { get; set; }
        public bool FullScreen { get; set; }
        public bool EncryptImages { get; set; }
        public bool AskForPassword { get; set; }
        public string Password { get; set; }
        public bool ProhibitCopyScreen { get; set; }
        public bool ExtendSlideshow { get; set; }
        public Guid GUID { get; set; }
        public int MaxViewTimes { get; set; }
        public int MaxPrintTimes { get; set; }
        public int MaxViewTime { get; set; }
        public int ExpireAfter { get; set; }
        public DateTime ViewDateFrom { get; set; }
        public DateTime ViewDateTo { get; set; }
        public DateTime ViewTimeFrom { get; set; }
        public DateTime ViewTimeTo { get; set; }
        public bool ViewUserNames { get; set; }
        public bool ViewDomainNames { get; set; }
        public bool ViewComputerNames { get; set; }
        public bool ViewMachineSignatures { get; set; }
        public string MessageOnStart { get; set; }
        public string MessageOnExit { get; set; }
        public string AboutBoxText { get; set; }
        public string WindowTitle { get; set; }
        public bool HasSplashScreen { get; set; }
        public string SplashScreenText { get; set; }
        public Color SplashScreenTextColor { get; set; }
        public Color SplashScreenBackColor { get; set; }
        public bool AllowDrawing { get; set; }
        public bool AllowExportImages { get; set; }
        public bool AllowPrinting { get; set; }
        public bool AllowSaveImage { get; set; }
        public bool AllowViewDocumentProperties { get; set; }
        public bool AllowFullscreen { get; set; }
        public bool HasViewTimeTo { get; set; }
        public bool HasViewTimeFrom { get; set; }
        public bool HasViewDateTo { get; set; }
        public bool HasViewDateFrom { get; set; }
        public TimeSpan expireAfterFirstViewTimeSpan { get; set; }
        public bool expiresAfterFirstView { get; set; }
        public TimeSpan maxViewTimeTimeSpan { get; set; }
        public int maxViewTimeSecs { get; set; }

        // ZSP-Specific
        public int ZSP_EncryptionAlgorithm { get; set; }
        public string ZSP_EncryptionPassword { get; set; }
        public bool ZSP_RunWithAdminRights { get; set; }
        public bool ZSP_BrowseDirectory { get; set; }
        public bool ZSP_AutomaticUnzip { get; set; }
        public string ZSP_UnzipDirectory { get; set; }
        public Process[] ZSP_CommandsBefore { get; set; }
        public bool[] ZSP_CommandsBefore_WaitForExit { get; set; }
        public Process[] ZSP_CommandsAfter { get; set; }
        public bool[] ZSP_CommandsAfter_WaitForExit { get; set; }
        public string ZSP_FilesFolder { get; set; }
        public bool ZSP_UseExistingZip { get; set; }
        public string ZSP_ExistingZipPath { get; set; }
        public string ZSP_MessageBefore { get; set; }
        public string ZSP_MessageAfter { get; set; }
        public string ZSP_AboutText { get; set; }
        public string ZSP_BrandingText { get; set; }
        public string ZSP_UnzipButtonText { get; set; }
        public int ZSP_PromptType { get; set; }
        public string ZSP_HeaderImage { get; set; }
        public string ZSP_Icon { get; set; }
        public bool ZSP_AutoSelectLanguage { get; set; }
        public bool ZSP_Encrypted { get; set; }
        public string[,] ZSP_Shortcuts { get; set; }
        public string[,] ZSP_Languages { get; set; }
        public Process[,] ZSP_Languages_Commands { get; set; }
        public bool[,] ZSP_Languages_Commands_WaitForExit { get; set; }
        public string[,] ZSP_Languages_Shortcuts { get; set; }

        private void InitializeSourceFile()
        {
            XmlNode xmlNode = null;
            if (isZSPProjectFile)
            {
                xmlNode = XMLSource.FirstChild.SelectSingleNode("./Encrypted");
                ZSP_Encrypted = bool.Parse(xmlNode.InnerText);
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./EncryptionAlgorithm");
                ZSP_EncryptionAlgorithm = int.Parse(xmlNode.InnerText);
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./AskForPassword");
		        AskForPassword = (xmlNode.InnerText == bool.TrueString);
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./Password");
		        Password = xmlNode.InnerText;
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./EncryptionPassword");
                ZSP_EncryptionPassword = xmlNode.InnerText;
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./UnzipDirectory");
                switch (xmlNode.InnerText)
                {
                    case "$ZIPDIR":
                        ZSP_UnzipDirectory = Path.GetFullPath(xmlNode.InnerText);
                        break;
                    case "$TEMP":
                        ZSP_UnzipDirectory = Path.GetTempPath();
                        break;
                    case "$APPDATA":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        break;
                    case "$COMMONAPPDATA":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                        break;
                    case "$COMMONPROGRAMFILES":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
                        break;
                    case "$DESKTOPDIRECTORY":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                        break;
                    case "$LOCALAPPDATA":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        break;
                    case "$MYCOMPUTER":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
                        break;
                    case "$MYDOCUMENTS":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        break;
                    case "$MYMUSIC":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                        break;
                    case "$MYPICTURES":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                        break;
                    case "$PROGRAMFILES":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                        break;
                    case "$PROGRAMS":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                        break;
                    case "$STARTMENU":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                        break;
                    case "$STARTUP":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                        break;
                    case "$SYSTEM":
                        ZSP_UnzipDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
                        break;
                    default:
                        ZSP_UnzipDirectory = xmlNode.InnerText;
                        break;
                }
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./BrowseDirectory");
                ZSP_BrowseDirectory = bool.Parse(xmlNode.InnerText);
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./UnzipAutomatically");
                ZSP_AutomaticUnzip = bool.Parse(xmlNode.InnerText);
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./AutoSelectLanguage");
                ZSP_AutoSelectLanguage = bool.Parse(xmlNode.InnerText);
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./CommandsBefore");
		        XmlNodeList xmlNodeList = xmlNode.SelectNodes("./Command");
		        for (int i = 0; i < xmlNodeList.Count; i++)
		        {
                    ZSP_CommandsBefore[i].StartInfo.FileName = xmlNodeList[i].Attributes["Filename"].Value;
                    ZSP_CommandsBefore[i].StartInfo.Arguments = xmlNodeList[i].Attributes["Arguments"].Value;
                    ZSP_CommandsBefore[i].StartInfo.CreateNoWindow = bool.Parse(xmlNodeList[i].Attributes["HideWindow"].Value);
                    ZSP_CommandsBefore_WaitForExit[i] = bool.Parse(xmlNodeList[i].Attributes["WaitForExit"].Value);
		        }
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./CommandsAfter");
		        xmlNodeList = xmlNode.SelectNodes("./Command");
		        for (int i = 0; i < xmlNodeList.Count; i++)
		        {
                    ZSP_CommandsAfter[i].StartInfo.FileName = xmlNodeList[i].Attributes["Filename"].Value;
                    ZSP_CommandsAfter[i].StartInfo.Arguments = xmlNodeList[i].Attributes["Arguments"].Value;
                    ZSP_CommandsAfter[i].StartInfo.CreateNoWindow = bool.Parse(xmlNodeList[i].Attributes["HideWindow"].Value);
                    ZSP_CommandsAfter_WaitForExit[i] = bool.Parse(xmlNodeList[i].Attributes["WaitForExit"].Value);
		        }
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./Shortcuts");
		        XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("./Shortcut");
		        for (int i = 0; i < xmlNodeList2.Count; i++)
		        {
                    ZSP_Shortcuts[i, 0] = xmlNodeList2[i].Attributes["FileShortcut"].Value;
                    ZSP_Shortcuts[i, 1] = xmlNodeList2[i].Attributes["Filename"].Value;
                    ZSP_Shortcuts[i, 2] = xmlNodeList2[i].Attributes["Description"].Value;
                    ZSP_Shortcuts[i, 3] = xmlNodeList2[i].Attributes["IconLocation"].Value;
                    ZSP_Shortcuts[i, 4] = xmlNodeList2[i].Attributes["TargetPath"].Value;
                    ZSP_Shortcuts[i, 5] = xmlNodeList2[i].Attributes["WorkingDirectory"].Value;
                    ZSP_Shortcuts[i, 6] = xmlNodeList2[i].Attributes["Arguments"].Value;
                    ZSP_Shortcuts[i, 7] = xmlNodeList2[i].Attributes["URL"].Value;
		        }
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./WindowTitle");
		        WindowTitle = xmlNode.InnerText;
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./MessageBefore");
                ZSP_MessageBefore = xmlNode.InnerText;
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./MessageAfter");
                ZSP_MessageAfter = xmlNode.InnerText;
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./AboutText");
                ZSP_AboutText = xmlNode.InnerText;
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./BrandingText");
                ZSP_BrandingText = xmlNode.InnerText;
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./UnzipButtonText");
                ZSP_UnzipButtonText = xmlNode.InnerText;
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./HeaderImage");
                ZSP_HeaderImage = xmlNode.InnerText;
		        xmlNode = XMLSource.FirstChild.SelectSingleNode("./PromptType");
                ZSP_PromptType = int.Parse(xmlNode.InnerText);
		        XmlNodeList xmlNodeList3 = XMLSource.FirstChild.SelectNodes("//Language");
		        for (int i = 0; i < xmlNodeList3.Count; i++)
		        {
			        string value = xmlNodeList3[i].Attributes["Description"].Value;
                    ZSP_Languages[i, 0] = value;
			        if (i != 0)
			        {
				        XmlNode xmlNode2 = xmlNodeList3[i];
				        xmlNode = xmlNode2.SelectSingleNode("./WindowTitle");
                        ZSP_Languages[i, 1] = xmlNode.InnerText;
				        xmlNode = xmlNode2.SelectSingleNode("./MessageBefore");
                        ZSP_Languages[i, 2] = xmlNode.InnerText;
				        xmlNode = xmlNode2.SelectSingleNode("./MessageAfter");
                        ZSP_Languages[i, 3] = xmlNode.InnerText;
				        xmlNode = xmlNode2.SelectSingleNode("./AboutText");
                        ZSP_Languages[i, 4] = xmlNode.InnerText;
				        xmlNode = xmlNode2.SelectSingleNode("./BrandingText");
                        ZSP_Languages[i, 5] = xmlNode.InnerText;
				        xmlNode = xmlNode2.SelectSingleNode("./UnzipButtonText");
                        ZSP_Languages[i, 6] = xmlNode.InnerText;
				        xmlNode = xmlNode2.SelectSingleNode("./HeaderImage");
                        ZSP_Languages[i, 7] = xmlNode.InnerText;
				        xmlNode = xmlNode2.FirstChild.SelectSingleNode("./PromptType");
                        ZSP_Languages[i, 8] = xmlNode.InnerText;
				        xmlNode = xmlNode2.SelectSingleNode("./Icon");
                        ZSP_Languages[i, 9] = xmlNode.InnerText;
				        XmlNodeList xmlNodeList4 = xmlNode2.SelectSingleNode("./CommandsBefore").SelectNodes("./Command");
				        for (int j = 0; j < xmlNodeList4.Count; j++)
				        {
                            ZSP_Languages_Commands[i, 0].StartInfo.FileName = xmlNode.Attributes["Filename"].Value;
                            ZSP_Languages_Commands[i, 1].StartInfo.Arguments = xmlNode.Attributes["Arguments"].Value;
                            ZSP_Languages_Commands[i, 2].StartInfo.CreateNoWindow = bool.Parse(xmlNode.Attributes.GetNamedItem("HideWindow").Value);
                            ZSP_Languages_Commands_WaitForExit[i, 0] = bool.Parse(xmlNode.Attributes.GetNamedItem("WaitForExit").Value);
				        }
                        xmlNodeList4 = xmlNode2.SelectSingleNode("./CommandsAfter").SelectNodes("./Command");
				        for (int j = 0; j < xmlNodeList4.Count; j++)
				        {
                            ZSP_Languages_Commands[i, 4].StartInfo.FileName = xmlNode.Attributes["Filename"].Value;
                            ZSP_Languages_Commands[i, 5].StartInfo.Arguments = xmlNode.Attributes["Arguments"].Value;
                            ZSP_Languages_Commands[i, 6].StartInfo.CreateNoWindow = bool.Parse(xmlNode.Attributes.GetNamedItem("HideWindow").Value);
                            ZSP_Languages_Commands_WaitForExit[i, 1] = bool.Parse(xmlNode.Attributes.GetNamedItem("WaitForExit").Value);
				        }
				        XmlNodeList xmlNodeList5 = xmlNode2.SelectSingleNode("./Shortcuts").SelectNodes("./Shortcut");
				        for (int j = 0; j < xmlNodeList5.Count; j++)
				        {
                            ZSP_Languages_Shortcuts[i, 0] = xmlNodeList5[j].Attributes["FileShortcut"].Value;
                            ZSP_Languages_Shortcuts[i, 1] = xmlNodeList5[j].Attributes["Description"].Value;
                            ZSP_Languages_Shortcuts[i, 2] = xmlNodeList5[j].Attributes["IconLocation"].Value;
                            ZSP_Languages_Shortcuts[i, 3] = xmlNodeList5[j].Attributes["TargetPath"].Value;
                            ZSP_Languages_Shortcuts[i, 4] = xmlNodeList5[j].Attributes["URL"].Value;
				        }
			        }
                }
            }
            else
            {
                xmlNode = XMLSource.SelectSingleNode("//Misc");
                StayOnSlide = decimal.Parse(xmlNode.Attributes.GetNamedItem("StayOnSlide").Value.Replace(".", ","));
                EffectDuration = decimal.Parse(xmlNode.Attributes.GetNamedItem("EffectDuration").Value.Replace(".", ","));
                ImageTransition = int.Parse(xmlNode.Attributes.GetNamedItem("ImageTransition").Value);
                BackgroundMusic = xmlNode.Attributes.GetNamedItem("BackgroundMusic").Value;
                BackgroundMusicVolume = int.Parse(xmlNode.Attributes.GetNamedItem("BackgroundMusicVolume").Value.Replace("%", null));
                LoopBackgroundMusic = bool.Parse(xmlNode.Attributes.GetNamedItem("BackgroundMusicLoop").Value);
                SizeMode = (SlideshowSizeMode)int.Parse(xmlNode.Attributes.GetNamedItem("SizeMode").Value);
                ImageAlign = int.Parse(xmlNode.Attributes.GetNamedItem("ImageAlign").Value);
                BackgroundColor = ColorTranslator.FromWin32(int.Parse(xmlNode.Attributes.GetNamedItem("BackgroundColor").Value));
                RotateEXIF = bool.Parse(xmlNode.Attributes.GetNamedItem("RotateEXIF").Value);
                LoopSlideshow = bool.Parse(xmlNode.Attributes.GetNamedItem("LoopSlideshow").Value);
                ExtendSlideshow = bool.Parse(xmlNode.Attributes.GetNamedItem("ExtendSlideshow").Value);
                FullScreen = bool.Parse(xmlNode.Attributes.GetNamedItem("ShowFullScreen").Value);
                EncryptImages = bool.Parse(xmlNode.Attributes.GetNamedItem("EncryptImages").Value);
                AllowDrawing = bool.Parse(xmlNode.Attributes.GetNamedItem("AllowDrawing").Value);
                AllowExportImages = bool.Parse(xmlNode.Attributes.GetNamedItem("AllowExportImages").Value);
                AllowPrinting = bool.Parse(xmlNode.Attributes.GetNamedItem("AllowPrinting").Value);
                AllowSaveImage = bool.Parse(xmlNode.Attributes.GetNamedItem("AllowSaveImage").Value);
                AllowViewDocumentProperties = bool.Parse(xmlNode.Attributes.GetNamedItem("AllowViewDocumentProperties").Value);
                AllowFullscreen = bool.Parse(xmlNode.Attributes.GetNamedItem("AllowFullscreen").Value);
                ProhibitCopyScreen = bool.Parse(xmlNode.Attributes.GetNamedItem("ProhibitCopyScreen").Value);
                GUID = new Guid(xmlNode.Attributes.GetNamedItem("GUID").Value);
                if (!string.IsNullOrEmpty(xmlNode.Attributes.GetNamedItem("ViewUserNames").Value))
                {
                    ViewUserNames = bool.Parse(Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("ViewUserNames").Value), "493589549485043859430889230823")));
                }
                if (!string.IsNullOrEmpty(xmlNode.Attributes.GetNamedItem("ViewDomainNames").Value))
                {
                    ViewDomainNames = bool.Parse(Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("ViewDomainNames").Value), "493589549485043859430889230823")));
                }
                if (!string.IsNullOrEmpty(xmlNode.Attributes.GetNamedItem("ViewComputerNames").Value))
                {
                    ViewComputerNames = bool.Parse(Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("ViewComputerNames").Value), "493589549485043859430889230823")));
                }
                if (!string.IsNullOrEmpty(xmlNode.Attributes.GetNamedItem("ViewMachineSignatures").Value))
                {
                    ViewMachineSignatures = bool.Parse(xmlNode.Attributes.GetNamedItem("ViewMachineSignatures").Value);
                }
                MessageOnStart = xmlNode.Attributes.GetNamedItem("MessageOnStart").Value;
                MessageOnExit = xmlNode.Attributes.GetNamedItem("MessageOnExit").Value;
                AboutBoxText = xmlNode.Attributes.GetNamedItem("AboutBoxText").Value;
                WindowTitle = xmlNode.Attributes.GetNamedItem("WindowTitle").Value;
                HasSplashScreen = bool.Parse(xmlNode.Attributes.GetNamedItem("HasSplashScreen").Value);
                SplashScreenText = xmlNode.Attributes.GetNamedItem("SplashScreenText").Value;
                SplashScreenTextColor = Color.FromArgb(int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenTextColor").Value.Substring(0, 3)), int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenTextColor").Value.Substring(4, 3)), int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenTextColor").Value.Substring(8, 3)));
                SplashScreenBackColor = Color.FromArgb(int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenBackColor").Value.Substring(0, 3)), int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenBackColor").Value.Substring(4, 3)), int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenBackColor").Value.Substring(8, 3)));
                if (!string.IsNullOrEmpty(xmlNode.Attributes.GetNamedItem("MaxViewTimes").Value))
                {
                    string text = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("MaxViewTimes").Value), "3434234-92323454534095"));
                    string[] array = text.Split(new string[] { "|||" }, StringSplitOptions.None);
                    MaxViewTimes = int.Parse(array[1]);
                }
                if (!string.IsNullOrEmpty(xmlNode.Attributes.GetNamedItem("MaxPrintTimes").Value))
                {
                    string text = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("MaxPrintTimes").Value), "3434234-92323454534095"));
                    string[] array = text.Split(new string[] { "|||" }, StringSplitOptions.None);
                    MaxPrintTimes = int.Parse(array[1]);
                }
                if (!string.IsNullOrEmpty(xmlNode.Attributes.GetNamedItem("MaxViewTime").Value))
                {
                    string text = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("MaxViewTime").Value), "3434234-92323454534095"));
                    string[] array = text.Split(new string[] { "|||" }, StringSplitOptions.None);
                    int num3 = int.Parse(array[1]);
                    maxViewTimeTimeSpan = new TimeSpan(0, 0, num3);
                    maxViewTimeSecs = num3;
                }
                if (!string.IsNullOrEmpty(xmlNode.Attributes.GetNamedItem("ExpireAfter").Value))
                {
                    string text = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("ExpireAfter").Value), "3434234-92323454534095"));
                    string[] array = text.Split(new string[] { "|||" }, StringSplitOptions.None);
                    int days = int.Parse(array[1]);
                    int num3 = int.Parse(array[2]);
                    expireAfterFirstViewTimeSpan = new TimeSpan(days, 0, 0, num3);
                    expiresAfterFirstView = true;
                }
                if (!string.IsNullOrEmpty(xmlNode.Attributes.GetNamedItem("ViewDateFrom").Value))
                {
                    HasViewDateFrom = true;
                    ViewDateFrom = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("ViewDateFrom").Value));
                }
                if (!string.IsNullOrEmpty(xmlNode.Attributes.GetNamedItem("ViewDateTo").Value))
                {
                    HasViewDateTo = true;
                    ViewDateTo = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("ViewDateTo").Value));
                }
                if (!string.IsNullOrEmpty(xmlNode.Attributes.GetNamedItem("ViewTimeFrom").Value))
                {
                    HasViewTimeFrom = true;
                    ViewTimeFrom = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("ViewTimeFrom").Value));
                }
                if (!string.IsNullOrEmpty(xmlNode.Attributes.GetNamedItem("ViewTimeTo").Value))
                {
                    HasViewTimeTo = true;
                    ViewTimeTo = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("ViewTimeTo").Value));
                }
                if (!string.IsNullOrEmpty(xmlNode.Attributes.GetNamedItem("AskForPasswordValue").Value))
                {
                    Password = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("AskForPasswordValue").Value), "493589549485043859430889230823"));
                }
            }
            AskForPassword = bool.Parse(xmlNode.Attributes.GetNamedItem("AskForPassword").Value);
        }

        public enum SlideshowSizeMode
        {
            Zoom = 0,
            AsIs = 1,
            Stretch = 2
        }
    }
}