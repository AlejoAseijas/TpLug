using Abstraccion;
using BE.execptions;
using BE.models;
using MPP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class VentaService : AbstractService<ClienteVenta>
    {
        private ClienteVentaMapper clienteVentaMapper = new ClienteVentaMapper();
        private InventarioMapper inventarioMapper = new InventarioMapper();
        private VentaMapper ventaMapper = new VentaMapper();

        public VentaService() 
        {
            base.Mapper = clienteVentaMapper;
        }

        public List<Venta> GetVentasByIdCliente(int IdCliente)
        {
            List<Venta> ventas = new List<Venta>();

            if (ModesOfPersistible.DB.Equals(mode)) 
            {
                ventas = clienteVentaMapper.GetVentasByIdCliente(IdCliente);
            }
            else 
            {
                //Filtro Desconectado
                DataTable dataTable = persistibleService.GetAll(Mapper.TABLE_NAME);

                if (dataTable != null) 
                {
                    foreach (DataRow row in dataTable.Rows)
                    {

                        if (!string.IsNullOrEmpty(row["IdCliente"].ToString()) && IdCliente == Convert.ToInt32(row["IdCliente"].ToString()))
                        {
                            ventas.Add(GetData(row));
                        }
                    }
                }

            }

            return ventas;
        }

        public bool checkVenta(Inventario inventario, int qtyVender)
        {
            bool valid = false;

            if (inventario.Stock - qtyVender > 0) 
            {
                inventario.Stock -= qtyVender;
                inventarioMapper.Update(inventario, inventario);
                valid = true;
            }

            return valid;
        }

        public override int Create(ClienteVenta entity)
        {
            int id = -1;

            if (!ModesOfPersistible.DB.Equals(mode)) 
            {
                Hashtable ventaData = GetDataVenta(entity);
                ventaData.Remove(ventaMapper.ID_COLUMN);

                int idVenta = base.persistibleService.save(ventaMapper.TABLE_NAME, ventaData, ventaMapper.ID_COLUMN);

                Hashtable hashtable = GetData(entity);
                hashtable.Remove(ventaMapper.ID_COLUMN);
                hashtable.Add(ventaMapper.ID_COLUMN, idVenta);

                base.persistibleService.save(base.Mapper.TABLE_NAME, hashtable, base.Mapper.ID_COLUMN);
            }
            else 
            {
                id = base.Create(entity);
            }

            return id;
        }
        private Hashtable GetData(ClienteVenta clienteVenta)
        {
            Hashtable data = new Hashtable();

            data.Add("IdCliente", clienteVenta.Cliente.Id);
            data.Add("Nombre", clienteVenta.Cliente.Nombre);
            data.Add("Apellido", clienteVenta.Cliente.Apellido);
            data.Add("DNI", clienteVenta.Cliente.DNI);

            data.Add("IdVenta", clienteVenta.Ventas[0].Id);
            data.Add("Producto", clienteVenta.Ventas[0].Producto);
            data.Add("Qty", clienteVenta.Ventas[0].Qty);
            data.Add("PrecioVenta", clienteVenta.Ventas[0].PrecioVenta);

            return data;
        }
        private Hashtable GetDataVenta(ClienteVenta clienteVenta) 
        {

            Hashtable data = new Hashtable();

            data.Add("IdVenta", clienteVenta.Ventas[0].Id);
            data.Add("Producto", clienteVenta.Ventas[0].Producto);
            data.Add("Qty", clienteVenta.Ventas[0].Qty);
            data.Add("PrecioVenta", clienteVenta.Ventas[0].PrecioVenta);

            return data;
        }
        private Venta GetData(DataRow data)
        {

            Venta venta = new Venta
            {
                Id = Convert.ToInt32(data["IdVenta"]),
                Producto = data["Producto"].ToString(),
                Qty = Convert.ToInt32(data["Qty"]),
                PrecioVenta = float.Parse(data["PrecioVenta"].ToString())
            };

            return venta;
        }





    }
}
