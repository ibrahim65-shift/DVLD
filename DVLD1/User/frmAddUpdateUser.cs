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
using WorkSphere.Global_Classes;

namespace DVLD1.User
{
  
    public partial class frmAddUpdateUser : Form
    {
        enum enMode { AddNew=0,Update=1};

        private enMode _Mode;
        private clsUser _User;
        private int _UserID=-1;
        public frmAddUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
            _Mode = enMode.Update;
        }
       
        private void _ResetDefaultValues()
        {
            if(_Mode == enMode.AddNew)
            {
                lblMode.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUser();
                tpLoginInfo.Enabled = false;
                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblMode.Text = "Update User";
                this.Text = "Update User";
                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }

            lblUserID.Text = "[????]";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            checkBox1.Checked = true;
        }

        private void _LoadData()
        {
            _User = clsUser.FindByUserID(_UserID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            if (_User == null)
            {
                MessageBox.Show("No User with id = " + _UserID, "NOT FOUND", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                this.Close();
                return;
            }

            
            lblUserID.Text=_User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            checkBox1.Checked = _User.IsActive;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);
        }
        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
          
            _ResetDefaultValues();
            if( _Mode == enMode.Update)
            {
                _LoadData();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            
            if(_Mode== enMode.Update)
            {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                tabControl1.SelectedTab = tpLoginInfo;
                return;
            }

            if(ctrlPersonCardWithFilter1.PersonID!=-1)
            {
                if(clsUser.isUserExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person Already has a user, choose another one", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                   
                }
                else
                {
                   
                    btnSave.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tabControl1.SelectedTab = tpLoginInfo;
                    
                }
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Error",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("some fileds are not valide!, put the mouse over the red icon to show the error",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _User.PersonID= ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text.Trim();

            if (!string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                string salt = clsSecurity.GenerateSalt();
                string hash = clsSecurity.HashPassword(txtPassword.Text.Trim(), salt);
                _User.Password = clsSecurity.Pack(salt, hash);
            }
            _User.IsActive = checkBox1.Checked;

            if(_User.Save())
            {
                ctrlPersonCardWithFilter1.FilterEnabled = false;
                lblUserID.Text = _User.UserID.ToString();
                _Mode = enMode.Update;
                lblMode.Text = "Update User";
                this.Text = "Update User";
                MessageBox.Show("Data Saved Successfullly", "Saved", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error : DATA IS NOT SAVED SUCCESSFULLY", "Error", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "Username cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtUserName, null);
            }

            if(_Mode==enMode.AddNew)
            {
                if(clsUser.isUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "Username Aready used , choose another one");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                }
            }
            else
            {
                if(_User.UserName != txtUserName.Text.Trim())
                {
                    if (clsUser.isUserExist(txtUserName.Text.Trim()))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "Username Aready used , choose another one");
                    }
                    else
                    {
                        errorProvider1.SetError(txtUserName, null);
                    }
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                }
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Confirm Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }

            if(txtConfirmPassword.Text.Trim()!= txtPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Confirmation Password does not match Passowrd!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }
    }
}
