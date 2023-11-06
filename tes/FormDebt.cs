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

        private void loadData()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
            string query = "select kode_brg, nama_brg, hargaBeli, masukBox, distributor, totalHarga, status, Tunai from tb_stok where payment = 'Kredit'";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                try
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int i = 0;
                        if (reader.HasRows)
                        {
                            dgv.Rows.Clear();
                            while (reader.Read())
                            {
                                string kodeBrg = reader.GetString(0);
                                string namaBrg = reader.GetString(1);
                                decimal hargaBeli = reader.GetDecimal(2);
                                int masukBox = reader.GetInt32(3);
                                string distributor = reader.GetString(4);
                                decimal totalHarga = reader.GetDecimal(5);
                                string status = reader.GetString(6);
                                decimal Tunai = reader.GetDecimal(7);
                                string TunaiText = Tunai.ToString("N0", new CultureInfo("id-ID"));
                                i++;
                                Image editIcon = Properties.Resources.icons8_delete_24px_1;

                                dgv.Rows.Add(i, kodeBrg, namaBrg, hargaBeli, masukBox, distributor, TunaiText, totalHarga, status, editIcon);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan ketika load data", ex.Message);
                }
            }
            connection.Close();
        }

        private void FormDebt_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.ColumnIndex == dgv.Columns["Column9"].Index && e.RowIndex >= 0)
            {
                string nofaktur = dgv.Rows[e.RowIndex].Cells["column1"].ToString();
                FormDebtDetails frmRetur = new FormDebtDetails();
                frmRetur.NoFaktur = nofaktur.ToString();
                //frmRetur.FormClosed += new FormClosedEventHandler(FormRetur_Closed);
                frmRetur.ShowDialog();
            }
        }
    }
}
