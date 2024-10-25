using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraccion
{
    public class PersistibleService
    {
        private DataManager dataManager = DataManager.GetInstance();

        public DataTable getTable(string tableName)
        {
            return dataManager.GetTableByName(tableName);
        }

        public int save(string tableName, Hashtable data, string Id)
        {
            DataTable table = getTable(tableName);
            int id = GetLastId(tableName);

            if (table != null)
            {
                DataRow row = table.NewRow();
                
                row[Id] = id;

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
                    updateDataSet(tableName, table);
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

                        updateDataSet(tableName, table);

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

                        updateDataSet(tableName, table);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al actualizar la fila: {ex.Message}");
                    }
                }
        
            }
        }


        private void updateDataSet(string tableName, DataTable table)
        {
            DataSet dataset = dataManager.GetDataSet();

            if (dataset.Tables.Contains(tableName))
            {
                dataset.Tables.Remove(tableName);
                dataset.Tables.Add(table);
            }

            dataManager.UpdateDataSet(dataset);
        }

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
