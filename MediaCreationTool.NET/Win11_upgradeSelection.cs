using System;
using System.Windows.Forms;

namespace MediaCreationTool.NET {
    public partial class win11_upgradeSelection : Form {
        public win11_upgradeSelection() {
            InitializeComponent();
        }

        private void Button_selectIso_Click(object sender, EventArgs e) {
            using (var fileDialog = new OpenFileDialog()) {
                fileDialog.InitialDirectory = "c:\\";
                fileDialog.Filter = "ISO file (*.iso)|*.iso";
                fileDialog.FilterIndex = 1;

                if (fileDialog.ShowDialog() == DialogResult.OK) {
                    Globals.isoPath = fileDialog.FileName;
                    Hide();
                    var installSystem = new Win11_installSystem();
                    installSystem.Show();
                }
            }
        }

        private void Button_downloadIso_Click(object sender, EventArgs e) {
            Hide();
            var downloadSelection = new win11_downloadSelection();
            downloadSelection.Show();
        }

        private void Exit(object sender, FormClosingEventArgs e) {
            Environment.Exit(0);
        }
    }
}