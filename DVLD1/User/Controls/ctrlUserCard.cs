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

namespace DVLD1.User.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        private clsUser _User;
        private int _UserID = -1;

        public int UserID
        {  get { return _UserID; } }

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(int UserID)
        {
            _User= clsUser.FindByUserID(UserID);
            if(_User==null)
            {
                _ResetPersonInfo();
                MessageBox.Show("User with ID = "+UserID.ToString(),"NOT FOUND", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            _UserID = _User.UserID;
            _FillUserInfo();
        }

        private void _FillUserInfo()
        {
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName;
            lblIsActive.Text = _User.IsActive == true ? "YES" : "NO";
            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
        }
  
        private void _ResetPersonInfo()
        {
            ctrlPersonCard1._ResetPersonInfo();        
        }
    }
}
