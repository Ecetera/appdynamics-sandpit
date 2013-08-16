using MultiColumnList.RentalService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SimulationApp
{
    public class Sim
    {
        public ObservableCollection<SimulationAction> methodSimulationMap = new ObservableCollection<SimulationAction>();
    }

    public class MethodInclusion
    {
        public Dictionary<string,string> list= new Dictionary<string,string>();
    }

    public class SimulationAction
    {
        public string methodname { get; set; }
        public int sleep { get; set; }
        public bool throwException { get; set; }
        public bool consumeheap { get; set; }
        public int consumeheapfactor { get; set; }
        public bool consumecpu{ get; set; }
    }

    public static class SimulationDL
    {
        public static Sim sim;
        public static MethodInclusion Mil;
        
        public static void GenerateMethodMap()
        {
            Mil = new MethodInclusion();
            string line;
            System.Configuration.AppSettingsReader r = new System.Configuration.AppSettingsReader();
            string path = (string)r.GetValue("MethodInclusionList", typeof(String));
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                Mil.list.Add(line, line);
            }
            file.Close();
            //read a file which contains information on what to apply
            //the file will be a key and values list, method name, followed by sleep time,
            //a boolean to throw method exceptions or not and a bool to hold onto a unmanaged reference
            Type myType = (typeof(RentalServiceClient));
            // Get the public methods.
            sim = new Sim();
            MethodInfo[] myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            // initialise methods
            foreach (MethodInfo methodInfo in myArrayMethodInfo)
            {
                SimulationAction sa = new SimulationAction();
                sa.methodname = methodInfo.Name;
                sa.consumeheap = false;
                sa.sleep = 1;
                sa.throwException = false;
                sa.consumecpu = false;
                if (Mil.list.ContainsKey(methodInfo.Name))
                {
                    sim.methodSimulationMap.Add(sa);
                }
            }
        }

        public static void SetMethodSimulation(SimulationAction saf, int sleep, bool throwexception, bool holdunmanagedref, bool consumecpu)
        {
            try
            {
                saf.consumeheap = holdunmanagedref;
                saf.sleep = sleep;
                saf.throwException = throwexception;
                saf.consumecpu = consumecpu;
            }
            catch (Exception e)
            {
                string t = e.Message;
            }
        }

        public static void SetMethodSimulation(string methodname, int sleep, bool throwexception, bool holdunmanagedref)
        {
            try
            {
                SimulationAction saf = sim.methodSimulationMap.ToList().Find(delegate(SimulationAction sa) { return sa.methodname == methodname; });
                saf.consumeheap = holdunmanagedref;
                saf.sleep = sleep;
                saf.throwException = throwexception;
            }
            catch (Exception e)
            {
                string t = e.Message;
            }
        }

        public static void Init()
        {
            //serialise and deserialise
            GenerateMethodMap();
            Persisttofile();
        }


        
        public static void Readfromfile()
        {
            System.Configuration.AppSettingsReader r = new System.Configuration.AppSettingsReader();
            string path = (string)r.GetValue("simulationsettings", typeof(String));

            var ser = new XmlSerializer(typeof(Sim));

            using (var ms = new StreamReader(path))
            {
                sim = (Sim)ser.Deserialize(ms);
                ms.Close();
            }
            

        }

        public static void Persisttofile()
        {
            System.Configuration.AppSettingsReader r = new System.Configuration.AppSettingsReader();
            string path = (string)r.GetValue("simulationsettings", typeof(String));

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