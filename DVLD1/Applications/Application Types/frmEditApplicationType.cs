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

namespace DVLD1.Applications.Application_Types
{
    public partial class frmEditApplicationType : Form
    {
        private clsApplicationType _ApplicationType;
        private int _ApplicationTypeID;
        public frmEditApplicationType(int applicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = applicationTypeID;
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            lblID.Text = _ApplicationTypeID.ToString();
            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);
            if (_ApplicationType != null)
            { 
                txtFees.Text = _ApplicationType.Fees.ToString();
                txtTitle.Text = _ApplicationType.Title;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valide!, put the mouse over the red Icon(s).",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _ApplicationType.Fees=Convert.ToSingle(txtFees.Text.Trim());
            _ApplicationType.Title= txtTitle.Text.Trim();

            if(_ApplicationType.Save())
            {
                MessageBox.Show("Application Type Updated Successfully.", "Update", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Application Type Updated Faild.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title cannot be empty!");
            }
            else
            {
                errorProvider1.SetError(txtTitle, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees cannot be empty!");
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }

            if(!clsValidatoin.IsNumber(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid Number!");
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }

        }
    }
}
