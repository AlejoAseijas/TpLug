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
    public class ProductoMapper : IGestor<Producto>
    {
        public int Create(Producto entity)
        {
            string columns = "Categoria, SubCategoria, Nombre, IdProveedor, PrecioCosto";
            string values = $"'{entity.Categoria}', '{entity.SubCategoria}', '{entity.Nombre}', '{entity.Proveedor.Id}', {entity.PrecioCosto}";

            if (entity is ProductoElectronico)
            {
                ProductoElectronico productoElectronico = (ProductoElectronico)entity;
                columns += ", Consumo";
                values += $", '{productoElectronico.Consumo}'";
            }
            else
            {
                ProductoAlimenticio productoAlimenticio = (ProductoAlimenticio)entity;
                columns += ", FechaDeVencimiento";
                DateTime date = productoAlimenticio.FechaDeVencimiento;
                values += $", '{date.ToString("yyyy-MM-dd")}'";

            }

            int id = -1;

            try
            {
                id = DatabaseSql.WriteAndReturnId(new SqlCommand($"INSERT INTO Productos ({columns}) VALUES ({values})"));
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
            SqlCommand sqlCommand = new SqlCommand($"Delete FROM Productos Where Id = {Id}");

            try
            {
                DatabaseSql.Write(sqlCommand);
            }
            catch (SqlException ex)
            { }
            catch (Exception ex)
            { }
        }

        public List<Producto> GetAll()
        {
            throw new NotImplementedException();
        }

        public Producto GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public Producto ToMap(DataRow row)
        {
            Producto producto;

            int Id = Convert.ToInt32(row["Id"]);
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
            string setData = $"Categoria = '{newData.Categoria}' , SubCategoria = '{newData.SubCategoria}' , Nombre = '{newData.Nombre}' , IdProveedor = {newData.Proveedor.Id}, PrecioCosto = {newData.PrecioCosto}";

            if (newData is ProductoElectronico)
            {
                ProductoElectronico productoElectronico = (ProductoElectronico)newData;
                setData += $" , Consumo = '{productoElectronico.Consumo}', FechaDeVencimiento = null";
            }
            else
            {
                ProductoAlimenticio productoAlimenticio = (ProductoAlimenticio)newData;
                DateTime date = productoAlimenticio.FechaDeVencimiento;

                setData += $" , FechaDeVencimiento = '{date.ToString("yyyy-MM-dd")}', Consumo = null";
            }

            try
            {
                DatabaseSql.Write(new SqlCommand($"UPDATE Productos SET {setData} WHERE Id = {docToUpdate.Id}"));
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
                Tabla = DatabaseSql.Read(new SqlCommand("SELECT Id, Nombre FROM Proveedores"));
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

                    proveedor.Id = Convert.ToInt32(fila["Id"]);
                    proveedor.Nombre = fila["Nombre"].ToString();

                    proveedors.Add(proveedor);
                }
            }

            return proveedors;
        }
    }
}
