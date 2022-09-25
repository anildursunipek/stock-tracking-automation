using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stock_tracking_automation
{
    public partial class frmMarka : Form
    {
        SqlConnection connect = new SqlConnection("Data Source=DESKTOP-UQ3M9IO;Initial Catalog=Stok_Takip;Integrated Security=True");/*Sql Bağlantısı*/
        public frmMarka()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            connect.Open();
            string query = "insert into markaBilgileri(kategori,marka) values('" + cmbKategori.Text + "','"+txtMarka.Text+"')";
            SqlCommand command = new SqlCommand(query,connect);
            command.ExecuteNonQuery();
            connect.Close();
            txtMarka.Text = "";
            cmbKategori.Text = "";
            MessageBox.Show("Marka Eklendi");
        }
        private void kategoriGetir()
        {
            connect.Open();
            string query = "select * from kategoriBilgileri";
            SqlCommand command = new SqlCommand(query, connect);
            SqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                cmbKategori.Items.Add(read["kategori"].ToString());
            }
            connect.Close();
        }

        private void frmMarka_Load(object sender, EventArgs e)
        {
            kategoriGetir();
        }
    }
}
