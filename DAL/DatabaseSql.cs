using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DAL
{
    public class DatabaseSql
    {
        private static SqlConnection GetConnection()
        {
            return new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=ProductosV2;Integrated Security=True;Encrypt=False");
        }

        public static DataTable Read(SqlCommand query, Hashtable queryParams)
        {
            DataTable tabla = new DataTable();
            using (SqlConnection connection = GetConnection())
            {
                try
                {
                    query.Connection = connection;
                    query.CommandType = CommandType.StoredProcedure;

                    AddParamsToQuery(query, queryParams);

                    using (SqlDataAdapter da = new SqlDataAdapter(query))
                    {
                        connection.Open();
                        da.Fill(tabla);
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return tabla;
        }

        public static bool Write(SqlCommand query, Hashtable queryParams)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using(SqlTransaction transaction = connection.BeginTransaction())
                {
                    query.Connection = connection;
                    query.Transaction = transaction;
                    query.CommandType = CommandType.StoredProcedure;

                    AddParamsToQuery(query, queryParams);
                    try
                    {
                        int respuesta = query.ExecuteNonQuery();
                        transaction.Commit();
                        return respuesta > 0;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"No se pudo insertar el dato en la BD {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public static int WriteAndGetId(SqlCommand query, Hashtable queryParams, string outputParamName)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    query.Connection = connection;
                    query.Transaction = transaction;
                    query.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros a la consulta
                    AddParamsToQuery(query, queryParams);

                    // Definir el parámetro de salida para el ID
                    SqlParameter outputParam = new SqlParameter(outputParamName, SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    query.Parameters.Add(outputParam); // Asegúrate de agregar el parámetro de salida

                    try
                    {
                        query.ExecuteNonQuery(); // Ejecutar el stored procedure
                        transaction.Commit();

                        // Capturar y retornar el valor del parámetro de salida
                        int newId = (int)outputParam.Value;
                        return newId;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"No se pudo insertar el dato en la BD {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }


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
