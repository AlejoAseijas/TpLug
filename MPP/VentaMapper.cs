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
    public class VentaMapper : IMappable<Venta>
    {
        public string TABLE_NAME => "Ventas";
        public string ID_COLUMN => "IdVenta";

        public int Create(Venta entity)
        {

            SqlCommand sqlCommand = new SqlCommand("CreateVenta");

            Hashtable queryParams = new Hashtable { { "@Producto", entity.Producto }, { "@Qty", entity.Qty }, { "@PrecioVenta", entity.PrecioVenta } };

            int id = -1;

            try
            {
                id = DatabaseSql.WriteAndGetId(sqlCommand, queryParams, "@IdVenta");
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
            SqlCommand sqlCommand = new SqlCommand("DeleteVentaById");

            Hashtable queryParams = new Hashtable { { "@IdVenta",  Id } };
            try
            {
                DatabaseSql.Write(sqlCommand, queryParams);
            }
            catch (SqlException ex)
            { }
            catch (Exception ex)
            { }
        }

        //public List<Venta> GetAll()
        //{
        //    List<Venta> ventas = new List<Venta>();

        //    DataTable Tabla = null;

        //    try
        //    {
        //        Tabla = DatabaseSql.Read(new SqlCommand("GetAllVentas"), null).Tables[0];
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
        //            ventas.Add(ToMap(fila));
        //        }
        //    }

        //    return ventas;
        //}

        public DataSet GetAll()
        {
            DataSet dataSet = null;

            try
            {
                dataSet = DatabaseSql.Read(new SqlCommand("GetAllVentas"), null);
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }

            return dataSet;
        }

        public Venta GetById(string Id)
        {
            Venta venta = new Venta();

            DataTable Tabla = null;

            try
            {
                Hashtable queryParams = new Hashtable { { "@IdVenta", int.Parse(Id)} };
                Tabla = DatabaseSql.Read(new SqlCommand("GetVentaById"), queryParams).Tables[0];
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
                Hashtable queryParams = new Hashtable { { "@Producto", newData.Producto }, { "@Qty", newData.Qty }, { "@PrecioVenta", newData.PrecioVenta }, { "@IdVenta", docToUpdate.Id } };
                DatabaseSql.Write(new SqlCommand("UpdateVentaById"), queryParams);
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
