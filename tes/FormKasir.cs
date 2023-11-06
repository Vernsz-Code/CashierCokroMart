using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace tes
{
    public partial class FormKasir : Form
    {
        string server = "localhost";
        string database = "cashier";
        string uid = "root";
        string password = "";

        public FormKasir()
        {
            InitializeComponent();
        }
        void LoadNoFaktur()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "SELECT MAX(no_faktur) FROM transaction";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        int highestFaktur = Convert.ToInt32(result);
                        int nextFaktur = highestFaktur + 1;
                        lbl_NOFAKTUR.Text = nextFaktur.ToString();
                    }
                    else
                    {
                        lbl_NOFAKTUR.Text = "1";
                    }
                }
            }
        }
        private void ClearAll()
        {
            txtBayar.Text = "";

            dgv.Rows.Clear();

            lbl_TotalBarang.Text = "0";
            lbl_TotalHarga.Text = "Rp0,00";
            lbl_TotalDiskon.Text = "Rp0,00";
            lbl_TotalBayar.Text = "Rp0,00";
            lbl_Kembalian.Text = "Rp0,00";
            LoadNoFaktur();
        }

        private void FormKasir_Load(object sender, EventArgs e)
        {
            txtScan.Focus();
            LoadNoFaktur();
        }
        private void LoadData(string kode_brg)
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string query = "SELECT * FROM product WHERE kode_brg = @kode_brg";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@kode_brg", kode_brg);

                bool found = false;

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells["KodeBarang"].Value != null && row.Cells["KodeBarang"].Value.ToString() == kode_brg)
                    {
                        int rowIndex = row.Index;
                        int qty = Convert.ToInt32(dgv.Rows[rowIndex].Cells["Qty"].Value) + 1;
                        dgv.Rows[rowIndex].Cells["Qty"].Value = qty;

                        decimal hargaJual = Convert.ToDecimal(row.Cells["HargaJual"].Value);
                        decimal subtotal = qty * hargaJual;
                        dgv.Rows[rowIndex].Cells["Subtotal"].Value = subtotal;

                        txtScan.Text = "";
                        UpdateTotalLabels();
                        found = true;

                        break;
                    }
                }

                if (!found)
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string kodeBarang = reader["kode_brg"].ToString();
                            string namaBarang = reader["nama_brg"].ToString();
                            string stok = reader["stok_akhir"].ToString();
                            decimal hargaJual = Convert.ToDecimal(reader["jual"]);
                            decimal markUp = Convert.ToDecimal(reader["mark_up"]);

                            int qty = 1;
                            decimal subtotal = qty * hargaJual;

                            Image deleteIcon = Properties.Resources.icons8_delete_24px_1;

                            dgv.Rows.Add(kodeBarang, namaBarang, hargaJual, qty, "0", "0", subtotal, stok, markUp, markUp, deleteIcon);
                            txtScan.Text = "";
                        }
                        else
                        {

                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void txtScan_TextChanged(object sender, EventArgs e)
        {
            LoadData(txtScan.Text);
        }
        private void UpdateTotalLabels()
        {
            decimal totalQty = 0;
            decimal totalHargaJual = 0;
            decimal totalDiskon = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["Qty"].Value != null)
                {
                    totalQty += Convert.ToDecimal(row.Cells["Qty"].Value);
                }

                if (row.Cells["Subtotal"].Value != null)
                {
                    totalHargaJual += Convert.ToDecimal(row.Cells["Subtotal"].Value);
                }

                if (row.Cells["Disc"].Value != null)
                {
                    totalDiskon += Convert.ToDecimal(row.Cells["Disc"].Value);
                }
            }

            decimal totalBayar = totalHargaJual - totalDiskon;

            lbl_TotalBarang.Text = totalQty.ToString();
            lbl_TotalHarga.Text = totalHargaJual.ToString("C", new CultureInfo("id-ID"));
            lbl_TotalDiskon.Text = totalDiskon.ToString("C", new CultureInfo("id-ID"));
            lbl_TotalBayar.Text = totalBayar.ToString("C", new CultureInfo("id-ID"));
        }

        private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateTotalLabels();
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                if (e.ColumnIndex == dgv.Columns["Qty"].Index)
                {
                    int newQty = Convert.ToInt32(row.Cells["Qty"].Value);
                    decimal hargaJual = Convert.ToDecimal(row.Cells["HargaJual"].Value);

                    decimal subtotal = newQty * hargaJual;
                    row.Cells["Subtotal"].Value = subtotal;

                    //Laba
                    decimal markUp = Convert.ToDecimal(row.Cells["MarkUp"].Value);

                    decimal laba = markUp * newQty;
                    row.Cells["Laba"].Value = laba;
                }
                else if (e.ColumnIndex == dgv.Columns["HargaJual"].Index)
                {
                    int qty = Convert.ToInt32(row.Cells["Qty"].Value);
                    decimal newHargaJual = Convert.ToDecimal(row.Cells["HargaJual"].Value);

                    decimal subtotal = qty * newHargaJual;
                    row.Cells["Subtotal"].Value = subtotal;
                }
                else if (e.ColumnIndex == dgv.Columns["DiscPercent"].Index)
                {
                    int qty = Convert.ToInt32(row.Cells["Qty"].Value);
                    decimal hargaJual = Convert.ToDecimal(row.Cells["HargaJual"].Value);
                    decimal discPercent = Convert.ToDecimal(row.Cells["DiscPercent"].Value);

                    decimal diskon = (hargaJual * discPercent / 100) * qty;
                    row.Cells["Disc"].Value = diskon;
                }
                else if (e.ColumnIndex == dgv.Columns["Disc"].Index)
                {
                    int qty = Convert.ToInt32(row.Cells["Qty"].Value);
                    decimal disc = Convert.ToDecimal(row.Cells["Disc"].Value);

                    decimal hargaJual = Convert.ToDecimal(row.Cells["HargaJual"].Value);
                    decimal discPercent = (disc / (hargaJual * qty)) * 100;
                    row.Cells["DiscPercent"].Value = discPercent;
                }
            }
        }

        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                if (e.ColumnIndex == dgv.Columns["HargaJual"].Index || e.ColumnIndex == dgv.Columns["Subtotal"].Index || e.ColumnIndex == dgv.Columns["Disc"].Index)
                {
                    if (e.Value is decimal)
                    {
                        e.Value = ((decimal)e.Value).ToString("C", new CultureInfo("id-ID"));
                    }
                }
            }
        }

        private void dgv_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateTotalLabels();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgv.Columns["btnDelete"].Index && e.RowIndex >= 0)
            {
                dgv.Rows.RemoveAt(e.RowIndex);

                UpdateTotalLabels();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string noFaktur = lbl_NOFAKTUR.Text;

            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    string kodeBarang = row.Cells["KodeBarang"].Value.ToString();
                    string nama = row.Cells["NamaBarang"].Value.ToString();
                    int qty = Convert.ToInt32(row.Cells["Qty"].Value);
                    decimal hargaJual = Convert.ToDecimal(row.Cells["HargaJual"].Value);
                    decimal subtotal = Convert.ToDecimal(row.Cells["Subtotal"].Value);
                    decimal markUp = Convert.ToDecimal(row.Cells["MarkUp"].Value);
                    decimal laba = Convert.ToDecimal(row.Cells["Laba"].Value);

                    string payment;

                    if (checkBox1.Checked)
                    {
                        payment = "kredit";
                    }
                    else
                    {
                        payment = "tunai";
                    }

                    string insertQuery = "INSERT INTO transaction (no_faktur, kode, nama, qty, harga, subtotal, mark_up, laba, payment) " +
                                            "VALUES (@no_faktur, @kode_barang, @nama_barang, @qty, @harga_jual, @subtotal, @mark_up, @laba, @payment)";

                    MySqlCommand command = new MySqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@no_faktur", noFaktur);
                    command.Parameters.AddWithValue("@kode_barang", kodeBarang);
                    command.Parameters.AddWithValue("@nama_barang", nama);
                    command.Parameters.AddWithValue("@qty", qty);
                    command.Parameters.AddWithValue("@harga_jual", hargaJual);
                    command.Parameters.AddWithValue("@subtotal", subtotal);
                    command.Parameters.AddWithValue("@mark_up", markUp);
                    command.Parameters.AddWithValue("@laba", laba);
                    command.Parameters.AddWithValue("@payment", payment);

                    command.ExecuteNonQuery();
                    Console.WriteLine("a");
                }
                ClearAll();
            }
        }

        private void btnBayar_Click(object sender, EventArgs e)
        {
            decimal totalBayar = decimal.Parse(lbl_TotalBayar.Text, NumberStyles.Currency, new CultureInfo("id-ID"));

            decimal jumlahBayar = decimal.Parse(txtBayar.Text, NumberStyles.Currency, new CultureInfo("id-ID"));

            if (jumlahBayar < totalBayar)
            {
                MessageBox.Show("Jumlah bayar kurang dari total bayar.\nHarap periksa kembali jumlah bayar.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                decimal kembalian = jumlahBayar - totalBayar;
                lbl_Kembalian.Text = kembalian.ToString("C", new CultureInfo("id-ID"));

                MessageBox.Show($"Kembalian: {kembalian.ToString("C", new CultureInfo("id-ID"))}", "Kembalian", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
