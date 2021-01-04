using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;

namespace WinFormsClient
{
    public partial class ChatForm : Form
    {
        public ChatForm()
        {
            InitializeComponent();
            var msgThread = new Thread(UpdateChatBox) { Name = "msgThread" };
            msgThread.Start();
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Program.Size[0], Program.Size[1]);
        }

        public static int UpdatePeriod = 200;
        public void UpdateChatBox()
        {
            var response = Program.Client.GetAsync(Program.AppPath + "/chat");
            var Messages =
                JsonConvert.DeserializeObject<List<Message>>(response.Result.Content.ReadAsStringAsync().Result);
            foreach (var msg in Messages)
            {
                chatTextBox.Invoke((Action) delegate
                {
                    chatTextBox.AppendText($"#{Messages.FindIndex(x => x == msg)}, {msg}");
                    chatTextBox.AppendText(Environment.NewLine);
                });
            }

            while (true)
            {
                response = Program.Client.GetAsync(Program.AppPath + "/chat");
                var newMessages =
                    JsonConvert.DeserializeObject<List<Message>>(response.Result.Content.ReadAsStringAsync().Result);
                if (newMessages.Count > Messages.Count)
                {
                    var messagesToAdd = new List<Message>();
                    for (var i = 0; i < newMessages.Count; i++)
                    {
                        if (i >= Messages.Count)
                        {
                            chatTextBox.Invoke((Action)delegate
                            {
                                chatTextBox.AppendText($"#{i}, {newMessages[i]}");
                                chatTextBox.AppendText(Environment.NewLine);
                            });
                            messagesToAdd.Add(newMessages[i]);
                        }
                    }
                    Messages.AddRange(messagesToAdd);
                    messagesToAdd.Clear();
                }
                else if (newMessages.Count < Messages.Count)
                {
                    chatTextBox.Invoke((Action)delegate
                    {
                        chatTextBox.Clear();
                    });
                    Messages = newMessages;
                    foreach (var msg in Messages)
                    {
                        chatTextBox.Invoke((Action)delegate
                        {
                            chatTextBox.AppendText($"#{Messages.FindIndex(x => x == msg)}, {msg}");
                            chatTextBox.AppendText(Environment.NewLine);
                        });
                    }
                }
                Thread.Sleep(UpdatePeriod);
            }
        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(messageTextBox.Text))
            {
                var msg = messageTextBox.Text;

                if ((msg == "/d all" || Regex.IsMatch(msg, @"^\/d [0-9]+$")) && Program.Role == "admin")
                {
                    var request = new HttpRequestMessage(HttpMethod.Delete, Program.AppPath + "/chat");
                    if (Regex.IsMatch(msg, @"^\/d [0-9]+$"))
                    {
                        var id = Int32.Parse(msg.Split()[1]);
                        request.RequestUri = new Uri(Program.AppPath + $"/chat/{id}");
                    }
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Program.Token);
                    Program.Client.SendAsync(request);
                }
                else
                {
                    var json = JsonConvert.SerializeObject(new Message(msg, Program.Username, DateTime.Now));
                    var request = new HttpRequestMessage(HttpMethod.Post, Program.AppPath + "/chat");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Program.Token);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    Program.Client.SendAsync(request);
                }

                messageTextBox.Clear();
            }
            else
            {
                MessageBox.Show("error");
            }
        }

        private void ChatForm_Resize(object sender, EventArgs e)
        {
            var properties = new Dictionary<string, string>
            {
                {"username", Program.Username},
                {"password", Program.Password},
                {"message_update", ChatForm.UpdatePeriod.ToString()},
                {"width", this.Size.Width.ToString() },
                {"height", this.Size.Height.ToString() }
            };

            var writer = new StreamWriter(GreeterForm.ConfigPath, false);
            writer.Write(JsonConvert.SerializeObject(properties));
            writer.Close();
            Program.Size[0] = this.Size.Width;
            Program.Size[1] = this.Size.Height;
        }
    }
}
