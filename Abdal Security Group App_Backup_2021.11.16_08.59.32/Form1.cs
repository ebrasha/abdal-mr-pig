using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Abdal_Security_Group_App
{
    public partial class Form1 : Telerik.WinControls.UI.RadForm
    {
        public Form1()
        {
            InitializeComponent();

            //change form title
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = "Abdal Web Traffic Generator" + " " + version.Major + "." + version.Minor; 

            // Call Global Chilkat Unlock
            Abdal_Security_Group_App.GlobalUnlockChilkat GlobalUnlock = new Abdal_Security_Group_App.GlobalUnlockChilkat();
            GlobalUnlock.unlock();

        }


        #region Dragable Form Start
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        #endregion

        private void EncryptToggleSwitch_ValueChanged(object sender, EventArgs e)
        {
            if (EncryptToggleSwitch.Value == true)
            {
                DecryptToggleSwitch.Value = false;
            }
            else
            {
                DecryptToggleSwitch.Value = true;
            }
        }

        private void DecryptToggleSwitch_ValueChanged(object sender, EventArgs e)
        {
            if (DecryptToggleSwitch.Value == true)
            {
                EncryptToggleSwitch.Value = false;
            }
            else
            {
                EncryptToggleSwitch.Value = true;
            }
        }

        

        private void radLabelElement4_Click(object sender, EventArgs e)
        {
        
            System.Diagnostics.Process.Start("https://abdalagency.ir/");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://donate.abdalagency.ir/");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/abdal-security-group/");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://gitlab.com/abdal-security-group/abdal-404-pentest");
        }
    }
}
