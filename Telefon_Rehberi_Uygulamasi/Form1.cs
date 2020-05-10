using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Telefon_Rehberi_Uygulamasi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            KisiListesi();
        }

        TelefonRehberi2Entities db = new TelefonRehberi2Entities();
        private void KisiListesi()
        {
            List<Kisiler> kisiler = db.Kisilers.ToList();
            listView1.Items.Clear();
            foreach (var liste in kisiler)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = liste.ID.ToString();
                lvi.SubItems.Add(liste.Ad);
                lvi.SubItems.Add(liste.Soyad);
                lvi.SubItems.Add(liste.Telefon);
                lvi.Tag = liste;
                listView1.Items.Add(lvi);
            }
        }

        private void KisiListesi(string parametre)
        {
            List<Kisiler> kisiler = db.Kisilers.Where(x => x.Ad.Contains(parametre) || x.Soyad.Contains(parametre) || x.Telefon.Contains(parametre)).ToList();
            listView1.Items.Clear();
            foreach (var liste in kisiler)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = liste.ID.ToString();
                lvi.SubItems.Add(liste.Ad);
                lvi.SubItems.Add(liste.Soyad);
                lvi.SubItems.Add(liste.Telefon);
                lvi.Tag = liste;
                listView1.Items.Add(lvi);
            }
        }

        private void Temizle()
        {
            txtAdi.Clear();
            txtSoyadi.Clear();
            txtTelefon.Clear();
            txtAra.Clear();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            // Veritabanına Ekleme(1. Yol):
            Kisiler k = new Kisiler();
            k.Ad = txtAdi.Text;
            k.Soyad = txtSoyadi.Text;
            k.Telefon = txtTelefon.Text;
            db.Kisilers.Add(k);
            int sonuc = db.SaveChanges();
            MessageBox.Show(sonuc > 0 ? "Kişi rehbere eklendi." : "Kişi ekleme sırasında hata oluştu.", "Rehber", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Temizle();
            KisiListesi();

            // Veritabanına Ekleme(2. Yol):
         /* db.Kisilers.Add(new Kisiler
            {
                Ad = txtAdi.Text,
                Soyad = txtSoyadi.Text,
                Telefon = txtTelefon.Text
            }); */
        }

        Kisiler guncellenecek_kisi;
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            guncellenecek_kisi = listView1.SelectedItems[0].Tag as Kisiler;
            txtAdi.Text = guncellenecek_kisi.Ad;
            txtSoyadi.Text = guncellenecek_kisi.Soyad;
            txtTelefon.Text = guncellenecek_kisi.Telefon;
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            guncellenecek_kisi.Ad = txtAdi.Text;
            guncellenecek_kisi.Soyad = txtSoyadi.Text;
            guncellenecek_kisi.Telefon = txtTelefon.Text;
            db.SaveChanges();
            KisiListesi();
            Temizle();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            int id = ((Kisiler)listView1.SelectedItems[0].Tag).ID;
            // Listview'da her eleman tag property'sin Kisiler tipinde kendi nesnesini tuttuğundan seçilen elemanın propertylerinden id propertysini aldık.
            Kisiler silinecek_kisi = db.Kisilers.Find(id);
            // Alınan id property'sine göre Find() methodu ile veritabanındaki kişiler tablosundan orjinal veriyi çektik.
            db.Kisilers.Remove(silinecek_kisi);
            // Remove methodu veritabanında silme işlemi yapabilmek için Kisiler tipinde parametre ister. Yukarıda Find ile bulduğumuz kişiyi Remove'a parametre olarak verdik.
            db.SaveChanges();
            // Veritabanındaki tabloda değişiklik olduğu için bunu da kaydettik.
            KisiListesi();
            Temizle();
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            KisiListesi(txtAra.Text);
        }
    }
}
// EntityTracker: Entity Framework'ün veritabanı ile uygulama arasındaki verilerin durumunu izler.
// EntityState.Add: Yeni kayıt.
// EntityState.Deleted: Silinen kayıt.
// EntityState.Modified: Güncellenmiş kayıt.