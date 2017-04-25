using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb.Classes
{
    class ThreadTask
    {
        public string shcode;
        public string data;

        public ThreadTask(string _shcode, string _logData)
        {
            this.shcode = _shcode;
            this.data = _logData;
        }
    }
}
