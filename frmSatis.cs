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
    public partial class frmSatis : Form
    {
        SqlConnection connect = new SqlConnection("Data Source=DESKTOP-UQ3M9IO;Initial Catalog=Stok_Takip;Integrated Security=True");/*Sql Bağlantısı*/
        DataSet ds = new DataSet();
        public frmSatis()
        {
            InitializeComponent();
        }
        private void frmSales_Load(object sender, EventArgs e)
        {
            sepetListele();
            hesapla();

        }
        private void hesapla()
        {
            connect.Open();
            string query = "select sum(toplamFiyati) from sepet";
            SqlCommand command = new SqlCommand(query, connect);
            lblGenelToplam.Text = command.ExecuteScalar() + "TL";
            connect.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmMüsteriEkle ekle = new frmMüsteriEkle();
            ekle.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmMusteriListele listele = new frmMusteriListele();
            listele.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmUrunEkleme ekle = new frmUrunEkleme();
            ekle.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmKategori ekle = new frmKategori();
            ekle.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmMarka ekle = new frmMarka();
            ekle.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmUrunListeleme listele = new frmUrunListeleme();
            listele.ShowDialog();
        }
        private void sepetListele()
        {
            connect.Open();
            string query = "select * from sepet";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
            adapter.Fill(ds, "sepet");
            dataGridView1.DataSource = ds.Tables["sepet"];
            connect.Close();
        }

        private void txtMusteriNo_TextChanged(object sender, EventArgs e)
        {
            if (txtMusteriNo.Text == "")
            {
                txtAdSoyad.Text = "";
                txtTel.Text = "";
            }
            connect.Open();
            string query = "select * from müsteri where musteriNo like '" + txtMusteriNo.Text + "'";
            SqlCommand command = new SqlCommand(query, connect);
            SqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                txtAdSoyad.Text = read["adSoyad"].ToString();
                txtTel.Text = read["telefon"].ToString();
            }
            connect.Close();
        }

        private void temizle()
        {
            if (txtUrunID.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMiktar)
                        {
                            item.Text = "";
                        }
                    }
                }
            }
        }
        private void txtBarkodNo_TextChanged(object sender, EventArgs e)
        {
            temizle();
            connect.Open();
            string query = "select * from urun where urunID like '" + txtUrunID.Text + "'";
            SqlCommand command = new SqlCommand(query, connect);
            SqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                txtUrunAdi.Text = read["urunAdi"].ToString();
                txtMiktar.Text = read["miktar"].ToString();
                txtSatisFiyati.Text = read["satisFiyati"].ToString();
                txtToplamFiyat.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            connect.Close();

        }
        bool flag;
        private void urunID_kontrol()
        {
            flag = true;
            connect.Open();
            string query = "select * from sepet";
            SqlCommand command = new SqlCommand(query, connect);
            SqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                if (txtUrunID.Text == read["urunID"].ToString())
                {
                    flag = false;
                }
            }
            connect.Close();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            urunID_kontrol();
            if (flag == true) {
                connect.Open();
                string query = "insert into sepet(musteriNo,adSoyad,telefon,urunID,urunAdi,miktar,satisFiyati,toplamFiyati,tarih) values(@musteriNo,@adSoyad,@telefon,@urunID,@urunAdi,@miktar,@satisFiyati,@toplamFiyati,@tarih)";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@musteriNo", int.Parse(txtMusteriNo.Text));
                command.Parameters.AddWithValue("@adSoyad", txtAdSoyad.Text);
                command.Parameters.AddWithValue("@telefon", txtTel.Text);
                command.Parameters.AddWithValue("@urunID", int.Parse(txtUrunID.Text));
                command.Parameters.AddWithValue("@urunAdi", txtUrunAdi.Text);
                command.Parameters.AddWithValue("@miktar", int.Parse(txtMiktar.Text));
                command.Parameters.AddWithValue("@satisFiyati", double.Parse(txtSatisFiyati.Text));
                command.Parameters.AddWithValue("@toplamFiyati", double.Parse(txtToplamFiyat.Text));
                command.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                command.ExecuteNonQuery();
                connect.Close();
            }
            else
            {
                connect.Open();
                string query2 = "update sepet set miktar = miktar + '" + int.Parse(txtMiktar.Text) + "' where urunID ='" + int.Parse(txtUrunID.Text) + "'";
                SqlCommand command2 = new SqlCommand(query2, connect);
                command2.ExecuteNonQuery();
                string query3 = "update sepet set toplamFiyati = miktar * satisFiyati where urunID ='" + int.Parse(txtUrunID.Text) + "'";
                SqlCommand command3 = new SqlCommand(query3, connect);
                command3.ExecuteNonQuery();
                connect.Close();
            }
            ds.Tables["sepet"].Clear();
            sepetListele();
            hesapla();
            txtMiktar.Text = "1";
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtMiktar)
                    {
                        item.Text = "";
                    }
                }
            }
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void txtMiktar_TextChanged(object sender, EventArgs e)
        {
            try {
                txtToplamFiyat.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception ex) {
                ;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            connect.Open();
            String query = String.Format("delete from sepet where urunID = '" + dataGridView1.CurrentRow.Cells["urunID"].Value.ToString()) + "'";
            SqlCommand command = new SqlCommand(query, connect);
            command.ExecuteNonQuery();
            connect.Close();
            MessageBox.Show("Ürün sepetten çıkarıldı");
            ds.Tables["sepet"].Clear();/*Güncelleme sonrası kayıtlar yeniden getirildi*/
            sepetListele();
            hesapla();
        }

        private void btnSatisİptal_Click(object sender, EventArgs e)
        {
            connect.Open();
            String query = "truncate table sepet";
            SqlCommand command = new SqlCommand(query, connect);
            command.ExecuteNonQuery();
            connect.Close();
            MessageBox.Show("Tüm Ürünler sepetten çıkarıldı");
            ds.Tables["sepet"].Clear();
            sepetListele();
            hesapla();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            frmSatisListele listele = new frmSatisListele();
            listele.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
            connect.Open();
            string query = "insert into stok_takip(musteriNo,adSoyad,telefon,urunID,urunAdi,miktar,satisFiyati,toplamFiyati,tarih) values(@musteriNo,@adSoyad,@telefon,@urunID,@urunAdi,@miktar,@satisFiyati,@toplamFiyati,@tarih)";
            SqlCommand command = new SqlCommand(query, connect);
            command.Parameters.AddWithValue("@musteriNo", int.Parse(dataGridView1.Rows[i].Cells["musteriNo"].Value.ToString()));
            command.Parameters.AddWithValue("@adSoyad", dataGridView1.Rows[i].Cells["adSoyad"].Value.ToString());
            command.Parameters.AddWithValue("@telefon", dataGridView1.Rows[i].Cells["telefon"].Value.ToString());
            command.Parameters.AddWithValue("@urunID", int.Parse(dataGridView1.Rows[i].Cells["urunID"].Value.ToString()));
            command.Parameters.AddWithValue("@urunAdi", dataGridView1.Rows[i].Cells["urunAdi"].Value.ToString());
            command.Parameters.AddWithValue("@miktar", int.Parse(dataGridView1.Rows[i].Cells["miktar"].Value.ToString()));
            command.Parameters.AddWithValue("@satisFiyati", double.Parse(dataGridView1.Rows[i].Cells["satisFiyati"].Value.ToString()));
            command.Parameters.AddWithValue("@toplamFiyati", double.Parse(dataGridView1.Rows[i].Cells["toplamFiyati"].Value.ToString()));
            command.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
            command.ExecuteNonQuery();
            string query2 = "update urun set miktar = miktar - '" + int.Parse(dataGridView1.Rows[i].Cells["miktar"].Value.ToString()) + "' where urunID = '" + int.Parse(dataGridView1.Rows[i].Cells["urunID"].Value.ToString())+"'";
            SqlCommand command2 = new SqlCommand(query2, connect);
            command2.ExecuteNonQuery();
            connect.Close();
            }
            connect.Open();
            String query3 = "truncate table sepet";
            SqlCommand command3 = new SqlCommand(query3, connect);
            command3.ExecuteNonQuery();
            connect.Close();
            ds.Tables["sepet"].Clear();
            sepetListele();
            hesapla();
        }
    } 
}
