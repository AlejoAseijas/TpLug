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
    public partial class UserView : Form
    {
        private AuthService authService = new AuthService();
        
        public UserView()
        {
            InitializeComponent();
        }

        private DataSet DataSet = new DataSet();

        private void UserView_Load(object sender, EventArgs e)
        {
            DataSet = authService.GetAll();
            refresh();
        }

        private void refresh() 
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = DataSet.Tables[0];

            textBox1.Text = string.Empty;

            userAuthForm1.DNI = string.Empty;
            userAuthForm1.Password = string.Empty;

            groupBox1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (authService.SaveData(DataSet))
            {
                refresh();
            }
            else 
            {
                MessageBox.Show("Error al guardar en la DB");
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            DataTable table = DataSet.Tables[0];
            int size = table.Rows.Count;

            if (size > 0) 
            {
                int id = Convert.ToInt32(table.Rows[size - 1][0].ToString());
                id++;
                textBox1.Text = id.ToString();
            }
            else 
            {
                textBox1.Text = "1";
            }

        }

        public static Boolean IsNumeric(string valor)
        {
            int result;
            return int.TryParse(valor, out result);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataRow row = DataSet.Tables[0].NewRow();

            row["IdUser"] = Convert.ToInt32(textBox1.Text);
            row["DNI"] = userAuthForm1.DNI;
            row["Password"] = authService.GenerateHash(userAuthForm1.Password);

            DataSet.Tables[0].Rows.Add(row);

            refresh();
        }
    }
}
