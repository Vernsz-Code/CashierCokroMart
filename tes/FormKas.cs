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
    public partial class FormKas : Form
    {
        string server = "localhost";
        string database = "cashier";
        string uid = "root";
        string password = "";
        private string Faktura = "";

        public FormKas()
        {
            InitializeComponent();
        }


        private void FormKas_Load(object sender, EventArgs e)
        {
            STARTDATE.Value = DateTime.Now;
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            FormAddKas form = new FormAddKas();
            form.NoFaktur = Faktura;
            form.condition = "insert";
            form.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FormAddKas form = new FormAddKas();
            form.NoFaktur = Faktura;
            form.condition = "update";
            form.ShowDialog();
        }

        private void STARTDATE_ValueChanged(object sender, EventArgs e)
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = "SELECT DATE(tgl) AS Tanggal, " +
              "SUM(CASE WHEN payment = 'tunai' THEN subtotal ELSE 0 END) as Pengeluaran, " +
              "SUM(CASE WHEN payment = 'kredit' THEN subtotal ELSE 0 END) as Hutang, " +
              "SUM(CASE WHEN payment = 'tunai' THEN subtotal ELSE -subtotal END) AS Total " +
              "FROM transaction " +
              "WHERE DATE_FORMAT(tgl, '%Y-%m') = @bulanTertentu " +
              "GROUP BY DATE(tgl)";
            string strTanggal = STARTDATE.Value.ToString("yyyy-MM");
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@bulanTertentu", strTanggal);
                // Buat adapter SQL
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();

                    // Isi data dari database ke DataTable
                    adapter.Fill(dataTable);

                    // Bind DataTable ke DataGridView
                    dgv.DataSource = dataTable;
                }
            }
        }
    }
}
