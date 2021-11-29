using Aveva.PDMS.PMLNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IPC_Client
{
    [PMLNetCallable()]
    public class Program
    {
       
        [PMLNetCallable()]
        public Program()
        {
        }

        /// <summary>
        /// Docking Form
        /// </summary>
        [PMLNetCallable]
        public void Run()//Docking()
        {
            Form form = new frmIPC_Client();
        } 
        [PMLNetCallable()]
        public void Assign(Program that)
        {
        }
    }
}
