using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model;

namespace BondErrorWebService
{
    public class WebController : ApiController
    {
        private static readonly ConcurrentDictionary<string, Bond> BondErrors = new ConcurrentDictionary<string, Bond>();

        public WebController() : base()
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        [HttpPost]
        [Route("BondError")]
        public HttpResponseMessage PostError(Bond bond)
        {
            BondErrors.AddOrUpdate(bond.Identifier, bond, (s, b) => bond);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("BondError")]
        public HttpResponseMessage ListErrors()
        {
            return Request.CreateResponse(HttpStatusCode.OK, BondErrors.Values.OrderBy((b) => b.Identifier).ToList());
        }

        [HttpPut]
        [Route("BondError")]
        public HttpResponseMessage HandleError(Bond bond)
        {
            if (BondErrors.ContainsKey(bond.Identifier))
            {
                Bond removed;
                if (BondErrors.TryRemove(bond.Identifier, out removed))
                {
                    Publisher.SendBond(bond);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}