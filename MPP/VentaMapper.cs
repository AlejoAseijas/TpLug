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
    public class VentaMapper : IGestor<Venta>
    {
        public int Create(Venta entity)
        {

            SqlCommand sqlCommand = new SqlCommand($"INSERT INTO Ventas (Producto, PrecioVenta, Qty) VALUES ('{entity.Producto}', '{entity.PrecioVenta}', '{entity.Qty}')");

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
            SqlCommand sqlCommand = new SqlCommand($"Delete FROM Ventas Where IdVenta = {Id}");

            try
            {
                DatabaseSql.Write(sqlCommand);
            }
            catch (SqlException ex)
            { }
            catch (Exception ex)
            { }
        }

        public List<Venta> GetAll()
        {
            List<Venta> ventas = new List<Venta>();

            DataTable Tabla = null;

            try
            {
                Tabla = DatabaseSql.Read(new SqlCommand("SELECT * FROM Ventas"));
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
                    ventas.Add(ToMap(fila));
                }
            }

            return ventas;
        }

        public Venta GetById(string Id)
        {
            Venta venta = new Venta();

            DataTable Tabla = null;

            try
            {
                Tabla = DatabaseSql.Read(new SqlCommand($"SELECT * FROM Ventas WHERE IdVenta = {Id}"));
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
                    venta = ToMap(fila);
                }
            }

            return venta;
        }

        public Venta ToMap(DataRow row)
        {
            Venta venta = new Venta();

            venta.Id = Convert.ToInt32(row["IdVenta"]);
            venta.Producto = row["Producto"].ToString();
            venta.PrecioVenta = row["PrecioVenta"].ToString() != null ? float.Parse(row["PrecioVenta"].ToString()) : -1;
            venta.Qty = Convert.ToInt32(row["Qty"]);


            return venta;
        }

        public void Update(Venta docToUpdate, Venta newData)
        {
            try
            {
                DatabaseSql.Write(new SqlCommand($"UPDATE Ventas SET Producto = '{newData.Producto}', PrecioVenta = '{newData.PrecioVenta}', Qty = '{newData.Qty}' WHERE IdVenta = {docToUpdate.Id}"));
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
