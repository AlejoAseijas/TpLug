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
    public class ProductoMapper : IMappable<Producto>
    {
        public int Create(Producto entity)
        {

            SqlCommand sqlCommand = new SqlCommand("CreateProducto");

            Hashtable queryParams = new Hashtable { { "@Categoria", entity.Categoria}, { "@SubCategoria", entity.SubCategoria}, { "@Nombre", entity.Nombre }, { "@IdProveedor", entity.Proveedor.Id }, { "@PrecioCosto", entity.PrecioCosto } };

            if (entity is ProductoElectronico)
            {
                ProductoElectronico productoElectronico = (ProductoElectronico)entity;
                queryParams.Add("@FechaDeVencimiento", null);
                queryParams.Add("@Consumo", productoElectronico.Consumo);
            }
            else
            {
                ProductoAlimenticio productoAlimenticio = (ProductoAlimenticio)entity;
                DateTime date = productoAlimenticio.FechaDeVencimiento;
                queryParams.Add("@FechaDeVencimiento", date.ToString("yyyy-MM-dd"));
                queryParams.Add("@Consumo", null);
            }

            int id = -1;

            try
            {
               id = DatabaseSql.WriteAndGetId(sqlCommand, queryParams, "@NewProductoID");
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
            SqlCommand sqlCommand = new SqlCommand("DeleteProductoById");
            Hashtable queryParams = new Hashtable { { "@IdProducto", Id } };

            try
            {
                DatabaseSql.Write(sqlCommand, queryParams);
            }
            catch (SqlException ex)
            { }
            catch (Exception ex)
            { }
        }

        public List<Producto> GetAll()
        {
            List<Producto> productos = new List<Producto>();
            DataTable Tabla = null;

            try
            {
                Tabla = DatabaseSql.Read(new SqlCommand("GetAllProductos"), null).Tables[0];

                if (Tabla != null && Tabla.Rows.Count > 0)
                {
                    foreach (DataRow fila in Tabla.Rows)
                    {
                        productos.Add(ToMap(fila));
                    }
                }

            }
            catch (SqlException)
            {
            }
            catch (Exception ex)
            {
            }


            return productos;
        }

        public Producto GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public Producto ToMap(DataRow row)
        {
            Producto producto;

            int Id = Convert.ToInt32(row["IdProducto"]);
            string Categoria = row["Categoria"].ToString();
            string SubCategoria = row["SubCategoria"].ToString();
            string Nombre = row["Nombre"].ToString();
            float precioCosto = float.Parse(row["PrecioCosto"].ToString());
            
            Proveedor proveedor = new Proveedor();
            proveedor.Id = Convert.ToInt32(row["IdProveedor"]);
            proveedor.Nombre = row["Proveedor"].ToString();

            if (!string.IsNullOrEmpty(row["Consumo"].ToString()))
            {
                producto = new ProductoElectronico(Id, Categoria, SubCategoria, Nombre, proveedor, precioCosto, row["Consumo"].ToString());
            }
            else
            {
                DateTime FechaVencimiento = DateTime.Parse(row["FechaDeVencimiento"].ToString());
                producto = new ProductoAlimenticio(Id, Categoria, SubCategoria, Nombre, proveedor, precioCosto, FechaVencimiento);
            }

            return producto;
        }

        public void Update(Producto docToUpdate, Producto newData)
        {
            Hashtable queryParams = new Hashtable { { "@Categoria", newData.Categoria }, { "@SubCategoria", newData.SubCategoria }, { "@Nombre", newData.Nombre }, { "@IdProveedor", newData.Proveedor.Id }, { "@PrecioCosto", newData.PrecioCosto }, { "@IdProductoModificar", docToUpdate.Id } };

            if (newData is ProductoElectronico)
            {
                ProductoElectronico productoElectronico = (ProductoElectronico)newData;
                queryParams.Add("@FechaDeVencimiento", null);
                queryParams.Add("@Consumo", productoElectronico.Consumo);
            }
            else
            {
                ProductoAlimenticio productoAlimenticio = (ProductoAlimenticio)newData;
                DateTime date = productoAlimenticio.FechaDeVencimiento;
                queryParams.Add("@FechaDeVencimiento", date.ToString("yyyy-MM-dd"));
                queryParams.Add("@Consumo", null);
            }

            try
            {
                DatabaseSql.Write(new SqlCommand("UpdateProductoById"), queryParams);
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        public List<Proveedor> ObtenerProveedores()
        {
            List<Proveedor> proveedors = new List<Proveedor>();

            DataTable Tabla = null;

            try
            {   
                Tabla = DatabaseSql.Read(new SqlCommand("GetAllProveedores"), null).Tables[0];
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
                    Proveedor proveedor = new Proveedor();

                    proveedor.Id = Convert.ToInt32(fila["IdProveedor"]);
                    proveedor.Nombre = fila["Nombre"].ToString();

                    proveedors.Add(proveedor);
                }
            }

            return proveedors;
        }
    }
}
