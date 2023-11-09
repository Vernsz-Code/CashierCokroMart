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
        
        private void FormDebt_Load(object sender, EventArgs e)
        {
        }

        private void STARTDATE_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}
