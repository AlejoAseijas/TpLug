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
    public class ProveedorMapper : IMappable<Proveedor>
    {
        public string TABLE_NAME => "Proveedores";

        public string ID_COLUMN => "IdProveedor";

        public int Create(Proveedor entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int Id)
        {
            throw new NotImplementedException();
        }

        public DataTable GetAll()
        {
            DataTable dataTable = new DataTable();

            try
            {
                dataTable = DatabaseSql.Read(new SqlCommand("GetAllProveedores"), null);
            }
            catch (SqlException ex)
            { }
            catch (Exception ex) 
            { }

            return dataTable;
        }

        public Proveedor GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public Proveedor ToMap(DataRow row)
        {
            throw new NotImplementedException();
        }

        public void Update(Proveedor docToUpdate, Proveedor newData)
        {
            throw new NotImplementedException();
        }

    }
}
