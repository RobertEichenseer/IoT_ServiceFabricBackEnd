using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleActor.Dto;

namespace VehicleActor.Interfaces
{
    public interface IRegionActor : IActor
    {
        Task<int> IngestSlowDownAsync(long vehicleId, int percentage);
        Task<bool> EnterLeaveRegion(long vehicleId, bool entering);
        Task<RegionHealth> GetRegionStateAsync(long Id);
    }
}
