using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace DAL
{
    public class DataDisconnected
    {
        private static DataSet dataSet = null;
        public static DataTable Read(string tableName, bool force) 
        {

            if (dataSet == null | force) 
            {
                //Cargo en el dataSet la tabla
                Read(tableName);
            }

            return dataSet.Tables[tableName];
        }
        private static DataTable Read(string tableName)
        {

            using (SqlConnection connection = DatabaseSql.GetConnection())
            {
                SqlCommand query = new SqlCommand("SELECT * FROM " + tableName);
                try
                {
                    query.Connection = connection;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query))
                    {
                        connection.Open();
                        dataSet = new DataSet();
                        adapter.Fill(dataSet);
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Error de SQL: {ex.Message}");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dataSet.Tables[tableName];
        }
        public static DataSet ReadAll() 
        {
           DataTable TablesNames = DatabaseSql.Read(new SqlCommand("GetAllTablesNames"), null);

            dataSet = dataSet == null ? new DataSet() : dataSet;

            if (TablesNames != null) 
            {
                foreach (DataRow Row in TablesNames.Rows) 
                {
                    string storeName = "GetAll" + Row[0].ToString();

                    DataTable data = DatabaseSql.Read(new SqlCommand(storeName), null);

                    if (data != null) 
                    {

                        DataTable clonedTable = data.Copy();
                        clonedTable.TableName = Row[0].ToString();

                        dataSet.Tables.Add(clonedTable);
                    }

                }
            }

            return dataSet;
        }
        public static void UpdateDB(DataTable table)
        {
            using (SqlConnection connection = DatabaseSql.GetConnection())
            {
                try
                {
                    if (table.TableName != "Table") 
                    {
                        SqlCommand query = new SqlCommand("SELECT * FROM " + table.TableName);
                        query.Connection = connection;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(query))
                        {
                            SqlCommandBuilder Cb = new SqlCommandBuilder(adapter);

                            adapter.UpdateCommand = Cb.GetUpdateCommand();
                            adapter.DeleteCommand = Cb.GetDeleteCommand();
                            adapter.InsertCommand = Cb.GetInsertCommand();
                            adapter.ContinueUpdateOnError = true;

                            adapter.Fill(dataSet);

                            adapter.Update(table);
                        }
                    } 
                    
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Error de SQL al actualizar: {table.TableName}, {ex.Message}");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public static int Write(string tableName, Hashtable data, string IdColumn) 
        {
            try 
            {
                DataTable table = dataSet.Tables[tableName];

                if (table != null)
                {
                    DataRow row = table.NewRow();

                    foreach (DictionaryEntry entry in data)
                    {
                        string columnName = entry.Key.ToString();

                        if (table.Columns.Contains(columnName))
                        {
                            row[columnName] = entry.Value ?? DBNull.Value;
                        }
                    }

                    table.Rows.Add(row);
                    UpdateDataSet(tableName, table);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error al guardar datos: {ex.Message}");
            }

            return Convert.ToInt32(data[IdColumn]);
        }
        public static bool delete(string tableName, int id, string idColumn)
        {
            DataTable table = dataSet.Tables[tableName];
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
        public static bool update(string tableName, int id, string idColumn, Hashtable data)
        {
            bool updated = false;

            if (dataSet.Tables.Contains(tableName))
            {
                DataTable table = dataSet.Tables[tableName];

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
                        updated = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al actualizar la fila en {tableName}: {ex.Message}");
                    }
                }
            }

            return updated;
        }
        private static void UpdateDataSet(string tableName, DataTable table)
        {

            if (dataSet.Tables.Contains(tableName))
            {
                dataSet.Tables.Remove(tableName);
                dataSet.Tables.Add(table);
            }

        }

    }

}
