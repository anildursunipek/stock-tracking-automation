using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace stock_tracking_automation
{
    public partial class frmKategori : Form
    {
        SqlConnection connect = new SqlConnection("Data Source=DESKTOP-UQ3M9IO;Initial Catalog=Stok_Takip;Integrated Security=True");/*Sql Bağlantısı*/
        public frmKategori()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connect.Open();
            string query = "insert into kategoriBilgileri(kategori) values('" + txtKategori.Text + "')";
            SqlCommand command = new SqlCommand(query,connect);
            command.ExecuteNonQuery();
            connect.Close();
            txtKategori.Text = "";
            MessageBox.Show("Kategori Eklendi");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmKategori_Load(object sender, EventArgs e)
        {

        }
    }
}
