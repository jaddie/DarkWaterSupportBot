using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DarkwaterSupportBot
{
    class XmlStorage
    {
        // Insertion
        public void InsertValue(string helpname,string helptext)
        {
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Indent = true; ;

            writerSettings.OmitXmlDeclaration = true;
            writerSettings.Encoding = Encoding.ASCII;
            string path = @"CommandsDB\" + "IrcAddedCommands.xml";

            using (XmlWriter writer = XmlWriter.Create(path, writerSettings))
            {
                if (writer != null)
                {
                    writer.WriteStartElement("Command");
                    writer.WriteStartAttribute("CmdName");
                    writer.WriteValue(helpname);
                    writer.WriteEndAttribute();
                    writer.WriteStartAttribute("CmdText");
                    writer.WriteValue(helptext);
                    writer.WriteEndAttribute();
                    writer.WriteEndElement();


                    writer.Flush();
                }
            }
        }

          // Deletion
          private void DeleteValue(string helpname)
          {
              try
              {
                  string path = @"CommandsDB\" +  "IrcAddedCommands.xml";
                  File.Delete(path);
                  MessageBox.Show("record for ID:" + txtDel.Text + " is deleted");
                  txtDel.Text = "";
              }
              catch (Exception f)
              {
                  MessageBox.Show("The desired record is not available");
              }
          }

          //Search
          private void SearchValue(string helpname)
          {
              try
              {
                  string path = @"CommandsDB\" + txtQuery.Text.ToString() + ".xml";
                  XmlDocument document = new XmlDocument();
                  document.Load(path);
                  XmlNode node = document.SelectSingleNode(@"//*");
                  rtbResults.Text = node.OuterXml.ToString();
                  btnUpdate.Enabled = true;
              }
              catch (Exception f)
              {
                  MessageBox.Show("ID:" + txtQuery.Text + " not found");
                  txtQuery.Text = "";
                  rtbResults.Text = "";
              }
          }
    }
}
