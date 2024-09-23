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
        private ClienteMapper clienteMapper = new ClienteMapper();

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
                Tabla = DatabaseSql.Read(new SqlCommand("SELECT * FROM ClienteVenta cv  INNER JOIN Clientes c ON cv.IdCliente = c.IdCliente INNER JOIN Ventas v ON cv.IdVenta = v.IdVenta ORDER BY cv.IdCliente"));
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }

            if (Tabla != null && Tabla.Rows.Count > 0)
            {
                Dictionary<Cliente, List<Venta>> ventasPorCliente = new Dictionary<Cliente, List<Venta>>();

                foreach (DataRow fila in Tabla.Rows)
                {
                    Cliente cliente = clienteMapper.ToMap(fila);
                    Venta venta = VentaMapper.ToMap(fila);

                    if (cliente != null) 
                    {
                        if (ventasPorCliente.ContainsKey(cliente))
                        {
                            ventasPorCliente[cliente].Add(venta);
                        }
                        else
                        {
                            ventasPorCliente[cliente] = new List<Venta> { venta };
                        }
                    }
                }

                foreach (var entry in ventasPorCliente)
                {
                    ClienteVenta clienteVenta = new ClienteVenta
                    {
                        Cliente = entry.Key,
                        Ventas = entry.Value
                    };
                    clientes.Add(clienteVenta);
                }
            }

            return clientes;
        }

        public ClienteVenta GetById(string Id)
        {
            ClienteVenta clientes = new ClienteVenta();

            DataTable Tabla = null;

            try
            {
                Tabla = DatabaseSql.Read(new SqlCommand("SELECT * FROM ClienteVenta cv  INNER JOIN Clientes c ON cv.IdCliente = c.IdCliente INNER JOIN Ventas v ON cv.IdVenta = v.IdVenta ORDER BY cv.IdCliente"));
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }

            if (Tabla != null && Tabla.Rows.Count > 0)
            {
                Dictionary<Cliente, List<Venta>> ventasPorCliente = new Dictionary<Cliente, List<Venta>>();

                foreach (DataRow fila in Tabla.Rows)
                {
                    Cliente cliente = clienteMapper.ToMap(fila);
                    Venta venta = VentaMapper.ToMap(fila);

                    if (cliente != null)
                    {
                        if (ventasPorCliente.ContainsKey(cliente))
                        {
                            ventasPorCliente[cliente].Add(venta);
                        }
                        else
                        {
                            ventasPorCliente[cliente] = new List<Venta> { venta };
                        }
                    }
                }

                if (ventasPorCliente.Count > 0)
                {
                    // Tomamos la primera entrada del diccionario (asumimos que hay un único cliente)
                    var entry = ventasPorCliente.First();
                    clientes = new ClienteVenta
                    {
                        Cliente = entry.Key,
                        Ventas = entry.Value
                    };
                }
            }

            return clientes;
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
            throw new NotImplementedException();
        }

        public void Update(ClienteVenta docToUpdate, ClienteVenta newData)
        {
            try
            {
                var query = $"UPDATE ClienteVenta SET IdCliente = {newData.Cliente.Id}, IdVenta = {newData.Ventas[0].Id} " +
                            $"WHERE IdCliente = {docToUpdate.Cliente.Id} AND IdVenta = {docToUpdate.Ventas[0].Id}";

                DatabaseSql.Write(new SqlCommand(query));
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
