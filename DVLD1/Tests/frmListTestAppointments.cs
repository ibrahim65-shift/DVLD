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

namespace DVLD1.Tests
{
    public partial class frmListTestAppointments : Form
    {
        private DataTable _dtLicenseTestAppointments;
        private int _LocalDrivingLicenseApplicationID;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        public frmListTestAppointments(int LocalDrivingAppID,clsTestType.enTestType TestType)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingAppID;
            _TestTypeID = TestType;
        }

        private void _LoadTestTypeImageAndTitle()
        {
            switch (_TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    lblTestType.Text = "Vision Test Appointment";
                    pictureBox1.Image = Resources.Vision_512;
                    break;

                case clsTestType.enTestType.WrittenTest:
                    lblTestType.Text = "Written Test Appointment";
                    pictureBox1.Image = Resources.Written_Test_512;
                    break;

                case clsTestType.enTestType.StreetTest:
                    lblTestType.Text = "Street Test Appointment";
                    pictureBox1.Image = Resources.driving_test_512;
                    break;
            }

        }
        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();

            ctrlDrivingLicenseApplication1.LoadApplicationInofByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
            _dtLicenseTestAppointments = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, _TestTypeID);

            dgvListAppointments.DataSource= _dtLicenseTestAppointments;
            lblRecords.Text = dgvListAppointments.Rows.Count.ToString();

            if(dgvListAppointments.Rows.Count>0)
            {
                dgvListAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvListAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvListAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvListAppointments.Columns[3].HeaderText = "Is Locked";
            }
            dgvListAppointments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);

            if(localDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestTypeID))
            {
                MessageBox.Show("Person Already Have an active appointment for this test", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTest LastTest = localDrivingLicenseApplication.GetLastTestPerTestType(_TestTypeID);
            if (LastTest==null)
            {
                frmScheduleTest frm1 = new frmScheduleTest(_LocalDrivingLicenseApplicationID,_TestTypeID);
                frm1.ShowDialog();
                frmListTestAppointments_Load(null, null);
                return;
            }

            if(LastTest.TestResult==true)
            {
                MessageBox.Show("This Person Already Passed this Test before","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm2 = new frmScheduleTest(LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID,_TestTypeID);
            frm2.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID,
                (int)dgvListAppointments.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTakeTest frm = new frmTakeTest((int)dgvListAppointments.CurrentRow.Cells[0].Value, _TestTypeID);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }
    }
}
