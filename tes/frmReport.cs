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
using System.Runtime.InteropServices.ComTypes;

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
        private void GetDataByDate()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "SELECT no_faktur, tgl, kode, nama, qty, harga, laba FROM transaction WHERE DATE(tgl) = DATE(@tgl) AND payment = 'tunai'";

            DateTime tgl = STARTDATE.Value;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    // Mengatur parameter tanggal
                    cmd.Parameters.Add("@tgl", MySqlDbType.DateTime).Value = tgl;

                    Console.WriteLine(tgl);
                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Mengecek apakah ada data yang bisa dibaca
                            dgv.Rows.Clear();
                            if (reader.HasRows)
                            {
                                // Bersihkan DataGridView jika sudah ada data sebelumnya

                                // Loop melalui hasil pembacaan
                                while (reader.Read())
                                {
                                    // Mengambil nilai dari hasil pembacaan
                                    string noFaktur = reader["no_faktur"].ToString();
                                    DateTime tanggal = Convert.ToDateTime(reader["tgl"]);
                                    string kode = reader["kode"].ToString();
                                    string nama = reader["nama"].ToString();
                                    int qty = Convert.ToInt32(reader["qty"]);
                                    decimal harga = Convert.ToDecimal(reader["harga"]);
                                    string strharga = harga.ToString("C", new CultureInfo("ID-id"));
                                    decimal laba = Convert.ToDecimal(reader["laba"]);
                                    string strlaba = laba.ToString("C", new CultureInfo("ID-id"));

                                    decimal subtotal = qty * harga;
                                    string tanggalFormatted = tanggal.ToString("yyyy-MM-dd");

                                    // Tambahkan data ke DataGridView
                                    dgv.Rows.Add(noFaktur, tanggalFormatted, kode, nama, qty, strharga, strlaba, laba, subtotal);
                                }
                                decimal total = 0;
                                decimal labas = 0;
                                int qtys = 0;
                                for (int i = 0; i < dgv.Rows.Count;)
                                {
                                    total += decimal.Parse(dgv.Rows[i].Cells[8].Value.ToString());
                                    labas += decimal.Parse(dgv.Rows[i].Cells[7].Value.ToString());
                                    qtys += int.Parse(dgv.Rows[i].Cells[6].Value.ToString());
                                    i++;
                                }
                                string totalText ="Total : " + total.ToString("C", new CultureInfo("id-ID"));
                                string labaText ="Laba : " + labas.ToString("C", new CultureInfo("id-ID"));
                                string qtyText ="Qty : " + qtys.ToString("C", new CultureInfo("id-ID"));
                                dgv.Rows.Add("", "", "", "", qtyText, totalText, labaText);
                            }
                            else
                            {
                                Console.WriteLine("A");
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



        private void frmReport_Load(object sender, EventArgs e)
        {
            STARTDATE.Value = DateTime.Now;
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FormRetur_Closed(object sender, FormClosedEventArgs e)
        {

        }

        private void STARTDATE_ValueChanged(object sender, EventArgs e)
        {
            GetDataByDate();
            DateTime tgl = STARTDATE.Value;

            string strTgl = tgl.ToString("yyyy-MM-dd");
            lbl_tanggal.Text = strTgl;
        }
    }
}
