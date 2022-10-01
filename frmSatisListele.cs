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
    public partial class frmSatisListele : Form
    {
        SqlConnection connect = new SqlConnection("Data Source=DESKTOP-UQ3M9IO;Initial Catalog=Stok_Takip;Integrated Security=True");/*Sql Bağlantısı*/
        DataSet ds = new DataSet();
        public frmSatisListele()
        {
            InitializeComponent();
        }
        private void stokListele()
        {
            connect.Open();
            string query = "select * from stok_takip";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
            adapter.Fill(ds, "stok_takip");
            dataGridView1.DataSource = ds.Tables["stok_takip"];
            connect.Close();
        }

        private void frmSatisListele_Load(object sender, EventArgs e)
        {
            stokListele();
        }
    }
}
