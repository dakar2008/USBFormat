using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace USBFormat
{
    class Logfile
    {
        public void GetDriveInfo(char DriveLetter, RichTextBox thistextbox)
        {
            string thisdriveletter = DriveLetter.ToString() + @":\";
            var thisdrive = DriveInfo.GetDrives().Where(m => m.Name == thisdriveletter).FirstOrDefault();
            thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Henter Informationer enheden - " + thisdrive.Name + "");
            var files = thisdrive.RootDirectory.GetFiles().Where(m => !m.Attributes.HasFlag(FileAttributes.Hidden)).ToList();
            int totalfiles = thisdrive.RootDirectory.GetFiles().Where(m => !m.Attributes.HasFlag(FileAttributes.Hidden)).Count();
            if (totalfiles > 1 || totalfiles == 0)
            {
                thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Der blev fundet " + totalfiles + " filer på usb'en");
                if(files.Where(m => m.Name.Contains("Eksamen_Aabybro")).Count() == 1)
                {
                    thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - En af filerne er standard dokumentet");
                }
                else
                {
                    thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Standard dokumentet blev ikke fundet");
                }
            }
            else
            {
                thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Der blev fundet " + totalfiles + " fil på usb'en");
                if (files.Where(m => m.Name.Contains("Eksamen_Aabybro")).Count() == 1)
                {
                    thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Den fundne fil er standard dokumentet");
                }
                else
                {
                    thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Den fundne fil er ikke standard dokumentet");
                }
            }
            thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Filsystem er: " + thisdrive.DriveFormat + "");
            long usedspace = (thisdrive.TotalSize - thisdrive.AvailableFreeSpace);
            thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Brugt Plads: " + usedspace.ToFileSize() + "");
            thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Total Plads: " + thisdrive.TotalSize.ToFileSize() + "");
        }

        public void StartFormatLog(char DriveLetter, RichTextBox thistextbox)
        {
            string thisdriveletter = DriveLetter.ToString() + @":\";
            var thisdrive = DriveInfo.GetDrives().Where(m => m.Name == thisdriveletter).FirstOrDefault();
            thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Formatering af " + thisdrive.Name + " er startet");
            thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Filsystem er: " + thisdrive.DriveFormat + "");
        }

        public void EndFormatLog(char DriveLetter, RichTextBox thistextbox)
        {
            string thisdriveletter = DriveLetter.ToString() + @":\";
            var thisdrive = DriveInfo.GetDrives().Where(m => m.Name == thisdriveletter).FirstOrDefault();
            thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Formatering af " + thisdrive.Name + " er afsluttet");
            int totalfiles = thisdrive.RootDirectory.GetFiles().Where(m => !m.Attributes.HasFlag(FileAttributes.Hidden)).Count();
            if (totalfiles > 1 || totalfiles == 0)
            {
                thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Der blev fundet " + totalfiles + " filer på usb'en");
            }
            else
            {
                thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Der blev fundet " + totalfiles + " fil på usb'en");
            }
            thistextbox.AppendText(Environment.NewLine);
        }

        public void CopyFilesStarted(char DriveLetter, RichTextBox thistextbox)
        {
            string thisdriveletter = DriveLetter.ToString() + @":\";
            var thisdrive = DriveInfo.GetDrives().Where(m => m.Name == thisdriveletter).FirstOrDefault();
            thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Kopiering af filer til " + thisdrive.Name + " er startet");
        }
        public void CopyFilesEnded(char DriveLetter, RichTextBox thistextbox)
        {
            string thisdriveletter = DriveLetter.ToString() + @":\";
            var thisdrive = DriveInfo.GetDrives().Where(m => m.Name == thisdriveletter).FirstOrDefault();
            thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Kopiering af filer til " + thisdrive.Name + " er afsluttet");
            int totalfiles = thisdrive.RootDirectory.GetFiles().Where(m => !m.Attributes.HasFlag(FileAttributes.Hidden)).Count();
            if (totalfiles > 1 || totalfiles == 0)
            {
                thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Der blev fundet " + totalfiles + " filer på usb'en");
            }
            else
            {
                thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - Der blev fundet " + totalfiles + " fil på usb'en");
            }
            thistextbox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] - " + thisdrive.Name + thisdrive.VolumeLabel + " er klar");
            thistextbox.AppendText(Environment.NewLine);
            thistextbox.AppendText(Environment.NewLine);
        }
    }
}
