using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Applications.Local_Driving_License
{
    public partial class frmAddUpdateLocalDrivingLicenseApplication : Form
    {
       public enum enMode {AddNew=0, Update=1};

        private enMode _Mode;
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _SelectedPersonID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicneseApplication;
        public frmAddUpdateLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            _Mode=enMode.AddNew;
        }
        public frmAddUpdateLocalDrivingLicenseApplication(int LocalID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID=LocalID;
            _Mode=enMode.Update;
        }

        private void _FillLicneseClassesInComboBox()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();
            foreach(DataRow row in dtLicenseClasses.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }
        }
        private void _ResetDefaultValues()
        {
            _FillLicneseClassesInComboBox();
            if(_Mode==enMode.AddNew)
            {
                lblMode.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                _LocalDrivingLicneseApplication = new clsLocalDrivingLicenseApplication();
                ctrlPersonCardWithFilter1.FilterFocus();
                tpApplicationInfo.Enabled = false;

                cbLicenseClass.SelectedIndex = 2;
                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense).Fees.ToString();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblCreateBy.Text = clsGlobal.CurrentUser.UserName;
            }
            else
            {
                lblMode.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";
                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }
        }

        private void _LoadData()
        {
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            _LocalDrivingLicneseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);
            if(_LocalDrivingLicneseApplication == null)
            {
                MessageBox.Show("No Application With ID = " + _LocalDrivingLicenseApplicationID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicneseApplication.ApplicantPersonID);
            lblDLApplicationID.Text = _LocalDrivingLicneseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = _LocalDrivingLicneseApplication.ApplicationDate.ToShortDateString();
            lblApplicationFees.Text = _LocalDrivingLicneseApplication.PaidFees.ToString();
            lblCreateBy.Text = clsUser.FindByUserID(_LocalDrivingLicneseApplication.CreatedByUserID).UserName;
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.Find(_LocalDrivingLicneseApplication.LicenseClassID).ClassName);
          
        }
        private void frmAddUpdateLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if(_Mode==enMode.Update)
            {
                _LoadData();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;

            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(ctrlPersonCardWithFilter1.PersonID, clsApplication.enApplicationType.NewDrivingLicense,LicenseClassID);

            if(ActiveApplicationID!=-1)
            {
                MessageBox.Show("Choose another License Class , the selected person already have an Active Application for the selected calss with id = " + ActiveApplicationID.ToString(),"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }

            if(clsLicense.IsLicenseExistByPersonID(ctrlPersonCardWithFilter1.PersonID,LicenseClassID))
            {
                MessageBox.Show("Person Already have a license with same applied driving " + ActiveApplicationID.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

            _LocalDrivingLicneseApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            _LocalDrivingLicneseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicneseApplication.ApplicationTypeID = 1;
            _LocalDrivingLicneseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalDrivingLicneseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicneseApplication.PaidFees = Convert.ToSingle(lblApplicationFees.Text);
            _LocalDrivingLicneseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicneseApplication.LicenseClassID = LicenseClassID;

            if(_LocalDrivingLicneseApplication.Save())
            {
                lblDLApplicationID.Text=_LocalDrivingLicneseApplication.LocalDrivingLicenseApplicationID.ToString();
                _Mode = enMode.Update;
                lblMode.Text = "Update Local Driving License Application";
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Data Is NOT Saved Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID=obj;
        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(_Mode==enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tabControl1.SelectedTab = tpApplicationInfo;
                return;
            }

            if(ctrlPersonCardWithFilter1.PersonID!=-1)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tabControl1.SelectedTab = tpApplicationInfo;
               
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }
    }
}
