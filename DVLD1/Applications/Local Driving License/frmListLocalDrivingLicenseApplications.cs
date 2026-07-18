using DVLD_Buisness;
using DVLD1.Licenses;
using DVLD1.Licenses.Local_Licenses;
using DVLD1.Tests;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD1.Applications.Local_Driving_License
{
    public partial class frmListLocalDrivingLicenseApplications : Form
    {
        private DataTable _dtAllLocalDrivingLicenseApplication;
        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void frmListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtAllLocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();
            dgvListLocalDrivingLicenseApplication.DataSource= _dtAllLocalDrivingLicenseApplication;
            lblRecords.Text=dgvListLocalDrivingLicenseApplication.Rows.Count.ToString();

            if(dgvListLocalDrivingLicenseApplication.Rows.Count>0)
            {
                dgvListLocalDrivingLicenseApplication.Columns[0].HeaderText = "L.D.L AppID";
                dgvListLocalDrivingLicenseApplication.Columns[1].HeaderText = "Driving Class";
                dgvListLocalDrivingLicenseApplication.Columns[2].HeaderText = "National No.";
                dgvListLocalDrivingLicenseApplication.Columns[3].HeaderText = "Full Name";
                dgvListLocalDrivingLicenseApplication.Columns[4].HeaderText = "Application Date";
                dgvListLocalDrivingLicenseApplication.Columns[5].HeaderText = "Passed Tests";
            }

            dgvListLocalDrivingLicenseApplication.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cbFindBy.SelectedIndex = 0;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFindBy.Text)
            {
                case "L.D.L AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Status":
                    FilterColumn = "Status";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            if(txtFilter.Text.Trim()=="" || FilterColumn == "None")
            {
                _dtAllLocalDrivingLicenseApplication.DefaultView.RowFilter = "";
                lblRecords.Text=dgvListLocalDrivingLicenseApplication.Rows.Count.ToString();  
                return;
            }

            if(FilterColumn== "LocalDrivingLicenseApplicationID")
            {
                _dtAllLocalDrivingLicenseApplication.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColumn, txtFilter.Text.Trim());
            }
            else
            {
                _dtAllLocalDrivingLicenseApplication.DefaultView.RowFilter = string.Format("{0} LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            }

            lblRecords.Text = dgvListLocalDrivingLicenseApplication.Rows.Count.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFindBy.Text=="L.D.L AppID")
            {
                e.Handled=(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar));
            }
        }

        private void cbFindBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cbFindBy.Text != "None");
            txtFilter.Text = "";
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApplicationInfo frm =new frmLocalDrivingLicenseApplicationInfo((int)dgvListLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication((int)dgvListLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this Application ? ","Confirm",MessageBoxButtons.YesNo,
                MessageBoxIcon.Information)==DialogResult.No)
            {
                return;
            }

            clsLocalDrivingLicenseApplication LocalDrivingLicesneApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID((int)dgvListLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);

            if(LocalDrivingLicesneApplication!=null)
            {
                if (LocalDrivingLicesneApplication.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListLocalDrivingLicenseApplications_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Could not delete application ", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Cancel this Application ? ", "Confirm", MessageBoxButtons.YesNo,
               MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }

            clsLocalDrivingLicenseApplication LocalDrivingLicesneApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID((int)dgvListLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);

            if (LocalDrivingLicesneApplication != null)
            {
                if (LocalDrivingLicesneApplication.Cancel())
                {
                    MessageBox.Show("Application Cancelled Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListLocalDrivingLicenseApplications_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Could not Canel application ", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvListLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                 clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID (LocalDrivingLicenseApplicationID);

            int TotalPassedTests = (int)dgvListLocalDrivingLicenseApplication.CurrentRow.Cells[5].Value;
            bool IsNew = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);
            bool LicenseExists = LocalDrivingLicenseApplication.IsLicenseIssued();

            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (TotalPassedTests == 3) && !LicenseExists;

            showLicneseToolStripMenuItem.Enabled = LicenseExists;
            editApplicationToolStripMenuItem.Enabled = !LicenseExists && (IsNew);
            sechduleTestsToolStripMenuItem.Enabled = !LicenseExists;

            cancelApplicationToolStripMenuItem.Enabled = IsNew;
            deleteApplicationToolStripMenuItem.Enabled = IsNew;

            
            bool PassedVisionTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest); ;
            bool PassedWrittenTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.StreetTest);

            sechduleTestsToolStripMenuItem.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && (IsNew);

            if (sechduleTestsToolStripMenuItem.Enabled)
            {
                sechduleVisionTestToolStripMenuItem.Enabled = !PassedVisionTest;
                sechduleWrittenTestToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;
                sechduleStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
            }

        }

        private void sechduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestAppointments frm = new frmListTestAppointments((int)dgvListLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value, clsTestType.enTestType.VisionTest);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void sechduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestAppointments frm = new frmListTestAppointments((int)dgvListLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value, clsTestType.enTestType.WrittenTest);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void sechduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestAppointments frm = new frmListTestAppointments((int)dgvListLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value, clsTestType.enTestType.StreetTest);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueDrivingLicenseFirstTime frm = new frmIssueDrivingLicenseFirstTime((int)dgvListLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void showLicneseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvListLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value;

            int LicenseID = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(
               LocalDrivingLicenseApplicationID).GetActiveLicenseID();

            if (LicenseID != -1)
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
                frm.ShowDialog();

            }
            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void showPersonLicneseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvListLocalDrivingLicenseApplication.CurrentRow.Cells[0].Value;

            int PersonID = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(
               LocalDrivingLicenseApplicationID).ApplicantPersonID;
            if (PersonID != -1)
            {
                frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(PersonID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
         
        }
    }
}
