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

namespace IPC_Server
{
    public partial class frmIPC_Server : Form
    {
        IpcServerChannel ServerChannel;
        RemoteObject ro;

        public frmIPC_Server()
        {
            InitializeComponent();
        }

        private void frmIPC_Server_Load(object sender, EventArgs e)
        {
            ServerChannel = new IpcServerChannel("remote");
            ChannelServices.RegisterChannel(ServerChannel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemoteObject), "Cnt", WellKnownObjectMode.Singleton);
            this.textBox1.AppendText("Listening on " + ServerChannel.GetChannelUri() + Environment.NewLine);
            ro = new RemoteObject();
            ro.SetCount(1);
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
