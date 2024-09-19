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

        }

        public List<ClienteVenta> GetAll()
        {
            throw new NotImplementedException();
        }

        public ClienteVenta GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public ClienteVenta ToMap(DataRow row)
        {
            throw new NotImplementedException();
        }

        public void Update(ClienteVenta docToUpdate, ClienteVenta newData)
        {
            throw new NotImplementedException();
        }
    }
}
