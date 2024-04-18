using MeTroUIDemo.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeTroUIDemo
{
    public partial class Login : MetroFramework.Forms.MetroForm
    {
        public user User;

        private UserRepository userRepository;
        private GeneralRepository generalRepository;
        public Login()
        {
            InitializeComponent();

            userRepository = new UserRepository();
            generalRepository = new GeneralRepository();

        }

        private void buttonSignIn_Click(object sender, EventArgs e)
        {
            string username = textBoxUserName.Text;
            string password = textBoxPassword.Text;


            if (userRepository.AuthenticateUser(username, password))
            {
                User = userRepository.GetUserByUsername(username);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                // User not found or incorrect credentials
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private void textBoxUserName_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Suppress the Enter key
                textBoxPassword.Focus(); // Move focus to the next text box
            }
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Suppress the Enter key
                buttonSignIn.PerformClick(); // Simulate button click
            }
        }
    }
}
