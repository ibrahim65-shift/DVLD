using DVLD_Buisness;
using DVLD1.Applications.International_License;
using DVLD1.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Licenses.International_Licenses
{
    public partial class frmListInternationalLicenseApplications : Form
    {
        private DataTable _dtAllInternationalLicenses;
        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewInternationalLicesne_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApplication frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();
            frmListInternationalLicenseApplications_Load(null, null);
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsDriver Driver = clsDriver.FindByDriverID((int)dgvListInternationalLicenses.CurrentRow.Cells[2].Value);
            frmShowPersonInfo frm = new frmShowPersonInfo(Driver.PersonID);
            frm.ShowDialog();
        }

        private void showLicesneDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo((int)dgvListInternationalLicenses.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsDriver Driver = clsDriver.FindByDriverID((int)dgvListInternationalLicenses.CurrentRow.Cells[2].Value);
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(Driver.PersonID);
            frm.ShowDialog();
        }

        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtAllInternationalLicenses=clsInternationalLicense.GetAllInternationalLicenses();

            dgvListInternationalLicenses.DataSource= _dtAllInternationalLicenses;
            lblRecords.Text=dgvListInternationalLicenses.Rows.Count.ToString();
            cbFindBy.SelectedIndex=0;   

            if(dgvListInternationalLicenses.Rows.Count > 0)
            {
                dgvListInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvListInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvListInternationalLicenses.Columns[2].HeaderText = "Driver ID";
                dgvListInternationalLicenses.Columns[3].HeaderText = "L.License ID";
                dgvListInternationalLicenses.Columns[4].HeaderText = "Issue Date";
                dgvListInternationalLicenses.Columns[5].HeaderText = "Expiration Date";
                dgvListInternationalLicenses.Columns[6].HeaderText = "Is Active";
            }

            dgvListInternationalLicenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void cbFindBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFindBy.Text=="Is Active")
            {
                cbIsActive.Visible = true;
                txtFilter.Visible = false;
                cbIsActive.SelectedIndex = 0;
            }
            else
            {
                txtFilter.Visible = (cbFindBy.Text != "None");
                txtFilter.Text = "";
                cbIsActive.Visible = false;
            }
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsActive.Text;

            switch (cbIsActive.Text)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0"; 
                    break;
            }

            if(FilterValue=="All")
            {
                _dtAllInternationalLicenses.DefaultView.RowFilter = "";
                lblRecords.Text = dgvListInternationalLicenses.Rows.Count.ToString();
                return;
            }

            _dtAllInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColumn, FilterValue);
            lblRecords.Text=dgvListInternationalLicenses.Rows.Count.ToString();

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {

            string FilterColumn = "";

            switch (cbFindBy.Text)
            {
                case "International License ID":
                    FilterColumn = "InternationalLicenseID";
                    break;
                case "Application ID":
                    FilterColumn = "ApplicationID";
                    break;
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;
                case "Local Licesne ID":
                    FilterColumn = "IssuedUsingLocalLicenseID";
                    break;
                default:
                    FilterColumn = "None";
                    break;

            }

            if(txtFilter.Text.Trim()=="" || FilterColumn=="None")
            {
                _dtAllInternationalLicenses.DefaultView.RowFilter = "";
                lblRecords.Text=dgvListInternationalLicenses.Rows.Count.ToString();
                return;
            }

            _dtAllInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColumn, txtFilter.Text.Trim());
            lblRecords.Text = dgvListInternationalLicenses.Rows.Count.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled=(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar));
        }
    }
}
