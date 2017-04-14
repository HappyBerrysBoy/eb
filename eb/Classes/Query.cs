using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eb.Classes
{
    class Query
    {
        public XA_DATASETLib.XAQuery query { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public bool isContinue { get; set; }

        public Query(string _name, string _key, bool _continue)
        {
            name = _name;
            key = _key;
            isContinue = _continue;
            query = null;
        }
    }
}
