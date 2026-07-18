using DVLD_Buisness;
using DVLD1.Licenses;
using DVLD1.Licenses.Detain_Licenses;
using DVLD1.Licenses.Local_Licenses;
using DVLD1.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Applications.Release_Detianed_Licenses
{
    public partial class frmListDetainedLicenses : Form
    {
        private clsLicense _License;
        private DataTable _dtAllDetainedLicenses;
        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            frmDetainedLicneseApplication frm= new frmDetainedLicneseApplication();
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication();
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);
        }

        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            _dtAllDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();

            dgvListDetainedLicenses.DataSource = _dtAllDetainedLicenses;
            lblRecords.Text=dgvListDetainedLicenses.Rows.Count.ToString();
            cbFindBy.SelectedIndex = 0;

            if(dgvListDetainedLicenses.Rows.Count>0)
            {
                dgvListDetainedLicenses.Columns[0].HeaderText = "D.ID";
                dgvListDetainedLicenses.Columns[1].HeaderText = "L.ID";
                dgvListDetainedLicenses.Columns[2].HeaderText = "D.Date";
                dgvListDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvListDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvListDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvListDetainedLicenses.Columns[6].HeaderText = "N.No";
                dgvListDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvListDetainedLicenses.Columns[8].HeaderText = "Release App.ID";
            }

            dgvListDetainedLicenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _License = clsLicense.Find((int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value);
            int PersonID = _License.DriverInfo.PersonID;

            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo((int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _License = clsLicense.Find((int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value);
            int PersonID = _License.DriverInfo.PersonID;
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(PersonID);
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication((int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            releaseDetainedLicenseToolStripMenuItem.Enabled = !(bool)dgvListDetainedLicenses.CurrentRow.Cells[3].Value;
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFindBy.Text== "Detain ID" ||  cbFindBy.Text== "Release Application ID")
            {
                e.Handled= (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar));
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
           
            switch(cbFindBy.Text)
            {
                case "Detain ID":
                    FilterColumn = "DetainID";
                    break;

                case "Is Released":
                    FilterColumn = "IsReleased";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Release Application ID":
                    FilterColumn = "ReleaseApplicationID";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if(txtFilter.Text=="" || FilterColumn=="None")
            {
                _dtAllDetainedLicenses.DefaultView.RowFilter = "";
                lblRecords.Text = dgvListDetainedLicenses.Rows.Count.ToString();
                return;
            }

            if(cbFindBy.Text == "Detain ID" || cbFindBy.Text == "Release Application ID")
            {
                _dtAllDetainedLicenses.DefaultView.RowFilter=string.Format("[{0}]={1}",FilterColumn,txtFilter.Text.Trim());
            }
            else
            {
                _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            }

            lblRecords.Text = dgvListDetainedLicenses.Rows.Count.ToString();
        }

        private void cbFindBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFindBy.Text=="Is Released")
            {
                cbIsReleased.Visible = true;
                cbIsReleased.SelectedIndex = 0;
                txtFilter.Visible = false;
                txtFilter.Text = "";
                return;
            }
            else
            {
                cbIsReleased.Visible = false;
                txtFilter.Visible = (cbFindBy.Text != "None");
                txtFilter.Text = "";
            }
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsReleased";
            string FilterValue = cbIsReleased.Text;

            switch (cbIsReleased.Text)
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
                _dtAllDetainedLicenses.DefaultView.RowFilter = "";
                lblRecords.Text = dgvListDetainedLicenses.Rows.Count.ToString();
            }
            else
            {
                _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColumn, FilterValue);
            }

            lblRecords.Text = dgvListDetainedLicenses.Rows.Count.ToString();
        }
    }
}
