using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using VehicleActor.Interfaces;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;

namespace VehicleActor
{
    [DataContract]
    
    public class VehicleActorState
    {
        [DataMember]
        public string FailureState { get; set; }

        [DataMember]
        public int IngestCount { get; set; }

        [DataMember]
        public DateTime? LastIngest { get; set; }

        [DataMember]
        public int? LastSpeed { get; set; }

        [DataMember]
        public long CurrentRegionId { get; set; }

        [DataMember]
        public long LastRegionId { get; set; }


        public VehicleActorState()
        {
            CurrentRegionId = -1;
            LastRegionId = -1;
            LastIngest = null;
            IngestCount = 0;
            FailureState = String.Empty;
        }

        public override string ToString()
        {
            string format = "VehicleState: {0} - {1} - {2}";
            return string.Format(CultureInfo.InvariantCulture, format, FailureState, IngestCount, LastIngest);
        }
    }
}