using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehicleActor.Interfaces;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Actors;
using VehicleActor.Dto;

namespace VehicleActor
{
    public class VehicleActor : Actor<VehicleActorState>, IVehicleActor
    {

        public override Task OnActivateAsync()
        {
            if (this.State == null)
            {
                this.State = new VehicleActorState() { };
            }

            ActorEventSource.Current.ActorMessage(this, "State initialized to {0}", this.State);
            return Task.FromResult(true);
        }

        public async Task<bool> IngestTelemetryAsync(Telemetry telemetry)
        {
            this.State.LastIngest = DateTime.Now;
            this.State.IngestCount++;

            //Get Region and Inform Region if vehicle is in region or left region
            long regionId = GetRegionId(telemetry.Latitude, telemetry.Longitude);
            IRegionActor regionActor; 
            if (regionId != this.State.LastRegionId && this.State.LastRegionId != -1)
            {
                regionActor = ActorProxy.Create<IRegionActor>(new ActorId(this.State.LastRegionId), "fabric:/VehicleActorApplication");
                await regionActor.EnterLeaveRegion(telemetry.Id, false);
            }

            regionActor = ActorProxy.Create<IRegionActor>(new ActorId(regionId), "fabric:/VehicleActorApplication");
            await regionActor.EnterLeaveRegion(telemetry.Id, true);

            //Calculate Speed Difference and inform region to check for traffic jam
            telemetry.Speed = telemetry.Speed ?? 0;
            this.State.LastSpeed = this.State.LastSpeed ?? 0;
            int speedDifference = this.State.LastSpeed.Value - telemetry.Speed.Value;

            if (speedDifference <= -20)
            {
                int slowDownVehicle = await regionActor.IngestSlowDownAsync(telemetry.Id, speedDifference);
                if (slowDownVehicle > 5)
                    this.State.FailureState = string.Format("Potential Traffic Jam: {0} vehicles reduced speed", slowDownVehicle);
                else
                    this.State.FailureState = "No failure State; at the moment ;-)";
            }
            
            return true;
        }

        public async Task<VehicleHealth> GetVehicleStateAsync(long Id)
        {
            return new VehicleHealth() {
                Id = this.GetActorId().GetLongId(),
                VehicleState = this.State.FailureState,
            };
        }
        
        private long GetRegionId(long latitude, long longitude)
        {
            if (latitude < 100 && longitude < 100)
                return 1001;

            return 2002;
        }
    }
}
