using Abstraccion;
using BE.models;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP
{
    public class ClienteMapper : IGestor<Cliente>
    {
        public int Create(Cliente entity)
        {
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO Clientes (Nombre, Apellido, DNI) VALUES (@Nombre, @Apellido, @DNI)");

            sqlCommand.Parameters.AddWithValue("@Nombre", entity.Nombre);
            sqlCommand.Parameters.AddWithValue("@Apellido", entity.Apellido);
            sqlCommand.Parameters.AddWithValue("@DNI", entity.DNI);

            int id = -1;

            try
            {
                id = DatabaseSql.WriteAndReturnId(sqlCommand);
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {
            }

            return id;
        }

        public void DeleteById(int Id)
        {
            try
            {
                DatabaseSql.Write(new SqlCommand($"DELETE FROM Clientes WHERE Id = {Id}"));
            }
            catch (SqlException ex)
            {
            }
            catch (Exception ex)
            {

            }
        }

        public List<Cliente> GetAll()
        {
            List<Cliente> clientes = new List<Cliente>();

            DataTable Tabla = null;

            try
            {
                Tabla = DatabaseSql.Read(new SqlCommand("SELECT Id, Nombre, Apellido, DNI FROM Clientes"));
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }

            if (Tabla != null && Tabla.Rows.Count > 0)
            {
                foreach (DataRow fila in Tabla.Rows)
                {
                    clientes.Add(ToMap(fila));
                }
            }

            return clientes;
        }

        public Cliente GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public Cliente ToMap(DataRow row)
        {
            Cliente cliente = new Cliente();

            cliente.Id = Convert.ToInt32(row["Id"]);
            cliente.Nombre = row["Nombre"].ToString();
            cliente.Apellido = row["Apellido"].ToString();
            cliente.DNI = row["DNI"].ToString();

            return cliente;
        }

        public void Update(Cliente docToUpdate, Cliente newData)
        {
            try
            {
                DatabaseSql.Write(new SqlCommand($"UPDATE Clientes SET Nombre = '{newData.Nombre}', Apellido = '{newData.Apellido}', DNI = '{newData.DNI}' WHERE Id = {docToUpdate.Id}"));
            }
            catch (SqlException ex)
            {
            }
            catch (Exception ex)
            {

            }
        }
    }
}
