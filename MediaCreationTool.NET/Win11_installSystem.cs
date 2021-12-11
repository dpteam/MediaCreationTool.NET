﻿using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaCreationTool.NET {
    public partial class Win11_installSystem : Form {
        public Win11_installSystem() {
            InitializeComponent();
        }

        private void BypassRequirements() {
            var powershell = PowerShell.Create();
            var script = @"
$N = 'Skip TPM Check on Dynamic Update'
  $0 = sp 'HKLM:\SYSTEM\Setup\MoSetup' 'AllowUpgradesWithUnsupportedTPMOrCPU' 1 -type dword -force -ea 0
  $B = gwmi -Class __FilterToConsumerBinding -Namespace 'root\subscription' -Filter ""Filter = """"__eventfilter.name='$N'"""""" -ea 0
  $C = gwmi -Class CommandLineEventConsumer -Namespace 'root\subscription' -Filter ""Name='$N'"" -ea 0
  $F = gwmi -Class __EventFilter -NameSpace 'root\subscription' -Filter ""Name='$N'"" -ea 0
  if ($B) { $B | rwmi } ; if ($C) { $C | rwmi } ; if ($F) { $F | rwmi }
  $C = ""cmd /q $N (c) AveYo, 2021 /d/x/r>nul (erase /f/s/q %systemdrive%\`$windows.~bt\appraiserres.dll""
  $C+= '&md 11&cd 11&ren vd.exe vdsldr.exe&robocopy ""../"" ""./"" ""vdsldr.exe""&ren vdsldr.exe vd.exe&start vd -Embedding)&rem;'
  $K = 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\vdsldr.exe'
  $0=ni $K; sp $K Debugger $C -force; write-host -fore 0xf -back 0x2 ""`n $N [INSTALLED] run again to remove ""
";
            powershell.AddScript(script).Invoke();
        }

        private void MountISO() {
            var powershell = PowerShell.Create();
            powershell.AddCommand("Mount-DiskImage").AddParameter("ImagePath", Globals.isoPath).Invoke();
            powershell.Commands.Clear();

            powershell.AddCommand("Get-DiskImage").AddParameter("ImagePath", Globals.isoPath);
            powershell.AddCommand("Get-Volume");

            foreach (var psObject in powershell.Invoke())
                Globals.mountedDriveLetter = psObject.Members["DriveLetter"].Value.ToString();
        }

        private void InstallSystem() {
            BypassRequirements();
            MountISO();
            var setupConfig = "/auto ";

            if (radCleanInstall.Checked)
                setupConfig += "clean";
            else if (radDataOnly.Checked)
                setupConfig += "dataonly";
            else
                setupConfig += "upgrade";

            setupConfig += " /eula accept /compat ignorewarning /priority normal";

            Process.Start(Globals.mountedDriveLetter + ":\\Setup.exe", setupConfig);
            Environment.Exit(0);
        }

        private void BtnInstallSystem_Click(object sender, EventArgs e) {
            btnInstallSystem.Text = "Инициализация, подождите ...";
            btnInstallSystem.Enabled = false;
            radCleanInstall.Enabled = false;
            radDataOnly.Enabled = false;
            radUpgrade.Enabled = false;

            Task.Run(() => InstallSystem());
        }

        private void Exit(object sender, FormClosingEventArgs e) {
            Environment.Exit(0);
        }

        private void Win11_installSystem_Load(object sender, EventArgs e)
        {
        }
    }
}