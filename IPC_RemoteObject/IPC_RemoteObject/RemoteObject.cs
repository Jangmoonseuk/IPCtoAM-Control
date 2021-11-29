using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPC_RemoteObject
{
    public class RemoteObject : MarshalByRefObject
    {
        private static string Count = "";

        public string GetCount()
        {
            return (Count);
        }

        public void SetCount(string cnt)
        {
            Count = cnt;
        }
    }
}
