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
    public class InventarioMapper : IMappable<Inventario>
    {
        private ProductoMapper productoMapper = new ProductoMapper();

        public int Create(Inventario entity)
        {
            int id = -1;
            
            if (entity != null && entity.Producto != null)
            {
                int idProducto = -1;

                try
                {
                    idProducto = productoMapper.Create(entity.Producto);

                    if (idProducto != -1)
                    {
                        SqlCommand sqlCommand = new SqlCommand($"INSERT INTO Inventarios(IdProducto, Stock) VALUES ({idProducto}, {entity.Stock})");
                        id = DatabaseSql.WriteAndReturnId(sqlCommand);
                    }
                }
                catch(SqlException ex) { }
                catch (Exception ex)
                {

                }
            }

            return id;
        }

        public void DeleteById(int Id)
        {
            Inventario inventario = GetById(Id.ToString());

            if(inventario != null)
            {
                try 
                {
                    productoMapper.DeleteById(inventario.Producto.Id);
                    DatabaseSql.Write(new SqlCommand($"DELETE FROM Inventarios Where IdInventario = {Id}"));
                }
                catch (SqlException ex)
                { }
                catch(Exception ex) { }
            }

        }

        public List<Inventario> GetAll()
        {
            List<Inventario> inventarios = new List<Inventario>();
            string query = @"SELECT i.IdInventario, i.Stock, p.IdProducto as 'IdProducto', p.Nombre, p.Categoria, p.SubCategoria, p.FechaDeVencimiento, p.PrecioCosto ,p.Consumo, po.IdProveedor as 'IdProveedor', po.Nombre as 'Proveedor' FROM Inventarios i INNER JOIN Productos p ON i.IdProducto = p.IdProducto INNER JOIN Proveedores po ON po.IdProveedor = p.IdProveedor";
            DataTable Tabla = null;

            try
            {
                Tabla = DatabaseSql.Read(new SqlCommand(query));
            }
            catch (SqlException)
            {
            }
            catch (Exception ex)
            {
            }

            if (Tabla != null && Tabla.Rows.Count > 0)
            {
                foreach (DataRow fila in Tabla.Rows)
                {
                    inventarios.Add(ToMap(fila));
                }
            }
            return inventarios;
        }

        public Inventario GetById(string Id)
        {
            Inventario inventario = null;
            string query = $"SELECT i.IdInventario, i.Stock, p.IdProducto as 'IdProducto', p.Nombre, p.Categoria, p.SubCategoria, p.FechaDeVencimiento, p.PrecioCosto ,p.Consumo, po.IdProveedor as 'IdProveedor', po.Nombre as 'Proveedor' FROM Inventarios i INNER JOIN Productos p ON i.IdProducto = p.IdProducto INNER JOIN Proveedores po ON po.IdProveedor = p.IdProveedor WHERE i.IdInventario = {Id}";
            DataTable Tabla = null;

            try
            {
                Tabla = DatabaseSql.Read(new SqlCommand(query));
            }
            catch (SqlException)
            {
            }
            catch (Exception ex)
            {
            }

            if (Tabla != null && Tabla.Rows.Count > 0)
            {
                foreach (DataRow fila in Tabla.Rows)
                {
                    inventario = ToMap(fila);
                }
            }
            return inventario;
        }

        public Inventario ToMap(DataRow row)
        {
            Inventario inventario = new Inventario();

            inventario.Id = Convert.ToInt32(row["IdInventario"]);
            inventario.Stock = Convert.ToInt32(row["Stock"].ToString());
            inventario.Producto = productoMapper.ToMap(row);

            return inventario;
        }

        public void Update(Inventario docToUpdate, Inventario newData)
        {
            try
            {
                productoMapper.Update(docToUpdate.Producto, newData.Producto);
                string setData = $"Stock = '{newData.Stock}'";

                DatabaseSql.Write(new SqlCommand($"UPDATE Inventarios SET {setData} WHERE IdInventario = {docToUpdate.Id}"));
            }
            catch (SqlException)
            {
            }
            catch (Exception ex)
            {
            }
        }
    }
}
