using DVLD.Classes;
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

namespace DVLD1.Licenses.Detain_Licenses
{
    public partial class frmDetainedLicneseApplication : Form
    {
        private int _DetainID = -1;
        private int _SelectedLicenseID = -1;
        public frmDetainedLicneseApplication()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar));
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
             _SelectedLicenseID = obj;

            if (_SelectedLicenseID == -1)
            {
                return;
            }

            if(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected License Already Detained, Choose another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtFees.Focus();
            lblLicenseID.Text = _SelectedLicenseID.ToString();
            btnDetain.Enabled = true;
            llLicenseHistory.Enabled = true;
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }
        }

        private void frmDetainedLicneseApplication_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
        }
        private void btnDetain_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valide, put the mouse over the red Icon(s).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(MessageBox.Show("Are you sure You want to Detain this License ? ", "Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.No)
            {
                return;
            }

            _DetainID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Detain(Convert.ToSingle(txtFees.Text.Trim()), clsGlobal.CurrentUser.UserID);

            if(_DetainID != -1)
            {
                lblDetainID.Text = _DetainID.ToString();
                MessageBox.Show("Licesne Detained Successfully With ID = "+_DetainID.ToString(),"Detain",MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnDetain.Enabled = false;
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                txtFees.Enabled = false;
                llShowNewLicenseInfo.Enabled = true;
            }
            else
            {
                MessageBox.Show("Licesne Detained Faild", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void llLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicensesHistory frm= new frmShowPersonLicensesHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(ctrlDriverLicenseInfoWithFilter1.LicenseID);
            frm.ShowDialog();
        }
    }
}
