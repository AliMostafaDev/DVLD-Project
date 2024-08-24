using DVLD_Business;
using DVLD_Presentation.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Presentation
{
    public partial class ctrlPersonCard : UserControl
    {
        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        private void FillFormWithPersonInfo(int PersonID)
        {
            clsPerson Person = clsPerson.Find(PersonID);
            lblPersonID.Text = PersonID.ToString();
            lblName.Text = Person.FirstName + " " + Person.SecondName + " " + Person.ThirdName + " " + Person.LastName;
            lblNationalNo.Text = Person.NationalNo;
            lblGender.Text = Person.Gender == 0 ? "Male" : "Female";
            lblEmail.Text = Person.Email;
            lblAddress.Text = Person.Address;
            lblDateOfBirth.Text = Person.DateOfBirth.ToShortDateString().ToString();
            lblPhone.Text = Person.Phone;
            lblCountry.Text = clsCountry.Find(Person.NationalityCountryID).CountryName;
            if (!string.IsNullOrEmpty(Person.ImagePath))
                pbImage.Load(Person.ImagePath);
        }
        //private void FillFormWithPersonInfo(string NationalNo)
        //{
        //    clsPerson Person = clsPerson.Find(NationalNo);
        //    lblPersonID.Text = Person.PersonID.ToString();
        //    lblName.Text = Person.FirstName + " " + Person.SecondName + " " + Person.ThirdName + " " + Person.LastName;
        //    lblNationalNo.Text = Person.NationalNo;
        //    lblGender.Text = Person.Gender == 0 ? "Male" : "Female";
        //    lblEmail.Text = Person.Email;
        //    lblAddress.Text = Person.Address;
        //    lblDateOfBirth.Text = Person.DateOfBirth.ToShortDateString().ToString();
        //    lblPhone.Text = Person.Phone;
        //    lblCountry.Text = clsCountry.Find(Person.NationalityCountryID).CountryName;
        //    pbImage.Load(@"C:\boy.png");
        //}

        private void FillFormWithDefaultInfo()
        {
            lblPersonID.Text = "[????]";
            lblName.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGender.Text = "[????]";
            lblEmail.Text = "[????]";
            lblAddress.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblPhone.Text = "[????]";
            lblCountry.Text = "[????]";
            pbImage.Image = Resources.person_man__4_;
        }

        public void Load(int PersonID)
        {
            if (clsPerson.IsPersonExist(PersonID))
            {
                FillFormWithPersonInfo(PersonID);
            }
            else
            {
                FillFormWithDefaultInfo();
                MessageBox.Show("No Person With PersonID = " + PersonID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Load(string NationalNo)
        {
            if (clsPerson.IsPersonExist(NationalNo))
            {
                FillFormWithPersonInfo(clsPerson.Find(NationalNo).PersonID);
            }
            else
            {
                FillFormWithDefaultInfo();
                MessageBox.Show("No Person With National-No = " + NationalNo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
