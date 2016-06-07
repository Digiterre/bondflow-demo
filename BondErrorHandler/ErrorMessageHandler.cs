using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using Common;
using EnergyTrading.Logging;
using EnergyTrading.Xml.Serialization;
using Messaging.Errors;
using Messaging.Transport;
using Model;
using Newtonsoft.Json;
using TransportMetadata.Errors;

namespace BondErrorHandler
{
    public class ErrorMessageHandler : IHandleIncomingMessages<ErrorMessageContent>
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ITransport transport;

        protected readonly JsonMediaTypeFormatter jsonMediaFormatter = new JsonMediaTypeFormatter { SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All } };

        public ErrorMessageHandler(ITransport transport)
        {
            if (transport == null)
            {
                throw new ArgumentNullException(nameof(transport));
            }
            this.transport = transport;
        }

        public void Handle(ErrorMessageContent content)
        {
            var originalPayload = transport.Message.GetSectionData(BuildErrorMessageRelatedData.PayloadKey);
            var bond = originalPayload.XmlDeserializer<Bond>();
            Logger.InfoFormat("Error Handler Received {0}", bond.LogFormat());
            using (var client = new HttpClient(new HttpClientHandler() {UseDefaultCredentials = true}))
            {
                var result = client.PostAsync("http://localhost:9990/BondError", bond, this.jsonMediaFormatter).Result;
                if (result.IsSuccessStatusCode)
                {
                    Logger.InfoFormat("Error Handler Forwarded to Resolution center {0}", bond.LogFormat());
                }
            }
        }
    }
}