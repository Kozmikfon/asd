using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GaleriOtomasyonu
{
    public partial class FrmSozlesme : Form
    {
        AracIslemleri aracsozlesme=new AracIslemleri();
        string baglanti = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\mahmu\\Desktop\\projeler\\galeriKisiselbilgi.mdb";
        public FrmSozlesme()
        {
            InitializeComponent();
            
        }

        private void FrmSozlesme_Load(object sender, EventArgs e)
        {
            BosAraclar();
            Yenile();
        }

        private void BosAraclar()
        {
            string sorgu = "Select * From AracBilgiler where Durumu='BOŞ' ";
            aracsozlesme.BosAraclar(CmboxArac, sorgu);
        }

        private void Yenile()
        {
            string sorgu2 = "Select * From SozlesmeBilgisi";
            OleDbDataAdapter adtr = new OleDbDataAdapter();
            dataGridView1.DataSource = aracsozlesme.Listele(adtr, sorgu2);
        }
 
        private void CmboxArac_SelectedIndexChanged(object sender, EventArgs e)
        {            
            string sorgu = "Select * From AracBilgiler where Plaka like '" + CmboxArac.SelectedItem + "' ";
            aracsozlesme.CombodanGetir(CmboxArac, CmboxMarka, CmboxSeri,TxtModelYil,CmboxRenk ,TxtKapora, sorgu);
        }

        private void CmboxKiraSekli_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sorgu = "Select * From AracBilgiler where Plaka like '" + CmboxArac.SelectedItem + "' ";
            aracsozlesme.UcretHesapla(CmboxKiraSekli,TxtKiraUcret,sorgu);
        }

        private void BtnHesapla_Click(object sender, EventArgs e)
        {
            TimeSpan gun = DateTime.Parse(DatePDonus.Text) - DateTime.Parse(DatePCikis.Text);
            int gun2 = gun.Days;
            TxtGun.Text=gun2.ToString();
            TxtTutar.Text = (gun2 * int.Parse(TxtKiraUcret.Text)).ToString();
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void Temizle()
        {
            DatePCikis.Text = DateTime.Now.ToString();
            DatePDonus.Text = DateTime.Now.ToString();
            CmboxKiraSekli.Text = "";
            TxtKiraUcret.Text = "";
            TxtGun.Text = "";
            TxtTutar.Text = "";
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            string sorgu = "insert into SozlesmeBilgisi(Tc,Ad,Soyad,Mail,EhliyetNo,E_Tarih,E_Yer,Plaka,Marka,Seri,ModelYili,Renk,KiraSekli,KiraUcreti,Gun,Tutar,CikisTarihi,DonusTarihi,KaporaMiktari) values(@Tc,@Ad,@Soyad,@Mail,@EhliyetNo,@E_Tarih,@E_Yer,@Plaka,@Marka,@Seri,@ModelYili,@Renk,@KiraSekli,@KiraUcreti,@Gun,@Tutar,@CikisTarihi,@DonusTarihi,@KaporaMiktari)";
            OleDbCommand komut = new OleDbCommand();
            komut.Parameters.AddWithValue("@Tc",TxtTC.Text);
            komut.Parameters.AddWithValue("@Ad",TxtAd.Text);
            komut.Parameters.AddWithValue("@Soyad",TxtSoyad.Text);
            komut.Parameters.AddWithValue("@Mail",TxtMail.Text);
            komut.Parameters.AddWithValue("@EhliyetNo",TxtEhliyet.Text);
            komut.Parameters.AddWithValue("@E_Tarih",DateTimPTarih.Text);
            komut.Parameters.AddWithValue("@E_Yer",TxtEYer.Text);
            komut.Parameters.AddWithValue("@Plaka",CmboxArac.Text);
            komut.Parameters.AddWithValue("@Marka",CmboxMarka.Text);
            komut.Parameters.AddWithValue("@Seri",CmboxSeri.Text);
            komut.Parameters.AddWithValue("@ModelYili",TxtModelYil.Text);
            komut.Parameters.AddWithValue("@Renk",CmboxRenk.Text);
            komut.Parameters.AddWithValue("@KiraSekli",CmboxKiraSekli.Text);
            komut.Parameters.AddWithValue("@KiraUcreti",TxtKiraUcret.Text);
            komut.Parameters.AddWithValue("@Gun",TxtGun.Text);
            komut.Parameters.AddWithValue("@Tutar",TxtTutar.Text);
            komut.Parameters.AddWithValue("@CikisTarihi",DatePCikis.Text);
            komut.Parameters.AddWithValue("@DonusTarihi",DatePDonus.Text);
            komut.Parameters.AddWithValue("@KaporaMiktari",TxtKapora.Text);
            aracsozlesme.EkleSilGuncelle(komut,sorgu);
            string sorgu2 = "update AracBilgiler set Durumu='DOLU' where Plaka ='"+CmboxArac.Text+"'";
            OleDbCommand komut2 = new OleDbCommand();
            aracsozlesme.EkleSilGuncelle(komut2,sorgu2);
            CmboxArac.Items.Clear();
            BosAraclar();
            Yenile();
            foreach (Control item in groupBox1.Controls) if (item is TextBox) item.Text = "";
            foreach (Control item in groupBox2.Controls) if (item is TextBox) item.Text = "";
            CmboxArac.Text = "";
            Temizle();
            MessageBox.Show("Sözleşme eklendi");
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            string sorgu = "update SozlesmeBilgisi set Tc='"+TxtTC.Text+"', Ad='"+TxtAd.Text+"', Soyad='"+TxtSoyad.Text+ "', Mail='"+TxtMail.Text+"', EhliyetNo='"+TxtEhliyet.Text+"', E_Tarih='"+DateTimPTarih.Text+"', E_Yer='"+TxtEYer.Text+"', Marka='"+CmboxMarka.Text+"' ,Seri='"+CmboxSeri.Text+"', ModelYili='"+TxtModelYil.Text+"', Renk='"+CmboxRenk.Text+"', KiraSekli='"+CmboxKiraSekli.Text+"', KiraUcreti='"+TxtKiraUcret.Text+"', Gun='"+TxtGun.Text+"', Tutar='"+TxtTutar.Text+"',CikisTarihi='"+DatePCikis.Text+"', DonusTarihi='"+DatePDonus.Text+"', KaporaMiktari='"+TxtKapora.Text+"' Where Plaka='"+CmboxArac.Text+"'";
            OleDbCommand komut = new OleDbCommand();
            komut.Parameters.AddWithValue("@Tc", TxtTC.Text);
            komut.Parameters.AddWithValue("@Ad", TxtAd.Text);
            komut.Parameters.AddWithValue("@Soyad", TxtSoyad.Text);
            komut.Parameters.AddWithValue("@Mail", TxtMail.Text);
            komut.Parameters.AddWithValue("@EhliyetNo", TxtEhliyet.Text);
            komut.Parameters.AddWithValue("@E_Tarih", DateTimPTarih.Text);
            komut.Parameters.AddWithValue("@E_Yer", TxtEYer.Text);           
            komut.Parameters.AddWithValue("@Marka", CmboxMarka.Text);
            komut.Parameters.AddWithValue("@Seri", CmboxSeri.Text);
            komut.Parameters.AddWithValue("@ModelYili", TxtModelYil.Text);
            komut.Parameters.AddWithValue("@Renk", CmboxRenk.Text);
            komut.Parameters.AddWithValue("@KiraSekli", CmboxKiraSekli.Text);
            komut.Parameters.AddWithValue("@KiraUcreti", TxtKiraUcret.Text);
            komut.Parameters.AddWithValue("@Gun", TxtGun.Text);
            komut.Parameters.AddWithValue("@Tutar", TxtTutar.Text);
            komut.Parameters.AddWithValue("@CikisTarihi", DatePCikis.Text);
            komut.Parameters.AddWithValue("@DonusTarihi", DatePDonus.Text);
            komut.Parameters.AddWithValue("@KaporaMiktari",TxtKapora.Text);
            komut.Parameters.AddWithValue("@Plaka", CmboxArac.Text);
            aracsozlesme.EkleSilGuncelle(komut, sorgu);
            
            CmboxArac.Items.Clear();
            BosAraclar();
            Yenile();
            foreach (Control item in groupBox1.Controls) if (item is TextBox) item.Text = "";
            foreach (Control item in groupBox2.Controls) if (item is TextBox) item.Text = "";
            CmboxArac.Text = "";
            Temizle();
            MessageBox.Show("Sözleşme güncellendi");
        }

        private void TxtTcAra_TextChanged(object sender, EventArgs e)
        {
            if (TxtTcAra.Text == "") foreach (Control item in groupBox1.Controls) if (item is TextBox) item.Text = "";

            string sorgu = "Select * From SozlesmeBilgisi where Tc like '" + TxtTcAra.Text + "' ";

            aracsozlesme.TcAra(TxtTcAra, TxtTC, TxtAd, TxtSoyad,TxtMail,TxtEhliyet,TxtEYer, sorgu);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow satir = dataGridView1.CurrentRow;

            TxtTC.Text = satir.Cells[0].Value.ToString();
            TxtAd.Text = satir.Cells[1].Value.ToString();
            TxtSoyad.Text = satir.Cells[2].Value.ToString();
            TxtMail.Text = satir.Cells[3].Value.ToString();
            TxtEhliyet.Text = satir.Cells[4].Value.ToString();
            DateTimPTarih.Text = satir.Cells[5].Value.ToString();
            TxtEYer.Text = satir.Cells[6].Value.ToString();
            CmboxArac.Text = satir.Cells[7].Value.ToString();
            CmboxMarka.Text = satir.Cells[8].Value.ToString();
            CmboxSeri.Text = satir.Cells[9].Value.ToString();
            TxtModelYil.Text = satir.Cells[10].Value.ToString();
            CmboxRenk.Text = satir.Cells[11].Value.ToString();
            CmboxKiraSekli.Text= satir.Cells[12].Value.ToString();
            TxtKiraUcret.Text = satir.Cells[13].Value.ToString();
            TxtGun.Text = satir.Cells[14].Value.ToString();
            TxtTutar.Text = satir.Cells[15].Value.ToString();
            DatePCikis.Text = satir.Cells[16].Value.ToString();
            DatePDonus.Text = satir.Cells[17].Value.ToString();
            TxtKapora.Text = satir.Cells[18].Value.ToString();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DataGridViewRow satir = dataGridView1.CurrentRow;
            string cumle = "delete from SozlesmeBilgisi where Tc='" + satir.Cells["Tc"].Value + "' ";
            OleDbCommand komut = new OleDbCommand(cumle);

            aracsozlesme.EkleSilGuncelle(komut, cumle);
            Yenile();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow satir=dataGridView1.CurrentRow;

            // gun farkı hesaplama
            DateTime bugun = DateTime.Parse(DateTime.Now.ToShortDateString());
            DateTime donus = DateTime.Parse(satir.Cells["DonusTarihi"].Value.ToString());
            int ucret = int.Parse(satir.Cells["KiraUcreti"].Value.ToString());
            TimeSpan gunFarki = donus-bugun;
            int _gunFarki=gunFarki.Days;
            int ucretFarki;
            //ucret farkı hesaplama
            ucretFarki = _gunFarki * ucret;
            TxtEkstra.Text = ucretFarki.ToString();
            // toplam tutar hesaplama

        }

        private void BtnAracTeslim_Click(object sender, EventArgs e)
        {
                
            if (int.Parse(TxtEkstra.Text)>=0 || int.Parse(TxtEkstra.Text)<0)
            {
                DataGridViewRow satir=dataGridView1.CurrentRow;
                DateTime bugun = DateTime.Parse(DateTime.Now.ToShortDateString());
                int ucret = int.Parse(satir.Cells["KiraUcreti"].Value.ToString());
                int Tutar = int.Parse(satir.Cells["Tutar"].Value.ToString());
                DateTime cikis = DateTime.Parse(satir.Cells["CikisTarihi"].Value.ToString());
                TimeSpan gun = bugun - cikis;
                int _gun = gun.Days;
                
                int toplamTutar = _gun * ucret;
                
                // toplamtutar _gun ve ucrey satis tablosuna aktarılacak

                string sorgu = "delete from SozlesmeBilgisi where Plaka ='" + satir.Cells["Plaka"].Value.ToString() +"'";
                OleDbCommand komut=new OleDbCommand();
                aracsozlesme.EkleSilGuncelle(komut,sorgu);

                string sorgu1 = "update AracBilgiler set Durumu='BOŞ' where Plaka='" + satir.Cells["Plaka"].Value.ToString() +"'";
                OleDbCommand komut1 = new OleDbCommand();
                aracsozlesme.EkleSilGuncelle(komut1,sorgu1);

                string sorgu2 = "insert into Kiralama(Tc,Ad,Soyad,Plaka,Marka,Seri,ModelYili,Renk,Gun,Fiyat,Tutar,Tarih1,Tarih2,KaporaMiktari) values(@Tc,@Ad,@Soyad,@Plaka,@Marka,@Seri,@ModelYili,@Renk,@Gun,@Fiyat,@Tutar,@Tarih1,@Tarih2,@KaporaMiktari)";
                OleDbCommand komut2 = new OleDbCommand();
                komut2.Parameters.AddWithValue("@Tc", satir.Cells["Tc"].Value.ToString());
                komut2.Parameters.AddWithValue("@Ad", satir.Cells["Ad"].Value.ToString());
                komut2.Parameters.AddWithValue("@Soyad", satir.Cells["Soyad"].Value.ToString());                               
                komut2.Parameters.AddWithValue("@Plaka", satir.Cells["Plaka"].Value.ToString());
                komut2.Parameters.AddWithValue("@Marka", satir.Cells["Marka"].Value.ToString());
                komut2.Parameters.AddWithValue("@Seri", satir.Cells["Seri"].Value.ToString());
                komut2.Parameters.AddWithValue("@ModelYili", satir.Cells["ModelYili"].Value.ToString());
                komut2.Parameters.AddWithValue("@Renk", satir.Cells["Renk"].Value.ToString());
                komut2.Parameters.AddWithValue("@Gun", satir.Cells["Gun"].Value.ToString());
                komut2.Parameters.AddWithValue("@Fiyat", ucret);
                komut2.Parameters.AddWithValue("@Tutar", toplamTutar);
                komut2.Parameters.AddWithValue("@Tarih1", satir.Cells["Cikistarihi"].Value.ToString() );
                komut2.Parameters.AddWithValue("@Tarih2", satir.Cells["DonusTarihi"].Value.ToString());
                komut2.Parameters.AddWithValue("@KaporaMiktari", satir.Cells["KaporaMiktari"].Value.ToString());
                
                aracsozlesme.EkleSilGuncelle(komut2, sorgu2);

                MessageBox.Show("Araç teslim edildi");
                CmboxArac.Text = "";
                CmboxArac.Items.Clear();
                BosAraclar();
                Yenile();
                foreach (Control item in groupBox1.Controls) if (item is TextBox) item.Text = "";
                foreach (Control item in groupBox2.Controls) if (item is TextBox) item.Text = "";
                CmboxArac.Text = "";
                Temizle();
                TxtEkstra.Text = "";
            }
            else 
            {
                MessageBox.Show("Lütfen seçim yapınız"," Uyarı");
            }
        }

        private void BtnKapora_Click(object sender, EventArgs e)
        {
            string musteriAdi = TxtAd.Text;
            string musteriSoyadi = TxtSoyad.Text;
            string aracMarka = CmboxMarka.Text;
            string aracSeri = CmboxSeri.Text;
            string aracModeli = TxtModelYil.Text;
            int kiraBedeli = Convert.ToInt32(TxtKiraUcret.Text);

            // Müşteri bilgilerini veritabanından al
            int musteriTc = GetMusteriTc(musteriAdi);

            if (musteriTc <= 0)
            {
                MessageBox.Show("Müşteri bilgisi alınamadı. Lütfen sistem yöneticisine başvurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kapora bilgilerini veritabanından al
            int kaporaMiktari = GetKaporaMiktari(musteriTc);

            if (kaporaMiktari <= 0)
            {
                MessageBox.Show("Kapora miktarı alınamadı. Lütfen sistem yöneticisine başvurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kira işlemi başlangıcı
            MessageBox.Show($"Sayın {musteriAdi} {musteriSoyadi}, {aracModeli} model {aracMarka} {aracSeri} aracını kiralama işlemine hoş geldiniz.");

            // Kapora ödeme işlemi
            MessageBox.Show($"Kira bedeli: {kiraBedeli:C}");
            MessageBox.Show($"Ödenecek kapora miktarı: {kaporaMiktari:C}");

            DialogResult cevap = MessageBox.Show("Kapora ödemesi yapmak istiyor musunuz?", "Kapora Ödeme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (cevap == DialogResult.Yes)
            {
                // Kapora ödeme işlemi başarılı
                MessageBox.Show($"Sayın {musteriAdi}, {kaporaMiktari:C} kapora ödemesi başarıyla yapıldı.");

                // Diğer işlemleri burada ekleyebilirsiniz.

                // Kira tamamlanır
                MessageBox.Show($"Kira işlemi tamamlandı. Toplam kalan ödeme miktarı: {kiraBedeli-kaporaMiktari:C}");
            }
            else
            {
                // Kapora ödeme işlemi iptal
                MessageBox.Show($"Sayın {musteriAdi}, kapora ödemesi iptal edildi.");
            }
        }
        private int GetMusteriTc(string musteriAdi)
        {
            int musteriTc = 1;

            using (OleDbConnection connection = new OleDbConnection(baglanti))
            {
                connection.Open();

                using (OleDbCommand command = new OleDbCommand("SELECT Tc FROM Kiralama WHERE Ad = @Ad", connection))
                {
                    command.Parameters.AddWithValue("@Ad", musteriAdi);

                    var result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out musteriTc))
                    {
                        return musteriTc;
                    }
                }
            }

            return musteriTc;
        }
        private int GetKaporaMiktari(int tc)
        {
            int kaporaMiktari = 400;

            using (OleDbConnection connection = new OleDbConnection(baglanti))
            {
                connection.Open();
                 
                using (OleDbCommand command = new OleDbCommand("SELECT KaporaMiktari FROM Kiralama WHERE Tc = @Tc", connection))
                {
                    command.Parameters.AddWithValue("@Tc", tc);

                    var result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out kaporaMiktari))
                    {
                        return kaporaMiktari;
                    }
                }
            }

            return kaporaMiktari;
        }


    }
}
