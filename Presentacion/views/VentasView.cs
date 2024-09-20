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
            Cliente cliente = (Cliente)this.dataGridViewClientes.CurrentRow.DataBoundItem;
            Inventario inventario = (Inventario)this.dataGridViewProductos.CurrentRow.DataBoundItem;
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

                    ventaService.Create(clientevVenta);
                    this.txtQty.Text = string.Empty;
                    refreshVentasByCliente(ventaService.GetVentasByIdCliente(cliente.Id));
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

        private void dataGridViewClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cliente cliente = (Cliente)this.dataGridViewClientes.CurrentRow.DataBoundItem;

            if (cliente != null) 
            {
                refreshVentasByCliente(ventaService.GetVentasByIdCliente(cliente.Id));
            }

        }

        private void refreshVentasByCliente(List<Venta> ventas)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = ventas;
        }
    }
}
