using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;

namespace RegionActor
{
    [DataContract]
    public class RegionActorState
    {
        public RegionActorState()
        {
            Vehicle = new List<long>();
            SlowDownVehicle = new Dictionary<long, int>(); 
        }

        [DataMember]
        public List<long> Vehicle { get; set; }

        [DataMember]
        public Dictionary<long, int> SlowDownVehicle { get; set; }
        
    }
}