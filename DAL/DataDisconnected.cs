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
        public static DataSet Read(SqlCommand query, Hashtable queryParams)
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection connection = DatabaseSql.GetConnection())
            {
                try
                {
                    query.Connection = connection;
                    query.CommandType = CommandType.StoredProcedure;

                    AddParamsToQuery(query, queryParams);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query))
                    {
                        connection.Open();
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

        public static void Update(SqlCommand query, DataSet dataSet, string tableName)
        {
            using (SqlConnection connection = DatabaseSql.GetConnection())
            {
                try
                {
                    query.Connection = connection;
                    query.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query))
                    {
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                        adapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                        adapter.DeleteCommand = commandBuilder.GetDeleteCommand();
                        adapter.InsertCommand = commandBuilder.GetInsertCommand();

                        connection.Open();

                        adapter.Update(dataSet, tableName);
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

        // Método privado para agregar parámetros al SqlCommand
        private static void AddParamsToQuery(SqlCommand query, Hashtable queryParams)
        {
            if (queryParams != null && queryParams.Count > 0)
            {
                foreach (string queryParam in queryParams.Keys)
                {
                    query.Parameters.AddWithValue(queryParam, queryParams[queryParam]);
                }
            }
        }
    }

}
