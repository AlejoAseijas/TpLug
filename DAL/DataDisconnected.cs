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

        public static DataSet Read(string tableName, bool force) 
        {

            if (dataSet == null | force) 
            {
                dataSet = Read(tableName);
            }

            return dataSet;
        }

        private static DataSet Read(string tableName)
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
            return dataSet;
        }

        public static void Update(DataSet dataSet, string tableName)
        {
            using (SqlConnection connection = DatabaseSql.GetConnection())
            {
                try
                {
                    SqlCommand query = new SqlCommand("SELECT * FROM " + tableName);
                    query.Connection = connection;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query))
                    {
                        SqlCommandBuilder Cb = new SqlCommandBuilder(adapter);

                        adapter.UpdateCommand = Cb.GetUpdateCommand();
                        adapter.DeleteCommand = Cb.GetDeleteCommand();
                        adapter.InsertCommand = Cb.GetInsertCommand();
                        adapter.ContinueUpdateOnError = true;
                        adapter.Fill(dataSet);

                        adapter.Update(dataSet.Tables[0]);
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Error de SQL al actualizar: {ex.Message}");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

    }

}
