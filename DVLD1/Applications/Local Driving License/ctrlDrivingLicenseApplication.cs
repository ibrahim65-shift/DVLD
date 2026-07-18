using DVLD_Buisness;
using DVLD1.Licenses.Local_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Applications.Local_Driving_License
{
    public partial class ctrlDrivingLicenseApplication : UserControl
    {
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicneseApplicationID = -1;
        private int _LicenseID = -1;
        public int LocalDrivingLicneseApplicationID
        {
            get { return _LocalDrivingLicneseApplicationID; }
        }
        public ctrlDrivingLicenseApplication()
        {
            InitializeComponent();
        }

        public void LoadApplicationInofByLocalDrivingAppID(int LocalDrivingAppID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingAppID);
            if(_LocalDrivingLicenseApplication==null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();
                MessageBox.Show("No Application With ID = "+ LocalDrivingAppID.ToString(),"Error",MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            _LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();
            _LocalDrivingLicneseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _FillLocalDrivingLicenseApplicationInfo();
        }
        public void LoadApplicationInofByApplicationID(int ApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByApplicationID(ApplicationID);
            if (_LocalDrivingLicenseApplication == null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();
                MessageBox.Show("No Application With ID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            _LicenseID= _LocalDrivingLicenseApplication.GetActiveLicenseID();
            _LocalDrivingLicneseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _FillLocalDrivingLicenseApplicationInfo();
        }

        private void _ResetLocalDrivingLicenseApplicationInfo()
        {
            lblDLAppID.Text = "[????]";
            lblAppliedForLicense.Text = "[????]";
            lblPassedTests.Text = "0/3";
          
        }
        private void _FillLocalDrivingLicenseApplicationInfo()
        {
            llShowLicenseInfo.Enabled = (_LicenseID != -1);
            lblDLAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblAppliedForLicense.Text = clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName;
            lblPassedTests.Text = _LocalDrivingLicenseApplication.GetPassedTestCount().ToString() + "/3";
            ctrlApplicationBasicInfo1.LoadApplicationInfo(_LocalDrivingLicenseApplication.ApplicationID);
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }
    }
}
