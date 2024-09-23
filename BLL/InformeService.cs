using BE.models;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class InformeService
    {
        private VentaService ventaService;

        public float TOTAL_FACTURADO = 0;
        public int QTY_PRODUCTOS_VENDIDOS = 0;
        public Dictionary<Cliente, int> CLIENTE_CON_MAS_VENTAS = null;

        public InformeService()
        {
            ventaService = new VentaService();
            CalculateInforme();
        }

        private void CalculateInforme()
        {
            List<ClienteVenta> ventas = ventaService.GetAll();

            Cliente clienteConMasVentas = null;
            int maxVentas = 0;

            foreach (ClienteVenta venta in ventas)
            {
                int qty = 0;
                float totalVendido = 0;

                if (venta.Ventas.Count > 0)
                {
                    foreach (Venta venta1 in venta.Ventas)
                    {
                        qty += venta1.Qty;
                        totalVendido += venta1.PrecioVenta;
                    }

                    if (CLIENTE_CON_MAS_VENTAS == null || qty > maxVentas)
                    {
                        clienteConMasVentas = venta.Cliente;
                        maxVentas = qty;
                        CLIENTE_CON_MAS_VENTAS = new Dictionary<Cliente, int> { { clienteConMasVentas, maxVentas } };
                    }

                    QTY_PRODUCTOS_VENDIDOS += qty;
                    TOTAL_FACTURADO += totalVendido;
                }
            }

        }
    }
}
