using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tes
{
    public partial class FormUserSettings : Form
    {
        public FormUserSettings()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            FormAddStaff frmAddStaff = new FormAddStaff();
            frmAddStaff.ShowDialog();
        }
    }
}
