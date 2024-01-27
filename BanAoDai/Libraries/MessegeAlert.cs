using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace BanAoDai.Libraries
{
    public class MessegeAlert
    {
        public string Type { get; set; }
        public string Msg { get; set; }
        public MessegeAlert(string type,string msg) 
        {
            this.Type = type;
            this.Msg = msg;
        }
       
    }
}