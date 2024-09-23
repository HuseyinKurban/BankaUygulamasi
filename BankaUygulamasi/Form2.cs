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

namespace BankaUygulamasi
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-32Q9FH5;Initial Catalog=DbBankaTest;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");

        public string ad, hesapno, tc, telefon;

        void paraartis()
        {

            //gönderilen hesabın para artışı
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update TBLHESAP set BAKIYE=BAKIYE+@p1 where HESAPNO=@p2", baglanti);
            komut.Parameters.AddWithValue("@p1", decimal.Parse(txttutar.Text));
            komut.Parameters.AddWithValue("@p2", msktxhesapno.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("İşlem Gerçekleşti");
        }

        void harekettablosu()
        {
            //Hareket Tablosu
            baglanti.Open();
            SqlCommand komut2 = new SqlCommand("insert into TBLHAREKET (GONDEREN,ALICI,TUTAR) values (@p1,@p2,@p3)", baglanti);
            komut2.Parameters.AddWithValue("@p1", hesapno);
            komut2.Parameters.AddWithValue("@p2", msktxhesapno.Text);
            komut2.Parameters.AddWithValue("@p3", txttutar.Text);
            komut2.ExecuteNonQuery();
            baglanti.Close();
        }

        void guncelle()
        {
            baglanti.Open();
            DataTable dt = new DataTable();
            SqlCommand komut = new SqlCommand("SELECT DISTINCT \r\n    k1.AD+' '+k1.SOYAD AS 'Gönderen',\r\n    k2.AD+' '+k2.SOYAD AS 'Alıcı',\r\n    TUTAR as 'Tutar'\r\nFROM TBLHAREKET \r\nINNER JOIN TBLKISILER AS k1 ON k1.HESAPNO = TBLHAREKET.GONDEREN\r\nINNER JOIN TBLKISILER AS k2 ON k2.HESAPNO = TBLHAREKET.ALICI\r\nWHERE TBLHAREKET.ALICI =@p1 OR TBLHAREKET.GONDEREN =@p1;", baglanti);
            komut.Parameters.AddWithValue("@p1", hesapno);
            SqlDataReader dr = komut.ExecuteReader();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }
        void bakiye()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select BAKIYE from TBLHESAP where HESAPNO=" + hesapno, baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                lblbakiye.Text = dr[0].ToString();
            }

            baglanti.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            lbladsoyad.Text = ad;
            lblhesapno.Text = hesapno;
            lbltckimlik.Text = tc;
            lbltelefon.Text = telefon;
            guncelle();
            bakiye();

        }

        private void btngonder_Click(object sender, EventArgs e)
        {
            
            baglanti.Open();
            SqlCommand parakontrol = new SqlCommand("Select SUM(Bakıye) from TBLHESAP where HESAPNO=" + hesapno, baglanti);


            decimal toplam = (decimal)parakontrol.ExecuteScalar(); // Para sayısı

            if (toplam <= 0 || toplam < Convert.ToDecimal(txttutar.Text))
            {
                MessageBox.Show("Bakiyeniz Yetersiz. Mevcut Bakiyeniz: " + toplam, "Bilgi");
                baglanti.Close();
            }
            else
            {
                //gönderen hesabın para azalışı
                SqlCommand komut1 = new SqlCommand("update TBLHESAP set BAKIYE=BAKIYE-@p1 where HESAPNO=@p2", baglanti);
                komut1.Parameters.AddWithValue("@p1", decimal.Parse(txttutar.Text));
                komut1.Parameters.AddWithValue("@p2", hesapno);
                komut1.ExecuteNonQuery();
                baglanti.Close();
                paraartis();
                harekettablosu();
                guncelle();
                bakiye();

            }


        }

    }
}
