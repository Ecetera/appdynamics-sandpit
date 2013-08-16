using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationApp
{
    public class Sim
    {
        public List<SimulationAction> methodSimulationMap = new System.Collections.Generic.List<SimulationAction>();
    }

    public class SimulationAction
    {
        public string methodname{get;set;}
        public int sleep{get;set;}
        public bool throwException { get; set; }
        public bool holdUnmanagedResource { get; set; }
    }
}
