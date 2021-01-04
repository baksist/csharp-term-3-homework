using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WinFormsClient
{
    public partial class GreeterForm : Form
    {
        public GreeterForm()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(loginTextBox.Text) || String.IsNullOrEmpty(passwordTextBox.Text)))
            {
                Authentication.Authenticate(loginTextBox.Text, passwordTextBox.Text);
                var onlineThread = new Thread(Activity.Update) { Name = "onlineThread" };
                onlineThread.Start();
                var form = new ChatForm();
                this.Hide();
                form.Closed += (s, args) => this.Close();
                form.Show();
            }
        }
    }
}
