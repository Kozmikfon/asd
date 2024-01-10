using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GaleriOtomasyonu
{
    public partial class FrmMusteriGiris : Form
    {
        public static string ad;
        public static string soyad;
        public static string mail_adresi;
        public static string sifre;
        string baglantiCumlesi="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\mahmu\\Desktop\\projeler\\galeriKisiselbilgi.mdb";
        public FrmMusteriGiris()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            FrmMusteriUye frmMusteriUye = new FrmMusteriUye();
            frmMusteriUye.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmAnaSayfa frmAnaSayfa = new FrmAnaSayfa();
            frmAnaSayfa.Show();
        }
        public void Getir()
        {
            OleDbConnection baglanti = new OleDbConnection(baglantiCumlesi);// veri tabanı bağlantısını ve verileri çekmemize yarar
            baglanti.Open();
            string komutCumlesi = "Select * From bilgiler where MailAdres like '" + TxtMail.Text + "' ";
            OleDbCommand komutYeni = new OleDbCommand(komutCumlesi, baglanti);  // veritabanıyla etkileşim sağlar ;select,uptade,delete,insert
            OleDbDataReader oku = komutYeni.ExecuteReader();  // sorgu sonucu dönen verileri satır satır okumaya yarar

            while (oku.Read())
            {
                ad = oku["Isim"].ToString(); // oledbdatareader nesnesi üzerinden firstname sütunun değerini string değişkenine atar
                soyad = oku["SoyIsim"].ToString();
                mail_adresi = oku["MailAdres"].ToString();
                sifre = oku["Sifre"].ToString();
            }
            baglanti.Close();
        }

        private void BtnGirisYap_Click(object sender, EventArgs e)
        {
            Getir();
            OleDbConnection baglanti = new OleDbConnection(baglantiCumlesi);
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("SELECT * FROM bilgiler", baglanti);
            OleDbDataReader oku = komut.ExecuteReader();// sorgu sonucu dönen verileri satır satır okumaya yarar

            while (oku.Read())
            {
                if (TxtMail.Text == oku["MailAdres"].ToString() && TxtSifre.Text == oku["Sifre"].ToString())
                {
                    FrmAracListele listele = new FrmAracListele();// girişbaşarılı adlı yeni bir forma geçer
                    listele.ShowDialog();// formun görüntülenmesini sağlar
                    this.Hide();// yeni açılan bir form için mevcut formu gizler ama yinede çalışmaya devam eder
                }
                else
                {
                    MessageBox.Show("Bilgileri yanlış girdiniz", " Durum", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            oku.Close();
            baglanti.Close();

        }

        private void PictureBoxGoz_Click(object sender, EventArgs e)
        {
            TxtSifre.PasswordChar = '\0';
            
        }
    }
}
