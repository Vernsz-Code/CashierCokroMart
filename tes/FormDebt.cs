﻿using System;
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
    public partial class FormDebt : Form
    {
        string server = "localhost";
        string database = "cashier";
        string uid = "root";
        string password = "";

        public FormDebt()
        {
            InitializeComponent();
        }

        private void LoadLaporan()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "SELECT * FROM transaction_in WHERE payment = 'kredit' and DATE(tgl) = @tgl";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    string tglv = STARTDATE.Value.ToString("yyyy-MM-dd");
                    cmd.Parameters.AddWithValue("@tgl", tglv);
                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dgv.Rows.Clear();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string ID = reader["id"].ToString();
                                    string No_faktur = reader["no_faktur"].ToString();
                                    DateTime Tgls = Convert.ToDateTime(reader["tgl"]);
                                    string Kode = reader["kode"].ToString();
                                    string Nama = reader["nama"].ToString();
                                    int Qty = Convert.ToInt32(reader["qty"]);
                                    string Supplier = reader["suplier"].ToString();
                                    string Payment = reader["payment"].ToString();
                                    decimal harga = Convert.ToDecimal(reader["harga"]);
                                    string Tgl = Tgls.ToString("yyyy-MM-dd");
                                    string hargaStr = harga.ToString("N0");

                                    dgv.Rows.Add(ID, No_faktur, Tgl, Kode, Nama, Qty, hargaStr, Supplier, Payment);
                                }
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

        private void FormDebt_Load(object sender, EventArgs e)
        {
            STARTDATE.Value = DateTime.Now;
        }

        private void STARTDATE_ValueChanged(object sender, EventArgs e)
        {
            LoadLaporan();
        }
    }
}
