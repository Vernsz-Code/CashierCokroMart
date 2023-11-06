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
        private string SelectedID;

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
            S_AWAL.Value = 0;
            HARGAJUAL1.Text = "";
            DISTRIBUTOR.Text = "";
            LABA.Text = "0";
            Harta.Text = "";
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
                persentase,
                suplier
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
                                string suplier = reader.GetString(13);

                                string beliRupiah = beli.ToString("N0", new CultureInfo("id-ID"));
                                string jualRupiah = jual.ToString("N0", new CultureInfo("id-ID"));
                                string markUpRupiah = mark_up.ToString("N0", new CultureInfo("id-ID"));
                                string pendapatanRupiah = pendapatan.ToString("N0", new CultureInfo("id-ID"));
                                string labaRupiah = laba.ToString("N0", new CultureInfo("id-ID"));
                                string hartaRupiah = harta.ToString("N0", new CultureInfo("id-ID"));

                                dgv.Rows.Add(no, kode_brg, nama_brg, stok_awal, stok_masuk, stok_keluar, stok_akhir, suplier, beliRupiah, jualRupiah, markUpRupiah, pendapatanRupiah, labaRupiah, hartaRupiah, persentase);
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
                from product where kode_brg LIKE '%" + SEARCH.Text + "%' " +
                "OR nama_brg LIKE '%" + SEARCH.Text + "%'";
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

                                dgv.Rows.Add(no, kode_brg, nama_brg, stok_awal, stok_masuk, stok_keluar, stok_akhir, "-", beliRupiah, jualRupiah, markUpRupiah, pendapatanRupiah, labaRupiah, hartaRupiah, persentase);
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
                String.IsNullOrEmpty(LABA.Text)
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
            btnInsert.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            ClearString();
        }

        private void JUMLAHBARANG_ValueChanged(object sender, EventArgs e)
        {
            string modalText = MODAL.Text;
            int jumlahBarang = Convert.ToInt32(S_AWAL.Value);
            int laba = int.Parse(LABA.Text);
            int persen = laba + 100;
            decimal totalHarga = 0;
            decimal keuntungan = 0;
            string konversiDecimal = keuntungan.ToString("N0", new CultureInfo("id-ID"));
            MARKUP.Text = totalHarga.ToString("N0", new CultureInfo("id-ID"));
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
                string totalHarga = MARKUP.Text;
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
                string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                string kueri = "INSERT INTO product(kode_brg, nama_brg, stok_awal, masuk, keluar, stok_keluar, suplier, beli, jual, mark_up, pendapatan, laba, harta, persentase) values(@kode_brg, @nama_brg, @stok_awal, @masuk, @keluar, @stok_keluar, @suplier, @beli, @jual, @mark_up, @pendapatan, @laba, @harta, @persentase);";

                //data
                string kode_brg = KODEBARANG.Text;
                string nama_brg = NAMABARANG.Text;
                int stok_awal = int.Parse(S_AWAL.Value.ToString());
                int masuk = int.Parse(B_MASUK.Value.ToString());
                int keluar = int.Parse(B_KELUAR.Value.ToString());
                int stok_akhir = int.Parse(S_AKHIR.Value.ToString());
                string sup = DISTRIBUTOR.Text;

                if (sup == "")
                {
                    sup = "-";
                }
                
                string beli = MODAL.Text;
                string jual = HARGAJUAL1.Text;
                string markup = MARKUP.Text;
                string pendapatan = PENDAPATAN.Text;

                decimal hargaBeli = decimal.Parse(beli);
                decimal hargaJual = decimal.Parse(jual);
                decimal result = hargaJual - hargaBeli;

                string laba = result.ToString();
                string harta = Harta.Text;
                decimal percen = decimal.Parse(LABA.Text);
                //data

                using (MySqlCommand cmd = new MySqlCommand(kueri, connection))
                {
                    cmd.Parameters.AddWithValue("@kode_brg", kode_brg);
                    cmd.Parameters.AddWithValue("@nama_brg", nama_brg);
                    cmd.Parameters.AddWithValue("@stok_awal", stok_awal);
                    cmd.Parameters.AddWithValue("@masuk", masuk);
                    cmd.Parameters.AddWithValue("@keluar", keluar);
                    cmd.Parameters.AddWithValue("@stok_akhir", stok_akhir);
                    cmd.Parameters.AddWithValue("@suplier", sup);
                    cmd.Parameters.AddWithValue("@beli", beli);
                    cmd.Parameters.AddWithValue("@jual", jual);
                    cmd.Parameters.AddWithValue("@mark_up", markup);
                    cmd.Parameters.AddWithValue("@pendapatan", pendapatan);
                    cmd.Parameters.AddWithValue("@laba", laba);
                    cmd.Parameters.AddWithValue("@harta", harta);
                    cmd.Parameters.AddWithValue("@persentase", percen);
                }
            }
            catch (Exception ex)
            {
                // Tangani kesalahan dengan pesan yang lebih deskriptif
                MessageBox.Show("Terjadi kesalahan saat menambahkan data: " + ex.Message);
            }
        }

        private void updateData()
        {
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (SaveSection != SaveSectionEnum.Insert)
                {
                    DataGridViewRow row = dgv.Rows[e.RowIndex];
                    KODEBARANG.Text = row.Cells["Column1"].Value.ToString();
                    NAMABARANG.Text = row.Cells["Column2"].Value.ToString();
                    S_AWAL.Text = row.Cells["Column3"].Value.ToString();
                    B_MASUK.Text = row.Cells["Column4"].Value.ToString();
                    B_KELUAR.Text = row.Cells["Column5"].Value.ToString();
                    S_AKHIR.Text = row.Cells["Column6"].Value.ToString();
                    string hargaBeli = row.Cells["Column7"].Value.ToString().Replace(".", "");
                    string hargaJual = row.Cells["Column8"].Value.ToString().Replace(".", "");
                    MARKUP.Text = row.Cells["Column9"].Value.ToString();
                    string Pendapatan = row.Cells["Column10"].Value.ToString().Replace(".", "");
                    Harta.Text = row.Cells["Column12"].Value.ToString().Replace(".", "");
                    LABA.Text = row.Cells["Column13"].Value.ToString();
                    DISTRIBUTOR.Text = row.Cells["Suplier"].Value.ToString();
                    HARGAJUAL1.Text = hargaJual;
                    MODAL.Text = hargaBeli;
                    PENDAPATAN.Text = Pendapatan;

                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                    btnInsert.Enabled = false;
                }
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleteData();
        }

        private void FrmMaster_Click(object sender, EventArgs e)
        {
        }

        private void SEARCH_TextChanged(object sender, EventArgs e)
        {
            searchData();
        }

        private void HARGAJUAL1_TextChanged(object sender, EventArgs e)
        {
        }

        private void LABA1_TextChanged(object sender, EventArgs e)
        {
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
