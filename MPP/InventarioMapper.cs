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
    public class InventarioMapper : IGestor<Inventario>
    {
        private ProductoMapper productoMapper = new ProductoMapper();

        public int Create(Inventario entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Inventario> GetAll()
        {
            List<Inventario> inventarios = new List<Inventario>();
            string query = @"SELECT i.Id, i.Stock, p.Id as 'IdProducto', p.Nombre, p.Categoria, p.SubCategoria, p.FechaDeVencimiento, p.PrecioCosto ,p.Consumo, po.Id as 'IdProveedor', po.Nombre as 'Proveedor' FROM Inventario i INNER JOIN Producto p ON i.IdProducto = p.Id INNER JOIN Proveedores po ON po.Id = p.IdProveedor";
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

            if (Tabla.Rows.Count > 0)
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
            throw new NotImplementedException();
        }

        public Inventario ToMap(DataRow row)
        {
            Inventario inventario = new Inventario();

            inventario.Id = Convert.ToInt32(row["Id"]);
            inventario.Stock = Convert.ToInt32(row["Stock"].ToString());
            inventario.Producto = productoMapper.ToMap(row);

            return inventario;
        }

        public void Update(Inventario docToUpdate, Inventario newData)
        {
            throw new NotImplementedException();
        }
    }
}
