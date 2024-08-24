using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Presentation.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        public ctrlUserCard()
        {
            InitializeComponent();
        }

        private void FillFormWithUserInfo(int UserID)
        {
            clsUser User = clsUser.Find(UserID);
            ctrlPersonCard1.Load(User.PersonID);

            lblUserID.Text = UserID.ToString();
            lblUserName.Text = User.UserName.ToString();
            lblIsActive.Text = (User.IsActive == 0) ? "No" : "Yes";
        }

        private void FillFormWithDefaultInfo()
        {
            lblUserID.Text = "???";
            lblUserName.Text = "???";
            lblIsActive.Text = "???";
        }

        public void Load(int UserID)
        {
            if (clsUser.IsUserExist(UserID))
            {
                FillFormWithUserInfo(UserID);
            }
            else
            {
                MessageBox.Show("No User With UserID = " + UserID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
