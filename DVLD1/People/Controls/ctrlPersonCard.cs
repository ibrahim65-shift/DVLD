using DVLD.Classes;
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
using System.IO;

namespace DVLD1.People.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private clsPerson _Person;
        private int _PersonID;
        public int PersonID
        {
            get { return _PersonID; }
        }

        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

       
        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            if(_Person==null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No Person with ID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _PersonID = _Person.PersonID; 
            _FillPersonInfo();
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);
            if (_Person == null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No Person with National = " + NationalNo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _PersonID = _Person.PersonID;
            _FillPersonInfo();
        }

        public void _ResetPersonInfo()
        {
            llEditPerson.Enabled = false;
            lblPersonID.Text = "[????]";
            lblName.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGender.Text = "[????]";
            lblEmail.Text = "[????]";
            lblAddress.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblPhone.Text = "[????]";
            lblCountry.Text = "[????]";
            pictureBox1.Image = Resources.Male_512;
        }
        private void _LoadPersonImage()
        {
            if (_Person.Gendor == 0)
                lblPersonImage.Image = Resources.Man_32;
            else
                lblPersonImage.Image = Resources.Woman_32;

            string ImagePath = _Person.ImagePath;
            if(ImagePath!="")
            {
                if(File.Exists(ImagePath))
                {
                    pictureBox1.ImageLocation= ImagePath;
                }
                else
                {
                    MessageBox.Show("Coulde Not Finde this Image = " + ImagePath,"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
        private void _FillPersonInfo()
        {
            llEditPerson.Enabled = true;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblName.Text = _Person.FullName;
            lblNationalNo.Text = _Person.NationalNo;
            lblGender.Text = _Person.Gendor == 0 ? "Male" : "Female";
            lblEmail.Text=_Person.Email;
            lblAddress.Text= _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblPhone.Text = _Person.Phone;
            lblCountry.Text = clsCountry.Find(_Person.NationalityCountryID).CountryName;
            _LoadPersonImage();
        }

        private void llEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonID);
            frm.ShowDialog();

            LoadPersonInfo(_PersonID);
        }
    }
}
