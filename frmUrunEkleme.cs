using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stock_tracking_automation
{
    public partial class frmUrunEkleme : Form
    {
        SqlConnection connect = new SqlConnection("Data Source=DESKTOP-UQ3M9IO;Initial Catalog=Stok_Takip;Integrated Security=True");/*Sql Bağlantısı*/
        public frmUrunEkleme()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnVarOlanaEkle_Click(object sender, EventArgs e)
        {
            connect.Open();
            String query = "";
            SqlCommand command = new SqlCommand(query,connect)
        }
        private void button1_Click(object sender, EventArgs e)
        {
            connect.Open();
            SqlCommand command = new SqlCommand("insert into urun(kategori,marka,urunAdi,miktar,alisFiyati,satisFiyati,tarih) values(@kategori,@marka,@urunAdi,@miktar,@alisFiyati,@satisFiyati,@tarih)", connect);
            command.Parameters.AddWithValue("@kategori", cmbKategori.Text);
            command.Parameters.AddWithValue("@marka", cmbMarka.Text);
            command.Parameters.AddWithValue("@urunAdi", txtUrunAdi.Text);
            command.Parameters.AddWithValue("@miktar",int.Parse(txtMiktari.Text));
            command.Parameters.AddWithValue("@alisFiyati",double.Parse(txtAlisFiyati.Text));
            command.Parameters.AddWithValue("@satisFiyati",double.Parse(txtSatisFiyati.Text));
            command.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
            command.ExecuteNonQuery();
            connect.Close();
            MessageBox.Show("Ürün Eklendi");
            cmbMarka.Items.Clear();
            foreach (Control item in groupBox1.Controls)
            {
                if(item is TextBox)
                {
                    item.Text = "";
                }
                if(item is ComboBox)
                {
                    item.Text = "";
                }
            }
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
        private void cmbKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbMarka.Items.Clear();
            cmbMarka.Text = "";
            connect.Open();
            string query = "select marka from markaBilgileri where kategori = '" + cmbKategori.SelectedItem + "'";
            SqlCommand command = new SqlCommand(query, connect);
            SqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                cmbMarka.Items.Add(read["marka"].ToString());
            }
            connect.Close();
        }

        private void frmUrunEkleme_Load(object sender, EventArgs e)
        {
            kategoriGetir();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void UrunIDtxt_TextChanged(object sender, EventArgs e)
        {
            if(UrunIDtxt.Text == "")
            {
                lblMiktar.Text = "";
                foreach(Control item in groupBox2.Controls)
                {
                    if(item is TextBox)
                    {
                        item.Text = "";
                    }
                }
            }
            connect.Open();
            String query = "select kategori,marka,urunAdi,miktar,alisFiyati,satisFiyati from urun where urunID = '" + UrunIDtxt.Text + "'";
            SqlCommand command = new SqlCommand(query,connect);
            SqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                KategoriTxt.Text = read["kategori"].ToString();
                MarkaTxt.Text = read["marka"].ToString();
                UrunAdiTxt.Text = read["urunAdi"].ToString();
                lblMiktar.Text = read["miktar"].ToString();
                AlisFiyatiTxt.Text = read["alisFiyati"].ToString();
                SatisFiyatiTxt.Text = read["satisFiyati"].ToString();
            }
            connect.Close();
        }
    }
}
