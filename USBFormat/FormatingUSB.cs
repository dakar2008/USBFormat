using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace USBFormat
{
    class FormatingUSB
    {
        public static void FormatUSB(USBDrive drive, RichTextBox richTextBox1, TextBox textBox1, CheckBox checkBox1)
        {
            Logfile logfile = new Logfile();
            char[] driveletterarray = drive.DriveLetter.ToCharArray();
            logfile.GetDriveInfo(driveletterarray[0], richTextBox1);
            string fileName = "";
            string destFile = drive.DriveLetter + @":\";
            string vuccurrentdate = "VUC" + DateTime.Now.ToString("dd-MM-yy");
            logfile.StartFormatLog(driveletterarray[0], richTextBox1);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool isDriveFormatted = DriveManager.FormatDrive_CommandLine(driveletterarray[0], vuccurrentdate, "FAT32", true, false, false);
            if (isDriveFormatted)
            {
                sw.Stop();
                if(sw.Elapsed.Minutes > 1)
                {
                    if(sw.Elapsed.Seconds > 1)
                    {
                        richTextBox1.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Det tog " + sw.Elapsed.Minutes + " minutter og " + sw.Elapsed.Seconds + " sekunder at formatere " + destFile);
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Det tog " + sw.Elapsed.Minutes + " minutter og " + sw.Elapsed.Seconds + " sekund at formatere " + destFile);
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                }
                else
                {
                    if(sw.Elapsed.Seconds > 1)
                    {
                        richTextBox1.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Det tog " + sw.Elapsed.Minutes + " minut og " + sw.Elapsed.Seconds + " sekunder at formatere " + destFile);
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Det tog " + sw.Elapsed.Minutes + " minut og " + sw.Elapsed.Seconds + " sekund at formatere " + destFile);
                        richTextBox1.AppendText(Environment.NewLine);
                    }
                }
                logfile.EndFormatLog(driveletterarray[0], richTextBox1);
                if (Directory.Exists(textBox1.Text) && !checkBox1.Checked)
                {
                    logfile.CopyFilesStarted(driveletterarray[0], richTextBox1);
                    string[] files = Directory.GetFiles(textBox1.Text);
                    foreach (var file in files)
                    {
                        fileName = Path.GetFileName(file);
                        FileInfo fi = new FileInfo(file);
                        if (!fi.Attributes.HasFlag(FileAttributes.Hidden))
                        {
                            string targetpath = Path.Combine(destFile, fileName);
                            File.Copy(file, targetpath, true);
                        }
                    }
                    logfile.CopyFilesEnded(driveletterarray[0], richTextBox1);
                }
            }
        }
    }
}
