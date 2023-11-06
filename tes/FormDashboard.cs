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
    public partial class FormDashboard : Form
    {
        string server = "localhost";
        string database = "cashier";
        string uid = "root";
        string password = "";

        public FormDashboard()
        {
            InitializeComponent();
        }

        void TotalPenjualan()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            DateTime date = DatePicker.Value;

            string query = "SELECT total_harga FROM tb_transaksi WHERE tgl = @date";

            // Buat sebuah HashSet untuk melacak nomor transaksi yang sudah diproses
            HashSet<int> processedTransactionNumbers = new HashSet<int>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    cmd.Parameters.Add("@date", MySqlDbType.Date).Value = date;

                    decimal totalPenjualan = 0;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            // Loop melalui hasil pembacaan
                            while (reader.Read())
                            {
                                decimal totalHarga = reader.GetInt32(0); // Gantilah indeks sesuai dengan indeks kolom nomor transaksi di tabel Anda

                                int t = Convert.ToInt32(totalHarga);

                                // Cek apakah nomor transaksi sudah diproses sebelumnya
                                if (!processedTransactionNumbers.Contains(t))
                                {
                                    totalPenjualan += totalHarga;
                                    processedTransactionNumbers.Add(t);
                                }
                            }

                            lbl_PENJUALAN.Text = totalPenjualan.ToString("C", new CultureInfo("id-ID"));
                        }
                        else
                        {
                            lbl_PENJUALAN.Text = "-";
                        }
                    }
                    connection.Close();
                }
            }
        }


        void resetBtn()
        {
            btnHari.FillColor = System.Drawing.Color.White;
            btnHari.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            btnBulan.FillColor = System.Drawing.Color.White;
            btnBulan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            btnTahun.FillColor = System.Drawing.Color.White;
            btnTahun.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnHari_Click(object sender, EventArgs e)
        {
            resetBtn();
            btnHari.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(156)))), ((int)(((byte)(56)))));
            btnHari.ForeColor = System.Drawing.Color.White;
        }

        private void btnBulan_Click(object sender, EventArgs e)
        {
            resetBtn();
            btnBulan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(156)))), ((int)(((byte)(56)))));
            btnBulan.ForeColor = System.Drawing.Color.White;
        }

        private void btnTahun_Click(object sender, EventArgs e)
        {
            resetBtn();
            btnTahun.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(156)))), ((int)(((byte)(56)))));
            btnTahun.ForeColor = System.Drawing.Color.White;
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            TotalPenjualan();
        }

        private void createTable()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string createTable = @"CREATE TABLE IF NOT EXISTS tb_stok (
                kode_brg TEXT NOT NULL,
                nama_brg TEXT NOT NULL,
                masukBox INT NOT NULL,
                isiBox INT NOT NULL,
                distributor TEXT NOT NULL,
                sisaBox INT NOT NULL,
                sisaPcs INT NOT NULL,
                hargaPcs DECIMAL(18, 2) NOT NULL,
                labaJual INT NOT NULL,
                hargaBeli DECIMAL(18, 2) NOT NULL,
                keuntungan DECIMAL(18, 2) NOT NULL,
                totalHarga DECIMAL(18, 2) NOT NULL,
                pcsKeluar INT NOT NULL,
                status VARCHAR(20) NOT NULL,
                Tunai DECIMAL(18, 2) NOT NULL,
                payment TEXT NOT NULL
            );";

            //string createTable = "CREATE TABLE IF NOT EXISTS tes(nama text(20), pass text(20));";
            string createTableKas = @"CREATE TABLE IF NOT EXISTS tb_kas (
                no INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                faktur TEXT NOT NULL,
                tgl DATE NOT NULL,
                jenis TEXT NOT NULL,
                kategori TEXT NOT NULL,
                pemasukan DECIMAL(18, 2) NOT NULL,
                pengeluaran DECIMAL(18, 2) NOT NULL,
                keterangan TEXT NOT NULL,
                operator TEXT NOT NULL
            );";

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(createTable, connection);
                MySqlCommand cmda = new MySqlCommand(createTableKas, connection);
                cmd.ExecuteNonQuery();
                cmda.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal terhubung ke database. Kesalahan: {ex.Message}");
            }
            finally
            {
                connection.Close(); // Pastikan selalu menutup koneksi, baik berhasil atau gagal.
            }

            string createTableRiwayat = @"CREATE TABLE IF NOT EXISTS tb_riwayat (
                id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                tgl DATE NOT NULL,
                no_faktur INT NOT NULL,
                harga DECIMAL(18, 2) NOT NULL
            );";
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(createTableRiwayat, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal terhubung ke database. Kesalahan: {ex.Message}");
            }
            finally
            {
                connection.Close(); // Pastikan selalu menutup koneksi, baik berhasil atau gagal.
            }
            string createTableCart = @"CREATE TABLE IF NOT EXISTS tb_cart (
                id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                no_faktur INT NOT NULL,
                kode_barang TEXT NOT NULL,
                nama_barang TEXT NOT NULL,
                harga DECIMAL(18, 2) NOT NULL,
                subtotal DECIMAL(18, 2) NOT NULL,
                QTY INT NOT NULL,
                isiBox INT NOT NULL
            );";

            string createTableTransaksi = @"CREATE TABLE IF NOT EXISTS tb_transaksi (
                id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                tgl DATETIME NOT NULL,
                no_faktur INT NOT NULL,
                kode_barang TEXT NOT NULL,
                nama_barang TEXT NOT NULL,
                nama_pelanggan TEXT NOT NULL,
                alamat TEXT NOT NULL,
                harga DECIMAL(18, 2) NOT NULL,
                subtotal DECIMAL(18, 2) NOT NULL,
                QTY INT NOT NULL,
                total_barang INT NOT NULL,
                jatuh_tempo DATE NOT NULL,
                total_harga	decimal(15,2) NOT NULL,
                payment TEXT NOT NULL,
                status TEXT NOT NULL,
                sisaHutang INT NOT NULL,
                retur INT NOT NULL,
                subretur decimal(15,2) NOT NULL,
                Tunai decimal(15,2) NOT NULL,
                qtyawal INT NOT NULL
            );";
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(createTableCart, connection);
                cmd.ExecuteNonQuery();
                MySqlCommand cmds = new MySqlCommand(createTableTransaksi, connection);
                cmds.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal terhubung ke database. Kesalahan: {ex.Message}");
            }
            finally
            {
                connection.Close(); // Pastikan selalu menutup koneksi, baik berhasil atau gagal.
            }
        }

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            createTable();
            DatePicker.Value = DateTime.Now;
        }
    }
}
