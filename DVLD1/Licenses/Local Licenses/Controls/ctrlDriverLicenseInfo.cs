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

namespace DVLD1.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private clsLicense _License;
        private int _LicenseID;
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        public int LicenseID
        { get { return _LicenseID; }}

        public clsLicense SelectedLicenseInfo
            { get { return _License; }}

        private void _LoadPersonImage()
        {
            if (_License.DriverInfo.PersonInfo.Gendor == 0)
                lblGenderImage.Image = Resources.Man_32;
            else
                lblGenderImage.Image = Resources.Woman_32;

            string ImagePath = _License.DriverInfo.PersonInfo.ImagePath;

            if(ImagePath !="")
            {
                if(File.Exists(ImagePath))
                {
                    pictureBox1.ImageLocation = ImagePath;
                }
                else
                {
                    MessageBox.Show("Could Not Found this Image = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void _FillLicenseInfo()
        {
            lblClass.Text = _License.LicenseClassIfo.ClassName;
            lblName.Text = _License.DriverInfo.PersonInfo.FullName;
            lblLicenseID.Text = _License.LicenseID.ToString();
            lblNationalNo.Text = _License.DriverInfo.PersonInfo.NationalNo;
            lblGender.Text = _License.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
            lblIssueDate.Text = _License.IssueDate.ToShortDateString();
            lblIssueReason.Text = _License.IssueReasonText;
            lblNotes.Text = _License.Notes == "" ? "No Notes" : _License.Notes;
            lblIsActive.Text = _License.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = _License.DriverInfo.PersonInfo.DateOfBirth.ToShortDateString();
            lblDriverID.Text = _License.DriverID.ToString();
            lblExpirationDate.Text = _License.ExpirationDate.ToShortDateString();
            lblIsDetained.Text = _License.IsDetained ? "Yes" : "No";
            _LoadPersonImage();
        }
        public void LoadInfo(int LicenseID)
        {
            _LicenseID = LicenseID;
            _License = clsLicense.Find(_LicenseID);
            if(_License == null)
            {
                MessageBox.Show("Could Not Find License with ID = " + _LicenseID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID = -1;
                return;
            }

            _FillLicenseInfo();
        }
    }
}
