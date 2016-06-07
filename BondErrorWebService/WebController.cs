using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Common;
using EnergyTrading.Logging;
using Model;

namespace BondErrorWebService
{
    public class WebController : ApiController
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
            Logger.InfoFormat("Resolution Center Received {0}", bond.LogFormat());
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
            Logger.InfoFormat("Resolution Center Resending {0}", bond.LogFormat());
            if (BondErrors.ContainsKey(bond.Identifier))
            {
                Bond removed;
                if (BondErrors.TryRemove(bond.Identifier, out removed))
                {
                    Publisher.SendBond(bond);
                    Logger.InfoFormat("Resolution Center Resent {0}", bond.LogFormat());
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}