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

        public void Write(string fileName, Hashtable dataToSave)
        {
            try
            {
                string file = fileName + ".xml";

                XDocument doc = LoadDocument(file);

                XElement root = doc.Root;

                if (root == null)
                {
                    root = new XElement("Clientes");
                    doc.Add(root);
                }

                XElement newElement = new XElement("Item");

                foreach (DictionaryEntry entry in dataToSave)
                {
                    newElement.Add(new XElement(entry.Key.ToString(), entry.Value.ToString().Trim()));
                }

                root.Add(newElement);

                doc.Save(file);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar el archivo XML: " + ex.Message);
            }
        }

        public DataTable Read(string fileName)
        {
            DataTable dataTable = new DataTable(fileName);

            try
            {
                // Cargar el documento XML
                XDocument doc = XDocument.Load(fileName);

                dataTable.Columns.Add("Key", typeof(string));
                dataTable.Columns.Add("Value", typeof(string));

                XElement root = doc.Root;

                if (root != null)
                {
                    foreach (XElement item in root.Elements("Item"))
                    {
                        foreach (XElement child in item.Elements())
                        {
                            // Crear una nueva fila
                            DataRow row = dataTable.NewRow();
                            row["Key"] = child.Name.ToString();
                            row["Value"] = child.Value.Trim();
                            dataTable.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el archivo XML: " + ex.Message);
            }

            return dataTable;
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
                // Crear un nuevo documento con un elemento raíz válido
                document = new XDocument(new XElement("Clientes"));
            }

            return document;
        }

    }
}
