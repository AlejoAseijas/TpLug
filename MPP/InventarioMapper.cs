﻿using Abstraccion;
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
                    DatabaseSql.Write(new SqlCommand($"DELETE FROM Inventarios Where Id = {Id}"));
                }
                catch (SqlException ex)
                { }
                catch(Exception ex) { }
            }

        }

        public List<Inventario> GetAll()
        {
            List<Inventario> inventarios = new List<Inventario>();
            string query = @"SELECT i.Id, i.Stock, p.Id as 'IdProducto', p.Nombre, p.Categoria, p.SubCategoria, p.FechaDeVencimiento, p.PrecioCosto ,p.Consumo, po.Id as 'IdProveedor', po.Nombre as 'Proveedor' FROM Inventarios i INNER JOIN Productos p ON i.IdProducto = p.Id INNER JOIN Proveedores po ON po.Id = p.IdProveedor";
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
            string query = $"SELECT i.Id, i.Stock, p.Id as 'IdProducto', p.Nombre, p.Categoria, p.SubCategoria, p.FechaDeVencimiento, p.PrecioCosto ,p.Consumo, po.Id as 'IdProveedor', po.Nombre as 'Proveedor' FROM Inventarios i INNER JOIN Productos p ON i.IdProducto = p.Id INNER JOIN Proveedores po ON po.Id = p.IdProveedor WHERE i.Id = {Id}";
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

            inventario.Id = Convert.ToInt32(row["Id"]);
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

                DatabaseSql.Write(new SqlCommand($"UPDATE Inventarios SET {setData} WHERE Id = {docToUpdate.Id}"));
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