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
            Penjualan();
        }

        private void Penjualan()
        {

            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = "SELECT SUM(subtotal) as Penjualan from transaction WHERE DATE(tgl) = @tgl";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                string tgl = DatePicker.Value.ToString("yyyy-MM-dd");
                connection.Open();
                cmd.Parameters.AddWithValue("@tgl", tgl);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        decimal p = 0;
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                p = reader.GetDecimal(0);
                                lbl_PENJUALAN.Text = p.ToString("C", new CultureInfo("ID-id"));
                            }
                            else
                            {
                                lbl_PENJUALAN.Text = "Rp0,00";
                            }
                        }
                        
                    }
                    else
                    {

                    }
                }
            }
        }

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            DatePicker.Value = DateTime.Now;
        }
    }
}
