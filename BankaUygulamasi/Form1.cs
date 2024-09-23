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
using System.Security.Policy;

namespace BankaUygulamasi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-32Q9FH5;Initial Catalog=DbBankaTest;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");

        private void lnkkayıtol_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form3 frm = new Form3();
            frm.Show();
        }

        private void btngiris_Click(object sender, EventArgs e)
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select * From TBLKISILER where HESAPNO=@p1 and SIFRE=@p2", baglanti);
            komut.Parameters.AddWithValue("@p1", msktxthesapno.Text);
            komut.Parameters.AddWithValue("@p2", txtsifre.Text);
            SqlDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                Form2 fr = new Form2();
                fr.ad = dr[1] + " " + dr[2];
                fr.tc = dr[3].ToString();
                fr.telefon = dr[4].ToString();
                fr.hesapno = msktxthesapno.Text;
                fr.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Bilgilerinizi Kontrol Ediniz", "Hata");
            }
            baglanti.Close();

        }
    }
}
