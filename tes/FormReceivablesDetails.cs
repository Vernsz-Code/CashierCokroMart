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
    public partial class FormReceivablesDetails : Form
    {
        public string NoFaktur { get; set; }
        string server = "localhost";
        string database = "cashier";
        string uid = "root";
        string password = "";
        decimal ifelsehutang = 0;
        private string SelectedID = "";
        private string SelectedKode;
        public FormReceivablesDetails()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void HapusDataBarangKeluar(int kodeBarangToDelete)
        {
            try
            {
                string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string deleteQuery = "DELETE FROM tb_transaksi WHERE id = @kodeBarangToDelete";

                MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection);
                deleteCmd.Parameters.AddWithValue("@kodeBarangToDelete", kodeBarangToDelete);

                int rowsAffected = deleteCmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    dgv.Rows.RemoveAt(dgv.SelectedRows[0].Index);
                    MessageBox.Show("Data berhasil dihapus dan stok sudah di Pulihkan, Tidak Perlu tekan tombol Save! Langsung Close saja", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan atau gagal dihapus");
                }
                this.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan 5: " + ex.Message);
            }
        }
        private void updateData()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            string harga = this.HARGA.Text;
            harga = harga.Replace(".", "");

            string subTotal = this.SUBTOTAL.Text;
            subTotal = subTotal.Replace(".", "");

            string subRetur = this.SUBRETUR.Text;
            subRetur = subRetur.Replace(".", "");

            string query = "UPDATE `tb_transaksi` SET " +
                    "`harga` = '" + harga + "', " +
                    "`subtotal` = '" + subTotal + "', " + 
                    "`QTY` = `qtyawal` - '" + this.DIRETUR.Text + "', " +
                    "" + "`retur` = '" + this.DIRETUR.Text + "', " +
                    "`subretur` = `subretur` + '" + subRetur + "' " +
                    "WHERE `id` = '" + SelectedID + "'";

            if (DIRETUR.Value != DIRETUR.Maximum)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        try
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Berhasil Retur barang dengan Kode : " + SelectedID);
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("" + ex.Message);
                        }
                    }
                }
                RefreshCart();
            }
            else
            {
                HapusDataBarangKeluar(Convert.ToInt32(SelectedID));
            }
        }

        public void TambahStok(string kodeBarang, int jumlahDikurangkan)
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "UPDATE tb_stok SET sisaPcs = sisaPcs + @jumlahDikurangkan WHERE kode_brg = @kodeBarang";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@kodeBarang", kodeBarang);
                    cmd.Parameters.AddWithValue("@jumlahDikurangkan", jumlahDikurangkan);

                    try
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan 1.3: " + ex.Message);
                    }
                }
            }
        }

        public int GetStokSatuan(string kodeBarang)
        {
            int jumlahBarangAwal = 0;

            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "SELECT sisaPcs FROM tb_stok WHERE kode_brg = @kodeBarang";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@kodeBarang", kodeBarang);

                    try
                    {
                        connection.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                            jumlahBarangAwal = Convert.ToInt32(result);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan 1.1: " + ex.Message);
                    }
                }
            }

            return jumlahBarangAwal;
        }

        private void UpdateStok(string kodeBarang)
        {
            int sisaBox = GetStokSatuan(kodeBarang);

            int retur = int.Parse(DIRETUR.Text);

            TambahStok(kodeBarang, retur);
        }

        private void getPembayaran()
        {

            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "select DATE(tgl) AS tgl, harga from tb_riwayat where no_faktur = @faktur";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.Parameters.AddWithValue("@faktur", NoFaktur);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dgv.Rows.Clear();

                                while (reader.Read())
                                {
                                    string tgl = reader.GetDateTime(0).ToString("yyyy-MM-dd");
                                    decimal pembayaran = reader.GetDecimal(1);
                                    string harga = pembayaran.ToString("N0", new CultureInfo("id-ID"));
                                    dgv.Rows.Add(tgl, harga);
                                }
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

        private void updateStatus()
        {
            try
            {
                string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
                string query = "update tb_transaksi set status = @status WHERE no_faktur = @faktur";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string status;
                    if (ifelsehutang <= 0)
                    {
                        status = "Lunas";
                    }
                    else
                    {
                        status = "Tidak Lunas";
                    }
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        cmd.Parameters.AddWithValue("@faktur", NoFaktur);
                        cmd.Parameters.AddWithValue("@status", status);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("terjadi kesalahan 2 : " + ex.Message);
            }
        }

        private void RefreshCart()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "SELECT kode_barang, nama_barang, nama_pelanggan, alamat, harga, subtotal, qty, id, status, total_harga, retur, qtyawal FROM tb_transaksi WHERE no_faktur = @noFaktur";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.Parameters.AddWithValue("@noFaktur", NoFaktur);

                        decimal totalHarga = 0;
                        int totalBarang = 0;
                        updateHutang();
                        updateStatus();

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                guna2DataGridView1.Rows.Clear();

                                while (reader.Read())
                                {
                                    string namaBarang = reader.GetString(1);
                                    string status = reader.GetString(8);
                                    string levelHarga = reader.GetString(2);
                                    decimal subtotal = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5);
                                    int id = reader.IsDBNull(7) ? 0 : (int)reader.GetInt32(7); // Perhatikan konversi ke int
                                    int retur = reader.IsDBNull(10) ? 0 : (int)reader.GetInt32(10); // Perhatikan konversi ke int
                                    int qtyawal = reader.IsDBNull(11) ? 0 : (int)reader.GetInt32(11); // Perhatikan konversi ke int
                                    decimal harga = reader.GetDecimal(4);
                                    int qty = reader.IsDBNull(6) ? 0 : (int)reader.GetInt32(6); // Perhatikan konversi ke int
                                    string nama = reader.GetString(2);
                                    string alamat = reader.GetString(3);
                                    int kode_barang = (int)reader.GetInt32(0); // Konversi ke int
                                    decimal total = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9);
                                    lbl_NAMA.Text = nama;
                                    lbl_NOFAKTUR.Text = NoFaktur;
                                    lbl_ALAMAT.Text = alamat;

                                    string hargaBrg = harga.ToString("N0", new CultureInfo("id-ID"));
                                    string subTotal = subtotal.ToString("N0", new CultureInfo("id-ID"));

                                    Image editIcon = Properties.Resources.icons8_delete_24px_1;

                                    if (status == "Lunas")
                                    {
                                        label6.Text = "LUNAS";
                                        guna2CustomGradientPanel1.FillColor = Color.FromArgb(66, 135, 245);
                                        guna2CustomGradientPanel1.FillColor2 = Color.FromArgb(66, 135, 245);
                                        guna2CustomGradientPanel1.FillColor3 = Color.FromArgb(66, 135, 245);
                                        guna2CustomGradientPanel1.FillColor4 = Color.FromArgb(66, 135, 245);
                                        pictureBox2.Image = Properties.Resources.icons8_ok_48px;
                                    }
                                    else
                                    {
                                        label6.Text = "TIDAK LUNAS";
                                        guna2CustomGradientPanel1.FillColor = Color.FromArgb(247, 137, 52);
                                        guna2CustomGradientPanel1.FillColor2 = Color.FromArgb(247, 137, 52);
                                        guna2CustomGradientPanel1.FillColor3 = Color.FromArgb(247, 137, 52);
                                        guna2CustomGradientPanel1.FillColor4 = Color.FromArgb(247, 137, 52);
                                        pictureBox2.Image = Properties.Resources.icons8_cancel_2_filled_48px;
                                    }
                                    guna2DataGridView1.Rows.Add(id, kode_barang, namaBarang, qty, retur, hargaBrg, subTotal, editIcon, qtyawal);

                                    totalHarga += subtotal;
                                    totalBarang += qty;
                                }

                                lbl_TOTAL.Text = totalHarga.ToString("C", new CultureInfo("id-ID"));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan oiioii 2: " + ex.Message);
                    }
                }
            }
        }

        private void addBayar()
        {
            if (string.IsNullOrEmpty(inputBayar.Text))
            {
                MessageBox.Show("Harap isi Columnya di isi!");
            }
            else
            {
                try
                {
                    string bayar = inputBayar.Text;
                    DateTime tanggal = DateTime.Now;
                    string tanggals = tanggal.ToString("yyyy-MM-dd");

                    string kueri = "INSERT INTO tb_riwayat (tgl, no_faktur, harga) VALUES (@tgl, @nofaktur, @harga)";
                    string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";

                    MySqlConnection connection = new MySqlConnection(connectionString);

                    using (MySqlCommand cmd = new MySqlCommand(kueri, connection))
                    {
                        cmd.Parameters.AddWithValue("@tgl", tanggals);
                        cmd.Parameters.AddWithValue("@nofaktur", NoFaktur);
                        cmd.Parameters.AddWithValue("@harga", bayar);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                    getPembayaran();
                    MessageBox.Show("berhasil ditambahkan");
                    inputBayar.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat menambahkan data: " + ex.Message);
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }

        private void updateHutang()
        {
            try
            {
                string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
                string query = "SELECT sum(harga) from tb_riwayat where no_faktur = @faktur";
                string kueri = "SELECT total_harga from tb_transaksi where no_faktur = @faktur";
                decimal hasilAkhir = 0;
                decimal hutangAwal = 0;
                decimal total;
                decimal jumlah = 0;
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@faktur", NoFaktur);
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        total = Convert.ToDecimal(result);
                    }
                    else
                    {
                        // Tentukan nilai default jika hasilnya null
                        total = 0; // Atau nilai default lain sesuai kebutuhan Anda
                    }

                }
                
                using (MySqlCommand cmd = new MySqlCommand(kueri, connection))
                {
                    cmd.Parameters.AddWithValue("@faktur", NoFaktur);
                    object test = cmd.ExecuteScalar();

                    if (test != null && test != DBNull.Value)
                    {
                        if(decimal.TryParse(test.ToString(), out jumlah))
                        {
                            hutangAwal = jumlah;
                        }
                        else
                        {
                            hutangAwal = 0;
                        }
                    }
                }
                connection.Close();

                hasilAkhir = hutangAwal - total;

                ifelsehutang = hasilAkhir;

                string ahasilAkhir = hasilAkhir.ToString("C", new CultureInfo("id-ID"));
                lbl_Sisa_Hutang.Text = ahasilAkhir;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }


        private void FormReceivablesDetails_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(NoFaktur);
            //NoFaktur = "1000";
            getPembayaran();
            RefreshCart();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            addBayar();
            RefreshCart();
        }

        private void DIRETUR_ValueChanged(object sender, EventArgs e)
        {
            string hargaText = HARGA.Text;

            int jumlahBarangRetur = Convert.ToInt32(DIRETUR.Value);

            int sumValue = Convert.ToInt32(sum.Value);

            int direturValue = Convert.ToInt32(DIRETUR.Value);

            decimal totalHargaRetur = 0;

            decimal result = sumValue - direturValue;

            decimal totalHarga = 0;

            int jumlahBarang = Convert.ToInt32(result);

            if (decimal.TryParse(hargaText, out decimal harga))
            {
                totalHargaRetur = harga * jumlahBarangRetur;
            }

            if (decimal.TryParse(hargaText, out decimal hargaa))
            {
                totalHarga = hargaa * jumlahBarang;
            }

            SUBRETUR.Text = totalHargaRetur.ToString();

            SUBTOTAL.Text = totalHarga.ToString();
        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateStok(SelectedKode);
            updateData();
        }
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.guna2DataGridView1.Rows[e.RowIndex];
                SelectedID = row.Cells["ColumnID"].Value.ToString();
                SelectedKode = row.Cells["column1"].Value.ToString();
                NAMABARANG.Text = row.Cells["column2"].Value.ToString();
                HARGA.Text = row.Cells["column4"].Value.ToString();
                DIBELI.Text = row.Cells["column3"].Value.ToString();
                SUBTOTAL.Text = row.Cells["column5"].Value.ToString();
                decimal diRetur = Convert.ToDecimal(row.Cells["column7"].Value);
                DIRETUR.Value = diRetur;
                sum.Text = row.Cells["dataGridViewTextBoxColumn1"].Value.ToString();
                DIRETUR.Maximum = sum.Value;
                DIRETUR.Enabled = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                guna2CustomGradientPanel2.Visible = true;
                //MessageBox.Show()
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            guna2CustomGradientPanel2.Visible = false;
        }

        private void UpdateStokDeleted(string kodeBarang, int retur)
        {
            int sisaBox = GetStokSatuan(kodeBarang);

            //Console.WriteLine("Var: " + kodeBarang + " | " + retur + " | " + lempengPerBox);

            TambahStok(kodeBarang, retur);
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == guna2DataGridView1.Columns["Column6"].Index && e.RowIndex >= 0)
            {
                int kodeBarangToDelete = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["ColumnID"].Value);
                int retur = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"].Value);
                string kodeBarang = guna2DataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
                HapusDataBarangKeluar(kodeBarangToDelete);
                UpdateStokDeleted(kodeBarang, retur);
            }
        }
    }
}
