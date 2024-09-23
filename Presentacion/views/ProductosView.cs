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
    public partial class ProductosView : Form
    {
        public ProductosView()
        {
            InitializeComponent();
        }

        private ProductoService productoService = new ProductoService();
        private InventarioService inventarioService = new InventarioService();


        private void button1_Click(object sender, EventArgs e)
        {
            Inventario inventario = getProductoFromUI();

            if (inventario != null)
            {
                int idProducto = inventarioService.Create(inventario);
                MessageBox.Show($"El id del inventario es {idProducto}");
            }

            refresh();
        }

        private void ProductosView_Load(object sender, EventArgs e)
        {
            this.groupBox3.Enabled = false;

            this.comboBox2.DataSource = null;
            this.comboBox2.DataSource = Enum.GetValues(typeof(TipoDeProducto));

            this.comboBox3.DataSource = null;
            this.comboBox3.DataSource = productoService.GetProveedores();

            refresh();
        }

        private Inventario getProductoFromUI()
        {
            Producto producto = null;

            #region Datos Producto
            string categoria = this.txtCategoria.Text;
            string subCategoria = this.txtSubCategoria.Text;
            string nombreProducto = this.txtProducto.Text;
            float precioCosto = float.Parse(this.txtPrecioCosto.Text);

            #region Provedor
            if (this.comboBox2.SelectedItem != null && this.comboBox3.SelectedItem != null) 
            {
                Proveedor provedor = (Proveedor)this.comboBox3.SelectedItem;

                #region Valido que radioButton esta checked para generar sus correspondiete Producto
                TipoDeProducto tipoDeProducto = (TipoDeProducto)this.comboBox2.SelectedItem;

                if (TipoDeProducto.ELECTRONICO.Equals(tipoDeProducto))
                {
                    string consumo = this.comboBox1.SelectedItem.ToString();
                    producto = new ProductoElectronico(categoria, subCategoria, nombreProducto, provedor, precioCosto, consumo);
                }

                if (TipoDeProducto.ALIMENTICIO.Equals(tipoDeProducto))
                {
                    DateTime fecha = dateTimePicker1.Value;
                    producto = new ProductoAlimenticio(categoria, subCategoria, nombreProducto, provedor, precioCosto, fecha);
                }
                #endregion
            }
            else
            {
                MessageBox.Show("No se completaron todos los campos");
            }
            #endregion


            #endregion


            #region Datos Inventario
            Inventario inventario = new Inventario();

            try
            {
                inventario.Stock = Int32.Parse(this.txtStock.Text);
                inventario.Producto = producto;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear inventario: {ex.Message}, No se puede crear el producto");
                return null;
            }

            #endregion

            return inventario;
        }

        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            Inventario inventario = (Inventario)this.dataGridView1.CurrentRow.DataBoundItem;
            Inventario nuevoInventario = getProductoFromUI();

            inventarioService.Update(inventario, nuevoInventario);

            refresh();
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            Inventario inventario = (Inventario)this.dataGridView1.CurrentRow.DataBoundItem;
            inventarioService.DeleteById(inventario.Id);
            refresh();
        }

        public void refresh()
        {

            this.txtCategoria.Text = string.Empty;
            this.txtSubCategoria.Text = string.Empty;
            this.txtProducto.Text = string.Empty;
            this.txtPrecioCosto.Text = string.Empty;
            this.txtStock.Text = string.Empty;


            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = inventarioService.GetAll();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedItem != null)
            {
                TipoDeProducto tipoDeProducto = (TipoDeProducto)this.comboBox2.SelectedItem;

                if (TipoDeProducto.ELECTRONICO.Equals(tipoDeProducto))
                {
                    this.groupBox3.Enabled = true;
                    this.comboBox1.Enabled = true;
                    this.dateTimePicker1.Enabled = false;
                }
                else
                {
                    this.groupBox3.Enabled = true;
                    this.comboBox1.Enabled = false;
                    this.dateTimePicker1.Enabled = true;
                }
            }
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            Inventario inventario = (Inventario)this.dataGridView1.CurrentRow.DataBoundItem;

            if (inventario != null)
            {
                this.txtCategoria.Text = inventario.Producto.Categoria;
                this.txtSubCategoria.Text = inventario.Producto.SubCategoria;
                this.txtProducto.Text = inventario.Producto.Nombre;

                this.txtStock.Text = inventario.Stock.ToString();
                this.txtPrecioCosto.Text = inventario.Producto.PrecioCosto.ToString();

                this.comboBox3.SelectedItem = inventario.Producto.Proveedor;

                if (inventario.Producto is ProductoElectronico)
                {
                    ProductoElectronico electronico = (ProductoElectronico)inventario.Producto;
                    this.comboBox2.SelectedItem = TipoDeProducto.ELECTRONICO;
                    this.comboBox1.SelectedItem = electronico.Consumo;
                }
                else
                {
                    ProductoAlimenticio alimenticio = (ProductoAlimenticio)inventario.Producto;
                    this.comboBox2.SelectedItem = TipoDeProducto.ALIMENTICIO;
                    this.dateTimePicker1.Value = alimenticio.FechaDeVencimiento;
                }
            }
        }

        private void txtPrecioCosto_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsNumber(e.KeyChar);
        }

        private void txtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsNumber(e.KeyChar);
        }
    }
}
