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
    public partial class frmReport : Form
    {
        string server = "localhost";
        string database = "cashier";
        string uid = "root";
        string password = "";

        public frmReport()
        {
            InitializeComponent();
        }
        private void GetDataByDateRange()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            DateTime startDate = STARTDATE.Value;
            DateTime endDate = ENDDATE.Value;
            string query = "SELECT DISTINCT no_faktur, DATE(tgl) AS tgl, nama_pelanggan, alamat, total_barang, total_harga, payment, Tunai FROM tb_transaksi where tgl BETWEEN @startDate and @endDate";
            string formattedsDate = startDate.ToString("dd MMMM yyyy", new CultureInfo("id-ID"));
            string formattedeDate = endDate.ToString("dd MMMM yyyy", new CultureInfo("id-ID"));
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        cmd.Parameters.Add("@startDate", MySqlDbType.Date).Value = startDate;
                        cmd.Parameters.Add("@endDate", MySqlDbType.Date).Value = endDate;

                        decimal totalPenjualan = 0;
                        decimal totalkeuntungan = 0;

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
                                    string alamat = reader.GetString(3);
                                    int totalBarang = reader.GetInt32(4);
                                    decimal totalHarga = reader.GetDecimal(5);

                                    string total_harga = totalHarga.ToString("C", new CultureInfo("id-ID"));
                                    total_harga = total_harga.Replace("Rp", "");
                                    string payment = reader.GetString(6);
                                    string Tunai = reader.GetString(7);

                                    Image editIcon = Properties.Resources.icons8_info_24px_1;
                                    Image returIcon = Properties.Resources.icons8_refund_2_20px;

                                    if (payment == "Tunai")
                                    {
                                        dgv.Rows.Add(no_faktur, tgl, namaPelanggan, alamat, totalBarang, total_harga, Tunai, editIcon, returIcon);
                                    }

                                    totalPenjualan += totalHarga;
                                }

                                lbl_TOTALPENJUALAN.Text = totalPenjualan.ToString("C", new CultureInfo("id-ID"));
                                lbl_TOTALKEUNTUNGAN.Text = totalkeuntungan.ToString("C", new CultureInfo("id-ID"));
                            }
                            else
                            {
                                lbl_TOTALPENJUALAN.Text = "-";
                                lbl_TOTALKEUNTUNGAN.Text = "-";
                                //MessageBox.Show("Tidak ada data yang ditemukan.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                    }
                }
            }
        }

        private void BarangKeluar()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "SELECT DISTINCT no_faktur, DATE(tgl) AS tgl, nama_pelanggan, alamat, total_barang, total_harga, payment, Tunai FROM tb_transaksi";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        decimal totalPenjualan = 0;
                        decimal totalkeuntungan = 0;

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
                                    string alamat = reader.GetString(3);
                                    int totalBarang = reader.GetInt32(4);
                                    decimal totalHarga = reader.GetDecimal(5);

                                    string total_harga = totalHarga.ToString("C", new CultureInfo("id-ID"));
                                    total_harga = total_harga.Replace("Rp", "");
                                    string payment = reader.GetString(6);
                                    string Tunai = reader.GetString(7);

                                    Image editIcon = Properties.Resources.icons8_info_24px_1;
                                    Image returIcon = Properties.Resources.icons8_refund_2_20px;

                                    if(payment == "Tunai" || payment == "")
                                    {
                                        dgv.Rows.Add(no_faktur, tgl, namaPelanggan, alamat, totalBarang, total_harga, Tunai, editIcon, returIcon);
                                    }

                                    totalPenjualan += totalHarga;
                                }

                                lbl_TOTALPENJUALAN.Text = totalPenjualan.ToString("C", new CultureInfo("id-ID"));
                                lbl_TOTALKEUNTUNGAN.Text = totalkeuntungan.ToString("C", new CultureInfo("id-ID"));
                            }
                            else
                            {
                                lbl_TOTALPENJUALAN.Text = "-";
                                lbl_TOTALKEUNTUNGAN.Text = "-";
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

        private void searchData()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "SELECT DISTINCT no_faktur, DATE(tgl) AS tgl, nama_pelanggan, alamat, total_barang, total_harga, payment, Tunai FROM tb_transaksi WHERE no_faktur LIKE '%" + SEARCH.Text + "%' OR nama_pelanggan LIKE '%" + SEARCH.Text + "%'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        decimal totalPenjualan = 0;
                        decimal totalkeuntungan = 0;

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
                                    string alamat = reader.GetString(3);
                                    int totalBarang = reader.GetInt32(4);
                                    decimal totalHarga = reader.GetDecimal(5);

                                    string total_harga = totalHarga.ToString("C", new CultureInfo("id-ID"));
                                    total_harga = total_harga.Replace("Rp", "");
                                    string payment = reader.GetString(6);
                                    string Tunai = reader.GetString(7);

                                    Image editIcon = Properties.Resources.icons8_info_24px_1;
                                    Image returIcon = Properties.Resources.icons8_refund_2_20px;

                                    if (payment == "Tunai")
                                    {
                                        dgv.Rows.Add(no_faktur, tgl, namaPelanggan, alamat, totalBarang, total_harga, Tunai, editIcon, returIcon);
                                    }

                                    totalPenjualan += totalHarga;
                                }

                                lbl_TOTALPENJUALAN.Text = totalPenjualan.ToString("C", new CultureInfo("id-ID"));
                                lbl_TOTALKEUNTUNGAN.Text = totalkeuntungan.ToString("C", new CultureInfo("id-ID"));
                            }
                            else
                            {
                                lbl_TOTALPENJUALAN.Text = "-";
                                lbl_TOTALKEUNTUNGAN.Text = "-";
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

        private void frmReport_Load(object sender, EventArgs e)
        {
            BarangKeluar();
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
                FormRetur frmRetur = new FormRetur();
                frmRetur.NoFaktur = nofaktur.ToString();
                frmRetur.FormClosed += new FormClosedEventHandler(FormRetur_Closed);
                frmRetur.ShowDialog();
            }
        }

        private void FormRetur_Closed(object sender, FormClosedEventArgs e)
        {
            BarangKeluar();
        }

        private void STARTDATE_ValueChanged(object sender, EventArgs e)
        {
            GetDataByDateRange();
        }

        private void ENDDATE_ValueChanged(object sender, EventArgs e)
        {
            GetDataByDateRange();
        }

        private void SEARCH_TextChanged(object sender, EventArgs e)
        {
            searchData();
        }
    }
}
