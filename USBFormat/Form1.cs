using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USBFormat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label8.Text = "Antal USB'er formateret: " + Properties.Settings.Default.USBCount;
            toolStripMenuItem4.Text = "Antal USB'er formateret: " + Properties.Settings.Default.USBCount;
            label7.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label8.Text = "Antal USB'er formateret: " + Properties.Settings.Default.USBCount;
            textBox1.Text = @"C:\USB-Eksamen-Skabeloner";
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            var drivelist = DriveInfo.GetDrives().Where(m => m.DriveType == DriveType.Removable);
            Properties.Settings.Default.FormatingRunning = false;
            Properties.Settings.Default.Save();
            checkedListBox1.Items.Clear();
            foreach (var drive in drivelist)
            {
                if(drive.DriveType == DriveType.Removable && !checkedListBox1.Items.Contains(drive.Name))
                {
                    if (drive.IsReady)
                    {
                        checkedListBox1.Items.Add(drive.Name + "" + drive.VolumeLabel);
                    }
                }
            }
            timer1.Enabled = true;
            if(drivelist.Count() > 0)
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
            label1.Text = "Antal USB'er fundet: " + drivelist.Count().ToString();
            richTextBox1.AppendText("[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Velkommen");
            richTextBox1.AppendText(Environment.NewLine);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var drivelist = DriveInfo.GetDrives().Where(m => m.DriveType == DriveType.Removable);
            checkedListBox1.Items.Clear();
            foreach (var drive in drivelist)
            {
                if (drive.DriveType == DriveType.Removable && !checkedListBox1.Items.Contains(drive.Name))
                {
                    if (drive.IsReady)
                    {
                        checkedListBox1.Items.Add(drive.Name + "" + drive.VolumeLabel);
                    }
                }
            }
            if(drivelist.Count() > 0)
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
            label1.Text = "Antal USB'er fundet: " + drivelist.Count().ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            var drivelist = DriveInfo.GetDrives().Where(m => m.DriveType == DriveType.Removable);
            checkedListBox1.Items.Clear();
            foreach (var drive in drivelist)
            {
                if (drive.DriveType == DriveType.Removable && !checkedListBox1.Items.Contains(drive.Name))
                {
                    if (drive.IsReady)
                    {
                        checkedListBox1.Items.Add(drive.Name + "" + drive.VolumeLabel);
                    }
                }
            }
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                Logfile logfile = new Logfile();
                richTextBox1.Clear();
                richTextBox1.AppendText("[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Velkommen");
                richTextBox1.AppendText(Environment.NewLine);
                button2.Enabled = false;
                button3.Enabled = false;
                button1.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                textBox1.Enabled = false;
                checkBox1.Enabled = false;
                toolStripMenuItem1.Enabled = false;
                toolStripMenuItem2.Enabled = false;
                toolStripMenuItem3.Enabled = false;
                this.ControlBox = false;
                Properties.Settings.Default.FormatingRunning = true;
                Properties.Settings.Default.Save();
                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                List<USBDrive> usbdrivelist = new List<USBDrive>();
                int currentusbcount = 0;
                int totalusbcount = checkedListBox1.CheckedItems.Count;
                progressBar1.Maximum = totalusbcount * 10;
                foreach (object itemChecked in checkedListBox1.CheckedItems)
                {
                    string currentvolumelabel = itemChecked.ToString();
                    USBDrive newusbdrive = new USBDrive();
                    string[] splitcheckitem = currentvolumelabel.Split('\\');
                    string driveletter = splitcheckitem[0];
                    string drivevolume = splitcheckitem[1];
                    var thisdriveinfo = DriveInfo.GetDrives().Where(m => m.DriveType == DriveType.Removable && m.VolumeLabel == drivevolume && m.Name.Contains(driveletter)).FirstOrDefault();
                    string[] splitdriveletter = thisdriveinfo.Name.Split(':');
                    newusbdrive.DriveLetter = splitdriveletter[0];
                    newusbdrive.VolumeLabel = thisdriveinfo.VolumeLabel;
                    usbdrivelist.Add(newusbdrive);
                }
                checkedListBox1.Enabled = false;
                foreach (var drive in usbdrivelist)
                {
                    await Task.Run(() => FormatingUSB.FormatUSB(drive, richTextBox1, textBox1, checkBox1));
                    currentusbcount = currentusbcount + 1;
                    Properties.Settings.Default.USBCount = Properties.Settings.Default.USBCount + 1;
                    Properties.Settings.Default.Save();
                    label8.Text = "Antal USB'er formateret: " + Properties.Settings.Default.USBCount;
                    toolStripMenuItem4.Text = "Antal USB'er formateret: " + Properties.Settings.Default.USBCount;
                    label8.Refresh();
                    progressBar1.PerformStep();
                }
                sw.Stop();
                TimeSpan time = sw.Elapsed;
                label7.Text = "Det tog " + time.Minutes + " minutter og " + time.Seconds + " sekunder at formatere " + usbdrivelist.Count() + " USB sticks";
                if (this.WindowState == FormWindowState.Minimized)
                {
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.BalloonTipTitle = "Færdig";
                    notifyIcon1.BalloonTipText = "Alle USB'er formateret...";
                    notifyIcon1.ShowBalloonTip(2000);
                }
                else
                {
                    MessageBox.Show("Alle USB'er formateret...", "Færdig", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    timer1.Enabled = true;
                }
                label7.Visible = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button1.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                textBox1.Enabled = true;
                checkedListBox1.Enabled = true;
                checkBox1.Enabled = true;
                toolStripMenuItem1.Enabled = true;
                toolStripMenuItem2.Enabled = true;
                toolStripMenuItem3.Enabled = true;
                Properties.Settings.Default.FormatingRunning = false;
                Properties.Settings.Default.Save();
                this.ControlBox = true;
                button1_Click(null, null);
            }
            else
            {
                MessageBox.Show("Ingen USB enheder valgt...", "Advarsel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void oneclickfunction(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                Logfile logfile = new Logfile();
                richTextBox1.Clear();
                richTextBox1.AppendText("[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Velkommen");
                richTextBox1.AppendText(Environment.NewLine);
                button2.Enabled = false;
                button3.Enabled = false;
                button1.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                textBox1.Enabled = false;
                checkBox1.Enabled = false;
                toolStripMenuItem1.Enabled = false;
                toolStripMenuItem2.Enabled = false;
                toolStripMenuItem3.Enabled = false;
                this.ControlBox = false;
                Properties.Settings.Default.FormatingRunning = true;
                Properties.Settings.Default.Save();
                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                List <USBDrive> usbdrivelist = new List<USBDrive>();
                int currentusbcount = 0;
                int totalusbcount = checkedListBox1.CheckedItems.Count;
                progressBar1.Maximum = totalusbcount * 10;
                foreach (object itemChecked in checkedListBox1.CheckedItems)
                {
                    string currentvolumelabel = itemChecked.ToString();
                    USBDrive newusbdrive = new USBDrive();
                    string[] splitcheckitem = currentvolumelabel.Split('\\');
                    string driveletter = splitcheckitem[0];
                    string drivevolume = splitcheckitem[1];
                    var thisdriveinfo = DriveInfo.GetDrives().Where(m => m.DriveType == DriveType.Removable && m.VolumeLabel == drivevolume && m.Name.Contains(driveletter)).FirstOrDefault();
                    string[] splitdriveletter = thisdriveinfo.Name.Split(':');
                    newusbdrive.DriveLetter = splitdriveletter[0];
                    newusbdrive.VolumeLabel = thisdriveinfo.VolumeLabel;
                    usbdrivelist.Add(newusbdrive);
                }
                checkedListBox1.Enabled = false;
                foreach (var drive in usbdrivelist)
                {
                    await Task.Run(() => FormatingUSB.FormatUSB(drive, richTextBox1, textBox1, checkBox1));
                    currentusbcount = currentusbcount + 1;
                    Properties.Settings.Default.USBCount = Properties.Settings.Default.USBCount + 1;
                    Properties.Settings.Default.Save();
                    label8.Text = "Antal USB'er formateret: " + Properties.Settings.Default.USBCount;
                    toolStripMenuItem4.Text = "Antal USB'er formateret: " + Properties.Settings.Default.USBCount;
                    label8.Refresh();
                    progressBar1.PerformStep();
                }
                sw.Stop();
                TimeSpan time = sw.Elapsed;
                label7.Text = "Det tog " + time.Minutes + " minutter og " + time.Seconds + " sekunder at formatere " + usbdrivelist.Count() + " USB sticks";
                if(this.WindowState == FormWindowState.Minimized)
                {
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.BalloonTipTitle = "Færdig";
                    notifyIcon1.BalloonTipText = "Alle USB'er formateret...";
                    notifyIcon1.ShowBalloonTip(2000);
                }
                else
                {
                    MessageBox.Show("Alle USB'er formateret...", "Færdig", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    timer1.Enabled = true;
                }
                label7.Visible = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button1.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                textBox1.Enabled = true;
                this.ControlBox = true;
                toolStripMenuItem1.Enabled = true;
                toolStripMenuItem2.Enabled = true;
                toolStripMenuItem3.Enabled = true;
                checkedListBox1.Enabled = true;
                checkBox1.Enabled = true;
                Properties.Settings.Default.FormatingRunning = false;
                Properties.Settings.Default.Save();
                button1_Click(null, null);
            }
            else
            {
                MessageBox.Show("Ingen USB enheder valgt...", "Advarsel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void oneclickfunctionminimized(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                Logfile logfile = new Logfile();
                richTextBox1.Clear();
                richTextBox1.AppendText("[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Velkommen");
                richTextBox1.AppendText(Environment.NewLine);
                Hide();
                notifyIcon1.Visible = true;
                button2.Enabled = false;
                button3.Enabled = false;
                button1.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                textBox1.Enabled = false;
                checkBox1.Enabled = false;
                toolStripMenuItem1.Enabled = false;
                toolStripMenuItem2.Enabled = false;
                toolStripMenuItem3.Enabled = false;
                this.ControlBox = false;
                Properties.Settings.Default.FormatingRunning = true;
                Properties.Settings.Default.Save();
                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                List<USBDrive> usbdrivelist = new List<USBDrive>();
                int currentusbcount = 0;
                int totalusbcount = checkedListBox1.CheckedItems.Count;
                progressBar1.Maximum = totalusbcount * 10;
                foreach (object itemChecked in checkedListBox1.CheckedItems)
                {
                    string currentvolumelabel = itemChecked.ToString();
                    USBDrive newusbdrive = new USBDrive();
                    string[] splitcheckitem = currentvolumelabel.Split('\\');
                    string driveletter = splitcheckitem[0];
                    string drivevolume = splitcheckitem[1];
                    var thisdriveinfo = DriveInfo.GetDrives().Where(m => m.DriveType == DriveType.Removable && m.VolumeLabel == drivevolume && m.Name.Contains(driveletter)).FirstOrDefault();
                    string[] splitdriveletter = thisdriveinfo.Name.Split(':');
                    newusbdrive.DriveLetter = splitdriveletter[0];
                    newusbdrive.VolumeLabel = thisdriveinfo.VolumeLabel;
                    usbdrivelist.Add(newusbdrive);
                }
                checkedListBox1.Enabled = false;
                foreach (var drive in usbdrivelist)
                {
                    await Task.Run(() => FormatingUSB.FormatUSB(drive, richTextBox1, textBox1, checkBox1));
                    currentusbcount = currentusbcount + 1;
                    Properties.Settings.Default.USBCount = Properties.Settings.Default.USBCount + 1;
                    Properties.Settings.Default.Save();
                    label8.Text = "Antal USB'er formateret: " + Properties.Settings.Default.USBCount;
                    toolStripMenuItem4.Text = "Antal USB'er formateret: " + Properties.Settings.Default.USBCount;
                    label8.Refresh();
                    progressBar1.PerformStep();
                }
                sw.Stop();
                TimeSpan time = sw.Elapsed;
                label7.Text = "Det tog " + time.Minutes + " minutter og " + time.Seconds + " sekunder at formatere " + usbdrivelist.Count() + " USB sticks";
                if (this.WindowState != FormWindowState.Minimized)
                {
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.BalloonTipTitle = "Færdig";
                    notifyIcon1.BalloonTipText = "Alle USB'er formateret...";
                    notifyIcon1.ShowBalloonTip(2000);
                }
                else
                {
                    MessageBox.Show("Alle USB'er formateret...", "Færdig", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    timer1.Enabled = true;
                }
                label7.Visible = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button1.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                textBox1.Enabled = true;
                this.ControlBox = true;
                toolStripMenuItem1.Enabled = true;
                toolStripMenuItem2.Enabled = true;
                toolStripMenuItem3.Enabled = true;
                checkedListBox1.Enabled = true;
                checkBox1.Enabled = true;
                Properties.Settings.Default.FormatingRunning = false;
                Properties.Settings.Default.Save();
                button1_Click(null, null);
            }
            else
            {
                MessageBox.Show("Ingen USB enheder valgt...", "Advarsel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Er du sikker på at du vil nulstille tælleren, dette kan ikke fortrydes?", "Advarsel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                Properties.Settings.Default.USBCount = 0;
                Properties.Settings.Default.TotalDiskSpace = 0;
                Properties.Settings.Default.Save();
                label8.Text = "Antal USB'er formateret: 0";
                toolStripMenuItem4.Text = "Antal USB'er formateret: 0";
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F5)
            {
                button1_Click(null, null);
            }
            if(e.KeyCode == Keys.F6)
            {
                var drivelist = DriveInfo.GetDrives().Where(m => m.DriveType == DriveType.Removable);
                if (drivelist.Count() > 0)
                {
                    button3_Click(null, null);
                }
            }
            if(e.KeyCode == Keys.F7)
            {
                var result = MessageBox.Show(null, "Er du sikker på at du vil formatere de valgte USBer?", "Advarsel", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if(result == DialogResult.OK)
                {
                    var drivelist = DriveInfo.GetDrives().Where(m => m.DriveType == DriveType.Removable);
                    if(drivelist.Count() > 0)
                    {
                        button2_Click(null, null);
                    }
                }
            }
            if (e.KeyCode == Keys.F10)
            {
                button5_Click(null, null);
            }
            if (ModifierKeys != Keys.Shift && e.KeyCode == Keys.F12)
            {
                timer1.Enabled = false;
                button1_Click(null, null);
                var result = MessageBox.Show(null, "Er du sikker på at du vil kører den automatiske funktion?", "Advarsel", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    oneclickfunction(null, null);
                }
            }
            if(ModifierKeys == Keys.Shift && e.KeyCode == Keys.F12)
            {
                timer1.Enabled = false;
                button1_Click(null, null);
                var result = MessageBox.Show(null, "Er du sikker på at du vil kører den automatiske funktion?", "Advarsel", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    oneclickfunctionminimized(null, null);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(null, "Er du sikker på at du vil rydde loggen...", "Advarsel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                richTextBox1.Clear();
                richTextBox1.AppendText("[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Velkommen");
                richTextBox1.AppendText(Environment.NewLine);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
            button5_Click(null, null);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
            button1_Click(null, null);
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            Show();
            Focus();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var drivelist = DriveInfo.GetDrives().Where(m => m.DriveType == DriveType.Removable);
            checkedListBox1.Items.Clear();
            foreach (var drive in drivelist)
            {
                if (drive.DriveType == DriveType.Removable)
                {
                    if (drive.IsReady)
                    {
                        checkedListBox1.Items.Add(drive.Name + "" + drive.VolumeLabel);
                    }
                }
            }
            label1.Text = "Antal USB'er fundet: " + drivelist.Count().ToString();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Properties.Settings.Default.FormatingRunning)
            {
                switch (e.CloseReason)
                {
                    case CloseReason.UserClosing:
                        e.Cancel = true;
                        break;
                }
                base.OnFormClosing(e);
            }
            else
            {
                switch (e.CloseReason)
                {
                    case CloseReason.UserClosing:
                        e.Cancel = false;
                        break;
                }
                base.OnFormClosing(e);
            }
        }
    }
}
