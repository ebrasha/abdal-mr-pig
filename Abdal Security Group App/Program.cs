using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Abdal_Security_Group_App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Abdal_Mr_Pig());
        }
    }
    public static class InputBox
    {
        public static bool Show(string prompt, string title, bool password, out string text)
        {
            Form form = new Form();
            form.Width = 425;
            form.Height = 120;
            form.Text = title;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowInTaskbar = false;

            Label labelText = new Label();
            labelText.Left = 10;
            labelText.Top = 10;
            labelText.Text = prompt;
            labelText.AutoSize = true;
            form.Controls.Add(labelText);

            TextBox textBox = new TextBox();
            textBox.Left = 10;
            textBox.Top = 30;
            textBox.Width = 400;
            textBox.Height = 21;
            if (password)
                textBox.PasswordChar = '*';
            form.Controls.Add(textBox);

            Button buttonOK = new Button();
            buttonOK.Text = "OK";
            buttonOK.Top = 57;
            buttonOK.Left = 254;
            buttonOK.Width = 75;
            buttonOK.DialogResult = DialogResult.OK;
            form.Controls.Add(buttonOK);
            form.AcceptButton = buttonOK;

            Button buttonCancel = new Button();
            buttonCancel.Text = "Cancel";
            buttonCancel.Top = 57;
            buttonCancel.Left = 335;
            buttonCancel.Width = 75;
            buttonCancel.DialogResult = DialogResult.Cancel;
            form.Controls.Add(buttonCancel);
            form.CancelButton = buttonCancel;

            if (form.ShowDialog() == DialogResult.OK)
            {
                text = textBox.Text;
                return true;
            }
            else
            {
                text = String.Empty;
                return false;
            }
        }
    }

}
