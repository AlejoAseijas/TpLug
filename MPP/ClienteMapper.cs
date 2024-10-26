using Abstraccion;
using BE.models;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP
{
    public class ClienteMapper : IMappable<Cliente>
    {
        public string TABLE_NAME => "Clientes";

        public string ID_COLUMN => "IdCliente";

        public int Create(Cliente entity)
        {
            Hashtable queryParams = new Hashtable { { "@Nombre", entity.Nombre}, {"@Apellido", entity.Apellido}, { "@DNI", entity.DNI} };
            SqlCommand sqlCommand = new SqlCommand("CreateCliente");

            int id = -1;

            try
            {
                DatabaseSql.Write(sqlCommand, queryParams);
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
                Hashtable queryParams = new Hashtable { { "@IdCliente", Id } };

                DatabaseSql.Write(new SqlCommand("DeleteClienteById"), queryParams);
            }
            catch (SqlException ex)
            {
            }
            catch (Exception ex)
            {

            }
        }

        //public List<Cliente> GetAll()
        //{
        //    List<Cliente> clientes = new List<Cliente>();

        //    DataTable Tabla = null;

        //    try
        //    {
        //        Tabla = DatabaseSql.Read(new SqlCommand("GetAllClientes"), null).Tables[0];
        //    }
        //    catch (SqlException ex)
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    if (Tabla != null && Tabla.Rows.Count > 0)
        //    {
        //        foreach (DataRow fila in Tabla.Rows)
        //        {
        //            clientes.Add(ToMap(fila));
        //        }
        //    }

        //    return clientes;
        //}

        public DataTable GetAll()
        {
            DataTable dataTable = null;

            try
            {
                dataTable = DatabaseSql.Read(new SqlCommand("GetAllClientes"), null);
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }

            return dataTable;
        }

        public Cliente GetById(string Id)
        {
            Cliente clientes = new Cliente();

            DataTable Tabla = null;

            try
            {
                Hashtable queryParams = new Hashtable { { "@IdCliente", int.Parse(Id) } };
                Tabla = DatabaseSql.Read(new SqlCommand("GetClienteById"), null);
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
                    clientes = ToMap(fila);
                }
            }

            return clientes;
        }

        public Cliente ToMap(DataRow row)
        {
            Cliente cliente = new Cliente();

            cliente.Id = Convert.ToInt32(row["IdCliente"]);
            cliente.Nombre = row["Nombre"].ToString();
            cliente.Apellido = row["Apellido"].ToString();
            cliente.DNI = row["DNI"].ToString();

            return cliente;
        }

        public void Update(Cliente docToUpdate, Cliente newData)
        {
            try
            {
                Hashtable queryParams = new Hashtable { { "@IdClienteActualizar", docToUpdate.Id }, { "@NombreNuevo", newData.Nombre }, { "@ApellidoNuevo", newData.Apellido }, { "@DNINuevo", newData.DNI} };
                DatabaseSql.Write(new SqlCommand($"UpdateClienteById"), queryParams);
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
