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

namespace DVLD1.User
{
    public partial class frmListUsers : Form
    {
        private DataTable _dtAllUsers;
        public frmListUsers()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _dtAllUsers=clsUser.GetAllUsers();
            dgvListUsers.DataSource = _dtAllUsers;
            cbFindBy.SelectedIndex = 0;
            lblRecord.Text=dgvListUsers.Rows.Count.ToString();

            if (dgvListUsers.Rows.Count > 0)
            {
                dgvListUsers.Columns[0].HeaderText = "User ID";
                dgvListUsers.Columns[1].HeaderText = "Person ID";
                dgvListUsers.Columns[2].HeaderText = "Full Name";
                dgvListUsers.Columns[3].HeaderText = "UserName";
                dgvListUsers.Columns[4].HeaderText = "Is Active";
            }
            dgvListUsers.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void cbFindBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFindBy.Text=="Is Active")
            {
                txtFilter.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
                cbIsActive.Focus();
            }
            else
            {
                txtFilter.Visible = (cbFindBy.Text != "None");
                cbIsActive.Visible = false;
                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFindBy.Text=="User ID" || cbFindBy.Text=="Person ID")
            {
                e.Handled = (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar));
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cbFindBy.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "UserName":
                    FilterColumn = "UserName";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }


            if(txtFilter.Text.Trim()==""||FilterColumn=="None")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecord.Text = dgvListUsers.Rows.Count.ToString();
                return;
            }

            if(FilterColumn!="FullName" && FilterColumn!="UserName")
            {
                _dtAllUsers.DefaultView.RowFilter=
                    string.Format("[{0}] = {1}",FilterColumn,txtFilter.Text.Trim());
            }
            else
            {
                _dtAllUsers.DefaultView.RowFilter = 
                    string.Format("[{0}] Like '{1}%'", FilterColumn,txtFilter.Text.Trim());
            }

            lblRecord.Text = dgvListUsers.Rows.Count.ToString();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsActive.Text;

            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }

            if(FilterValue=="All")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecord.Text = dgvListUsers.Rows.Count.ToString(); 
                return;
            }
            else
            {
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}]={1}",FilterColumn,FilterValue);
            }

            lblRecord.Text = dgvListUsers.Rows.Count.ToString();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo((int)dgvListUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
            frmListUsers_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser((int)dgvListUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListUsers_Load(null, null);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this user  with id ["
                + dgvListUsers.CurrentRow.Cells[0].Value.ToString()+ "] ? ","Delete",MessageBoxButtons.YesNo,
                MessageBoxIcon.Information)==DialogResult.Yes)
            {
                if(clsUser.DeleteUser((int)dgvListUsers.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("User has been deleted successfully", "Delete", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    frmListUsers_Load(null, null);
                }
                else
                {
                    MessageBox.Show("User is not deleted due to data connected to it ", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword((int)dgvListUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
            frmListUsers_Load(null, null);
        }
    }
}
