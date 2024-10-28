using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            Password += textBox2.Text;

            Regex regex = new Regex(@"^.{7,}$");

            if (regex.IsMatch(Password))
            {
                textBox2.BackColor = System.Drawing.Color.White;
            }
            else
            {
                textBox2.BackColor = System.Drawing.Color.LightCoral;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DNI += textBox1.Text;
        }
    }
}
