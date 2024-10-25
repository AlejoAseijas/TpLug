using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataManager
    {
        private DataSet DATA_SET = null;
        private static DataManager instance = null;
        private DataXml dataXml = new DataXml();
        private bool fromDB = false;
        private DataManager() {}

        public static DataManager GetInstance()
        {
            return instance = instance == null ? new DataManager() : instance;
        }

        public DataSet GetDataSet(bool force) 
        {

            if(DATA_SET == null || force) 
            {
                if (fromDB)
                {
                    DATA_SET = DataDisconnected.ReadAll();
                }
                else
                {
                    DATA_SET = dataXml.ReadAll();
                }
            }

            return DATA_SET;
        }

        public DataSet GetDataSet()
        {
            return GetDataSet(false);
        }

        public DataTable GetTableByName(string tableName) 
        {
            DATA_SET = DATA_SET == null ? GetDataSet(true) : DATA_SET;
            return DATA_SET.Tables[tableName];
        }
        
        public DataTable getTable(string tableName)
        {
            return this.GetTableByName(tableName);
        }

        public void UpdateDataSet(DataSet data) 
        {
            if (data != null) 
            {
                DATA_SET = data;
            }
        }

        private void UpdateDataSet(string tableName, DataTable table)
        {
            DataSet dataset = this.GetDataSet();

            if (dataset.Tables.Contains(tableName))
            {
                dataset.Tables.Remove(tableName);
                dataset.Tables.Add(table);
            }

            this.UpdateDataSet(dataset);
        }

        //CRUD 
        public int save(string tableName, Hashtable data, string Id)
        {
            DataTable table = getTable(tableName);
            int id = GetLastId(tableName);

            if (table != null)
            {
                DataRow row = table.NewRow();

                data.Add(Id, id);

                foreach (DictionaryEntry entry in data)
                {
                    string columnName = entry.Key.ToString();

                    if (table.Columns.Contains(columnName))
                    {
                        row[columnName] = entry.Value ?? DBNull.Value;
                    }
                }

                try
                {
                    table.Rows.Add(row);
                    UpdateDataSet(tableName, table);

                    if (!fromDB) 
                    {
                        dataXml.Write(tableName, data);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al guardar datos: {ex.Message}");

                }
            }
            return id;

        }

        public bool delete(string tableName, int id, string idColumn)
        {
            DataTable table = getTable(tableName);
            bool delete = false;

            if (table != null)
            {
                DataRow[] rowsToDelete = table.Select($"{idColumn} = {id}");

                if (rowsToDelete.Length > 0)
                {
                    try
                    {
                        foreach (DataRow row in rowsToDelete)
                        {
                            table.Rows.Remove(row);
                        }

                        UpdateDataSet(tableName, table);

                        if (!fromDB) 
                        {
                            dataXml.DeleteById(tableName, idColumn, id);
                        }

                        delete = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al eliminar la fila: {ex.Message}");
                    }
                }

            }


            return delete;
        }

        public void update(string tableName, string idColumn, int id, Hashtable data)
        {
            DataTable table = getTable(tableName);

            if (table != null)
            {
                DataRow[] rowsToUpdate = table.Select($"{idColumn} = {id}");

                if (rowsToUpdate.Length > 0)
                {
                    try
                    {
                        DataRow rowToUpdate = rowsToUpdate[0];

                        foreach (DictionaryEntry entry in data)
                        {
                            string columnName = entry.Key.ToString();

                            if (table.Columns.Contains(columnName))
                            {
                                rowToUpdate[columnName] = entry.Value ?? DBNull.Value;
                            }
                        }

                        UpdateDataSet(tableName, table);
                        
                        if (!fromDB) 
                        {
                            dataXml.DeleteById(tableName, idColumn, id);
                            data.Add(idColumn, id);
                            dataXml.Write(tableName, data);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al actualizar la fila: {ex.Message}");
                    }
                }

            }
        }

        //Utils
        private int GetLastId(string tableName)
        {
            int id = -1;
            DataTable table = getTable(tableName);

            if (table != null)
            {
                if (table.Rows.Count > 0)
                {
                    id = Convert.ToInt32(table.Rows[table.Rows.Count - 1][0]);
                    id++;
                }
                else
                {
                    id = 0;
                }
            }


            return id;
        }
    }
}
