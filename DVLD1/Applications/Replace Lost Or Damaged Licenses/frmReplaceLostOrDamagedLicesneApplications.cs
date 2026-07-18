using DVLD.Classes;
using DVLD_Buisness;
using DVLD1.Licenses;
using DVLD1.Licenses.Local_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Applications.Replace_Lost_Or_Damaged_Licenses
{
    public partial class frmReplaceLostOrDamagedLicesneApplications : Form
    {
        private int _NewLicenseID = -1;
        public frmReplaceLostOrDamagedLicesneApplications()
        {
            InitializeComponent();
        }

        private int _GetApplicationTypeID()
        {
            if(rbDamagedLicense.Checked)
                return (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;
            else
                return (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;


        }

        private clsLicense.enIssueReason _GetIssueReason()
        {
            if (rbDamagedLicense.Checked)
                return clsLicense.enIssueReason.DamagedReplacement;
            else
                return clsLicense.enIssueReason.LostReplacement;
        }
        private void frmReplaceLostOrDamagedLicesneApplications_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedBy.Text=clsGlobal.CurrentUser.UserName;
            rbDamagedLicense.Checked = true;
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement For Damaged Licenses";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID()).Fees.ToString();
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement For Lost Licenses";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID()).Fees.ToString();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;

            if(SelectedLicenseID==-1)
            {
                return;
            }

            lblOldLicenseID.Text = SelectedLicenseID.ToString();
            llLicenseHistory.Enabled = true;

            if(!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is Not Active , Choose an Active License",
                  "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueRepacement.Enabled = false;
                return;
            }

            btnIssueRepacement.Enabled = true;
        }

        private void btnIssueRepacement_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to Issue a Replacement for this License ? ","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.No)
            {
                return;
            }

            clsLicense NewLicense =
                ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Replace(_GetIssueReason(), clsGlobal.CurrentUser.UserID);

            if(NewLicense==null)
            {
                MessageBox.Show("Faild to Issue Replacement for this License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _NewLicenseID= NewLicense.LicenseID;
            lblLRApplicationID.Text = NewLicense.ApplicationID.ToString();
            lblReplacedLicenseID.Text=_NewLicenseID.ToString();

            MessageBox.Show("Licensed Replaced Successfully With ID = " + _NewLicenseID.ToString(), "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueRepacement.Enabled = false;
            gbReplacement.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowNewLicenseInfo.Enabled = true;
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
    }
}
