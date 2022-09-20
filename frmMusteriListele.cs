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
    public partial class frmMusteriListele : Form
    {
        public frmMusteriListele()
        {
            InitializeComponent();
        }
        /*Data base bağlantısı sağlandı*/
        SqlConnection connect = new SqlConnection("Data Source=DESKTOP-UQ3M9IO;Initial Catalog=Stok_Takip;Integrated Security=True");
        DataSet ds = new DataSet();

        private void frmMusteriListele_Load(object sender, EventArgs e)
        {
            kayıt_goster();
        }
        private void kayıt_goster() {
            connect.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from müsteri", connect);
            adapter.Fill(ds, "müsteri");
            dataGridView1.DataSource = ds.Tables["müsteri"];
            connect.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTc.Text = dataGridView1.CurrentRow.Cells["tc"].Value.ToString();
            txtAdSoyad.Text = dataGridView1.CurrentRow.Cells["adSoyad"].Value.ToString();
            txtTelefon.Text = dataGridView1.CurrentRow.Cells["telefon"].Value.ToString();
            txtAdres.Text = dataGridView1.CurrentRow.Cells["adres"].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells["email"].Value.ToString();
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            connect.Open();
            SqlCommand command = new SqlCommand("update müsteri set adSoyad = @adSoyad, telefon = @telefon, adres = @adres, email = @email where tc = @tc",connect);
            command.Parameters.AddWithValue("@tc", txtTc.Text);
            command.Parameters.AddWithValue("@adSoyad", txtAdSoyad.Text);
            command.Parameters.AddWithValue("@telefon", txtTelefon.Text);
            command.Parameters.AddWithValue("@adres", txtAdres.Text);
            command.Parameters.AddWithValue("@email", txtEmail.Text);
            command.ExecuteNonQuery();/*Database komutu çalıştırıldı*/
            connect.Close(); /*Database bağlantısı kapatıldı*/
            ds.Tables["müsteri"].Clear();
            kayıt_goster();/*Güncelleme sonrası kayıtlar yeniden getirildi*/
            MessageBox.Show("Müsteri Kaydı Güncellendi");
            foreach (Control item in this.Controls)
            /*Müşteri eklendikten sonra textBox içerisindeki yazılar silindi*/
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }
        private void btnSil_Click(object sender, EventArgs e)
        {
            connect.Open();
            String query = String.Format("delete from müsteri where tc = '"+ dataGridView1.CurrentRow.Cells["tc"].Value.ToString()) + "'";
            SqlCommand command = new SqlCommand(query, connect);
            command.ExecuteNonQuery();
            connect.Close();
            MessageBox.Show("Kayıt Silindi");
            ds.Tables["müsteri"].Clear();/*Güncelleme sonrası kayıtlar yeniden getirildi*/
            kayıt_goster();
        }

        private void textAra_TextChanged(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            connect.Open();
            String query = "Select * from müsteri where tc like '%"+txtAra.Text+ "%' or adSoyad like '%"+txtAra.Text+ "%' or telefon like '%"+txtAra.Text+ "%' or adres like '%"+txtAra.Text+ "%' or email like '%"+txtAra.Text+"%'";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            connect.Close();
        }
    }
}
