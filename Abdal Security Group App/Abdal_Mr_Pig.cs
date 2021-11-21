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
using nsoftware.SecureBlackbox;
using System.Collections;
using System.Media;
using Abdal_Mr_Pig;


namespace Abdal_Security_Group_App
{
    public partial class Abdal_Mr_Pig : Telerik.WinControls.UI.RadForm
    {
        private Sftpclient client;
        private Abdal_Mr_Pig_Connection Abdal_Mr_Pig_Connection = new Abdal_Mr_Pig_Connection();

        public Abdal_Mr_Pig()
        {
            InitializeComponent();

            
            radWaitingBar1.StopWaiting();
            //change form title
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = "Abdal Mr Pig" + " " + version.Major + "." + version.Minor;

            // Call Global Chilkat Unlock
            Abdal_Security_Group_App.GlobalUnlockChilkat GlobalUnlock =
                new Abdal_Security_Group_App.GlobalUnlockChilkat();
            GlobalUnlock.unlock();

            listObjects.ListViewItemSorter = new ObjectsSorter();

            client = new Sftpclient();

            client.OnError += new Sftpclient.OnErrorHandler(Client_OnError);
            client.OnFileOperation += new Sftpclient.OnFileOperationHandler(Client_OnFileOperation);
            client.OnFileOperationResult += new Sftpclient.OnFileOperationResultHandler(Client_OnFileOperationResult);
            client.OnListEntry += new Sftpclient.OnListEntryHandler(Client_OnListEntry);
            client.OnKnownKeyReceived += new Sftpclient.OnKnownKeyReceivedHandler(Client_OnKnownKeyReceived);
            client.OnUnknownKeyReceived += new Sftpclient.OnUnknownKeyReceivedHandler(Client_OnUnknownKeyReceived);
            client.OnPasswordChangeRequest +=
                new Sftpclient.OnPasswordChangeRequestHandler(Client_PasswordChangeRequest);
            client.OnAuthAttempt += new Sftpclient.OnAuthAttemptHandler(Client_AuthAttempt);
            client.OnAuthFailed += new Sftpclient.OnAuthFailedHandler(Client_AuthFailed);
            client.OnAuthSucceeded += new Sftpclient.OnAuthSucceededHandler(Client_AuthSucceeded);
            client.OnDisconnect += new Sftpclient.OnDisconnectHandler(Client_Disconnect);

            UpdateControls();
        }


        #region Dragable Form Start

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr) (HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        #endregion

        private void Client_OnFileOperation(object Sender, SftpclientFileOperationEventArgs e)
        {
            string S = OperationName(e.Operation) + ".";
            if (e.LocalPath.Length != 0)
                S = S + " Local path: " + e.LocalPath;
            if (e.RemotePath.Length != 0)
                S = S + " Remote path: " + e.RemotePath;

            editOutput.Text = editOutput.Text + S + System.Environment.NewLine;
        }


        private void Client_OnFileOperationResult(object Sender, SftpclientFileOperationResultEventArgs e)
        {
            string S = "";
            if (e.ErrorCode != 0)
                S = S + "Error " + e.ErrorCode.ToString() + ".";
            S = S + "Result of" + OperationName(e.Operation) + ".";
            if (e.LocalPath.Length != 0)
                S = S + " Local path: " + e.LocalPath;
            if (e.RemotePath.Length != 0)
                S = S + " Remote path: " + e.RemotePath;
            if (e.Comment.Length != 0)
                S = S + " Comment: " + e.Comment;

            editOutput.Text = editOutput.Text + S + System.Environment.NewLine;
        }

        private void Client_OnError(object Sender, SftpclientErrorEventArgs e)
        {
            string S = "Error " + e.ErrorCode.ToString();
            if (e.Description.Length > 0)
                S = S + ". Description: " + e.Description;
            editOutput.Text = editOutput.Text + S + System.Environment.NewLine;
        }

        private void Client_OnListEntry(object Sender, SftpclientListEntryEventArgs e)
        {
            ListViewItem item = new ListViewItem(client.CurrentListEntry.Name);
            item.Tag = client.CurrentListEntry;

            if (!client.CurrentListEntry.Directory)
            {
                item.SubItems.Add(client.CurrentListEntry.Size.ToString());
                item.ImageIndex = 1;
            }
            else
            {
                item.SubItems.Add("");
                item.ImageIndex = 0;
            }

            item.SubItems.Add(client.CurrentListEntry.MTime);
            item.SubItems.Add(client.CurrentListEntry.Owner);
            item.SubItems.Add(FormatRights(client.CurrentListEntry));

            listObjects.Items.Add(item);
        }

        private void Client_OnKnownKeyReceived(object Sender, SftpclientKnownKeyReceivedEventArgs e)
        {
            editOutput.Text = editOutput.Text + "KnownKey Received" + System.Environment.NewLine;
        }

        private void Client_OnUnknownKeyReceived(object Sender, SftpclientUnknownKeyReceivedEventArgs e)
        {
            e.Action = 2; // catAcceptPermanently
        }

        private void Client_PasswordChangeRequest(object Sender, SftpclientPasswordChangeRequestEventArgs e)
        {
            e.Cancel = false;
        }

        private void Client_AuthAttempt(object Sender, SftpclientAuthAttemptEventArgs e)
        {
            editOutput.Text = editOutput.Text + "Authentication type: " + e.AuthType.ToString() +
                              System.Environment.NewLine;
        }

        private void Client_AuthFailed(object Sender, SftpclientAuthFailedEventArgs e)
        {
            editOutput.Text = editOutput.Text + "Authentication type [" + e.AuthType.ToString() + "] failed" +
                              System.Environment.NewLine;
        }

        private void Client_AuthSucceeded(object Sender, SftpclientAuthSucceededEventArgs e)
        {
            editOutput.Text = editOutput.Text + "Authentication succeeded" + System.Environment.NewLine;
        }

        private void Client_Disconnect(object Sender, SftpclientDisconnectEventArgs e)
        {
            listObjects.Items.Clear();
            UpdateControls();
            editOutput.Text = editOutput.Text + "Сonnection is closed" + System.Environment.NewLine;
        }


        private class ObjectsSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                SFTPListEntry objX = (x as ListViewItem).Tag as SFTPListEntry;
                SFTPListEntry objY = (y as ListViewItem).Tag as SFTPListEntry;

                if (objX == null)
                {
                    return -1;
                }
                else if (objY == null)
                {
                    return 1;
                }
                else if (objX.Directory)
                {
                    if (objY.Directory)
                        return String.Compare(objX.Name, objY.Name);
                    else
                        return -1;
                }
                else
                {
                    if (objY.Directory)
                        return 1;
                    else
                        return String.Compare(objX.Name, objY.Name);
                }
            }
        }

        private string OperationName(int operation)
        {
            switch (operation)
            {
                case 0: //csfoDownloadFile
                    return "Download file";
                case 1: //csfoUploadFile
                    return "Upload file";
                case 2: //csfoDeleteFile
                    return "Delete file";
                case 3: //csfoMakeDir
                    return "Make directory";
                default:
                    return "Unknown";
            }
        }

        private string FormatRights(SFTPListEntry entry)
        {
            string res = "";
            if (entry.Directory)
            {
                res = res + "d";
            }

            if (entry.UserRead)
            {
                res = res + "r";
            }
            else
            {
                res = res + "-";
            }

            if (entry.UserWrite)
            {
                res = res + "w";
            }
            else
            {
                res = res + "-";
            }

            if (entry.UserExecute)
            {
                res = res + "x";
            }
            else
            {
                res = res + "-";
            }

            if (entry.GroupRead)
            {
                res = res + 'r';
            }
            else
            {
                res = res + '-';
            }

            if (entry.GroupWrite)
            {
                res = res + 'w';
            }
            else
            {
                res = res + '-';
            }

            if (entry.GroupExecute)
            {
                res = res + 'x';
            }
            else
            {
                res = res + '-';
            }

            if (entry.OtherRead)
            {
                res = res + 'r';
            }
            else
            {
                res = res + '-';
            }

            if (entry.OtherWrite)
            {
                res = res + 'w';
            }
            else
            {
                res = res + '-';
            }

            if (entry.OtherExecute)
            {
                res = res + 'x';
            }
            else
            {
                res = res + '-';
            }

            return res;
        }

        private void UpdateControls()
        {
            buttonConnect.Enabled = !client.Connected;
            buttonDisconnect.Enabled = client.Connected;

            buttonNewFolder.Enabled = client.Connected;

            buttonUpload.Enabled = client.Connected;
            buttonDownload.Enabled = client.Connected && (listObjects.FocusedItem != null) &&
                                     (listObjects.FocusedItem.Tag != null) &&
                                     (!(listObjects.FocusedItem.Tag as SFTPListEntry).Directory);

            buttonDelete.Enabled = client.Connected && listObjects.Focused && (listObjects.FocusedItem != null) &&
                                   (listObjects.FocusedItem.Tag != null) &&
                                   !((listObjects.FocusedItem.Tag as SFTPListEntry).Name.Equals(".")) &&
                                   !((listObjects.FocusedItem.Tag as SFTPListEntry).Name.Equals(".."));
        }

        private void LoadRoot()
        {
            LoadObjects();
        }

        private void ChangeDir(string path)
        {
            try
            {
                client.ChangeDir(path);

                LoadObjects();
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }

        private void LoadObjects()
        {
            lPath.Text = client.GetCurrentDir();

            listObjects.BeginUpdate();
            try
            {
                listObjects.Items.Clear();

                UseWaitCursor = true;
                try
                {
                    try
                    {
                        string content = client.ListDir(true, true);
                    }
                    catch (Exception E)
                    {
                        MessageBox.Show(E.Message);
                    }
                }
                finally
                {
                    UseWaitCursor = false;
                }
            }
            finally
            {
                listObjects.EndUpdate();
                UpdateControls();
            }
        }


        private void radLabelElement4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://abdalagency.ir/");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/abdal-security-group/abdal-mr-pig");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://donate.abdalagency.ir/");
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://gitlab.com/abdal-security-group/abdal-mr-pig");
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (backgroundWorker_connector.IsBusy != true)
            {
                radWaitingBar1.StartWaiting();
                backgroundWorker_connector.RunWorkerAsync();
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (backgroundWorker_disconnect.IsBusy != true)
            {
               
                using (var soundPlayer = new SoundPlayer(@"iner.wav"))
                {
                    soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                }

                backgroundWorker_disconnect.RunWorkerAsync();
            }


        }

        private void buttonNewFolder_Click(object sender, EventArgs e)
        {
            string newName;

            if (!InputBox.Show("Enter a name for new folder:", this.Text, false, out newName))
                return;

            try
            {
                client.MakeDir(newName);
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }

            LoadObjects();
            UpdateControls();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            if (dialogOpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    client.UploadFile(dialogOpenFile.FileName, System.IO.Path.GetFileName(dialogOpenFile.FileName));

                    LoadObjects();
                }
                catch (Exception err)
                {
                    MessageBox.Show(String.Format("Failed to upload file. Reason: {0}", err.Message), this.Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateControls();
                    return;
                }
            }
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            SFTPListEntry obj = listObjects.FocusedItem.Tag as SFTPListEntry;
            if (!String.IsNullOrEmpty(dialogSaveFile.FileName))
                dialogSaveFile.InitialDirectory = System.IO.Path.GetFullPath(dialogSaveFile.FileName);
            dialogSaveFile.FileName = obj.Name;
            if (dialogSaveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    client.DownloadFile(obj.Name, dialogSaveFile.FileName);
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message);
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            SFTPListEntry obj = null;

            if (listObjects.Focused && (listObjects.FocusedItem != null))
                obj = listObjects.FocusedItem.Tag as SFTPListEntry;

            if (obj == null)
            {
                MessageBox.Show("Please select a file or a folder you wish to delete", this.Text, MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                UpdateControls();
                return;
            }

            if (MessageBox.Show(String.Format("Are you sure you want to delete the object \"{0}\"?", obj.Name),
                this.Text,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                UpdateControls();
                return;
            }

            try
            {
                if (obj.Directory)
                    client.DeleteDir(obj.Name);
                else
                    client.DeleteFile(obj.Name);
            }
            catch (Exception err)
            {
                MessageBox.Show(String.Format("Failed to delete the object. Reason: {0}", err.Message), this.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateControls();
                return;
            }


            if (listObjects.Focused)
            {
                listObjects.FocusedItem.Remove();
                listObjects_SelectedIndexChanged(listObjects, null);
            }

            UpdateControls();
        }

        private void listObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void listObjects_Enter(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void listObjects_DoubleClick(object sender, EventArgs e)
        {
            if (listObjects.FocusedItem == null)
                return;

            SFTPListEntry obj = listObjects.FocusedItem.Tag as SFTPListEntry;

            if (obj.Directory)
            {
                ChangeDir(obj.Name);
            }
            else
                buttonDownload.PerformClick();
        }

        private void Abdal_Mr_Pig_Load(object sender, EventArgs e)
        {

        }

        private void toolsMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void backgroundWorker_connector_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Connecting_part
            client.RuntimeLicense = "53424E4641423155523135355434544430574E313041444445565430303153315200000000000000585447465343534500005846384741385344534B33420000";

            if (client.Connected)
            {
                using (var soundPlayer = new SoundPlayer(@"error.wav"))
                {
                    soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                }
                MessageBox.Show("Already connected, please disconnect first");

                return;
            }

            if (Abdal_Mr_Pig_Connection.ShowDialog(this) == DialogResult.OK)
            {
                editOutput.Text = "";

                client.Username = Abdal_Mr_Pig_Connection.editUsername.Text;
                client.Password = Abdal_Mr_Pig_Connection.editPassword.Text;

                if (Abdal_Mr_Pig_Connection.txtPrivateKey.Text != "")
                {
                    try
                    {
                        Sshkeymanager keymanager = new Sshkeymanager();
                        keymanager.ImportFromFile(Abdal_Mr_Pig_Connection.txtPrivateKey.Text,
                            Abdal_Mr_Pig_Connection.txtPrivateKeyPassword.Text);

                        client.Key = keymanager.Key;
                    }
                    catch (Exception E)
                    {
                        using (var soundPlayer = new SoundPlayer(@"error.wav"))
                        {
                            soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                        }
                        radWaitingBar1.StopWaiting();

                        MessageBox.Show(E.Message);
                    }
                }

                client.TrustedKeysFile = Abdal_Mr_Pig_Connection.txtTrustedKeys.Text;

                try
                {
                    client.Connect(Abdal_Mr_Pig_Connection.editAddress.Text,
                        (int)Abdal_Mr_Pig_Connection.editPort.Value);

                    LoadRoot();
                    UpdateControls();

                    // Connecting Sound
                    using (var soundPlayer = new SoundPlayer(@"start.wav"))
                    {
                        soundPlayer.PlaySync();
                    }


                }
                catch (Exception E)
                {
                    using (var soundPlayer = new SoundPlayer(@"error.wav"))
                    {
                        soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                    }

                    radWaitingBar1.StopWaiting();
                    MessageBox.Show(E.Message);
                }
            }

           

            #endregion
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (client.Connected)
            {
                try
                {
                    client.Disconnect();

                    listObjects.Items.Clear();

                    UpdateControls();
                    radWaitingBar1.StopWaiting();
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message);
                }
            }

        }
    }
}