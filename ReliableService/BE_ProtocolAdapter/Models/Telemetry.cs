using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolAdapter.Models
{

    public class Telemetry
    {
        public long Id { get; set; }

        public int MessageNumber { get; set; }
        
        public long Latitude { get; set; }

        public long Longitude { get; set; }

        public int? Speed { get; set; }

        public int? PoIType { get; set; }

        public int? EmergencySituation { get; set; }
    }


}
