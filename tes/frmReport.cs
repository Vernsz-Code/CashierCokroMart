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
            string query = "SELECT no_faktur, tgl, kode, nama, qty, harga, laba FROM transaction WHERE DATE(tgl) = DATE(@tgl)";

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
                            if (reader.HasRows)
                            {
                                // Bersihkan DataGridView jika sudah ada data sebelumnya
                                dgv.Rows.Clear();

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
                                    decimal laba = Convert.ToDecimal(reader["laba"]);

                                    // Tambahkan data ke DataGridView
                                    dgv.Rows.Add(noFaktur, tanggal, kode, nama, qty, harga, laba);
                                }
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
        }

        private void SEARCH_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
