using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace tes
{
    public partial class FrmStok : Form
    {
        string server = "localhost";
        string database = "cashier";
        string uid = "root";
        string password = "";
        decimal StokAllPcs;
        private string SelectedID;

        bool sisaPcsValidation = false, sisaBoxValidation = false;

        public FrmStok()
        {
            InitializeComponent();
        }
        private enum SaveSectionEnum
        {
            None,
            Insert,
            Update
        }

        private SaveSectionEnum SaveSection;

        private void btnInsert_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            Tunai.Enabled = true;
            ClearString();
            SaveSection = SaveSectionEnum.Insert;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false; 
            HideTextBox();
            SaveSection = SaveSectionEnum.None;
        }

        private void ClearString()
        {
            KODEBARANG.Text = "";
            NAMABARANG.Text = "";
            MODAL.Text = "";
            JUMLAHBARANG.Value = 0;
            HARGAJUAL1.Text = "";
            ISIPCS.Value = 1;
            DISTRIBUTOR.Text = "";
            LABA1.Text = "0";
            HargaPcs.Text = "";
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            
        }
        private void FrmMaster_Load(object sender, EventArgs e)
        {
            loadData();
        }


        private void loadData()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = @"select
                kode_brg,
                nama_brg,
                stok_awal,
                masuk,
                keluar,
                stok_akhir,
                beli,
                jual,
                mark_up,
                pendapatan,
                laba,
                harta,
                persentase
                from product;";
            connection.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                try
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dgv.Rows.Clear();
                            int no = 0;
                            while (reader.Read())
                            {
                                no++;
                                string kode_brg = reader.GetString(0);
                                string nama_brg = reader.GetString(1);
                                int stok_awal = reader.GetInt32(2);
                                int stok_masuk = reader.GetInt32(3);
                                int stok_keluar = reader.GetInt32(4);
                                int stok_akhir = reader.GetInt32(5);
                                decimal beli = reader.GetDecimal(6);
                                decimal jual = reader.GetDecimal(7);
                                decimal mark_up = reader.GetDecimal(8);
                                decimal pendapatan = reader.GetDecimal(9);
                                decimal laba = reader.GetDecimal(10);
                                decimal harta = reader.GetDecimal(11);
                                string persentase = reader.GetString(12);

                                string beliRupiah = beli.ToString("C", new CultureInfo("id-ID"));
                                string jualRupiah = jual.ToString("C", new CultureInfo("id-ID"));
                                string markUpRupiah = mark_up.ToString("C", new CultureInfo("id-ID"));
                                string pendapatanRupiah = pendapatan.ToString("C", new CultureInfo("id-ID"));
                                string labaRupiah = laba.ToString("C", new CultureInfo("id-ID"));
                                string hartaRupiah = harta.ToString("C", new CultureInfo("id-ID"));

                                dgv.Rows.Add(no, kode_brg, nama_brg, stok_awal, stok_masuk, stok_keluar, stok_akhir, beliRupiah, jualRupiah, markUpRupiah, pendapatanRupiah, labaRupiah, hartaRupiah, persentase);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan (Master - 1): " + ex.Message);
                }
            }
        }
        private void searchData()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = @"select
                kode_brg,
                nama_brg,
                stok_awal,
                masuk,
                keluar,
                stok_akhir,
                beli,
                jual,
                mark_up,
                pendapatan,
                laba,
                harta,
                persentase
                from product;";
            connection.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                try
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dgv.Rows.Clear();

                            while (reader.Read())
                            {
                                string kode_brg = reader.GetString(0);
                                string nama_brg = reader.GetString(1);
                                int stok_awal = reader.GetInt32(2);
                                int stok_masuk = reader.GetInt32(3);
                                int stok_keluar = reader.GetInt32(5);
                                int stok_akhir = reader.GetInt32(6);
                                decimal beli = reader.GetDecimal(7);
                                decimal jual = reader.GetDecimal(8);
                                decimal mark_up = reader.GetDecimal(9);
                                decimal pendapatan = reader.GetDecimal(10);
                                decimal laba = reader.GetDecimal(11);
                                decimal harta = reader.GetDecimal(12);
                                string persentase = reader.GetString(13);

                                string beliRupiah = beli.ToString("C", new CultureInfo("id-ID"));
                                string jualRupiah = jual.ToString("C", new CultureInfo("id-ID"));
                                string markUpRupiah = mark_up.ToString("C", new CultureInfo("id-ID"));
                                string pendapatanRupiah = pendapatan.ToString("C", new CultureInfo("id-ID"));
                                string labaRupiah = laba.ToString("C", new CultureInfo("id-ID"));
                                string hartaRupiah = harta.ToString("C", new CultureInfo("id-ID"));

                                dgv.Rows.Add(kode_brg, nama_brg, stok_awal, stok_masuk, stok_keluar, stok_akhir, beliRupiah, jualRupiah, markUpRupiah, pendapatanRupiah, labaRupiah, hartaRupiah, persentase);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan (Master - 1): " + ex.Message);
                }
            }
        }

        private void deleteData()
        {
            string msg = "Apakah kamu yakin untuk menghapus data ID " + SelectedID + "";

            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "DELETE FROM tb_stok WHERE kode_brg = '" + SelectedID + "'";


            if (MessageBox.Show(msg, "Delete Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        try
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Berhasil hapus Barang dengan Kode : " + SelectedID);
                            connection.Close();
                            btnUpdate.Enabled = false;
                            btnDelete.Enabled = false;
                            btnInsert.Enabled = true;
                            loadData();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("" + ex.Message);
                        }
                        loadData();
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (
                String.IsNullOrEmpty(KODEBARANG.Text) ||
                String.IsNullOrEmpty(NAMABARANG.Text) ||
                String.IsNullOrEmpty(MODAL.Text) ||
                String.IsNullOrEmpty(DISTRIBUTOR.Text) ||
                String.IsNullOrEmpty(LABA1.Text)
                )
            {
                MessageBox.Show("Semua kolom harus diisi.");
            }
            else
            {
                if (SaveSection == SaveSectionEnum.Insert)
                {
                    InsertKas();
                }
                else if (SaveSection == SaveSectionEnum.Update)
                {
                    updateData();
                }
            }
        }
        private void HideTextBox()
        {
            groupBox2.Visible = false;
            gBstok.Visible = false;
            btnInsert.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            ClearString();
        }

        private void JUMLAHBARANG_ValueChanged(object sender, EventArgs e)
        {
            string modalText = MODAL.Text;
            int jumlahIsi = Convert.ToInt32(ISIPCS.Value);
            int jumlahBarang = Convert.ToInt32(JUMLAHBARANG.Value);
            int laba = int.Parse(LABA1.Text);
            int persen = laba + 100;
            decimal totalHarga = 0;
            decimal hj = 0;
            decimal keuntungan = 0;
            string konversiDecimal = keuntungan.ToString("N0", new CultureInfo("id-ID"));
            StokAllPcs = jumlahIsi * jumlahBarang;

            if (decimal.TryParse(modalText, out decimal modal))
            {
                totalHarga = modal * jumlahBarang;
                hj = modal * persen / 100;
                hj = hj / jumlahIsi;
                keuntungan = hj * jumlahIsi * jumlahBarang - totalHarga;
            }
            decimal t = 0;
            if (decimal.TryParse(modalText, out decimal modals))
            {
                t = modals;
            }
            decimal pcs = t / jumlahIsi;
            HargaPcs.Text = pcs.ToString("N0", new CultureInfo("id-ID"));
            TOTALHARGA.Text = totalHarga.ToString("N0", new CultureInfo("id-ID"));
            //KEUNTUNGAN.Text = keuntungan.ToString("N0", new CultureInfo("id-ID"));
            //HARGAJUAL1.Text = hj.ToString("N0", new CultureInfo("id-ID"));
        }

        private void InsertKas()
        {
            try
            {
                string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                string kueri = "INSERT INTO `tb_kas` (`no`, `faktur`, `tgl`, `jenis`, `kategori`, `pemasukan`, `pengeluaran`, `keterangan`, `operator`) VALUES (NULL, @faktur, @tgl, @jenis, @kategori, 0, @pengeluaran, @ket, @op);";
                DateTime tgl = DateTime.Now;
                string tglString = tgl.ToString("yyyy/MM/dd");
                string kodeBarang = "PBL-" + tglString + "-" + KODEBARANG.Text;
                kodeBarang = kodeBarang.Replace(".", "");
                string namaBarang = NAMABARANG.Text;
                namaBarang = namaBarang.Replace(".", "");
                string totalHarga = TOTALHARGA.Text;
                totalHarga = totalHarga.Replace(".", "");
                string jenis = "Pengeluaran";
                string Kategori = "Pembelian";
                string ket = "Pembelian barang : " + namaBarang;
                string op = "Manager";

                using (MySqlCommand cmd = new MySqlCommand(kueri, connection))
                {
                    cmd.Parameters.AddWithValue("@faktur", kodeBarang);
                    cmd.Parameters.AddWithValue("@tgl", tglString);
                    cmd.Parameters.AddWithValue("@jenis", jenis);
                    cmd.Parameters.AddWithValue("@kategori", Kategori);
                    cmd.Parameters.AddWithValue("@pengeluaran", totalHarga);
                    cmd.Parameters.AddWithValue("@ket", ket);
                    cmd.Parameters.AddWithValue("@op", op);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                InsertData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kesalahan dalam Insert uang Kas", ex.Message);
            }
        }

        private void InsertData()
        {
            try
            {
                // Menggunakan parameterisasi untuk menghindari injeksi SQL
                string kueri = "INSERT INTO tb_stok (kode_brg, nama_brg, masukBox, isiBox, distributor, sisaBox, sisaPcs, hargaPcs, labaJual, hargaBeli, keuntungan, totalHarga, pcsKeluar, payment, status, Tunai) VALUES (@kodeBarang, @namaBarang, @jumlahBarang, @isiPcs, @distributor, @sisaBox, @sisaPcs,  @hargaJual1, @labaJual, @modalText, @keuntungan, @totalHarga, @pcsKeluar, @payment, @status, @Tunai)";
                string totalHarga = TOTALHARGA.Text;
                totalHarga = totalHarga.Replace(".", "");

                string distributor = DISTRIBUTOR.Text;
                distributor = distributor.Replace(".", "");

                string kodeBarang = KODEBARANG.Text;
                kodeBarang = kodeBarang.Replace(".", "");

                string modalText = MODAL.Text;
                modalText = modalText.Replace(".", "");
                string hargaJual1 = HARGAJUAL1.Text;
                hargaJual1 = hargaJual1.Replace(".", "");

                string namaBarang = NAMABARANG.Text;
                namaBarang = namaBarang.Replace(".", "");

                string keuntungan = HargaPcs.Text;
                keuntungan = keuntungan.Replace(".", "");

                decimal isiPcs = ISIPCS.Value;

                decimal laba = decimal.Parse(LABA1.Text);

                decimal jumlahBarang = JUMLAHBARANG.Value;

                decimal stokR = stokPcs.Value;

                decimal stokBox = JUMLAHBARANG.Value;

                string payment = "";

                string status = "";

                string TunaiString = Tunai.Text;

                string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";

                MySqlConnection connection = new MySqlConnection(connectionString);

                if (guna2CheckBox1.Checked == true)
                {
                    payment = "Kredit";
                    status = "Belum Lunas";
                }
                else
                {
                    payment = "Tunai";
                    status = "Lunas";
                }

                using (MySqlCommand cmd = new MySqlCommand(kueri, connection))
                {
                    // Mengatur parameter-parameter dengan nilai yang sesuai
                    cmd.Parameters.AddWithValue("@kodeBarang", kodeBarang);
                    cmd.Parameters.AddWithValue("@namaBarang", namaBarang);
                    cmd.Parameters.AddWithValue("@jumlahBarang", jumlahBarang);
                    cmd.Parameters.AddWithValue("@isiPcs", isiPcs);
                    cmd.Parameters.AddWithValue("@distributor", distributor);
                    cmd.Parameters.AddWithValue("@pcsKeluar", 0);
                    cmd.Parameters.AddWithValue("@sisaBox", jumlahBarang);
                    cmd.Parameters.AddWithValue("@sisaPcs", StokAllPcs);
                    cmd.Parameters.AddWithValue("@hargaJual1", hargaJual1);
                    cmd.Parameters.AddWithValue("@labaJual", laba);
                    cmd.Parameters.AddWithValue("@modalText", modalText);
                    cmd.Parameters.AddWithValue("@keuntungan", keuntungan);
                    cmd.Parameters.AddWithValue("@totalHarga", totalHarga);
                    cmd.Parameters.AddWithValue("@payment", payment);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@Tunai", TunaiString);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                MessageBox.Show("Data berhasil ditambahkan");
                HideTextBox();
                loadData();
            }
            catch (Exception ex)
            {
                // Tangani kesalahan dengan pesan yang lebih deskriptif
                MessageBox.Show("Terjadi kesalahan saat menambahkan data: " + ex.Message);
            }
        }

        private void updateData()
        {
            string updateQuery = "UPDATE tb_stok SET nama_brg = @namaBarang, masukBox = @jumlahBarang, isiBox = @isiPcs, distributor = @distributor, sisaBox = @sisaBox, sisaPcs = @sisaPcs, hargaPcs = @hargaJual1, labaJual = @labaJual, hargaBeli = @modalText, keuntungan = @keuntungan, totalHarga = @totalHarga, payment = @payment, status = @status WHERE kode_brg = @selectedID";
            string totalHarga = TOTALHARGA.Text;
            totalHarga = totalHarga.Replace(".", "");

            string distributor = DISTRIBUTOR.Text;
            distributor = distributor.Replace(".", "");

            string modalText = MODAL.Text;
            modalText = modalText.Replace(".", "");
            string hargaJual1 = HARGAJUAL1.Text;
            hargaJual1 = hargaJual1.Replace(".", "");

            string namaBarang = NAMABARANG.Text;
            namaBarang = namaBarang.Replace(".", "");

            string keuntungan = HargaPcs.Text;
            keuntungan = keuntungan.Replace(".", "");

            decimal isiPcs = ISIPCS.Value;

            decimal laba = decimal.Parse(LABA1.Text);

            decimal StokBox = stokSatuan.Value;

            decimal StokPcs = stokPcs.Value;

            decimal jumlahBarang = JUMLAHBARANG.Value;

            string status;

            string payment = "";

            if (guna2CheckBox1.Checked == true)
            {
                payment = "Kredit";
                status = "Belum Lunas";
            }
            else
            {
                payment = "Tunai";
                status = "Lunas";
            }

            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";

            MySqlConnection connection = new MySqlConnection(connectionString);

            using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
            {
                // Mengatur parameter-parameter dengan nilai yang sesuai
                cmd.Parameters.AddWithValue("@namaBarang", namaBarang);
                cmd.Parameters.AddWithValue("@jumlahBarang", jumlahBarang);
                cmd.Parameters.AddWithValue("@isiPcs", isiPcs);
                cmd.Parameters.AddWithValue("@distributor", distributor);
                cmd.Parameters.AddWithValue("@sisaBox", StokBox);
                cmd.Parameters.AddWithValue("@sisaPcs", StokPcs);
                cmd.Parameters.AddWithValue("@hargaJual1", hargaJual1);
                cmd.Parameters.AddWithValue("@labaJual", laba);
                cmd.Parameters.AddWithValue("@modalText", modalText);
                cmd.Parameters.AddWithValue("@keuntungan", keuntungan);
                cmd.Parameters.AddWithValue("@totalHarga", totalHarga);
                cmd.Parameters.AddWithValue("@payment", payment);
                cmd.Parameters.AddWithValue("@selectedID", SelectedID);
                cmd.Parameters.AddWithValue("@status", status);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }

            MessageBox.Show("Data " + SelectedID + " berhasil diPerbaharui");
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnInsert.Enabled = true;
            HideTextBox();
            loadData();
        }


        private void ISIPCS_ValueChanged(object sender, EventArgs e)
        {
            string modalText = MODAL.Text;
            int jumlahIsi = Convert.ToInt32(ISIPCS.Value);
            int jumlahBarang = Convert.ToInt32(JUMLAHBARANG.Value);
            int laba = int.Parse(LABA1.Text);
            int persen = laba + 100;
            decimal totalHarga = 0;
            decimal hj = 0;
            decimal keuntungan = 0;
            string konversiDecimal = keuntungan.ToString("N0", new CultureInfo("id-ID"));
            StokAllPcs = jumlahIsi * jumlahBarang;

            if (decimal.TryParse(modalText, out decimal modal))
            {
                totalHarga = modal * jumlahBarang;
                hj = modal * persen / 100;
                hj = hj / jumlahIsi;
                keuntungan = hj * jumlahIsi * jumlahBarang - totalHarga;
            }
            decimal t = 0;
            if (decimal.TryParse(modalText, out decimal modals))
            {
                t = modals;
            }
            decimal pcs = t / jumlahIsi;
            HargaPcs.Text = pcs.ToString("N0", new CultureInfo("id-ID"));
            TOTALHARGA.Text = totalHarga.ToString("N0", new CultureInfo("id-ID"));
            //KEUNTUNGAN.Text = keuntungan.ToString("N0", new CultureInfo("id-ID"));
            //HARGAJUAL1.Text = hj.ToString("N0", new CultureInfo("id-ID"));

        }

        /*
         private void LABA1_ValueChanged(object sender, EventArgs e)
        {
            string modalText = MODAL.Text;
            int jumlahIsi = Convert.ToInt32(ISIPCS.Value);
            int jumlahBarang = Convert.ToInt32(JUMLAHBARANG.Value);
            int laba = Convert.ToInt32(LABA1.Value);
            int persen = laba + 100;
            decimal totalHarga = 0;
            decimal hj = 0;
            decimal keuntungan = 0;

            if (decimal.TryParse(modalText, out decimal modal))
            {
                totalHarga = modal * jumlahBarang;
                hj = modal * persen / 100;
                hj = hj / jumlahIsi;
                keuntungan = hj * jumlahIsi * jumlahBarang - totalHarga;
            }

            TOTALHARGA.Text = totalHarga.ToString("N0", new CultureInfo("id-ID"));
            KEUNTUNGAN.Text = keuntungan.ToString("N0", new CultureInfo("id-ID"));
            HARGAJUAL1.Text = hj.ToString("N0", new CultureInfo("id-ID"));
        }
         */
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            KODEBARANG.Enabled = false;
            groupBox2.Visible = true;
            Tunai.Enabled = false; 
            gBstok.Visible = true;
            SaveSection = SaveSectionEnum.Update;
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (SaveSection != SaveSectionEnum.Insert)
                {
                    DataGridViewRow row = dgv.Rows[e.RowIndex];
                    string hargaPcs = row.Cells["Column8"].Value.ToString().Replace(".", "");
                    string hargaBeli = row.Cells["Column9"].Value.ToString().Replace(".", "");
                    string keuntungan = row.Cells["Column10"].Value.ToString().Replace(".", "");
                    string totalHarga = row.Cells["Column11"].Value.ToString().Replace(".", "");

                    SelectedID = row.Cells["Column1"].Value.ToString();
                    KODEBARANG.Text = row.Cells["Column1"].Value.ToString();
                    NAMABARANG.Text = row.Cells["Column2"].Value.ToString();
                    JUMLAHBARANG.Text = row.Cells["Column3"].Value.ToString();
                    ISIPCS.Text = row.Cells["Column4"].Value.ToString();
                    DISTRIBUTOR.Text = row.Cells["Column5"].Value.ToString();
                    stokSatuan.Text = row.Cells["Column6"].Value.ToString();
                    stokPcs.Text = row.Cells["Column7"].Value.ToString();
                    HARGAJUAL1.Text = hargaPcs;
                    MODAL.Text = hargaBeli;
                    HargaPcs.Text = keuntungan;
                    TOTALHARGA.Text = totalHarga;

                    // Ganti query dengan parameterized query untuk menghindari SQL injection
                    string query = "SELECT labaJual, Tunai, payment FROM tb_stok WHERE kode_brg = @kodeBrg";
                    string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
                    MySqlConnection connection = new MySqlConnection(connectionString);
                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        try
                        {
                            // Ganti dengan parameterized query
                            cmd.Parameters.AddWithValue("@kodeBrg", SelectedID);

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    LABA1.Text = reader["labaJual"].ToString();
                                    Tunai.Text = reader["Tunai"].ToString();
                                    string t = reader["payment"].ToString();

                                    if (t == "Kredit")
                                    {
                                        guna2CheckBox1.Checked = true;
                                    }
                                    else
                                    {
                                        guna2CheckBox1.Checked = false;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Terjadi kesalahan (Master - 1): " + ex.Message);
                        }
                    }

                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                    btnInsert.Enabled = false;
                    connection.Close();
                }
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleteData();
        }

        private void FrmMaster_Click(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnInsert.Enabled = true;
            groupBox2.Visible = false;
            gBstok.Visible = false;
        }

        private void SEARCH_TextChanged(object sender, EventArgs e)
        {
            searchData();
        }

        private void stokSatuan_ValueChanged(object sender, EventArgs e)
        {
            /*sisaBoxValidation = true;
            if (sisaBoxValidation)
            {
                sisaPcsValidation = false;
                string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
                string query = "select sisaPcs, sisaBox from tb_stok where kode_brg = @kode";
                string kode = KODEBARANG.Text;
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@kode", kode);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int box = 0;
                                decimal StokPcs = 0;
                                if (!reader.IsDBNull(0))
                                {
                                    StokPcs = reader.GetInt32(0);
                                    box = reader.GetInt32(1);
                                }
                                int StokBox = (int)stokSatuan.Value - box;
                                decimal isiPcs = ISIPCS.Value;
                                decimal hasil = StokBox * isiPcs;
                                decimal StockPcs = stokPcs.Value;
                                stokPcs.Value = StokPcs + hasil;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show( ex.Message);
                    }

                }
            }*/
        }

        private void HARGAJUAL1_TextChanged(object sender, EventArgs e)
        {
            if (HARGAJUAL1.Text != "0" && HargaPcs.Text != "0")
            {
                string modalText = MODAL.Text;
                string hargaJual1Text = HARGAJUAL1.Text;

                //MessageBox.Show(modalText);
                if (decimal.TryParse(modalText, out decimal modal) && decimal.TryParse(hargaJual1Text, out decimal hargaJual1))
                {
                    decimal persen = ISIPCS.Value;
                    modal = modal / persen;
                    // Menghitung laba1 dari modal dan hargaJual1
                    decimal laba1 = (hargaJual1 - modal) / modal * 100;

                    // Menampilkan laba1
                    LABA1.Text = laba1.ToString("N0");
                }
            }
        }

        private void LABA1_TextChanged(object sender, EventArgs e)
        {
            /*string modalText = MODAL.Text;
            int jumlahIsi = Convert.ToInt32(ISIPCS.Value);
            int jumlahBarang = Convert.ToInt32(JUMLAHBARANG.Value);
            decimal laba = decimal.Parse(LABA1.Text);
            decimal persen = laba + 100;
            decimal totalHarga = 0;
            decimal hj = 0;
            decimal keuntungan = 0;

            if (decimal.TryParse(modalText, out decimal modal))
            {
                totalHarga = modal * jumlahBarang;
                hj = modal * persen / 100;
                hj = hj / jumlahIsi;
                keuntungan = hj * jumlahIsi * jumlahBarang - totalHarga;
            }

            TOTALHARGA.Text = totalHarga.ToString("N0", new CultureInfo("id-ID"));
            KEUNTUNGAN.Text = keuntungan.ToString("N0", new CultureInfo("id-ID"));
            HARGAJUAL1.Text = hj.ToString("N0", new CultureInfo("id-ID"));*/
        }

        private void stokPcs_ValueChanged(object sender, EventArgs e)
        {
            sisaPcsValidation = true;
            if (sisaPcsValidation)
            {
                sisaBoxValidation = false;
                decimal sisaPcs = stokPcs.Value;
                decimal pembagi = ISIPCS.Value;
                decimal hasil = sisaPcs / pembagi;

                int hasilAkhir = (int)Math.Ceiling(hasil);

                stokSatuan.Value = hasilAkhir;
            }
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked == true)
            {
                label3.Text = "Dp";
            }
            else
            {
                label3.Text = "Tunai";
            }
        }

        private void stokSatuan_ValueChange()
        {
            sisaBoxValidation = true;
            if (sisaBoxValidation)
            {
                sisaPcsValidation = false;
                string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
                string query = "select sisaPcs, sisaBox from tb_stok where kode_brg = @kode";
                string kode = KODEBARANG.Text;
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@kode", kode);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int box = 0;
                                decimal StokPcs = 0;
                                if (!reader.IsDBNull(0))
                                {
                                    StokPcs = reader.GetDecimal(0); // Menggunakan GetDecimal untuk nilai desimal
                                    box = reader.GetInt32(1);
                                }
                                int StokBox = (int)stokSatuan.Value - box;
                                decimal isiPcs = ISIPCS.Value;
                                decimal hasil = StokBox * isiPcs;
                                decimal StockPcs = stokPcs.Value;
                                stokPcs.Value = StokPcs + hasil;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Kesalahan: " + ex.Message); // Mengubah pesan kesalahan
                    }
                }
            }
        }
    }
}
