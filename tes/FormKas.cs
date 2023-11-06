using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace tes
{
    public partial class FormKas : Form
    {
        string server = "localhost";
        string database = "cashier";
        string uid = "root";
        string password = "";
        private string Faktura = "";

        public FormKas()
        {
            InitializeComponent();
        }

        private void loadData()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string kueri = "select no, faktur, DATE(tgl) AS tgl, jenis, kategori, pemasukan, pengeluaran, keterangan, operator from tb_kas;";

            MySqlConnection connection = new MySqlConnection(connectionString);
            using (MySqlCommand cmd = new MySqlCommand(kueri, connection))
            {
                try
                {
                    connection.Open();
                    decimal totalPengeluaran = 0;
                    decimal totalPemasukan = 0;
                    decimal saldoSelisih = 0;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Mengecek apakah ada data yang bisa dibaca
                        if (reader.HasRows)
                        {
                            // Bersihkan DataGridView jika sudah ada data sebelumnya
                            dgv.Rows.Clear();

                            // Loop melalui hasil pembacaan
                            while (reader.Read())
                            {
                                int no = reader.GetInt32(0);
                                string faktur = reader.GetString(1);
                                string tgl = reader.GetDateTime(2).ToString("yyyy-MM-dd");
                                string jenis = reader.GetString(3);
                                string kategori = reader.GetString(4);
                                decimal pemasukan = reader.GetDecimal(5);
                                decimal pengeluaran = reader.GetDecimal(6);
                                string ket = reader.GetString(7);
                                string op = reader.GetString(8);

                                string pemasukanText = pemasukan.ToString("N0", new CultureInfo("id-ID"));
                                string pengeluaranText = pengeluaran.ToString("N0", new CultureInfo("id-ID"));

                                dgv.Rows.Add(no, faktur, tgl, jenis, kategori, pemasukanText, pengeluaranText, ket, op);
                                totalPengeluaran += pengeluaran;
                                totalPemasukan += pemasukan;
                                saldoSelisih = totalPemasukan - totalPengeluaran;
                            }
                        }
                    }
                    connection.Close();
                    lblKasKeluar.Text = totalPengeluaran.ToString("N0", new CultureInfo("id-ID"));
                    lblKasMasuk.Text = totalPemasukan.ToString("N0", new CultureInfo("id-ID"));
                    lblSelisih.Text = saldoSelisih.ToString("N0", new CultureInfo("id-ID"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void searchData()
        {

            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string kueri = "select no, faktur, DATE(tgl) AS tgl, jenis, kategori, pemasukan, pengeluaran, keterangan, operator from tb_kas where no LIKE '%" + SEARCH.Text + "%' OR faktur LIKE '%" + SEARCH.Text + "%'";

            MySqlConnection connection = new MySqlConnection(connectionString);
            using (MySqlCommand cmd = new MySqlCommand(kueri, connection))
            {
                try
                {
                    connection.Open();
                    decimal totalPengeluaran = 0;
                    decimal totalPemasukan = 0;
                    decimal saldoSelisih = 0;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Mengecek apakah ada data yang bisa dibaca
                        if (reader.HasRows)
                        {
                            // Bersihkan DataGridView jika sudah ada data sebelumnya
                            dgv.Rows.Clear();

                            // Loop melalui hasil pembacaan
                            while (reader.Read())
                            {
                                int no = reader.GetInt32(0);
                                string faktur = reader.GetString(1);
                                string tgl = reader.GetDateTime(2).ToString("yyyy-MM-dd");
                                string jenis = reader.GetString(3);
                                string kategori = reader.GetString(4);
                                decimal pemasukan = reader.GetDecimal(5);
                                decimal pengeluaran = reader.GetDecimal(6);
                                string ket = reader.GetString(7);
                                string op = reader.GetString(8);

                                string pemasukanText = pemasukan.ToString("N0", new CultureInfo("id-ID"));
                                string pengeluaranText = pengeluaran.ToString("N0", new CultureInfo("id-ID"));

                                dgv.Rows.Add(no, faktur, tgl, jenis, kategori, pemasukanText, pengeluaranText, ket, op);

                                totalPengeluaran += pengeluaran;
                                totalPemasukan += pemasukan;
                                saldoSelisih = totalPemasukan - totalPengeluaran;
                            }
                        }
                    }
                    connection.Close();
                    lblKasKeluar.Text = totalPengeluaran.ToString("N0", new CultureInfo("id-ID"));
                    lblKasMasuk.Text = totalPemasukan.ToString("N0", new CultureInfo("id-ID"));
                    lblSelisih.Text = saldoSelisih.ToString("N0", new CultureInfo("id-ID"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void FormKas_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void SEARCH_TextChanged(object sender, EventArgs e)
        {
            searchData();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            FormAddKas form = new FormAddKas();
            form.NoFaktur = Faktura;
            form.condition = "insert";
            form.ShowDialog();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnDelete.Enabled = true;
            btnUpdate.Enabled = true;
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = dgv.Rows[e.RowIndex];
                Faktura = row.Cells["Column1"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FormAddKas form = new FormAddKas();
            form.NoFaktur = Faktura;
            form.condition = "update";
            form.ShowDialog();
        }
    }
}
