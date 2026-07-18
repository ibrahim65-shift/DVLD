using DVLD.Classes;
using DVLD_Buisness;
using DVLD1.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.People
{
    public partial class frmAddUpdatePerson : Form
    {
        public delegate void DataBackEventHandler(object sender, int PersonID);
        public event DataBackEventHandler DataBack;

        enum enMode { AddNew=0,Update=1};
        enum enGender { Male=0,Female=1};

        private enMode _Mode;
        private int _PersonID=-1;
        private clsPerson _Person;
        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdatePerson(int personID)
        {
            InitializeComponent();

            _Mode = enMode.Update;
            _PersonID = personID;
        }

        private void _FillCountriesInCombobox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();
            foreach(DataRow row in dtCountries.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
        }

        private void _ResetDefualtValues()
        {
            _FillCountriesInCombobox();

            if(_Mode == enMode.AddNew)
            {
                lblMode.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else
            {
                lblMode.Text = "Update Person";
            }

            if(rbMale.Checked)
            {
                pictureBox1.Image = Resources.Male_512;
            }
            else
            {
                pictureBox1.Image = Resources.Female_512;
            }

            llRemove.Visible = (pictureBox1.ImageLocation != null);

            dateTimePicker1.MaxDate = DateTime.Now.AddYears(-18);
            dateTimePicker1.Value = dateTimePicker1.MaxDate;
            dateTimePicker1.MinDate = DateTime.Now.AddYears(-100);

            cbCountry.SelectedIndex = cbCountry.FindString("Jordan");


            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            txtPhone.Text = "";
            rbMale.Checked = true;
            txtEmail.Text = "";
            txtAddress.Text = "";

        }

        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);

            if(_Person==null)
            {
                MessageBox.Show("No Person with id = " + _PersonID, "NOT FOUND", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblPersonID.Text= _PersonID.ToString();
            txtFirstName.Text= _Person.FirstName;
            txtSecondName.Text= _Person.SecondName;
            txtThirdName.Text= _Person.ThirdName;
            txtLastName.Text= _Person.LastName;
            txtNationalNo.Text= _Person.NationalNo;
            txtPhone.Text= _Person.Phone;
            txtEmail.Text= _Person.Email;
            if(_Person.Gendor==0)
            {
                rbMale.Checked = true;
            }
            else
            {
                rbFemale.Checked = true;
            }
            dateTimePicker1.Value = _Person.DateOfBirth;
            txtAddress.Text= _Person.Address;
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);

            if(_Person.ImagePath!="")
            {
                pictureBox1.ImageLocation = _Person.ImagePath;
            }

            llRemove.Visible = (_Person.ImagePath != "");


        }
        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if(_Mode==enMode.Update)
            {
                _LoadData();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("some fileds are not valide!, put the mouse over the red icon to show the error",
                    "Error" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!HadlePersonImage())
            {
                return;
            }

            int NationalityCountryID = clsCountry.Find(cbCountry.Text).ID;

            _Person.FirstName= txtFirstName.Text.Trim();
            _Person.SecondName=txtSecondName.Text.Trim();
            _Person.ThirdName=txtThirdName.Text.Trim();
            _Person.LastName= txtLastName.Text.Trim();
            _Person.Phone= txtPhone.Text.Trim();
            _Person.NationalNo= txtNationalNo.Text.Trim();
            _Person.DateOfBirth = dateTimePicker1.Value;
            _Person.Address= txtAddress.Text.Trim();
            _Person.Email= txtEmail.Text.Trim();

            if (rbMale.Checked)
                _Person.Gendor = (short)enGender.Male;
            else
                _Person.Gendor = (short)enGender.Female;

            _Person.NationalityCountryID= NationalityCountryID;

            if (pictureBox1.ImageLocation != null)
                _Person.ImagePath = pictureBox1.ImageLocation;
            else
                _Person.ImagePath = "";

            if(_Person.Save())
            {
                lblPersonID.Text = _Person.PersonID.ToString();
                _Mode = enMode.Update;
                lblMode.Text = "Update Person";

                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
            {
                MessageBox.Show("Error: Data Is Not Saved Successfully", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox temp = ((TextBox)sender);
            if(string.IsNullOrEmpty(temp.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(temp, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
                return;

            if(!clsValidatoin.ValidateEmail(txtEmail.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
            else
            {
                errorProvider1.SetError(txtEmail,null);
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            if(txtNationalNo.Text.Trim()!=_Person.NationalNo && clsPerson.isPersonExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National No Number is used by another person , choose another one");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (pictureBox1.ImageLocation == null)
                pictureBox1.Image = Resources.Male_512;
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (pictureBox1.ImageLocation == null)
                pictureBox1.Image = Resources.Female_512;
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Images Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp;";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                pictureBox1.Load(selectedFilePath);
                llRemove.Visible = true;
            }
        }

        private void llRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pictureBox1.ImageLocation=null;

            if(rbMale.Checked)
                pictureBox1.Image=Resources.Male_512;
            else
                pictureBox1.Image= Resources.Female_512;

            llRemove.Visible = false;
        }

        private bool HadlePersonImage()
        {
            if(_Person.ImagePath!=pictureBox1.ImageLocation)
            {
                if(_Person.ImagePath!="")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch(IOException iox)
                    {
                        clsUtil.WriteToEventLog(iox.Message, EventLogEntryType.Error);
                        MessageBox.Show($"Error : {iox.Message}");
                    }
                }
            }

            if(pictureBox1.ImageLocation!=null)
            {
                string sourceFileName = pictureBox1.ImageLocation.ToString();

                if(clsUtil.CopyImageToProjectImagesFolder(ref sourceFileName))
                {
                    pictureBox1.ImageLocation = sourceFileName;
                    return true;
                }
                else
                {
                    MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
