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
    public partial class frmUrunListeleme : Form
    {
        SqlConnection connect = new SqlConnection("Data Source=DESKTOP-UQ3M9IO;Initial Catalog=Stok_Takip;Integrated Security=True");/*Sql Bağlantısı*/
        DataSet ds = new DataSet(); // Ürünleri geçici olarak tutacağımız dataset. Database'den ürünler buraya aktarılacak
        public frmUrunListeleme()
        {
            InitializeComponent();
        }

        private void frmUrunListeleme_Load(object sender, EventArgs e)
        {
            urunListele();
            kategoriGetir();
        }
        private void urunListele()
        {
            connect.Open();
            string query = "select * from urun";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
            adapter.Fill(ds, "urun");
            dataGridView1.DataSource = ds.Tables["urun"];
            connect.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtUrunID.Text = dataGridView1.CurrentRow.Cells["urunID"].Value.ToString();
            lblKategori.Text = dataGridView1.CurrentRow.Cells["kategori"].Value.ToString();
            lblMarka.Text = dataGridView1.CurrentRow.Cells["marka"].Value.ToString();
            txtUrunAdi.Text = dataGridView1.CurrentRow.Cells["urunAdi"].Value.ToString();
            txtMiktari.Text = dataGridView1.CurrentRow.Cells["miktar"].Value.ToString();
            txtAlisFiyati.Text = dataGridView1.CurrentRow.Cells["alisFiyati"].Value.ToString();
            txtSatisFiyati.Text = dataGridView1.CurrentRow.Cells["satisFiyati"].Value.ToString();
            cmbKategori.Text = dataGridView1.CurrentRow.Cells["kategori"].Value.ToString();
            cmbMarka.Text = dataGridView1.CurrentRow.Cells["marka"].Value.ToString();

        }

        private void txtUrunAra_TextChanged(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            connect.Open();
            String query = "Select * from urun where urunID like '%" + txtUrunAra.Text + "%' or kategori like '%" + txtUrunAra.Text + "%' or marka like '%" + txtUrunAra.Text + "%' or urunAdi like '%" + txtUrunAra.Text + "%'";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            connect.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if(txtUrunID.Text != "" || lblKategori.Text != "" || lblMarka.Text != "")
            {
                connect.Open();
                string query = "update urun set urunAdi = @urunAdi, miktar = @miktar, alisFiyati = @alisFiyati, satisFiyati = @satisFiyati where urunID = @urunID";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@urunID", int.Parse(txtUrunID.Text));
                command.Parameters.AddWithValue("@urunAdi", txtUrunAdi.Text);
                command.Parameters.AddWithValue("@miktar", int.Parse(txtMiktari.Text));
                command.Parameters.AddWithValue("@alisFiyati", double.Parse(txtAlisFiyati.Text));
                command.Parameters.AddWithValue("@satisFiyati", double.Parse(txtSatisFiyati.Text));
                command.ExecuteNonQuery();/*Database komutu çalıştırıldı*/
                connect.Close(); /*Database bağlantısı kapatıldı*/
                ds.Tables["urun"].Clear();
                urunListele(); ;/*Güncelleme sonrası kayıtlar yeniden getirildi*/
                MessageBox.Show("Ürün Kaydı Güncellendi");
                foreach (Control item in groupBox1.Controls)
                {
                    if (item is TextBox)
                    {
                        item.Text = "";
                    }
                }
            }
            else
            {
                MessageBox.Show("Urun Seçmediniz!!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(txtUrunID.Text != ""){ 
                connect.Open();
                string query = "update urun set kategori = @kategori, marka = @marka where urunID = @urunID";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@urunID", int.Parse(txtUrunID.Text));
                command.Parameters.AddWithValue("@kategori", cmbKategori.Text);
                command.Parameters.AddWithValue("@marka", cmbMarka.Text);
                command.ExecuteNonQuery();/*Database komutu çalıştırıldı*/
                connect.Close(); /*Database bağlantısı kapatıldı*/
                ds.Tables["urun"].Clear();
                urunListele(); ;/*Güncelleme sonrası kayıtlar yeniden getirildi*/
                MessageBox.Show("Ürün Kaydı Güncellendi");
            }
            else
            {
                MessageBox.Show("Urun ID girilmemiş!!");
            }
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox || item is ComboBox)
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

        private void btnSil_Click(object sender, EventArgs e)
        {
            connect.Open();
            String query = String.Format("delete from urun where urunID = '" + dataGridView1.CurrentRow.Cells["urunID"].Value.ToString()) + "'";
            SqlCommand command = new SqlCommand(query, connect);
            command.ExecuteNonQuery();
            connect.Close();
            MessageBox.Show("Kayıt Silindi");
            ds.Tables["urun"].Clear();/*Güncelleme sonrası kayıtlar yeniden getirildi*/
            urunListele();
        }
    }
}
