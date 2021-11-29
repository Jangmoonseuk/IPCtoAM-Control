using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using IPC_RemoteObject;

namespace IPC_Client
{
    public partial class frmIPC_Client : Form
    {
        IpcClientChannel ClientChannel;
        RemoteObject ro;

        Timer Tmr = new Timer();
        int CheckCnt = 0;
        int TempCnt = 0;

        public frmIPC_Client()
        {
            InitializeComponent();
        }

        private void frmIPC_Client_Load(object sender, EventArgs e)
        {
            try
            {
                ClientChannel = new IpcClientChannel();
                ChannelServices.RegisterChannel(ClientChannel, false);
                RemotingConfiguration.RegisterWellKnownClientType(typeof(RemoteObject), "ipc://remote/Cnt");
                ro = new RemoteObject();

                Tmr.Interval = 1000;
                Tmr.Enabled = true;

                Tmr.Tick += new EventHandler(Tmr_Tick);
                CheckCnt = ro.GetCount();
            }
            catch
            {
            }
        }

        void Tmr_Tick(object sender, EventArgs e)
        {
            try
            {
                TempCnt = CheckCnt;
                if (CheckCnt != ro.GetCount())
                {
                    CheckCnt = ro.GetCount();
                    this.textBox1.AppendText("Data Change - Before : " + TempCnt + ", Now : " + ro.GetCount() + Environment.NewLine);
                }                
            }
            catch
            {
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            ro.SetCount(ro.GetCount() - 1);
            this.textBox1.AppendText("Cnt Get : " + ro.GetCount() + Environment.NewLine);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            ro.SetCount(ro.GetCount() + 1);
            this.textBox1.AppendText("Cnt Get : " + ro.GetCount() + Environment.NewLine);
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            this.textBox1.AppendText("Cnt Get : " + ro.GetCount() + Environment.NewLine);
        }
    }
}
