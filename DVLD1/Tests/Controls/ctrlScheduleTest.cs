using DVLD.Classes;
using DVLD_Buisness;
using DVLD1.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Tests.Controls
{
    public partial class ctrlScheduleTest : UserControl
    {
        public enum enMode { AddNew=0,Update=1};
        private enMode _Mode=enMode.AddNew;

        public enum enCreationMode { FirstTimeSchedule=0, RetakeTestSchedule=1};
        private enCreationMode _CreationMode=enCreationMode.FirstTimeSchedule;

        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID = -1;

        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID = -1;

       
        public clsTestType.enTestType TestTypeID
        {
            get { return _TestTypeID; }

            set
            {
                _TestTypeID = value;

                switch (_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        gbTestType.Text = "Vision Test";
                        pictureBox1.Image = Resources.Vision_512;
                        break;

                    case clsTestType.enTestType.WrittenTest:
                        gbTestType.Text = "Written Test";
                        pictureBox1.Image = Resources.Written_Test_512;
                        break;

                    case clsTestType.enTestType.StreetTest:
                        gbTestType.Text = "Street Test";
                        pictureBox1.Image = Resources.driving_test_512;
                        break;
                }

            }
        }
        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);
            if (_TestAppointment == null)
            {
                MessageBox.Show("Error : NO Appointment with id = " + _TestAppointmentID.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lblFees.Text= clsTestType.Find(_TestTypeID).Fees.ToString();

            dateTimePicker1.Value = _TestAppointment.AppointmentDate;

            dateTimePicker1.MinDate= DateTime.Now;

            if(_TestAppointment.RetakeTestApplicationID==-1)
            {
                lblRTestAppID.Text = "N/A";
                lblRAppFees.Text = "0";
            }
            else
            {
                lblRAppFees.Text = _TestAppointment.RetakeTestAppInfo.PaidFees.ToString();
                gbRetakeTest.Enabled = true;
                lblRTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
                lblModeTest.Text = "Schedule Retake Test";
            }

            return true;
        }
        private bool _HandleActiveTestAppointmentConstraint()
        {
            if(_Mode==enMode.AddNew && clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID,_TestTypeID))
            {
                lblUserMessage.Text = "Person Already have an active appointment for this test";
                btnSave.Enabled = false;
                dateTimePicker1.Enabled = false;
                return false;
            }
            return true;
        }
        private bool _HandleAppointmentLockedConstraint()
        {
            if(_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person Already Sat for this test, appointment Locked";
                btnSave.Enabled = false;
                dateTimePicker1.Enabled=false;
                return false;
            }
            else
            {
                lblUserMessage.Visible = false;
            }
                return true;
        }
        private bool _HandlePreviousTestConstraint()
        {
            switch (TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    lblUserMessage.Visible = false;
                    return true;
                   

                case clsTestType.enTestType.WrittenTest:
                     if(!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest))
                    {
                        lblUserMessage.Visible = true;
                        lblUserMessage.Text = "Cannot Schedule , Vision Test Should be Passed First";
                        btnSave.Enabled=false;
                        dateTimePicker1.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dateTimePicker1.Enabled = true;
                    }
                    return true;

                case clsTestType.enTestType.StreetTest:
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                    {
                        lblUserMessage.Visible = true;
                        lblUserMessage.Text = "Cannot Schedule , Written Test Should be Passed First";
                        btnSave.Enabled = false;
                        dateTimePicker1.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dateTimePicker1.Enabled = true;
                    }
                    return true;

            }

            return true;
        }
        private bool _HandleRetakeTestApplication()
        {
            if(_Mode==enMode.AddNew && _CreationMode==enCreationMode.RetakeTestSchedule)
            {
                clsApplication Application  = new clsApplication();

                Application.ApplicantPersonID = _LocalDrivingLicenseApplication.ApplicantPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees;
                Application.CreatedByUserID=clsGlobal.CurrentUser.UserID;

                if(!Application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                _TestAppointment.RetakeTestApplicationID = Application.ApplicationID;
            }
            return true;
        }
        public void LoadInfo(int LocalDrivingLicenseAppID,int AppointmentID=-1)
        {
            if (AppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _LocalDrivingLicenseApplicationID= LocalDrivingLicenseAppID;
            _TestAppointmentID= AppointmentID;

            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);
            if(_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error : No Local Driving License Application With ID = " + LocalDrivingLicenseAppID.ToString(),
                    "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            if (_LocalDrivingLicenseApplication.DoesAttendTestType(_TestTypeID))
                _CreationMode = enCreationMode.RetakeTestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;


            if(_CreationMode==enCreationMode.RetakeTestSchedule)
            {
                lblRAppFees.Text=clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees.ToString();
                gbRetakeTest.Enabled = true;
                lblModeTest.Text = "Schedule Retake Test";
                lblRTestAppID.Text = "0";
            }
            else
            {
                lblRAppFees.Text = "0";
                gbRetakeTest.Enabled = false;
                lblModeTest.Text = "Schedule Test";
                lblRTestAppID.Text = "N/A";
            }

            lblDLAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblName.Text = _LocalDrivingLicenseApplication.PersonFullName;

            lblTrial.Text = _LocalDrivingLicenseApplication.TotalTrialsPerTest(_TestTypeID).ToString();

            if(_Mode==enMode.AddNew)
            {
                lblFees.Text=clsTestType.Find(_TestTypeID).Fees.ToString();
                dateTimePicker1.MinDate = DateTime.Now;
                lblRTestAppID.Text = "N/A";

                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppointmentData())
                    return;
            }

            lblTotalFees.Text = ( Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRAppFees.Text) ).ToString();

            if (!_HandleActiveTestAppointmentConstraint())
                return;
            if (!_HandleAppointmentLockedConstraint())
                return;
            if (!_HandlePreviousTestConstraint())
                return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeTestApplication())
                return;

            _TestAppointment.AppointmentDate = dateTimePicker1.Value;
            _TestAppointment.PaidFees=Convert.ToSingle(lblTotalFees.Text);
            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplicationID;
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if(_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Error : Data Is Not Saved Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }
    }
}
