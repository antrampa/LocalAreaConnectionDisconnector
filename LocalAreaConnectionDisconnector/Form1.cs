using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Threading;

namespace LocalAreaConnectionDisconnector
{
    public partial class mainForm : Form
    {
        #region Fields 
        private string _name;
        private string _status;
        #endregion

        #region Constructors 
        public mainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods 
        private void EnableDisableInNewThread()
        {
            Thread newThread = new Thread(EnableDisable);
            newThread.Start();

        }
        private void EnableDisable()
        {
            try
            {
                _name = nameTextBox.Text;
                SelectQuery wmiQuery = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");
                ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(wmiQuery);
                foreach (ManagementObject item in searchProcedure.Get())
                {
                    if (((string)item["NetConnectionId"]) == _name) //"Local Network Connection"
                    {
                        if(_status == "Disable")
                        {
                            item.InvokeMethod("Enable", null);
                            _status = "Enable";
                        }
                        else
                        {
                            item.InvokeMethod("Disable", null);
                            _status = "Disable";
                        }

                        if(statusLable.InvokeRequired)
                        {
                            //label5.Invoke((MethodInvoker)(() => label5.Text = "Requested" + repeats + "Times"));
                            statusLable.Invoke((MethodInvoker)(() => statusLable.Text = _status));
                        }
                        else
                        {
                            statusLable.Text = _status;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            
        }
        #endregion

        #region Events 
        private void bttnStart_Click(object sender, EventArgs e)
        {
            EnableDisableInNewThread();
        }
        #endregion
    }
}
