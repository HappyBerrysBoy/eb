using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eb.common
{
    static class Common
    {
        public static string getSign(string sign)
        {
            if (sign.Equals("1"))
                return "↑";
            else if (sign.Equals("2"))
                return "▲";
            else if (sign.Equals("4"))
                return "↓";
            else if (sign.Equals("5"))
                return "▼";
            return "";
        }   
    }
}
