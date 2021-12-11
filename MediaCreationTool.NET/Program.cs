using System;
using System.Windows.Forms;

namespace MediaCreationTool.NET {
    internal static class Program {
        [STAThread]
        private static void Main() {
            if (!Environment.Is64BitOperatingSystem) {
                MessageBox.Show("32-битные версии ОС не поддерживаются!");
                Environment.Exit(0);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Win11_upgradeSelection());
        }
    }
}
