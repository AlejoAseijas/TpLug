using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class UserAuthForm : UserControl
    {
        public UserAuthForm()
        {
            InitializeComponent();
        }

        public string DNI = string.Empty;
        public string Password = string.Empty;

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Password = textBox2.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DNI += textBox1.Text;
        }
    }
}
