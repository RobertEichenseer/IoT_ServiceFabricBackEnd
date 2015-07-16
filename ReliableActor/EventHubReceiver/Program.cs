using VehicleActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleActor.Dto;

namespace VehicleActor.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //Vehicle No 1
            ActorId vehicleId = ActorId.NewId();
            Telemetry telemetry = new Telemetry()
            {
                Id = vehicleId.GetLongId(),
                EmergencySituation = null,
                Speed = 100,
            };
            var vehicle = ActorProxy.Create<IVehicleActor>(vehicleId, "fabric:/VehicleActorApplication");

            Task<bool> ingestTask = vehicle.IngestTelemetryAsync(telemetry);
            ingestTask.Wait();

            telemetry.Speed = telemetry.Speed - 40;
            ingestTask = vehicle.IngestTelemetryAsync(telemetry);
            ingestTask.Wait();

            Task<VehicleHealth> health = vehicle.GetVehicleStateAsync(vehicleId.GetLongId());
            health.Wait();
            Console.WriteLine(health.Result.VehicleState);

            for (int i=0; i<10; i++)
            { 
                //Vehicle No 2
                vehicleId = ActorId.NewId();
                telemetry = new Telemetry()
                {
                    Id = vehicleId.GetLongId(),
                    EmergencySituation = null,
                    Speed = 100,
                };
                vehicle = ActorProxy.Create<IVehicleActor>(ActorId.NewId(), "fabric:/VehicleActorApplication");

                ingestTask = vehicle.IngestTelemetryAsync(telemetry);
                ingestTask.Wait();

                telemetry.Speed = telemetry.Speed - 40;
                ingestTask = vehicle.IngestTelemetryAsync(telemetry);
                ingestTask.Wait();

                health = vehicle.GetVehicleStateAsync(vehicleId.GetLongId());
                health.Wait();
                Console.WriteLine(health.Result.VehicleState);
            }
            
            Console.ReadLine();
        }
    }
}
