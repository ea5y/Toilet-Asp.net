using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServer.Src
{
    public class BaseRes
    {
    }

    public class FlashRes : BaseRes
    {
        public string STATUS;
        public string CTRL;
        public string DATA;
    }
}