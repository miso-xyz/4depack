using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Drawing;
using FourDepack.ProjectFileTypes;
using FourDepack.Security;

namespace FourDepack
{
    public class XMLProject
    {
        public XmlDocument XMLSource = new XmlDocument();
        public XMLProjectProperties Properties = new XMLProjectProperties();
        public XMLLockedDocumentProjectProperties LockedDocumentProperties = new XMLLockedDocumentProjectProperties();
        public XMLEXESlideshowProjectProperties EXESlideshowProperties = new XMLEXESlideshowProjectProperties();
        private bool isLockedDocument = false;

        public XMLProject(string RawXML, string appPath)
        {
            try { XMLSource.LoadXml(RawXML); }
            catch { try { XMLSource.LoadXml(Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(RawXML), "4dotsSoftware012301230123"))); isLockedDocument = true; } catch { throw; } } // Standalon EXE Locker has an encrypted project.xml
            if (!isLockedDocument) { InitializeProject(); }
            else
            {
                switch (new Protections(appPath).PackerName)
                {
                    case "EXESlideshow":
                        InitializeEXESlideshowProject();
                        break;
                    case "LockedDocument":
                        InitializeLockedDocumentProject();
                        break;
                    default:
                        InitializeProject();
                        break;
                }
            }
        }

        public void Refresh() { if (!isLockedDocument) { InitializeProject(); } else { InitializeLockedDocumentProject(); } }

        private void InitializeLockedDocumentProject()
        {
            XmlNode xmlNode = XMLSource.SelectSingleNode("//Project");
            LockedDocumentProperties.sName = xmlNode.Attributes.GetNamedItem("Name").Value;
			LockedDocumentProperties.sPassword = xmlNode.Attributes.GetNamedItem("Password").Value;
			LockedDocumentProperties.MessageOnStart = xmlNode.Attributes.GetNamedItem("MessageOnStart").Value.ToString();
			LockedDocumentProperties.MessageOnExit = xmlNode.Attributes.GetNamedItem("MessageOnExit").Value.ToString();
			LockedDocumentProperties.AboutBoxText = xmlNode.Attributes.GetNamedItem("AboutBoxText").Value.ToString();
			LockedDocumentProperties.WindowTitle = xmlNode.Attributes.GetNamedItem("WindowTitle").Value.ToString();
			LockedDocumentProperties.HasSplash = (xmlNode.Attributes.GetNamedItem("HasSplashScreen").Value.ToString() == bool.TrueString);
			LockedDocumentProperties.SplashText = xmlNode.Attributes.GetNamedItem("SplashScreenText").Value.ToString();
			LockedDocumentProperties.SplashTextColor = Color.FromArgb(int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenTextColor").Value.ToString().Substring(0, 3)), int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenTextColor").Value.ToString().Substring(4, 3)), int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenTextColor").Value.ToString().Substring(8, 3)));
			LockedDocumentProperties.SplashBackColor = Color.FromArgb(int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenBackColor").Value.ToString().Substring(0, 3)), int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenBackColor").Value.ToString().Substring(4, 3)), int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenBackColor").Value.ToString().Substring(8, 3)));
			LockedDocumentProperties.EncryptFiles = bool.Parse(xmlNode.Attributes.GetNamedItem("EncryptFiles").Value.ToString());
			LockedDocumentProperties.CheckCRC = bool.Parse(xmlNode.Attributes.GetNamedItem("CheckCRC").Value.ToString());
			LockedDocumentProperties.RunOnlyAndDelete = bool.Parse(xmlNode.Attributes.GetNamedItem("RunOnlyAndDelete").Value.ToString());
			LockedDocumentProperties.GUID = xmlNode.Attributes.GetNamedItem("GUID").Value.ToString();
            if (xmlNode.Attributes.GetNamedItem("ViewUserNames").Value.ToString() != string.Empty)
            {
                LockedDocumentProperties.ViewUserNames = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("ViewUserNames").Value.ToString()), "493589549485043859430889230823"));
            }
            if (xmlNode.Attributes.GetNamedItem("ViewDomainNames").Value.ToString() != string.Empty)
            {
                LockedDocumentProperties.ViewDomainNames = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("ViewDomainNames").Value.ToString()), "493589549485043859430889230823"));
            }
            if (xmlNode.Attributes.GetNamedItem("ViewComputerNames").Value.ToString() != string.Empty)
            {
                LockedDocumentProperties.ViewComputerNames = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("ViewComputerNames").Value.ToString()), "493589549485043859430889230823"));
            }
			LockedDocumentProperties.ViewMachineSignatures = xmlNode.Attributes.GetNamedItem("ViewMachineSignatures").Value.ToString();
			LockedDocumentProperties.KeepCreationDate = bool.Parse(xmlNode.Attributes.GetNamedItem("KeepCreationDate").Value.ToString());
			LockedDocumentProperties.KeepLastModDate = bool.Parse(xmlNode.Attributes.GetNamedItem("KeepLastModDate").Value.ToString());
			LockedDocumentProperties.CreationDate = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("CreationDate").Value.ToString()));
			LockedDocumentProperties.LastModDate = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("LastModDate").Value.ToString()));
			if (xmlNode.Attributes.GetNamedItem("MaxViewTimes").Value.ToString() != string.Empty)
			{
				string text2 = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("MaxViewTimes").Value.ToString()), "3434234-92323454534095"));
				string[] array = text2.Split("|||".ToCharArray());
				LockedDocumentProperties.MaxViewTimes = int.Parse(array[1]);
			}
			if (xmlNode.Attributes.GetNamedItem("ExpireAfter").Value.ToString() != string.Empty)
			{
				string text2 = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("ExpireAfter").Value.ToString()), "3434234-92323454534095"));
				string[] array = text2.Split("|||".ToCharArray());
				int days = int.Parse(array[1]);
				int seconds = int.Parse(array[2]);
				LockedDocumentProperties.ExpireAfterFirstViewTimeSpan = new TimeSpan(days, 0, 0, seconds);
				LockedDocumentProperties.ExpiresAfterFirstView = true;
			}
			if (xmlNode.Attributes.GetNamedItem("ViewDateFrom").Value.ToString() != string.Empty)
			{
				LockedDocumentProperties.HasViewDateFrom = true;
				LockedDocumentProperties.ViewDateFrom = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("ViewDateFrom").Value.ToString()));
			}
			if (xmlNode.Attributes.GetNamedItem("ViewDateTo").Value.ToString() != string.Empty)
			{
				LockedDocumentProperties.HasViewDateTo = true;
				LockedDocumentProperties.ViewDateTo = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("ViewDateTo").Value.ToString()));
			}
			if (xmlNode.Attributes.GetNamedItem("ViewTimeFrom").Value.ToString() != string.Empty)
			{
				LockedDocumentProperties.HasViewTimeFrom = true;
				LockedDocumentProperties.ViewTimeFrom = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("ViewTimeFrom").Value.ToString()));
			}
			if (xmlNode.Attributes.GetNamedItem("ViewTimeTo").Value.ToString() != string.Empty)
			{
				LockedDocumentProperties.HasViewTimeTo = true;
				LockedDocumentProperties.ViewTimeTo = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("ViewTimeTo").Value.ToString()));
			}
        }

        private void InitializeEXESlideshowProject()
        {
            XmlNode xmlNode = XMLSource.SelectSingleNode("//Misc");
            EXESlideshowProperties.StayDuration = decimal.Parse(xmlNode.Attributes.GetNamedItem("StayOnSlide").Value.ToString().Replace(",", "."), new CultureInfo("en-US"));
            EXESlideshowProperties.EffectDuration = decimal.Parse(xmlNode.Attributes.GetNamedItem("EffectDuration").Value.ToString().Replace(",", "."), new CultureInfo("en-US"));
            EXESlideshowProperties.ImageTransition = int.Parse(xmlNode.Attributes.GetNamedItem("ImageTransition").Value.ToString());
            EXESlideshowProperties.BackgroundMusic = xmlNode.Attributes.GetNamedItem("BackgroundMusic").Value.ToString();
            EXESlideshowProperties.BackgroundMusicVolume = xmlNode.Attributes.GetNamedItem("BackgroundMusicVolume").Value.ToString();
            EXESlideshowProperties.BackgroundMusicLoop = (xmlNode.Attributes.GetNamedItem("BackgroundMusicLoop").Value.ToString() == bool.TrueString);
            EXESlideshowProperties.SizeMode = (SlideshowSizeModeEnum)Enum.Parse(typeof(SlideshowSizeModeEnum), xmlNode.Attributes.GetNamedItem("SizeMode").Value.ToString());
            EXESlideshowProperties.ImageAlign = int.Parse(xmlNode.Attributes.GetNamedItem("ImageAlign").Value.ToString());
            EXESlideshowProperties.BackgroundColor = ColorTranslator.FromWin32(int.Parse(xmlNode.Attributes.GetNamedItem("BackgroundColor").Value.ToString()));
            EXESlideshowProperties.RotateEXIF = (xmlNode.Attributes.GetNamedItem("RotateEXIF").Value.ToString() == bool.TrueString);
            EXESlideshowProperties.LoopSlideshow = (xmlNode.Attributes.GetNamedItem("LoopSlideshow").Value.ToString() == bool.TrueString);
            EXESlideshowProperties.ExtendSlideshow = (xmlNode.Attributes.GetNamedItem("ExtendSlideshow").Value.ToString() == bool.TrueString);
            try
            {
                EXESlideshowProperties.HideMouseCursor = (xmlNode.Attributes.GetNamedItem("HideMouseCursor").Value.ToString() == bool.TrueString);
            }
            catch { }
            try
            {
                EXESlideshowProperties.ExitMouse = (xmlNode.Attributes.GetNamedItem("ExitMouse").Value.ToString() == bool.TrueString);
            }
            catch { }
            XmlNodeList xmlNodeList = XMLSource.SelectNodes("//Exception");
            if (xmlNodeList != null)
            {
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    SlideException ex = new SlideException();
                    if (xmlNodeList[i].Attributes.GetNamedItem("SlideNumber") != null)
                    {
                        ex.SlideNumber = int.Parse(xmlNodeList[i].Attributes.GetNamedItem("SlideNumber").Value.ToString());
                    }
                    if (xmlNodeList[i].Attributes.GetNamedItem("Duration") != null)
                    {
                        ex.StayDuration = decimal.Parse(xmlNodeList[i].Attributes.GetNamedItem("Duration").Value.ToString().Replace(",", "."), new CultureInfo("en-US"));
                    }
                    if (xmlNodeList[i].Attributes.GetNamedItem("Transition") != null)
                    {
                        ex.ImageTransition = int.Parse(xmlNodeList[i].Attributes.GetNamedItem("Transition").Value.ToString());
                    }
                    if (xmlNodeList[i].Attributes.GetNamedItem("AudioFilepath") != null)
                    {
                        ex.AudioFilepath = xmlNodeList[i].Attributes.GetNamedItem("AudioFilepath").Value.ToString();
                    }
                    if (xmlNodeList[i].Attributes.GetNamedItem("Delay") != null)
                    {
                        ex.AudioDelay = decimal.Parse(xmlNodeList[i].Attributes.GetNamedItem("Delay").Value.ToString().Replace(",", "."), new CultureInfo("en-US"));
                    }
                    EXESlideshowProperties.SlideExceptions.Add(ex);
                }
            }
        }

        private void InitializeProject()
        {
            XmlNode xmlNode = XMLSource.SelectSingleNode("//Misc");
            Properties.StayDuration = decimal.Parse(xmlNode.Attributes.GetNamedItem("StayOnSlide").Value.ToString().Replace(",", "."), new CultureInfo("en-US"));
            Properties.EffectDuration = decimal.Parse(xmlNode.Attributes.GetNamedItem("EffectDuration").Value.ToString().Replace(",", "."), new CultureInfo("en-US"));
            Properties.ImageTransition = int.Parse(xmlNode.Attributes.GetNamedItem("ImageTransition").Value.ToString());
            Properties.BackgroundMusic = xmlNode.Attributes.GetNamedItem("BackgroundMusic").Value.ToString();
            Properties.BackgroundMusicVolume = xmlNode.Attributes.GetNamedItem("BackgroundMusicVolume").Value.ToString();
            Properties.BackgroundMusicLoop = (xmlNode.Attributes.GetNamedItem("BackgroundMusicLoop").Value.ToString() == bool.TrueString);
            Properties.SizeMode = (SlideshowSizeModeEnum)Enum.Parse(typeof(SlideshowSizeModeEnum), xmlNode.Attributes.GetNamedItem("SizeMode").Value.ToString());
            Properties.ImageAlign = int.Parse(xmlNode.Attributes.GetNamedItem("ImageAlign").Value.ToString());
            Properties.BackgroundColor = ColorTranslator.FromWin32(int.Parse(xmlNode.Attributes.GetNamedItem("BackgroundColor").Value.ToString()));
            Properties.RotateEXIF = (xmlNode.Attributes.GetNamedItem("RotateEXIF").Value.ToString() == bool.TrueString);
            Properties.LoopSlideshow = (xmlNode.Attributes.GetNamedItem("LoopSlideshow").Value.ToString() == bool.TrueString);
            Properties.ExtendSlideshow = (xmlNode.Attributes.GetNamedItem("ExtendSlideshow").Value.ToString() == bool.TrueString);
            Properties.ShowFullScreen = (xmlNode.Attributes.GetNamedItem("ShowFullScreen").Value.ToString() == bool.TrueString);
            Properties.AskForPassword = (xmlNode.Attributes.GetNamedItem("AskForPassword").Value.ToString() == bool.TrueString);
            Properties.EncryptImages = (xmlNode.Attributes.GetNamedItem("EncryptImages").Value.ToString() == bool.TrueString);
            Properties.AllowDrawing = (xmlNode.Attributes.GetNamedItem("AllowDrawing").Value.ToString() == bool.TrueString);
            Properties.AllowExportImages = (xmlNode.Attributes.GetNamedItem("AllowExportImages").Value.ToString() == bool.TrueString);
            Properties.AllowPrinting = (xmlNode.Attributes.GetNamedItem("AllowPrinting").Value.ToString() == bool.TrueString);
            Properties.AllowSaveImage = (xmlNode.Attributes.GetNamedItem("AllowSaveImage").Value.ToString() == bool.TrueString);
            Properties.AllowViewDocumentProperties = (xmlNode.Attributes.GetNamedItem("AllowViewDocumentProperties").Value.ToString() == bool.TrueString);
            Properties.AllowFullscreen = (xmlNode.Attributes.GetNamedItem("AllowFullscreen").Value.ToString() == bool.TrueString);
            Properties.ProhibitCopyScreen = (xmlNode.Attributes.GetNamedItem("ProhibitCopyScreen").Value.ToString() == bool.TrueString);
            Properties.GUID = xmlNode.Attributes.GetNamedItem("GUID").Value.ToString();
            if (xmlNode.Attributes.GetNamedItem("ViewUserNames").Value.ToString() != string.Empty)
            {
                Properties.ViewUserNames = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("ViewUserNames").Value.ToString()), "493589549485043859430889230823"));
            }
            if (xmlNode.Attributes.GetNamedItem("ViewDomainNames").Value.ToString() != string.Empty)
            {
                Properties.ViewDomainNames = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("ViewDomainNames").Value.ToString()), "493589549485043859430889230823"));
            }
            if (xmlNode.Attributes.GetNamedItem("ViewComputerNames").Value.ToString() != string.Empty)
            {
                Properties.ViewComputerNames = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("ViewComputerNames").Value.ToString()), "493589549485043859430889230823"));
            }
            Properties.ViewMachineSignatures = xmlNode.Attributes.GetNamedItem("ViewMachineSignatures").Value.ToString();
            Properties.MessageOnStart = xmlNode.Attributes.GetNamedItem("MessageOnStart").Value.ToString();
            Properties.MessageOnExit = xmlNode.Attributes.GetNamedItem("MessageOnExit").Value.ToString();
            Properties.AboutBoxText = xmlNode.Attributes.GetNamedItem("AboutBoxText").Value.ToString();
            Properties.WindowTitle = xmlNode.Attributes.GetNamedItem("WindowTitle").Value.ToString();
            Properties.HasSplash = (xmlNode.Attributes.GetNamedItem("HasSplashScreen").Value.ToString() == bool.TrueString);
            Properties.SplashText = xmlNode.Attributes.GetNamedItem("SplashScreenText").Value.ToString();
            Properties.SplashTextColor = Color.FromArgb(int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenTextColor").Value.ToString().Substring(0, 3)), int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenTextColor").Value.ToString().Substring(4, 3)), int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenTextColor").Value.ToString().Substring(8, 3)));
            Properties.SplashBackColor = Color.FromArgb(int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenBackColor").Value.ToString().Substring(0, 3)), int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenBackColor").Value.ToString().Substring(4, 3)), int.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenBackColor").Value.ToString().Substring(8, 3)));
            if (xmlNode.Attributes.GetNamedItem("MaxViewTimes").Value.ToString() != string.Empty)
            {
                string text = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("MaxViewTimes").Value.ToString()), "3434234-92323454534095"));
                string[] array = text.Split("|||".ToCharArray());
                Properties.MaxViewTimes = int.Parse(array[1]);
            }
            if (xmlNode.Attributes.GetNamedItem("MaxPrintTimes").Value.ToString() != string.Empty)
            {
                string text = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("MaxPrintTimes").Value.ToString()), "3434234-92323454534095"));
                string[] array = text.Split("|||".ToCharArray());
                Properties.MaxPrintTimes = int.Parse(array[1]);
            }
            if (xmlNode.Attributes.GetNamedItem("MaxViewTime").Value.ToString() != string.Empty)
            {
                string text = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("MaxViewTime").Value.ToString()), "3434234-92323454534095"));
                string[] array = text.Split("|||".ToCharArray());
                int num3 = int.Parse(array[1]);
                Properties.MaxViewTimeTimeSpan = new TimeSpan(0, 0, num3);
                Properties.MaxViewTimeSecs = num3;
            }
            if (xmlNode.Attributes.GetNamedItem("ExpireAfter").Value.ToString() != string.Empty)
            {
                string text = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.Attributes.GetNamedItem("ExpireAfter").Value.ToString()), "3434234-92323454534095"));
                string[] array = text.Split("|||".ToCharArray());
                int days = int.Parse(array[1]);
                int num3 = int.Parse(array[2]);
                Properties.ExpireAfterFirstViewTimeSpan = new TimeSpan(days, 0, 0, num3);
                Properties.ExpiresAfterFirstView = true;
            }
            if (xmlNode.Attributes.GetNamedItem("ViewDateFrom").Value.ToString() != string.Empty)
            {
                Properties.HasViewDateFrom = true;
                Properties.ViewDateFrom = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("ViewDateFrom").Value.ToString()));
            }
            if (xmlNode.Attributes.GetNamedItem("ViewDateTo").Value.ToString() != string.Empty)
            {
                Properties.HasViewDateTo = true;
                Properties.ViewDateTo = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("ViewDateTo").Value.ToString()));
            }
            if (xmlNode.Attributes.GetNamedItem("ViewTimeFrom").Value.ToString() != string.Empty)
            {
                Properties.HasViewTimeFrom = true;
                Properties.ViewTimeFrom = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("ViewTimeFrom").Value.ToString()));
            }
            if (xmlNode.Attributes.GetNamedItem("ViewTimeTo").Value.ToString() != string.Empty)
            {
                Properties.HasViewTimeTo = true;
                Properties.ViewTimeTo = DateTime.FromFileTimeUtc(long.Parse(xmlNode.Attributes.GetNamedItem("ViewTimeTo").Value.ToString()));
            }
            string text2 = xmlNode.Attributes.GetNamedItem("AskForPasswordValue").Value.ToString();
            if (text2 != string.Empty) { Properties.Password = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(text2), "493589549485043859430889230823")); }
            else { Properties.Password = ""; }
            XmlNodeList xmlNodeList = XMLSource.SelectNodes("//DocProperty");
            if (xmlNodeList != null)
            {
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    Properties.dictMetadata.Add(xmlNodeList[i].Attributes.GetNamedItem("name").Value, xmlNodeList[i].Attributes.GetNamedItem("value").Value);
                }
            }
            XmlNodeList xmlNodeList2 = XMLSource.SelectNodes("//Exception");
            if (xmlNodeList2 != null)
            {
                for (int i = 0; i < xmlNodeList2.Count; i++)
                {
                    SlideException ex = new SlideException();
                    if (xmlNodeList2[i].Attributes.GetNamedItem("SlideNumber") != null)
                    {
                        ex.SlideNumber = int.Parse(xmlNodeList2[i].Attributes.GetNamedItem("SlideNumber").Value.ToString());
                    }
                    if (xmlNodeList2[i].Attributes.GetNamedItem("Duration") != null)
                    {
                        ex.StayDuration = decimal.Parse(xmlNodeList2[i].Attributes.GetNamedItem("Duration").Value.ToString().Replace(",", "."), new CultureInfo("en-US"));
                    }
                    if (xmlNodeList2[i].Attributes.GetNamedItem("Transition") != null)
                    {
                        ex.ImageTransition = int.Parse(xmlNodeList2[i].Attributes.GetNamedItem("Transition").Value.ToString());
                    }
                    if (xmlNodeList2[i].Attributes.GetNamedItem("AudioFilepath") != null)
                    {
                        ex.AudioFilepath = xmlNodeList2[i].Attributes.GetNamedItem("AudioFilepath").Value.ToString();
                    }
                    if (xmlNodeList2[i].Attributes.GetNamedItem("Delay") != null)
                    {
                        ex.AudioDelay = decimal.Parse(xmlNodeList2[i].Attributes.GetNamedItem("Delay").Value.ToString().Replace(",", "."), new CultureInfo("en-US"));
                    }
                    Properties.SlideExceptions.Add(ex);
                }
            }
        }
    }
}
