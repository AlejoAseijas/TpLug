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
    public partial class InformesView : Form
    {
        public InformesView()
        {
            InitializeComponent();
        }
        private InformeService service = new InformeService();

        private void InformesView_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = service.TOTAL_FACTURADO.ToString();
            this.textBox2.Text = service.QTY_PRODUCTOS_VENDIDOS.ToString();
            this.textBox3.Text = service.CLIENTE_CON_MAS_VENTAS.Keys.First().ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
