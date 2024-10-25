using BE.models;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion.views
{
    public partial class VentasView : Form
    {
        public VentasView()
        {
            InitializeComponent();
        }
        private ClienteService clienteService = new ClienteService();
        private InventarioService inventarioService = new InventarioService();
        private VentaService ventaService = new VentaService();
        private void VentasView_Load(object sender, EventArgs e)
        {
            refreshClientes();
            refreshProductos();
            refreshVentasByCliente(null);
        }

        private void refreshClientes()
        {
            this.dataGridViewClientes.DataSource = null;
            this.dataGridViewClientes.DataSource = clienteService.GetAll();
        }

        private void refreshProductos()
        {

            List<Producto> productos = new List<Producto>();
            this.dataGridViewProductos.DataSource = null;
            this.dataGridViewProductos.DataSource = inventarioService.GetAll();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Cliente cliente = GetClienteByDataGridView();
            Inventario inventario = GetInventarioByDataGridView();
            int qty = Convert.ToInt32(this.txtQty.Text);

            if (cliente != null && inventario != null)
            {
                try
                {
                    ClienteVenta clientevVenta = new ClienteVenta();
                    Venta venta = new Venta();

                    venta.Producto = inventario.Producto.Categoria + "  " + inventario.Producto.SubCategoria + "    " + inventario.Producto.Nombre;
                    venta.PrecioVenta = inventario.Producto.ObtenerPrecio();
                    venta.Qty = qty;

                    clientevVenta.Cliente = cliente;
                    clientevVenta.Ventas = new List<Venta>();
                    clientevVenta.Ventas.Add(venta);

                    if(ventaService.checkVenta(inventario, qty)) 
                    {
                        ventaService.Create(clientevVenta);
                        this.txtQty.Text = string.Empty;
                        refreshVentasByCliente(ventaService.GetVentasByIdCliente(cliente.Id));
                        refreshProductos();
                    } else
                    {
                        MessageBox.Show("Stock no valido");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cliente cliente = (Cliente)this.dataGridViewClientes.CurrentRow.DataBoundItem;
            Venta venta = this.dataGridView1.CurrentRow != null ? (Venta)this.dataGridView1.CurrentRow.DataBoundItem : null;

            if (cliente != null && venta != null) 
            {
                ventaService.DeleteById(venta.Id);
                refreshVentasByCliente(ventaService.GetVentasByIdCliente(cliente.Id));
            }

        }

        public Inventario GetInventarioByDataGridView()
        {
            Inventario inventario = new Inventario();
            DataGridViewRow data = (DataGridViewRow)this.dataGridViewProductos.CurrentRow;

            if (data != null)
            {
                Producto producto = null;
                string Consumo = data.Cells["Consumo"].Value.ToString();

                if (!string.IsNullOrEmpty(Consumo))
                {
                    ProductoElectronico electronico = new ProductoElectronico();
                    electronico.Consumo = Consumo;
                    producto = electronico;
                }
                else
                {
                    ProductoAlimenticio productoAlimenticio = new ProductoAlimenticio();
                    productoAlimenticio.FechaDeVencimiento = DateTime.Parse(data.Cells["FechaDeVencimiento"].Value.ToString());
                    producto = productoAlimenticio;
                }

                producto.Id = Convert.ToInt32(data.Cells["IdProducto"].Value.ToString());
                producto.Categoria = data.Cells["Categoria"].Value.ToString();
                producto.SubCategoria = data.Cells["SubCategoria"].Value.ToString();
                producto.Nombre = data.Cells["Nombre"].Value.ToString();
                producto.PrecioCosto = 1;

                Proveedor proveedor = new Proveedor();
                proveedor.Id = Convert.ToInt32(data.Cells["IdProveedor"].Value.ToString());
                proveedor.Nombre = data.Cells["Proveedor"].Value.ToString();

                producto.Proveedor = proveedor;

                inventario.Producto = producto;
                inventario.Stock = Convert.ToInt32(data.Cells["Stock"].Value.ToString());

            }

            return inventario;
        }

        private void dataGridViewClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cliente cliente = GetClienteByDataGridView();

            if (cliente != null) 
            {
                refreshVentasByCliente(ventaService.GetVentasByIdCliente(cliente.Id));
            }

        }

        public Cliente GetClienteByDataGridView()
        {
            Cliente cliente = new Cliente();
            DataGridViewRow data = (DataGridViewRow)this.dataGridViewClientes.CurrentRow;

            if (data != null)
            {
                cliente.Id = Convert.ToInt32(data.Cells["IdCliente"].Value.ToString());
                cliente.Nombre = data.Cells["Nombre"].Value.ToString();
                cliente.Apellido = data.Cells["Apellido"].Value.ToString();
                cliente.DNI = data.Cells["DNI"].Value.ToString();
            }


            return cliente;
        }


        private void refreshVentasByCliente(List<Venta> ventas)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = ventas;
        }
    }
}
