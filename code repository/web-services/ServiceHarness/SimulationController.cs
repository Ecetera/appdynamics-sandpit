using ServiceHarness.RentalService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace ServiceHarness
{
    public class SimulationAction
    {
        public string methodname;
        public int sleep;
        public bool throwException;
        public bool holdUnmanagedResource;
    }

    public class Sim
    {
        public List<SimulationAction> methodSimulationMap = new List<SimulationAction>();
    }

    public class SimulationController
    {
        public Sim sim = new Sim();

        public void Init()
        {
            //read a file which contains information on what to apply
            //the file will be a key and values list, method name, followed by sleep time,
            //a boolean to throw method exceptions or not and a bool to hold onto a unmanaged reference
            Type myType = (typeof(RentalServiceClient));
            // Get the public methods.

            MethodInfo[] myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            // initialise methods
            foreach (MethodInfo methodInfo in myArrayMethodInfo)
            {
                SimulationAction sa = new SimulationAction();
                sa.methodname = methodInfo.Name;
                sa.holdUnmanagedResource = false;
                sa.sleep = 100;
                sa.throwException = false;
                sim.methodSimulationMap.Add(sa);
            }
        }

        public void SetMethodSimulation(string methodname, int sleep, bool throwexception, bool holdunmanagedref)
        {
            try
            {
                SimulationAction saf = sim.methodSimulationMap.Find(delegate(SimulationAction sa) { return sa.methodname == methodname; });
                saf.holdUnmanagedResource = holdunmanagedref;
                saf.sleep = sleep;
                saf.throwException = throwexception;
            }
            catch (Exception e)
            {
                string t = e.Message;
            }
        }

        public void Persisttofile()
        {
            System.Configuration.AppSettingsReader r = new System.Configuration.AppSettingsReader();
            string path = (string)r.GetValue("simulationsettings",typeof(String));

            var ser = new XmlSerializer(typeof(Sim));
            using (var ms = new MemoryStream())
            {
                ser.Serialize(ms, sim);
                var bytes = ms.ToArray();
                System.IO.File.WriteAllBytes(path, bytes);
            }
        }
    }
}
