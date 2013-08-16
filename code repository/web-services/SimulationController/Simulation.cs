using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SimulationManager
{
    public class Simulator
    {
        public void SomeMethod([CallerMemberName]string memberName = "")
        {
            Console.WriteLine(memberName); //output will me name of calling method
        }

    }
    public class Sim
    {
        public ObservableCollection<SimulationAction> methodSimulationMap = new ObservableCollection<SimulationAction>();
    }

    public class SimulationAction
    {
        public string methodname { get; set; }
        public int sleep { get; set; }
        public bool throwException { get; set; }
        public bool holdUnmanagedResource { get; set; }
    }

    public static class SimulationDL
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