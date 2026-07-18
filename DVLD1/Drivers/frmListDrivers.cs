using DVLD_Buisness;
using DVLD1.Licenses;
using DVLD1.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Drivers
{
    public partial class frmListDrivers : Form
    {
        private DataTable _dtListDrivers;
        public frmListDrivers()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _dtListDrivers = clsDriver.GetAllDrivers();
            dgvListDrivers.DataSource = _dtListDrivers;

            cbFindBy.SelectedIndex = 0;
            lblRecord.Text=dgvListDrivers.Rows.Count.ToString();

            if(dgvListDrivers.Rows.Count>0)
            {
                dgvListDrivers.Columns[0].HeaderText = "Driver ID";
                dgvListDrivers.Columns[1].HeaderText = "Person ID";
                dgvListDrivers.Columns[2].HeaderText = "National No.";
                dgvListDrivers.Columns[3].HeaderText = "Full Name";
                dgvListDrivers.Columns[4].HeaderText = "Date";
                dgvListDrivers.Columns[5].HeaderText = "Active Licenses";
            }

            dgvListDrivers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFindBy.Text=="Driver ID" || cbFindBy.Text=="Person ID")
            {
                e.Handled = (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar));
            }
        }

        private void cbFindBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cbFindBy.Text != "None");
            txtFilter.Text = "";
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFindBy.Text)
            {
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if(txtFilter.Text=="" || FilterColumn=="None")
            {
                _dtListDrivers.DefaultView.RowFilter = "";
                lblRecord.Text=dgvListDrivers.Rows.Count.ToString();
                return;
            }

            if(FilterColumn=="DriverID"||FilterColumn=="PersonID")
            {
                _dtListDrivers.DefaultView.RowFilter=string.Format("[{0}]={1}",FilterColumn,txtFilter.Text.Trim());
            }
            else
            {
                _dtListDrivers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            }

            lblRecord.Text = dgvListDrivers.Rows.Count.ToString();
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo((int)dgvListDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory((int)dgvListDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }
    }
}
