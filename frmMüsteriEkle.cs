using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;/*Database bağlantısı için gerekli kütüphane eklendi*/

namespace stock_tracking_automation
{
    public partial class frmMüsteriEkle : Form
    {
        public frmMüsteriEkle()
        {
            InitializeComponent();
        }
        /*Data base bağlantısı sağlandı*/
        SqlConnection connect = new SqlConnection("Data Source=DESKTOP-UQ3M9IO;Initial Catalog=Stok_Takip;Integrated Security=True");
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmMüsteriEkle_Load(object sender, EventArgs e)
        {

        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            connect.Open();
            SqlCommand command = new SqlCommand("insert into müsteri(musteriNo,adSoyad,telefon,adres,email) values(@musteriNo,@adSoyad,@telefon,@adres,@email) ", connect);
            command.Parameters.AddWithValue("@musteriNo", txtMusteriNo.Text);/*textBox içerisine girilen parametreler sql komutu içerisine aktarıldı*/
            command.Parameters.AddWithValue("@adSoyad", txtAdSoyad.Text);
            command.Parameters.AddWithValue("@telefon", txtTelefon.Text);
            command.Parameters.AddWithValue("@adres", txtAdres.Text);
            command.Parameters.AddWithValue("@email", txtEmail.Text);
            command.ExecuteNonQuery();/*Database komutu çalıştırıldı*/
            connect.Close(); /*Database bağlantısı kapatıldı*/
            MessageBox.Show("Müsteri Kaydı Eklendi");
            foreach (Control item in this.Controls)
                /*Müşteri eklendikten sonra textBox içerisindeki yazılar silindi*/
            {
                if(item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
