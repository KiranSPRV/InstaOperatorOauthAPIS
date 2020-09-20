using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaOperatorOauthAPIS.Models
{
    public class ExceptionLog
    {
        public string ExcepionMessage { get; set; }

        public string Module { get; set; }
        public string Procedure { get; set; }
        public string Method { get; set; }
        public string ApplicationType { get; set; }
    }
}