using DVLD_Business;
using DVLD_Presentation.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD_Presentation
{
    public partial class frmAddEditPersonInfo : Form
    {
        
        private enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;
        private int _PersonID;
        private clsPerson _Person;
        public frmAddEditPersonInfo(int PersonID)
        {
            InitializeComponent();

            if (PersonID == -1)
            {
                _Mode = enMode.AddNew;
            }
            else 
            {
                _PersonID = PersonID;
                _Mode = enMode.Update;
            }
        }

        private void _LoadData()
        {
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-80);

            rbMale.Checked = true;

            DataTable dtCountries = clsCountry.GetAllCountries();
            foreach (DataRow row in dtCountries.Rows)
            {
                cbCountry.Items.Add((string)row["CountryName"]);
            }
            cbCountry.SelectedItem = "Lebanon";

            if (_Mode == enMode.AddNew)
            {
                _Person = new clsPerson();
                lblMode.Text = "Add New Person";

                
            }
            else
            {
                _Person = clsPerson.Find(_PersonID);   
                if (_Person == null)
                {
                    MessageBox.Show("No Person With Person ID " + _PersonID);
                    this.Close();
                    return;
                }
                lblMode.Text = "Update Person";

                lblPersonID.Text = _PersonID.ToString();
                txtFirst.Text = _Person.FirstName;
                txtSecond.Text = _Person.SecondName;
                txtThird.Text = _Person.ThirdName;
                txtLast.Text = _Person.LastName;
                txtNationalNo.Text = _Person.NationalNo;

                if (_Person.Gender == 0)
                    rbMale.Checked = true;
                else
                    rbFemale.Checked = true;
                
                txtEmail.Text = _Person.Email;
                txtAddress.Text = _Person.Address;
                dtpDateOfBirth.Value = _Person.DateOfBirth;
                txtPhone.Text = _Person.Phone;
                cbCountry.Text = clsCountry.Find(_Person.NationalityCountryID).CountryName;
                if (!string.IsNullOrEmpty(_Person.ImagePath))
                    pbImage.Image = Image.FromFile(_Person.ImagePath);
            }
        }
        private void frmAddEditPersonInfo_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked)
                pbImage.Image = Resources.person_man__4_;
            else
                pbImage.Image = Resources.person_woman;

        }

        private bool HasValidationErrors()
        {
            foreach (Control control in groupBox1.Controls)
            {
                if (!string.IsNullOrEmpty(errorProvider1.GetError(control)))
                {
                    return true; 
                }
            }
            return false; 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (HasValidationErrors())
            {
                MessageBox.Show("Please correct the errors before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _Person.FirstName = txtFirst.Text;
            _Person.SecondName = txtSecond.Text;
            _Person.ThirdName = txtThird.Text;
            _Person.LastName = txtLast.Text;
            _Person.NationalNo = txtNationalNo.Text;
            _Person.Gender = (short)(rbMale.Checked ? 0 : 1);
            _Person.Email = txtEmail.Text;
            _Person.Address = txtAddress.Text;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.Phone = txtPhone.Text;
            _Person.NationalityCountryID = clsCountry.Find((cbCountry.Text)).CountryID;

            if (_Person.Save())
            {
                if (_Mode == enMode.AddNew)
                {
                    MessageBox.Show("Person Added Successfully");
                    lblMode.Text = "Update Person";
                    lblPersonID.Text = _Person.PersonID.ToString();
                    _Mode = enMode.Update;
                }
                else
                    MessageBox.Show("Person Updated Successfully");

            }
            else
            {
                MessageBox.Show("Person Is Not Updated");
            }
        }

    

        private void btnClose_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void txtBox_Validating(object sender, CancelEventArgs e)
        {

            if (sender is System.Windows.Forms.TextBox textBox)
            {
                
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    
                    errorProvider1.SetError(textBox, "The text box cannot be empty.");
                    //e.Cancel = true; 
                }
                else
                {
                    
                    errorProvider1.SetError(textBox, string.Empty);
                }
            }
            else
            {
                MessageBox.Show("Error In Validating Function For A Non TextBox Control");
            }
        }

        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if ((!IsValidEmail(txtEmail.Text)) || (string.IsNullOrEmpty(txtEmail.Text)))
            {
                errorProvider1.SetError(txtEmail, "Please enter a valid email address.");
            }
            else
            {
                errorProvider1.SetError(txtEmail, string.Empty);
            }
        }

        private void llblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files|*.*";
                openFileDialog.Title = "Select an Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    string fileExtension = Path.GetExtension(selectedFilePath);
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    string targetFilePath = Path.Combine(@"C:\DVLD-People-Images", uniqueFileName);

                    // Create the directory if it doesn't exist
                    Directory.CreateDirectory(@"C:\DVLD-People-Images");

                    // Copy the file to the target directory
                    File.Copy(selectedFilePath, targetFilePath, true);

                    // Display the image in a PictureBox (optional)
                    pbImage.Image = Image.FromFile(targetFilePath);

                    _Person.ImagePath = targetFilePath;
                }
            }
        }

        private void llblRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_Person.ImagePath) && File.Exists(_Person.ImagePath))
            {
                try
                {
                    // Ensure the PictureBox is cleared
                    if (pbImage.Image != null)
                    {
                        pbImage.Image.Dispose(); // Dispose the current image
                        pbImage.Image = null; // Clear the PictureBox
                    }

                    // Remove the image file from the "mypics" directory
                    File.Delete(_Person.ImagePath);

                    // Clear the imageFilePath variable
                    _Person.ImagePath = "";

                    // Optionally set a default image
                    pbImage.Image = Resources.person_man__4_;

                    MessageBox.Show("Image removed successfully.");
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Error accessing or deleting the image file: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No image to remove or image already deleted.");
            }
        }
    }
}

