using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DAL
{
    public class DataXml
    {
        public DataSet ReadAll()
        {

            return new DataSet();
        }

        public void Write(string fileName, Hashtable dataToSave) 
        {
            string file = "./xmls/" + fileName + ".xml";

            XDocument doc = LoadDocument(file);

            XElement newElement = new XElement("");

            foreach (DictionaryEntry entry in dataToSave)
            {
                newElement.Add(new XElement(entry.Key.ToString(), entry.Value.ToString().Trim()));
            }

            doc.Save(file);
        }

        public XDocument LoadDocument(string fileName)
        {
            XDocument document;

            if (File.Exists(fileName))
            {
                document = XDocument.Load(fileName);
            }
            else
            {
                document = new XDocument(new XElement(fileName));
            }

            return document;
        }
    }
}
