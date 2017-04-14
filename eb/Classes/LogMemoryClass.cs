using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eb.Classes
{
    class LogMemoryClass
    {
        public string shcode { get; set; }
        public List<string> fileNameList { get; set; }
        public ArrayList logData { get; set; }

        public LogMemoryClass()
        {
            shcode = "";
            fileNameList = new List<string>();
            logData = new ArrayList();
        }
    }
}
