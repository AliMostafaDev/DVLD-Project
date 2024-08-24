using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business;

namespace DVLD_Presentation
{
    public partial class frmListPeople : Form
    {
        public frmListPeople()
        {
            InitializeComponent();
        }

        private DataTable _GetDataTable()
        {
            DataTable dt = clsPerson.GetAllPeople();
            dt.Columns.Add("GenderDescription", typeof(string));
            dt.Columns.Add("Nationality", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                row["GenderDescription"] = row["Gender"].ToString() == "0" ? "Male" : "Female";
                row["Nationality"] = clsCountry.Find((int)row["NationalityCountryID"]).CountryName;
            }
            dt.Columns.Remove("NationalityCountryID");
            dt.Columns.Remove("Gender");
            dt.Columns.Remove("ImagePath");
            dt.Columns.Remove("Address");

            dt.Columns["GenderDescription"].SetOrdinal(6);
            dt.Columns["Nationality"].SetOrdinal(8);
            dt.Columns["GenderDescription"].ColumnName = "Gender";
            

            return dt;
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            DataTable dt = _GetDataTable();
            dgvListPeople.DataSource = dt;
            lblRecords.Text = dt.Rows.Count.ToString();

            cbFilterBy.SelectedIndex = 0;
            
            
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 0)
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = _GetDataTable();
            DataView dv = dt.DefaultView;

            if (cbFilterBy.SelectedItem == "Person ID")
                dv.RowFilter = $"Convert(personID, 'System.String') LIKE '%{txtFilterBy.Text}%'";           
            else if (cbFilterBy.SelectedItem == "National No")
                dv.RowFilter = $"NationalNo like '%{txtFilterBy.Text}%'";
            else if (cbFilterBy.SelectedItem == "First Name")
                dv.RowFilter = $"FirstName like '%{txtFilterBy.Text}%'";
            else if (cbFilterBy.SelectedItem == "Second Name")
                dv.RowFilter = $"SecondName like '%{txtFilterBy.Text}%'";
            else if (cbFilterBy.SelectedItem == "Third Name")
                dv.RowFilter = $"ThirdName like '%{txtFilterBy.Text}%'";
            else if (cbFilterBy.SelectedItem == "Last Name")
                dv.RowFilter = $"LastName like '%{txtFilterBy.Text}%'";
            else if (cbFilterBy.SelectedItem == "Gender")
                dv.RowFilter = $"Gender like '{txtFilterBy.Text}%'";
            else if (cbFilterBy.SelectedItem == "Nationality")
                dv.RowFilter = $"Nationality like '%{txtFilterBy.Text}%'";
            else if (cbFilterBy.SelectedItem == "Phone")
                dv.RowFilter = $"Phone like '%{txtFilterBy.Text}%'";
            else if (cbFilterBy.SelectedItem == "Email")
                dv.RowFilter = $"Email like '%{txtFilterBy.Text}%'";

            dgvListPeople.DataSource = dv;
            lblRecords.Text = dv.Count.ToString();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Clear();
            txtFilterBy.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPersonInfo frm = new frmAddEditPersonInfo(-1);
            frm.ShowDialog();
            dgvListPeople.DataSource = _GetDataTable();
        }

        private void tsmiAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPersonInfo frm = new frmAddEditPersonInfo(-1);
            frm.ShowDialog();
            dgvListPeople.DataSource = _GetDataTable();
        }

        private void tsmiEdit_Click(object sender, EventArgs e)
        {
            frmAddEditPersonInfo frm = new frmAddEditPersonInfo((int)dgvListPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            dgvListPeople.DataSource = _GetDataTable();
            
        }

        private void tsmiShowDetails_Click(object sender, EventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails((int)dgvListPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            dgvListPeople.DataSource = _GetDataTable();
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvListPeople.CurrentRow.Cells[0].Value;
            if (MessageBox.Show("Are You Sure You Want To Delete Person [" + PersonID + "]", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (clsPerson.DeleteContact(PersonID))
                {
                    MessageBox.Show("Person Deleted Successfully!");
                }
                else
                {
                    MessageBox.Show("Person Was Not Deleted!");
                }
            }
            dgvListPeople.DataSource = _GetDataTable();
        }
    }
}
