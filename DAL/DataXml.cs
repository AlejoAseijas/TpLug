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

                XElement element = doc.Root;

                if (element == null)
                {
                    element = new XElement("Clientes");
                    doc.Add(element);
                }

                XElement newElement = new XElement("Item");

                foreach (DictionaryEntry entry in dataToSave)
                {
                    newElement.Add(new XElement(entry.Key.ToString(), entry.Value.ToString().Trim()));
                }

                element.Add(newElement);

                doc.Save(file);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar el archivo XML: " + ex.Message);
            }
        }

        public DataSet ReadAll()
        {
            DataSet dataSet = new DataSet();

            List<string> files = getAllFileNames();

            foreach (string file in files)
            {
                DataTable dt = this.Read(file);

                if (dt != null)
                {
                    dataSet.Tables.Add(dt);
                }
            }

            return dataSet;
        }

        public DataTable Read(string fileName)
        {
            // Crear un DataTable con el nombre del archivo
            DataTable dataTable = new DataTable(fileName);
            dataTable.TableName = fileName;

            try
            {
                XDocument doc = XDocument.Load(fileName+".xml");

                XElement element = doc.Root;

                if (element != null)
                {
                    XElement firstItem = element.Element("Item");
                    if (firstItem != null)
                    {
                        foreach (XElement child in firstItem.Elements())
                        {
                            dataTable.Columns.Add(child.Name.ToString(), typeof(string));
                        }

                        foreach (XElement item in element.Elements("Item"))
                        {
                            DataRow row = dataTable.NewRow();
                            foreach (XElement child in item.Elements())
                            {
                                row[child.Name.ToString()] = child.Value.Trim();
                            }
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

        public void DeleteById(string fileName, string IdColumn, int Id)
        {
            try
            {
                string file = fileName + ".xml";

                XDocument doc = XDocument.Load(file);

                XElement element = doc.Root;
                if (element != null)
                {
                    XElement itemToDelete = element.Elements("Item")
                        .FirstOrDefault(item =>
                            (int)item.Element(IdColumn) == Id);

                    if (itemToDelete != null)
                    {
                        itemToDelete.Remove();

                        doc.Save(file);
                    }
              
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar el elemento del archivo XML: " + ex.Message);
            }
        }

        private XDocument LoadDocument(string fileName)
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

        private List<string> getAllFileNames()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            string[] xmlFiles = Directory.GetFiles(directoryPath, "*.xml");
            List<string> fileNames = new List<string>();

            foreach (string file in xmlFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                fileNames.Add(fileName);
            }

            return fileNames;
        }
    }
}
