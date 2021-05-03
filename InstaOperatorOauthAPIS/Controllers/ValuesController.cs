using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace InstaOperatorOauthAPIS.Controllers
{
    public class ValuesController : ApiController
    {

        [HttpGet]
        public string Get(string id)
        {
            return id;
        }










    }
}
