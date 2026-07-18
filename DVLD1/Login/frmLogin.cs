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
using WorkSphere.Global_Classes;

namespace DVLD1.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser user=clsUser.FindByUserName(txtUserName.Text.Trim());

            if (user != null && clsSecurity.Verify(txtPassword.Text.Trim() , user.Password))
            {
                if(chkRemeberMe.Checked)
                {
                    clsGlobal.RememberUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                }
                else
                {
                    clsGlobal.DeleteCredentialsFromRegistry();
                }

                if(!user.IsActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("Your Account is not Active, Contact Admin", "In Active",MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                clsGlobal.CurrentUser = user;

                frmMain frm = new frmMain(this);
                frm.ShowDialog();
            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credential", 
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string Username = "", Password = "";

            if (clsGlobal.GetStoredCredential(ref Username, ref Password))
            {
                txtUserName.Text = Username;
                txtPassword.Text = Password;
                chkRemeberMe.Checked = true;
            }
            else
            {
                chkRemeberMe.Checked = false;
            }
        }

       
    }
}
