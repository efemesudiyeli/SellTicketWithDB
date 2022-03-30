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


namespace CinemaApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listView1.FullRowSelect = true;
            veriGetir();
        }

        SqlConnection conn = new SqlConnection("Data Source=MSI;Initial Catalog=CinemaDB;Integrated Security=True");
        private void veriGetir()
        {
            
            conn.Open();
            SqlCommand commandoku = new SqlCommand("SELECT * FROM Musteri", conn);
            SqlDataReader read = commandoku.ExecuteReader();
            while (read.Read())
            {
                ListViewItem liste = new ListViewItem();
                liste.Text = read["SatisNo"].ToString();
                liste.SubItems.Add(read["FilmAdi"].ToString());
                liste.SubItems.Add(read["SalonAdi"].ToString());
                liste.SubItems.Add(read["FilmTarihi"].ToString());
                liste.SubItems.Add(read["FilmSeansi"].ToString());
                liste.SubItems.Add(read["KoltukNo"].ToString());
                liste.SubItems.Add(read["Ad"].ToString());
                liste.SubItems.Add(read["Soyad"].ToString());
                liste.SubItems.Add(read["Ucret"].ToString());
                listView1.Items.Add(liste);
                

            }
            conn.Close();
            

        }


        // Müşteri satış kaydı ekleme --> bilet sat.
        private void veriEkle() 
        {

            // TEXTBOX BOŞLUK KONTROLÜ
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Film adını boş bırakamazsınız!");
                conn.Close();
            }

            else if (String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Salon adını boş bırakamazsınız!");
                conn.Close();
            }

            else if (String.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Koltuk numarasını boş bırakamazsınız!");
                conn.Close();
            }

            else if (String.IsNullOrEmpty(textBox9.Text))
            {
                MessageBox.Show("Ad kısmını boş bırakamazsınız!");
                conn.Close();
            }

            else if (String.IsNullOrEmpty(textBox8.Text))
            {
                MessageBox.Show("Soyad kısmını boş bırakamazsınız!");
                conn.Close();
            }
            else if (String.IsNullOrEmpty(textBox7.Text))
            {
                MessageBox.Show("Ücret kısmını boş bırakamazsınız!");
                conn.Close();
            }

            else
            {
                string fAdi = textBox1.Text;
                string sAdi = textBox2.Text;

                DateTime fTarih = dateTimePicker1.Value.Date;
                MessageBox.Show(fTarih.ToString());

                TimeSpan fSeans;
                fSeans = dateTimePicker2.Value.TimeOfDay;

                string koltukNo = textBox4.Text;
                string ad = textBox9.Text;
                string soyad = textBox8.Text;
                string ucret = textBox7.Text;




                SqlCommand commandekle = new SqlCommand("INSERT INTO Musteri (FilmAdi, SalonAdi, FilmTarihi, FilmSeansi, KoltukNo" +
                    ", Ad, Soyad, Ucret) VALUES ('" + fAdi + "','" + sAdi + "','" + fTarih + "','" + fSeans + "','" + koltukNo + "','" + ad + "','" + soyad + "','" + ucret + "')", conn);
                commandekle.ExecuteNonQuery();

                // SATIŞTAN SONRA SIFIRLAMA İŞLEMLERİ
                textBox1.Clear();
                textBox2.Clear();
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
                textBox4.Value = 0;
                textBox9.Clear();
                textBox8.Clear();
                textBox7.Value = 0;
       
                conn.Close();
            }



            


        }


        // Kayıt Ekleme -- Bilet Satış
        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            veriEkle();
            listView1.Items.Clear();
            veriGetir();
            
            
            

        }

        // Yenile
        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            veriGetir();
        }


        private void VeriSil()
        {

            var kNo = textBox3.Text;
            conn.Open();
            SqlCommand commanddelete = new SqlCommand(
                "DELETE FROM Musteri WHERE KoltukNo = '"+kNo+"' ", conn);

            commanddelete.ExecuteNonQuery();
            conn.Close();


        }

        // Bilet İptal
        private void button3_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Koltuk numarasını girmeden iptal işlemi gerçekleştiremezsiniz.");

            }
            else
            {  
                VeriSil();
                listView1.Items.Clear();
                veriGetir(); 
            }
            
        }
        
    
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            string  _fAdi, _sAdi, _fTarihi, _fSeansi, _kNo, _Ad, _Soyad, _Ucret;
            int _sNo;

            _sNo = Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text);
            _fAdi = listView1.SelectedItems[0].SubItems[1].Text;
            _sAdi = listView1.SelectedItems[0].SubItems[2].Text;
            _fTarihi = listView1.SelectedItems[0].SubItems[3].Text;
            _fSeansi = listView1.SelectedItems[0].SubItems[4].Text;
            _kNo = listView1.SelectedItems[0].SubItems[5].Text;
            _Ad = listView1.SelectedItems[0].SubItems[6].Text;
            _Soyad = listView1.SelectedItems[0].SubItems[7].Text;
            _Ucret = listView1.SelectedItems[0].SubItems[8].Text;

            textBox3.Text = _kNo;
            textBox1.Text = _fAdi;
            textBox2.Text = _sAdi;
            numericUpDown1.Value = _sNo;

            DateTime _dfTarihi = Convert.ToDateTime(_fTarihi);
            dateTimePicker1.Value = _dfTarihi;

            DateTime _dfSeansi = Convert.ToDateTime(_fSeansi);
            dateTimePicker2.Value = _dfSeansi;

            textBox4.Text = _kNo;
            textBox9.Text = _Ad;
            textBox8.Text = _Soyad;
            textBox7.Text = _Ucret;

        }

        // UPDATE DÜZENLEME İŞLEMLERİ
        private void veriDuzenle()
        {

           // GİRİLEN SATISNOYA GÖRE DÜZENLEME İŞLEMİ
            conn.Open();
            SqlCommand commandedit = new SqlCommand(
                "UPDATE Musteri " +
                "SET FilmAdi = '"+textBox1.Text+"', SalonAdi = '"+textBox2.Text+"', FilmTarihi = '"+dateTimePicker1.Value.Date+"', FilmSeansi = '"+dateTimePicker2.Value.TimeOfDay+"', KoltukNo = '"+textBox4.Value+"', Ad = '"+textBox9.Text+"', Soyad = '"+textBox8.Text+"', Ucret = '"+textBox7.Value+"' WHERE SatisNo = '"+numericUpDown1.Value+"' ", conn);

            commandedit.ExecuteNonQuery();
            conn.Close();

        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            veriDuzenle();
            listView1.Items.Clear();
            veriGetir();
        }
    }
}
