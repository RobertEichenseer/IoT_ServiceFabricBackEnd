using ProtocolAdapter.Models;
using ProtocolAdapter.ServiceFabricHosting;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProtocolAdapter.Controllers
{
    public class BatchedIngestController : ApiController
    {
        [HttpGet]
        [Route("api/BatchedIngest")]
        public HttpResponseMessage GetLayoutBatch()
        {
            return Request.CreateResponse<List<Telemetry>>(HttpStatusCode.Found, new List<Telemetry>() {
                new Telemetry(), 
                new Telemetry(), 
            });
        }
        
        [HttpPost]
        [Route("api/BatchedIngest")]
        public HttpResponseMessage BatchedTelemetryIngest([FromBody]List<Telemetry> ingestBatch)
        {
            if (ingestBatch == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No Data"); 

            try
            {
                foreach(Telemetry telemetry in ingestBatch)
                {
                    //Todo: Ingest in EventHub
                }
                return Request.CreateResponse(HttpStatusCode.Created); 
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Malformated Data");
            }
            
        }
        
    }
}
