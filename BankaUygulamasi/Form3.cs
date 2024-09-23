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

namespace BankaUygulamasi
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-32Q9FH5;Initial Catalog=DbBankaTest;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");

        void temizle()
        {
            txtad.Text = "";
            txtsoyad.Text = "";
            msktckimlik.Text = "";
            msktelefon.Text = "";
            mskhesapno.Text = "";
            txtsifre.Text = "";
            txtad.Focus();
        }

        private void btnkaydol_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut=new SqlCommand("insert into TBLKISILER (AD,SOYAD,TC,TELEFON,HESAPNO,SIFRE) values (@p1,@p2,@p3,@p4,@p5,@p6)",baglanti);
            komut.Parameters.AddWithValue("@p1", txtad.Text);
            komut.Parameters.AddWithValue("@p2", txtsoyad.Text);
            komut.Parameters.AddWithValue("@p3", msktckimlik.Text);
            komut.Parameters.AddWithValue("@p4", msktelefon.Text);
            komut.Parameters.AddWithValue("@p5", mskhesapno.Text);
            komut.Parameters.AddWithValue("@p6", txtsifre.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kaydınız Oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bnhesapnoolustur_Click(object sender, EventArgs e)
        {
            Random rnd=new Random();
            int rasgele=rnd.Next(100000,1000000);
            mskhesapno.Text= rasgele.ToString();

            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT COUNT(*) FROM TBLKISILER WHERE HESAPNO="+mskhesapno.Text, baglanti);
            

            int count = (int)komut.ExecuteScalar(); // Kayıt sayısını al

            if (count > 0) // Eğer hesap numarası mevcutsa yani coun 1 ise
            {
                MessageBox.Show(mskhesapno.Text+" nolu hesap numarası kullanılmakta, lütfen yeniden oluşturunuz.","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                mskhesapno.Text = "";
            }
          
            baglanti.Close();



        }
    }
}
