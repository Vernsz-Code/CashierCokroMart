using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;

namespace tes
{
    public partial class FormMain : Form
    {
        Button currentButton;
        public FormMain()
        {
            InitializeComponent();
        }

        bool transaksi = false;
        bool laporan = false;

        public void LoadForm(object Form)
        {
            Form previousForm = mainPanel.Controls.OfType<Form>().FirstOrDefault();
            if (previousForm != null)
            {
                previousForm.Dispose();
                mainPanel.Controls.Remove(previousForm);
            }

            mainPanel.Visible = true;
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(f);

            f.Show();
        }
        void activebutton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    resetbutton();
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = Color.FromArgb(18, 25, 48);
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }

        void resetbutton()
        {
            btnDashboard.BackColor = Color.FromArgb(27, 37, 71);
            btnDashboard.ForeColor = Color.Gainsboro;
            btnDashboard.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            btnMaster.BackColor = Color.FromArgb(27, 37, 71);
            btnMaster.ForeColor = Color.Gainsboro;
            btnMaster.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            btnlaporan.BackColor = Color.FromArgb(27, 37, 71);
            btnlaporan.ForeColor = Color.Gainsboro;
            btnlaporan.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            btnClientDebt.BackColor = Color.FromArgb(27, 37, 71);
            btnClientDebt.ForeColor = Color.Gainsboro;
            btnClientDebt.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            btnDebt.BackColor = Color.FromArgb(27, 37, 71);
            btnDebt.ForeColor = Color.Gainsboro;
            btnDebt.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            btnFaktur.BackColor = Color.FromArgb(27, 37, 71);
            btnFaktur.ForeColor = Color.Gainsboro;
            btnFaktur.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            button2.BackColor = Color.FromArgb(27, 37, 71);
            button2.ForeColor = Color.Gainsboro;
            button2.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            btnKas.BackColor = Color.FromArgb(27, 37, 71);
            btnKas.ForeColor = Color.Gainsboro;
            btnKas.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            LoadForm(new FormDashboard());
            activebutton(sender);
        }

        private void btnMaster_Click(object sender, EventArgs e)
        {
            LoadForm(new FrmStok());
            activebutton(sender);
        }

        private void guna2ImageButton2_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2ImageButton1_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Apakah kamu yakin ingin keluar dari app?", "Exit", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            System.Diagnostics.Process.Start("https://wa.link/utc65e");
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadForm(new FormDashboard());
        }

        private void btnUserSettings_Click(object sender, EventArgs e)
        {
            activebutton(sender);
        }
        

        private void btnFaktur_Click(object sender, EventArgs e)
        {
            LoadForm(new FormKasir());
            activebutton(sender);
        }

        private void btnlaporan_Click(object sender, EventArgs e)
        {
            LoadForm(new frmReport());
            activebutton(sender);
        }

        private void btnDebt_Click(object sender, EventArgs e)
        {
            LoadForm(new FormDebt());
            activebutton(sender);
        }

        private void btnClientDebt_Click(object sender, EventArgs e)
        {
            LoadForm(new FormReceivables());
            activebutton(sender);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadForm(new FormKas());
            activebutton(sender);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            activebutton(sender);
            laporan = !laporan;
            if (laporan == true)
            {
                btnClientDebt.Visible = true;
                btnDebt.Visible = true;
                btnlaporan.Visible = true;
            }
            else
            {
                btnlaporan.Visible = false;
                btnDebt.Visible = false;
                btnClientDebt.Visible = false;
            }
        }

    }
}
