using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using SimulationManager;

namespace SimulationCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            SimulationController.Init();
            SimulationManager.Simulator s = new Simulator(SimulationController.sim);
        }

        
    }    
    public static class SimulationController
    {
        public static Sim sim;

        public static void Init()
        {
            //read and deserialise
            Readfromfile();
            //prit out
            
            foreach (SimulationAction sa in sim.methodSimulationMap)
            {
                Console.WriteLine("{0}, {1}", sa.methodname, sa.sleep);
            }
            Console.Read();
        }

        public static void Readfromfile()
        {
            System.Configuration.AppSettingsReader r = new System.Configuration.AppSettingsReader();
            string path = (string)r.GetValue("simulationsettings",typeof(String));

            var ser = new XmlSerializer(typeof(Sim));
            
            using (var ms = new StreamReader(path))
            {
                sim = (Sim)ser.Deserialize(ms);
                ms.Close();
            }
        }
    }
}