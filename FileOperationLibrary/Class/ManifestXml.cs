using System;
using System.Xml;

namespace FileOperationLibrary
{
    public class ManifestPlist
    {
        public void CreateManifest(string plistPath, string ipaPath, string bundleIdentifier, string bundleVersion, string projectName)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");
            settings.CloseOutput = true;
            settings.OmitXmlDeclaration = true;
            using (XmlWriter writer = XmlWriter.Create(plistPath + "/manifest.plist", settings))
            {
                writer.WriteStartElement("plist");
                writer.WriteAttributeString("version", "1.0");
                writer.WriteStartElement("dict");
                writer.WriteElementString("key", "items");
                writer.WriteStartElement("array");
                writer.WriteStartElement("dict");
                writer.WriteElementString("key", "assets");
                writer.WriteStartElement("array");
                writer.WriteStartElement("dict");
                writer.WriteElementString("key", "kind");
                writer.WriteElementString("string", "software-package");
                writer.WriteElementString("key", "url");
                writer.WriteElementString("string", ipaPath);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteElementString("key", "metadata");
                writer.WriteStartElement("dict");
                writer.WriteElementString("key", "bundle-identifier");
                writer.WriteElementString("string", bundleIdentifier);
                writer.WriteElementString("key", "bundle-version");
                writer.WriteElementString("string", bundleVersion);
                writer.WriteElementString("key", "kind");
                writer.WriteElementString("string", "software");
                writer.WriteElementString("key", "title");
                writer.WriteElementString("string", projectName);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Flush();
            }
        }
    }
}
