using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using VehicleActor.Dto;

namespace VehicleActor.Interfaces
{
    public interface IVehicleActor : IActor
    {
        Task<bool> IngestTelemetryAsync(Telemetry telemetry);

        Task<VehicleHealth> GetVehicleStateAsync(long Id);
    }
}
