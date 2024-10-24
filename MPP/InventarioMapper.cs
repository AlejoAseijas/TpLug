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
    public class InventarioMapper : IMappable<Inventario>
    {
        private ProductoMapper productoMapper = new ProductoMapper();

        public string TABLE_NAME => "Inventarios";

        public int Create(Inventario entity)
        {
            int id = -1;
            
            if (entity != null && entity.Producto != null)
            {
                int idProducto = -1;

                try
                {
                    idProducto = productoMapper.Create(entity.Producto);

                    Hashtable queryParams = new Hashtable { { "@IdProducto", idProducto }, { "@Stock", entity.Stock } };
                    id = DatabaseSql.WriteAndGetId(new SqlCommand("CreateInventario"), queryParams, "@NewInventarioID");
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
                    Hashtable queryParams = new Hashtable { { "@IdInventario", Id } };
                    DatabaseSql.Write(new SqlCommand("DeleteInventarioById"), queryParams);
                }
                catch (SqlException ex)
                { }
                catch(Exception ex) { }
            }

        }

        public List<Inventario> GetAll()
        {
            List<Inventario> inventarios = new List<Inventario>();
            DataTable Tabla = null;

            try
            {
                Tabla = DatabaseSql.Read(new SqlCommand("GetAllInventarios"), null).Tables[0];
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
            DataTable Tabla = null;

            try
            {
                Hashtable queryParams = new Hashtable { { "@IdInventario", int.Parse(Id) } };
                Tabla = DatabaseSql.Read(new SqlCommand("GetInventarioById"), queryParams).Tables[0];
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

                Hashtable queryParams = new Hashtable { { "@Stock", newData.Stock }, { "@IdInventarioModificar", docToUpdate.Id} };

                DatabaseSql.Write(new SqlCommand("UpdateInventarioById"), queryParams);
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
