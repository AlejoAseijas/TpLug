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
    public class ClienteVentaMapper : IGestor<ClienteVenta>
    {
        private VentaMapper VentaMapper = new VentaMapper();

        public int Create(ClienteVenta entity)
        {
            int id = -1;

            if (entity != null && entity.Ventas != null)
            {
                int IdVenta = -1;

                try
                {
                    IdVenta = VentaMapper.Create(entity.Ventas.First());

                    if (IdVenta != -1)
                    {
                        SqlCommand sqlCommand = new SqlCommand($"INSERT INTO ClienteVenta(IdVenta, IdCliente) VALUES ({IdVenta}, {entity.Cliente.Id})");
                        id = DatabaseSql.WriteAndReturnId(sqlCommand);
                    }
                }
                catch (SqlException ex) { }
                catch (Exception ex)
                {

                }
            }

            return id;
        }

        public void DeleteById(int Id)
        {
            SqlCommand sqlCommand = new SqlCommand($"Delete FROM ClienteVenta Where IdClienteVenta = {Id}");

            try
            {
                VentaMapper.DeleteById(Id);
                DatabaseSql.Write(sqlCommand);
            }
            catch (SqlException ex)
            { }
            catch (Exception ex)
            { }
        }

        public List<ClienteVenta> GetAll()
        {
            List<ClienteVenta> clientes = new List<ClienteVenta>();

            DataTable Tabla = null;

            try
            {
                Tabla = DatabaseSql.Read(new SqlCommand("SELECT IdClienteVenta, Nombre, Apellido, DNI FROM Clientes"));
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

        public ClienteVenta GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public List<Venta> GetVentasByIdCliente(int IdCliente)
        {
            List<Venta> ventas = new List<Venta>();

            DataTable Tabla = null;

            try
            {
                Tabla = DatabaseSql.Read(new SqlCommand($"SELECT cv.Id as 'IdClienteVenta', cv.IdCliente, cv.IdVenta, v.Id as 'IdVenta', v.PrecioVenta, v.Producto, v.Qty FROM ClienteVenta cv INNER JOIN Ventas v ON cv.IdVenta = v.Id Where cv.IdCliente = {IdCliente}"));
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
                    ventas.Add(VentaMapper.ToMap(fila));
                }
            }

            return ventas;
        }

        public ClienteVenta ToMap(DataRow row)
        {
            ClienteVenta clienteVenta = new ClienteVenta();


            return clienteVenta;
        }

        public void Update(ClienteVenta docToUpdate, ClienteVenta newData)
        {
            throw new NotImplementedException();
        }
    }
}
