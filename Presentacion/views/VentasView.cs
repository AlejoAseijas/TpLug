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
using System.Windows.Forms.DataVisualization.Charting;

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
            this.comboBox1.DataSource = Enum.GetValues(typeof(SeriesChartType));
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
                producto.PrecioCosto = Convert.ToInt32(data.Cells["PrecioCosto"].Value.ToString());

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
            groupBox1.Visible = false;
            if (ventas != null && ventas.Count>0) 
            {
                this.dataGridView1.DataSource = null;
                this.dataGridView1.DataSource = ventas;
                loadGraph(ventas);
            }
        }

        private void loadGraph(List<Venta> ventas)
        {
            groupBox1.Visible = true;
            chart1.Series.Clear();

            Series serie = new Series("Ventas")
            {
                ChartType = (SeriesChartType)this.comboBox1.SelectedItem,
                XValueType = ChartValueType.Int32,
                YValueType = ChartValueType.Double
            };

            foreach (var venta in ventas)
            {
                serie.Points.AddXY(venta.Qty, venta.PrecioVenta);
            }

            chart1.Series.Add(serie);

            chart1.ChartAreas[0].AxisX.Title = "Cantidad (Qty)";
            chart1.ChartAreas[0].AxisY.Title = "Precio de Venta (PrecioVenta)";
            chart1.ChartAreas[0].AxisX.Interval = 1; 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cliente cliente = GetClienteByDataGridView();

            chart1.Series.Clear();

            Series serie = new Series("Ventas")
            {
                ChartType = (SeriesChartType)this.comboBox1.SelectedItem,
                XValueType = ChartValueType.Int32,
                YValueType = ChartValueType.Double
            };

            foreach (var venta in ventaService.GetVentasByIdCliente(cliente.Id))
            {
                serie.Points.AddXY(venta.Qty, venta.PrecioVenta);
            }

            chart1.Series.Add(serie);

            chart1.ChartAreas[0].AxisX.Title = "Cantidad (Qty)";
            chart1.ChartAreas[0].AxisY.Title = "Precio de Venta (PrecioVenta)";
            chart1.ChartAreas[0].AxisX.Interval = 1;
        }
    }
}
