using ProtocolAdapter.Models;
using ProtocolAdapter.ServiceFabricHosting;
using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using Microsoft.ServiceFabric.Data.Collections;

namespace ProtocolAdapter.Controllers
{
    public class PartialIngestController : ApiController
    {
        private IReliableStateManager _reliableStateManager;
        
        public PartialIngestController()
        {
            _reliableStateManager = StateManagerHelper.ReliableStateManager;
        }


        [HttpGet]
        [Route("api/PartialIngest")]
        public HttpResponseMessage GetLayoutTelemetryFrame()
        {
            return Request.CreateResponse<Telemetry>(HttpStatusCode.Found, new Telemetry()); 
        }

        [HttpGet]
        [Route("api/PartialIngest/{id}")]
        public async Task<HttpResponseMessage> GetFullTelemetryFrame(int id)
        {
            
            if (id == 0)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No Data");


            var dataCollection = await _reliableStateManager.GetOrAddAsync<IReliableDictionary<int, Telemetry>>(String.Format("VehicleId-{0}", id));

            using (ITransaction transaction = _reliableStateManager.CreateTransaction())
            {
                ConditionalResult<Telemetry> result = await dataCollection.TryGetValueAsync(transaction, 0);

                if (result.HasValue)
                {

                    HttpResponseMessage httpResponseMessage = Request.CreateResponse<Telemetry>(result.Value);
                    httpResponseMessage.StatusCode = HttpStatusCode.Found;
                    return httpResponseMessage;
                }
                else
                {
                    HttpResponseMessage httpResponseMessage = Request.CreateResponse();
                    httpResponseMessage.StatusCode = HttpStatusCode.NotFound;
                    return httpResponseMessage;
                }
            }
        }

        [HttpPost]
        [Route("api/PartialIngest")]
        public async Task<HttpResponseMessage> TelemetryIngest([FromBody]Telemetry telemetry)
        {
            if (telemetry == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No Data");

            var dataCollection = await _reliableStateManager.GetOrAddAsync<IReliableDictionary<int, Telemetry>>(String.Format("VehicleId-{0}", telemetry.Id));
            
            bool fullFrameIngest = (telemetry.MessageNumber % 10 == 0) || (telemetry.MessageNumber == 1);

            Telemetry fullTelemetry = new Telemetry();
            if (fullFrameIngest)
            {
                fullTelemetry = telemetry;
            }
            else
            {
                fullTelemetry.Speed = telemetry.Speed ?? fullTelemetry.Speed;
                fullTelemetry.EmergencySituation = telemetry.EmergencySituation ?? fullTelemetry.EmergencySituation;
                fullTelemetry.PoIType = telemetry.PoIType ?? fullTelemetry.PoIType;
            }


            using (ITransaction transaction = _reliableStateManager.CreateTransaction())
            {
                ConditionalResult<Telemetry> result = await dataCollection.TryGetValueAsync(transaction, 0);
                
                if (!result.HasValue)
                {
                    await dataCollection.TryAddAsync(transaction, 0, fullTelemetry);
                }
                else
                {
                    await dataCollection.AddOrUpdateAsync(transaction, 0, fullTelemetry, (int key, Telemetry data) => fullTelemetry );
                }

                await transaction.CommitAsync();
            }

            //TODO: Add EventHub Ingest

            return Request.CreateResponse(HttpStatusCode.Created); 
        }
    }
}

