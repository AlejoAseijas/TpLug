using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DatabaseSql
    {
        private static SqlConnection GetConnection()
        {
            return new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=ProductosV2;Integrated Security=True;Encrypt=False");
        }

        public static DataTable Read(SqlCommand query)
        {
            DataTable tabla = new DataTable();
            using (SqlConnection connection = GetConnection())
            {
                try
                {
                    query.Connection = connection;
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

        public static bool Write(SqlCommand query)
        {
            using (SqlConnection connection = GetConnection())
            {
                try
                {
                    query.Connection = connection;
                    connection.Open();
                    int respuesta = query.ExecuteNonQuery();
                    return respuesta > 0;
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
        }

        public static int WriteAndReturnId(SqlCommand query)
        {
            using (SqlConnection connection = GetConnection())
            {
                try
                {
                    query.Connection = connection;
                    connection.Open();

                    query.CommandText += "; SELECT SCOPE_IDENTITY();";

                    object result = query.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int id))
                    {
                        return id;
                    }
                    else
                    {
                        throw new Exception("No se pudo obtener el ID del nuevo registro.");
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
        }
    }
}
