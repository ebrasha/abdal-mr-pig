using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using  Abdal_Mr_Pig;

namespace Abdal_Mr_Pig
{
    public partial class Abdal_Mr_Pig_Connection : Telerik.WinControls.UI.RadForm
    {




     

        public Abdal_Mr_Pig_Connection()
        {
            InitializeComponent();
        }

        private void Abdal_Mr_Pig_Connection_Load(object sender, EventArgs e)
        {

        }

        private void cmdTrustedKeys_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtTrustedKeys.Text = openFileDialog1.FileName;
            }
        }

        private void cmdPrivateKey_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtPrivateKey.Text = openFileDialog1.FileName;
            }
        }

        private void cmdPrivateKey_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtPrivateKey.Text = openFileDialog1.FileName;
            }
        }

        private void cmdTrustedKeys_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtTrustedKeys.Text = openFileDialog1.FileName;
            }
        }

       

        private void btnOk_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void gbProps_Enter(object sender, EventArgs e)
        {

        }
    }
}
