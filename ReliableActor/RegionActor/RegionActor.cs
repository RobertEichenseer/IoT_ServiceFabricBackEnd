using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;
using VehicleActor.Dto;
using VehicleActor.Interfaces;

namespace RegionActor
{
    public class RegionActor : Actor<RegionActorState>, IRegionActor
    {
        public override Task OnActivateAsync()
        {
            if (this.State == null)
            {
                this.State = new RegionActorState()
                {
                    Vehicle = new List<long>(),
                    SlowDownVehicle = new Dictionary<long, int>(),
                };
            }
            //ActorEventSource.Current.ActorMessage(this, "State initialized to {0}", this.State);
            return Task.FromResult(true);
        }

        public async Task<bool> EnterLeaveRegion(long vehicleId, bool entering)
        {
            if (entering)
            { 
                if (!this.State.Vehicle.Contains(vehicleId))
                    this.State.Vehicle.Add(vehicleId);
            }
            else
            {
                if (this.State.Vehicle.Contains(vehicleId))
                    this.State.Vehicle.Remove(vehicleId);
            }

            return true;    
        }


        public async Task<int> IngestSlowDownAsync(long vehicleId, int percentage)
        {

            var existingVehicleId = this.State.SlowDownVehicle.Keys; 
            if (!existingVehicleId.Contains(vehicleId))
                this.State.SlowDownVehicle.Add(vehicleId, percentage);

            return this.State.SlowDownVehicle.Count();
        }

        public async Task<RegionHealth> GetRegionStateAsync(long Id)
        {
            return new RegionHealth();
        }




    }
}
