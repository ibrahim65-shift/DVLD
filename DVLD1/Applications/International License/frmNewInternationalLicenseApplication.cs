using DVLD.Classes;
using DVLD_Buisness;
using DVLD1.Licenses;
using DVLD1.Licenses.International_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Applications.International_License
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        private int _InternationalLicenseID;
        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm= new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }

        private void llLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text= DateTime.Now.ToShortDateString();
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees.ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;
            
            if(SelectedLicenseID == -1)
            {
                return;
            }

            lblLocalLicenseID.Text= SelectedLicenseID.ToString();

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClass!=3)
            {
                MessageBox.Show("Selected License Should be Class3 , select another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int InterNationalLicenseID = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID);

            if (InterNationalLicenseID!=-1)
            {
                MessageBox.Show("Person Already have an active International License With ID = " + InterNationalLicenseID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowNewLicenseInfo.Enabled = true;
                _InternationalLicenseID=InterNationalLicenseID;
                return;
            }

            if(!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License Is Not Active , Choose another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnIssue.Enabled = true;
            llLicenseHistory.Enabled = true;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to issue this License ? ","Issue",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
            {
                return;
            }

            clsInternationalLicense NewLicense = new clsInternationalLicense();

            NewLicense.ApplicantPersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;
            NewLicense.ApplicationDate=DateTime.Now;
            NewLicense.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            NewLicense.LastStatusDate = DateTime.Now;
            NewLicense.PaidFees = Convert.ToSingle(lblApplicationFees.Text.Trim());
            NewLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            NewLicense.DriverID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID;
            NewLicense.IssuedUsingLocalLicenseID = int.Parse(lblLocalLicenseID.Text.Trim());
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(1);
            NewLicense.IsActive = true;
            NewLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if(!NewLicense.Save())
            {
               MessageBox.Show("Faild to Issue International License" , "Faild",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            lblILApplicationID.Text = NewLicense.ApplicationID.ToString();
            _InternationalLicenseID = NewLicense.InternationalLicenseID;
            lblIntLLicenseID.Text =_InternationalLicenseID.ToString();
            MessageBox.Show("International License Issued Successfully With ID = " + NewLicense.InternationalLicenseID.ToString(), "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnIssue.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowNewLicenseInfo.Enabled = true;
        }
    }
}
