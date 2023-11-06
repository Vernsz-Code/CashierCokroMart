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
    public partial class FormReceivables : Form
    {
        string server = "localhost";
        string database = "cashier";
        string uid = "root";
        string password = "";

        public FormReceivables()
        {
            InitializeComponent();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            FormReceivablesDetails Form = new FormReceivablesDetails();
            Form.ShowDialog();
        }

        private void searchData()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "SELECT DISTINCT no_faktur, DATE(tgl) AS tgl, nama_pelanggan, alamat, total_barang, total_harga, payment, DATE(jatuh_tempo) AS jatuh_tempo, status FROM tb_transaksi WHERE no_faktur LIKE '%" + SEARCH.Text + "%' OR nama_pelanggan LIKE '%" + SEARCH.Text + "%'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        decimal totalPenjualan = 0;

                        connection.Open();
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
                                    int no_faktur = reader.GetInt32(0);
                                    string tgl = reader.GetDateTime(1).ToString("yyyy-MM-dd");
                                    string namaPelanggan = reader.GetString(2);
                                    string status = reader.GetString(8);
                                    string alamat = reader.GetString(3);
                                    int totalBarang = reader.GetInt32(4);
                                    decimal totalHarga = reader.GetDecimal(5);

                                    string jatuh_tempo = reader.GetDateTime(7).ToString("yyyy-MM-dd");
                                    string total_harga = totalHarga.ToString("C", new CultureInfo("id-ID"));
                                    total_harga = total_harga.Replace("Rp", "");
                                    string payment = reader.GetString(6);

                                    Image editIcon = Properties.Resources.icons8_info_24px_1;
                                    Image returIcon = Properties.Resources.icons8_edit_20px;

                                    if (payment == "kredit")
                                    {
                                        dgv.Rows.Add(no_faktur, tgl, namaPelanggan, alamat, totalBarang, total_harga, jatuh_tempo, status, editIcon, returIcon);
                                    }

                                    totalPenjualan += totalHarga;
                                }

                            }
                            else
                            {

                                //MessageBox.Show("Tidak ada data yang ditemukan.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan 1: " + ex.Message);
                    }
                }
            }
        }

        private void BarangKeluar()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "SELECT DISTINCT no_faktur, DATE(tgl) AS tgl, nama_pelanggan, alamat, total_barang, total_harga, payment, DATE(jatuh_tempo) AS jatuh_tempo, status FROM tb_transaksi";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        decimal totalPenjualan = 0;
                       
                        connection.Open();
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
                                    int no_faktur = reader.GetInt32(0);
                                    string tgl = reader.GetDateTime(1).ToString("yyyy-MM-dd");
                                    string namaPelanggan = reader.GetString(2);
                                    string status = reader.GetString(8);
                                    string alamat = reader.GetString(3);
                                    int totalBarang = reader.GetInt32(4);
                                    decimal totalHarga = reader.GetDecimal(5);

                                    string jatuh_tempo = reader.GetDateTime(7).ToString("yyyy-MM-dd");
                                    string total_harga = totalHarga.ToString("C", new CultureInfo("id-ID"));
                                    total_harga = total_harga.Replace("Rp", "");
                                    string payment = reader.GetString(6);

                                    Image editIcon = Properties.Resources.icons8_info_24px_1;
                                    Image returIcon = Properties.Resources.icons8_edit_20px;

                                    if (payment == "Kredit")
                                    {
                                        dgv.Rows.Add(no_faktur, tgl, namaPelanggan, alamat, totalBarang, total_harga, jatuh_tempo, status, editIcon, returIcon);
                                    }

                                    totalPenjualan += totalHarga;
                                }

                            }
                            else
                            {
                                
                                //MessageBox.Show("Tidak ada data yang ditemukan.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan 1: " + ex.Message);
                    }
                }
            }
        }
        private void FormReceivables_Load(object sender, EventArgs e)
        {
            BarangKeluar();
        }

        private void SEARCH_TextChanged(object sender, EventArgs e)
        {
            searchData();
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgv.Columns["column7"].Index && e.RowIndex >= 0)
            {
                int nofaktur = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["column1"].Value);
                //FrmCetakFaktur frmCetak = new FrmCetakFaktur();
                //frmCetak.NoFaktur = nofaktur.ToString();
                //frmCetak.ShowDialog();
            }
            else if (e.ColumnIndex == dgv.Columns["column8"].Index && e.RowIndex >= 0)
            {
                int nofaktur = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["column1"].Value);
                FormReceivablesDetails frmRetur = new FormReceivablesDetails();
                frmRetur.NoFaktur = nofaktur.ToString();
                frmRetur.FormClosed += new FormClosedEventHandler(FormReceivablesDetails_Closed);
                frmRetur.ShowDialog();
            }
        }

        private void FormReceivablesDetails_Closed(object sender, FormClosedEventArgs e)
        {
            BarangKeluar();
        }
    }
}
