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
using Newtonsoft.Json;
using System.IO;

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
                Program.Username = loginTextBox.Text;
                Program.Password = passwordTextBox.Text;
                if (configCheckBox.Checked)
                    SaveConfig();
                Authentication.Authenticate(Program.Username, Program.Password);
                var onlineThread = new Thread(Activity.Update) { Name = "onlineThread" };
                onlineThread.Start();
                var form = new ChatForm();
                this.Hide();
                form.Closed += (s, args) => this.Close();
                form.Show();
            }
            else if (configCheckBox.Checked)
            {
                LoadConfig();
                Authentication.Authenticate(Program.Username, Program.Password);
                var onlineThread = new Thread(Activity.Update) { Name = "onlineThread" };
                onlineThread.Start();
                var form = new ChatForm();
                this.Hide();
                form.Closed += (s, args) => this.Close();
                form.Show();
            }
        }
        private const string ConfigPath = "config.json";

        private void LoadConfig()
        {
            try
            {
                var reader = new StreamReader(ConfigPath);
                var properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());
                Program.Username = properties["username"];
                Program.Password = properties["password"];
                ChatForm.UpdatePeriod = Int32.Parse(properties["message_update"]);
                Program.Size[0] = Int32.Parse(properties["width"]);
                Program.Size[1] = Int32.Parse(properties["height"]);
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("error");
            }
        }

        private void SaveConfig()
        {
            var properties = new Dictionary<string, string>
            {
                {"username", Program.Username},
                {"password", Program.Password},
                {"message_update", ChatForm.UpdatePeriod.ToString()},
                {"width", this.Size.Width.ToString() },
                {"height", this.Size.Height.ToString() }
            };

            var writer = new StreamWriter(ConfigPath, false);
            writer.Write(JsonConvert.SerializeObject(properties));
            writer.Close();
            Program.Size[0] = this.Size.Width;
            Program.Size[1] = this.Size.Height;
        }
    }
}
