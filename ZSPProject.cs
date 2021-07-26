using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FourDepack.ProjectFileTypes;
using FourDepack.Security;

namespace FourDepack
{
    public class ZSPProject
    {
        public XmlDocument XMLSource = new XmlDocument();
        public List<string> Languages = new List<string>();
        public ZSPProjectPropeties MainProperties = new ZSPProjectPropeties();
        public List<ZSPProjectPropeties> LanguageProperties = new List<ZSPProjectPropeties>();

        public ZSPProject(string RawXML)
        {
            XMLSource.LoadXml(RawXML);
            InitializeProject();
        }

        public void Refresh() { InitializeProject(); }

        private void InitializeProject()
        {
            XmlNode xmlNode = XMLSource.FirstChild.SelectSingleNode("./Encrypted");
            MainProperties.Encrypted = (xmlNode.InnerText == bool.TrueString);
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./EncryptionAlgorithm");
            MainProperties.EncryptionAlgorithm = int.Parse(xmlNode.InnerText);
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./AskForPassword");
            MainProperties.AskForPassword = (xmlNode.InnerText == bool.TrueString);
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./Password");
            MainProperties.Password = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.InnerText), "BAD0B46C-EDB8-4BAE-B538-F7C99556A023"));
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./EncryptionPassword");
            MainProperties.EncryptionPassword = Encoding.Default.GetString(CryptoHelper.DecryptBytes(Convert.FromBase64String(xmlNode.InnerText), "BAD0B46C-EDB8-4BAE-B538-F7C99556A023"));
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./UnzipDirectory");
            MainProperties.UnzipDirectory = xmlNode.InnerText;
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./BrowseDirectory");
            MainProperties.BrowseDirectory = (xmlNode.InnerText == bool.TrueString);
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./UnzipAutomatically");
            MainProperties.UnzipAutomatically = (xmlNode.InnerText == bool.TrueString);
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./AutoSelectLanguage");
            MainProperties.AutoSelectLanguage = (xmlNode.InnerText == bool.TrueString);
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./CommandsBefore");
            XmlNodeList xmlNodeList = xmlNode.SelectNodes("./Command");
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                Command command = new Command();
                command.Filename = xmlNodeList[i].Attributes["Filename"].Value;
                command.Arguments = xmlNodeList[i].Attributes["Arguments"].Value;
                command.HideWindow = (xmlNodeList[i].Attributes["HideWindow"].Value == bool.TrueString);
                command.WaitForExit = (xmlNodeList[i].Attributes["WaitForExit"].Value == bool.TrueString);
                MainProperties.CommandsBefore.Add(command);
            }
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./CommandsAfter");
            xmlNodeList = xmlNode.SelectNodes("./Command");
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                Command command = new Command();
                command.Filename = xmlNodeList[i].Attributes["Filename"].Value;
                command.Arguments = xmlNodeList[i].Attributes["Arguments"].Value;
                command.HideWindow = (xmlNodeList[i].Attributes["HideWindow"].Value == bool.TrueString);
                command.WaitForExit = (xmlNodeList[i].Attributes["WaitForExit"].Value == bool.TrueString);
                MainProperties.CommandsAfter.Add(command);
            }
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./Shortcuts");
            XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("./Shortcut");
            for (int i = 0; i < xmlNodeList2.Count; i++)
            {
                Shortcut shortcut = new Shortcut();
                shortcut.FileShortcut = (xmlNodeList2[i].Attributes["FileShortcut"].Value == bool.TrueString);
                shortcut.Filename = xmlNodeList2[i].Attributes["Filename"].Value;
                shortcut.Description = xmlNodeList2[i].Attributes["Description"].Value;
                shortcut.IconLocation = xmlNodeList2[i].Attributes["IconLocation"].Value;
                shortcut.TargetPath = xmlNodeList2[i].Attributes["TargetPath"].Value;
                shortcut.WorkingDirectory = xmlNodeList2[i].Attributes["WorkingDirectory"].Value;
                shortcut.Arguments = xmlNodeList2[i].Attributes["Arguments"].Value;
                shortcut.URL = xmlNodeList2[i].Attributes["URL"].Value;
                MainProperties.Shortcuts.Add(shortcut);
            }
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./WindowTitle");
            MainProperties.WindowTitle = xmlNode.InnerText;
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./MessageBefore");
            MainProperties.MessageBefore = xmlNode.InnerText;
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./MessageAfter");
            MainProperties.MessageAfter = xmlNode.InnerText;
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./AboutText");
            MainProperties.AboutText = xmlNode.InnerText;
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./BrandingText");
            MainProperties.BrandingText = xmlNode.InnerText;
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./UnzipButtonText");
            MainProperties.UnzipButtonText = xmlNode.InnerText;
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./HeaderImage");
            MainProperties.HeaderImage = xmlNode.InnerText;
            xmlNode = XMLSource.FirstChild.SelectSingleNode("./PromptType");
            MainProperties.PromptType = int.Parse(xmlNode.InnerText);
            XmlNodeList xmlNodeList3 = XMLSource.FirstChild.SelectNodes("//Language");
            for (int i = 0; i < xmlNodeList3.Count; i++)
            {
                string value = xmlNodeList3[i].Attributes["Description"].Value;
                Languages.Add(value);
                if (i != 0)
                {
                    ZSPProjectPropeties LanguageProperty = new ZSPProjectPropeties();
                    XmlNode xmlNode2 = xmlNodeList3[i];                    
                    xmlNode = xmlNode2.SelectSingleNode("./WindowTitle");
                    LanguageProperty.WindowTitle = xmlNode.InnerText;
                    xmlNode = xmlNode2.SelectSingleNode("./MessageBefore");
                    LanguageProperty.MessageBefore = xmlNode.InnerText;
                    xmlNode = xmlNode2.SelectSingleNode("./MessageAfter");
                    LanguageProperty.MessageAfter = xmlNode.InnerText;
                    xmlNode = xmlNode2.SelectSingleNode("./AboutText");
                    LanguageProperty.AboutText = xmlNode.InnerText;
                    xmlNode = xmlNode2.SelectSingleNode("./BrandingText");
                    LanguageProperty.BrandingText = xmlNode.InnerText;
                    xmlNode = xmlNode2.SelectSingleNode("./UnzipButtonText");
                    LanguageProperty.UnzipButtonText = xmlNode.InnerText;
                    xmlNode = xmlNode2.SelectSingleNode("./HeaderImage");
                    LanguageProperty.HeaderImage = xmlNode.InnerText;
                    xmlNode = xmlNode2.FirstChild.SelectSingleNode("./PromptType");
                    LanguageProperty.PromptType = int.Parse(xmlNode.InnerText);
                    xmlNode = xmlNode2.SelectSingleNode("./Icon");
                    LanguageProperty.Icon = xmlNode.InnerText;
                    XmlNode xmlNode3 = xmlNode2.SelectSingleNode("./CommandsBefore");
                    XmlNodeList xmlNodeList4 = xmlNode3.SelectNodes("./Command");
                    for (int j = 0; j < xmlNodeList4.Count; j++)
                    {
                        Command command = new Command();
                        xmlNode = xmlNodeList4[j];
                        command.Filename = xmlNode.Attributes["Filename"].Value;
                        command.Arguments = xmlNode.Attributes["Arguments"].Value;
                        command.HideWindow = (xmlNode.Attributes.GetNamedItem("HideWindow").Value == bool.TrueString);
                        command.WaitForExit = (xmlNode.Attributes.GetNamedItem("WaitForExit").Value == bool.TrueString);
                        LanguageProperty.CommandsBefore.Add(command);
                    }
                    xmlNode3 = xmlNode2.SelectSingleNode("./CommandsAfter");
                    xmlNodeList4 = xmlNode3.SelectNodes("./Command");
                    for (int j = 0; j < xmlNodeList4.Count; j++)
                    {
                        Command command = new Command();
                        xmlNode = xmlNodeList4[j];
                        command.Filename = xmlNode.Attributes["Filename"].Value;
                        command.Arguments = xmlNode.Attributes["Arguments"].Value;
                        command.HideWindow = (xmlNode.Attributes.GetNamedItem("HideWindow").Value == bool.TrueString);
                        command.WaitForExit = (xmlNode.Attributes.GetNamedItem("WaitForExit").Value == bool.TrueString);
                        LanguageProperty.CommandsAfter.Add(command);
                    }
                    XmlNode xmlNode4 = xmlNode2.SelectSingleNode("./Shortcuts");
                    XmlNodeList xmlNodeList5 = xmlNode4.SelectNodes("./Shortcut");
                    for (int j = 0; j < xmlNodeList5.Count; j++)
                    {
                        Shortcut shortcut = new Shortcut();
                        shortcut.FileShortcut = (xmlNodeList5[j].Attributes["FileShortcut"].Value == bool.TrueString);
                        shortcut.Description = xmlNodeList5[j].Attributes["Description"].Value;
                        shortcut.IconLocation = xmlNodeList5[j].Attributes["IconLocation"].Value;
                        shortcut.TargetPath = xmlNodeList5[j].Attributes["TargetPath"].Value;
                        shortcut.URL = xmlNodeList5[j].Attributes["URL"].Value;
                        LanguageProperty.Shortcuts.Add(shortcut);
                    }
                    LanguageProperties.Add(LanguageProperty);
                }
            }
        }
    }
}
