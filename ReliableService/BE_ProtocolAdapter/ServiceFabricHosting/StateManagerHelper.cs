using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolAdapter.ServiceFabricHosting
{
    public static class StateManagerHelper
    {
        public static IReliableStateManager ReliableStateManager { get; set; }
    }
}
