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

namespace DVLD1.People.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public event Action<int> OnPersonSelected;
        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;
            if(handler != null)
            {
                handler(PersonID);
            }
        }

        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get { return _ShowAddPerson; }
            set 
            { 
                _ShowAddPerson = value;
                btnAddPerson.Visible = _ShowAddPerson;
            }
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get { return _FilterEnabled; }
            set
            {
                _FilterEnabled = value;
                groupBox1.Enabled = _FilterEnabled;
            }
        }

        private int _PersonID = -1;
        public int PersonID
        {
            get { return ctrlPersonCard1.PersonID; }
        }

        public clsPerson SelectedPersonInfo
        {
            get { return ctrlPersonCard1.SelectedPersonInfo; }
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        private void _FindNow()
        {

            switch (cbFindPerson.Text)
            {
                case "Person ID.":
                    {
                        ctrlPersonCard1.LoadPersonInfo(int.Parse(txtFindPerson.Text));
                        break;
                    }
                case "National No.":
                    {
                        ctrlPersonCard1.LoadPersonInfo(txtFindPerson.Text);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if(OnPersonSelected!=null && FilterEnabled)
            {
                OnPersonSelected(ctrlPersonCard1.PersonID);
            }
        }
        public void LoadPersonInfo(int PersonID)
        {
            cbFindPerson.SelectedIndex = 1;
            txtFindPerson.Text=PersonID.ToString();
            _FindNow();
        }

        private void cbFindPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFindPerson.Text = "";
            txtFindPerson.Focus();
        }

        private void btnSearchOnPerson_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Field are no valide , put the mouse over the red icon to show the error",
                    "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FindNow();
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFindPerson.SelectedIndex = 0;
            txtFindPerson.Focus();
        }

        private void txtFindPerson_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFindPerson.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFindPerson, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(txtFindPerson, null);
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.DataBack += DataBackEvent;
            frm.ShowDialog();
        }

        private void DataBackEvent(object sender, int PersonID)
        {
            cbFindPerson.SelectedIndex = 1;
            txtFindPerson.Text=PersonID.ToString();
            ctrlPersonCard1.LoadPersonInfo(PersonID);
        }

        public void FilterFocus()
        {
            txtFindPerson.Focus();
        }

        private void txtFindPerson_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==(char)13)
            {
                btnSearchOnPerson.PerformClick();
            }

            if(cbFindPerson.Text=="Person ID.")
            {
                e.Handled = ( !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) );
            }
        }
    }
}
