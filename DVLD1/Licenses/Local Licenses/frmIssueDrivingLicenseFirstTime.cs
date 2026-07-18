using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Licenses.Local_Licenses
{
    public partial class frmIssueDrivingLicenseFirstTime : Form
    {
        private clsLocalDrivingLicenseApplication _LocalDrivingLicesneApplication;
        private int _LocalDrivingLicenseApplicationID;
        public frmIssueDrivingLicenseFirstTime(int localDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmIssueDrivingLicenseFirstTime_Load(object sender, EventArgs e)
        {
            _LocalDrivingLicesneApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);
            if(_LocalDrivingLicesneApplication==null)
            {
                MessageBox.Show("No Application With ID = " + _LocalDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if(!_LocalDrivingLicesneApplication.PassedAllTests())
            {
                MessageBox.Show("Person Should Pass All Test First" , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int LicenseID = _LocalDrivingLicesneApplication.GetActiveLicenseID();
            if(LicenseID!=-1)
            {
                MessageBox.Show("Person Already has license before with License ID = " + LicenseID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlDrivingLicenseApplication1.LoadApplicationInofByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            int LicenseID = _LocalDrivingLicesneApplication.IssueLicenseForTheFirtTime(txtNotes.Text.Trim(), clsGlobal.CurrentUser.UserID);

            if(LicenseID!=-1)
            {
                MessageBox.Show("License Issued Successfully With ID = " + LicenseID.ToString(), "Issue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("License Was Not Issued " ,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
