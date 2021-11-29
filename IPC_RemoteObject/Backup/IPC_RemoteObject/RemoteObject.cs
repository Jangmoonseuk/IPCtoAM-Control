using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPC_RemoteObject
{
    public class RemoteObject : MarshalByRefObject
    {
        private static int Count = 0;

        public int GetCount()
        {
            return (Count);
        }

        public void SetCount(int cnt)
        {
            Count = cnt;
        }
    }
}
