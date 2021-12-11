using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace MediaCreationTool.NET {
    public partial class Win11_downloadSystem : Form {
        public Win11_downloadSystem() {
            InitializeComponent();
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            BeginInvoke((MethodInvoker) delegate {
                var percentage = (double)(e.BytesReceived * 100) / e.TotalBytesToReceive;
                lblDownloadPercentage.Text = $"{Math.Round(percentage, 1)} %";
                progressDownload.Value = (int)percentage;
            });
        }

        private void DownloadCompleted(object sender, AsyncCompletedEventArgs e) {
            BeginInvoke((MethodInvoker) delegate {
                Globals.isoPath = Directory.GetCurrentDirectory() + "/win11.iso";
                Hide();
                var installSystem = new Win11_installSystem();
                installSystem.Show();
            });
        }

        private void FormLoad(object sender, EventArgs e) {
            var client = new WebClient();
            client.DownloadProgressChanged += DownloadProgressChanged;
            client.DownloadFileCompleted += DownloadCompleted;
            client.DownloadFileAsync(new Uri(Globals.downloadURL), @"win11.iso");
        }

        private void Exit(object sender, FormClosingEventArgs e) {
            Environment.Exit(0);
        }
    }
}
