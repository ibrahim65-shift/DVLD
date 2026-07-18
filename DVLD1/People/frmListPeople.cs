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

namespace DVLD1.People
{
    public partial class frmListPeople : Form
    {
        private static DataTable _dtAllPeople = clsPerson.GetAllPeople();
        private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");
        public frmListPeople()
        {
            InitializeComponent();
        }

        private void _RefreshPeoplList()
        {
            _dtAllPeople = clsPerson.GetAllPeople();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");

            dgvListPeople.DataSource = _dtPeople;
            lblRecords.Text = dgvListPeople.Rows.Count.ToString();
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            dgvListPeople.DataSource = _dtPeople;
            cbFindBy.SelectedIndex = 0;

            lblRecords.Text=dgvListPeople.Rows.Count.ToString();  
            
            if(dgvListPeople.Rows.Count > 0)
            {
                dgvListPeople.Columns[0].HeaderText = "Person ID";
                dgvListPeople.Columns[1].HeaderText = "National No.";
                dgvListPeople.Columns[2].HeaderText = "First Name";
                dgvListPeople.Columns[3].HeaderText = "Second Name";
                dgvListPeople.Columns[4].HeaderText = "Third Name";
                dgvListPeople.Columns[5].HeaderText = "Last Name";
                dgvListPeople.Columns[6].HeaderText = "Gender";
                dgvListPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvListPeople.Columns[8].HeaderText = "Country";
                dgvListPeople.Columns[9].HeaderText = "Phone";
                dgvListPeople.Columns[10].HeaderText = "Email";

                dgvListPeople.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo((int)dgvListPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            _RefreshPeoplList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson((int)dgvListPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshPeoplList();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            _RefreshPeoplList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete person [" + dgvListPeople.CurrentRow.Cells[0].Value.ToString() + "] ?",
                "Delete",MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if(clsPerson.DeletePerson((int)dgvListPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person Deleted Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshPeoplList();
                }
                else
                {
                    MessageBox.Show("Person was not deleted because it has data linked to it.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFindBy.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "National No.":
                    FilterColumn = "NationalNo";
                    break;
                case "First Name":
                    FilterColumn = "FirstName";
                    break;
                case "Second Name":
                    FilterColumn = "SecondName";
                    break;
                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;
                case "Last Name":
                    FilterColumn = "LastName";
                    break;
                case "Nationality":
                    FilterColumn = "CountryName";
                    break;
                case "Gender":
                    FilterColumn = "GendorCaption";
                    break;
                case "Phone":
                    FilterColumn = "Phone";
                    break;
                case "Email":
                    FilterColumn = "Email";
                    break;
                default:
                    FilterColumn = "None";
                    break;

            }

            if(txtFilter.Text.Trim()=="" || FilterColumn=="None")
            {
                _dtPeople.DefaultView.RowFilter = "";
                lblRecords.Text=dgvListPeople.Rows.Count.ToString();
                return;
            }

            if(FilterColumn=="PersonID")
            {
                _dtPeople.DefaultView.RowFilter=string.Format("[{0}]={1}",FilterColumn,txtFilter.Text.Trim());
            }
            else
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            }

            lblRecords.Text = dgvListPeople.Rows.Count.ToString();
        }

        private void cbFindBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cbFindBy.Text != "None");
            if(txtFilter.Visible)
            {
                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFindBy.Text == "Person ID")
                e.Handled = (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar));
        }
    }
}
