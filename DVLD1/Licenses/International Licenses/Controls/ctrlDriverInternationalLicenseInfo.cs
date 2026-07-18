using DVLD_Buisness;
using DVLD1.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Licenses.International_Licenses.Controls
{
    public partial class ctrlDriverInternationalLicenseInfo : UserControl
    {
        private int _InternationalLicenseID = -1;
        private clsInternationalLicense _InternationalLicense;

        public int InternationalLicenseID
        {
            get { return _InternationalLicenseID; }
        }
        public ctrlDriverInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        private void _LoadImage()
        {
            if (_InternationalLicense.DriverInfo.PersonInfo.Gendor == 0)
            {
                lblImageGender.Image = Resources.Man_32;
                pictureBox1.Image = Resources.Male_512;
            }
            else
            {
                lblImageGender.Image = Resources.Woman_32;
                pictureBox1.Image = Resources.Female_512;
            }

            string ImagePath = _InternationalLicense.DriverInfo.PersonInfo.ImagePath;

            if(ImagePath!="")
            {
                if(File.Exists(ImagePath))
                {
                    pictureBox1.ImageLocation = ImagePath;
                }
                else
                {
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void _LoadInternationalLicenseInfo()
        {
            lblName.Text = _InternationalLicense.DriverInfo.PersonInfo.FullName;
            lblIntLicenseID.Text=_InternationalLicense.InternationalLicenseID.ToString();
            lblLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblNationalNo.Text = _InternationalLicense.DriverInfo.PersonInfo.NationalNo;
            lblGender.Text = (_InternationalLicense.DriverInfo.PersonInfo.Gendor == 0) ? "Male" : "Female";
            lblIssueDate.Text=_InternationalLicense.IssueDate.ToShortDateString();
            lblApplicationID.Text=_InternationalLicense.ApplicationID.ToString();
            lblIsActive.Text = (_InternationalLicense.IsActive) ? "Yes" : "No";
            lblDateOfBirth.Text = _InternationalLicense.DriverInfo.PersonInfo.DateOfBirth.ToShortDateString();
            lblDriverID.Text=_InternationalLicense.DriverID.ToString();
            lblExpirationDate.Text = _InternationalLicense.ExpirationDate.ToShortDateString();
            _LoadImage();
        }

        private void _ResetDefaultValues()
        {
            lblName.Text = "[????]";
            lblIntLicenseID.Text = "[????]";
            lblLicenseID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGender.Text = "[????]";
            lblIssueDate.Text = "[????]";
            lblApplicationID.Text = "[????]";
            lblIsActive.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblDriverID.Text = "[????]";
            lblExpirationDate.Text = "[????]";
        }
        public void LoadInfo(int InterLicenseID)
        {
            _InternationalLicense = clsInternationalLicense.Find(InterLicenseID);
            if(_InternationalLicense==null)
            {
                _ResetDefaultValues();
                MessageBox.Show("No International License With ID = " + InterLicenseID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _InternationalLicenseID= InterLicenseID;
            _LoadInternationalLicenseInfo();
        }
    }
}
