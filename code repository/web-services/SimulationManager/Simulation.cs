using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SimulationManager
{
    public static class MyCache1 { public static Dictionary<int, string> list = new Dictionary<int, string>(); public static int indexer = 0;}
    public static class MyCache2 { public static Dictionary<int, string> list = new Dictionary<int, string>(); public static int indexer = 0;}
    public static class MyCache3 { public static Dictionary<int, string> list = new Dictionary<int, string>(); public static int indexer = 0;}
    public static class MyCache4 { public static Dictionary<int, string> list = new Dictionary<int, string>(); public static int indexer = 0;}
    public static class MyCache5 { public static Dictionary<int, string> list = new Dictionary<int, string>(); public static int indexer = 0;}
    public static class MyCache6 { public static Dictionary<int, string> list = new Dictionary<int, string>(); public static int indexer = 0;}
    public static class MyCache7 { public static Dictionary<int, string> list = new Dictionary<int, string>(); public static int indexer = 0;}

    public class Person
    {
        private string name;
        private string social;
        private int age;


        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }


        public string SocialSecurity
        {
            get { return social; }
            set { this.social = value; }
        }


        public int Age
        {
            get { return age; }
            set { this.age = value; }
        }


        public Person() { }
        public Person(string name, string ss, int age)
        {
            this.name = name; this.social = ss; this.age = age;
        }
    }
    public class Simulator
    {
        Random rnd = new Random();

        SimulationAction sa;
        public Sim sim;
        Thread workerThread;
        Worker workerObject;
        int TABLE_SIZE = 100000000;
        bool keepConsuming = false;
        string methodName;

        public void GetSimulationData() { }

        public Simulator()
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            System.Configuration.AppSettingsReader r = new System.Configuration.AppSettingsReader();
            string path = (string)r.GetValue("simulationsettings", typeof(String));

            watcher.Path = Path.GetDirectoryName(path);
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = Path.GetFileName(path);
            // Add event handlers.
            watcher.Changed += watcher_Changed;
            // Begin watching.
            watcher.EnableRaisingEvents = true;
            //create a thread for cpu maxing
            workerObject = new Worker();
            workerThread = new Thread(workerObject.DoWork);
        }

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            SimulationDL.Readfromfile();//this is enough for simulation, 
            this.sim = SimulationDL.sim;
            if (methodName != null)
            {
                var query = from p in sim.methodSimulationMap
                            where p.methodname == methodName
                            select p;

                sa = query.ToList()[0];
                keepConsuming = sa.consumeheap;
            }
        }

        public Simulator(Sim sim)
            : base()
        {
            this.sim = sim;
        }

        //public void Check(string msg)
        //{
        //    //System.Configuration.AppSettingsReader r = new System.Configuration.AppSettingsReader();
        //    //string path = (string)r.GetValue("GeneralLog", typeof(String));
        //    //using (StreamWriter outfile = File.AppendText(path))
        //    //{
        //    //    outfile.WriteLine("Check was called with : " + msg);
        //    //    outfile.Close();
        //    //}
        //}

        public void PerformanceSimulation()
        {
            SimulationDL.Readfromfile();//this is enough for simulation, 
            this.sim = SimulationDL.sim;
            StackTrace stackTrace = new StackTrace();
            MethodBase methodBase = stackTrace.GetFrame(1).GetMethod();
            methodName = methodBase.Name;
            var query = from p in sim.methodSimulationMap
                        where p.methodname == methodName
                        select p;
            sa = query.ToList()[0];
            if (sa.throwException)
            {
                throw new Exception();
            }
            System.Threading.Thread.Sleep(sa.sleep * 1000); //convert seconds to milliseconds
            if (sa.consumeheap)
            {
                keepConsuming = true;
                Runconsumeheap(sa.consumeheapfactor);
            }
            if (sa.consumecpu)
            {
                workerThread.Start();
            }
        }

        public void Runconsumeheap(int hf)
        {
            //Check("Runconsumeheap: factor" + hf.ToString());
            //an object bigger than 85kb will get put into large object heap, this will get collected in generation 2 (of 0,1,2)
            //static variables are long lived so they possibly go into heap
            //growth factor
            int lohSizeThreshHoldKB = 100; //large than 85 kb puts into LOH
            string line;
            int basesize = 1000;
            int arraysize = basesize * 1;

            //just construct an 100 byte string
            string myString = new string('*', lohSizeThreshHoldKB);

            /*System.Configuration.AppSettingsReader r = new System.Configuration.AppSettingsReader();
            string path = (string)r.GetValue("HeapGrowthRate", typeof(String));
            using (System.IO.StreamReader file = new System.IO.StreamReader(path))
            {
                line = file.ReadToEnd();
                file.Close();
            }
            int growthFactor = 1;
            int.TryParse(line, out growthFactor);
            */
            //a growth factor of 1 this is almost an MB per invocation of this method, so after a hundred reqs it would be 100MB
            //if growth factor is 5 then a 100 request would cause the array size to reach 500MB 
            //its a static variable hence it will get put into LOH and have trouble being collected
            //to simulate collection we should wait for out of memory exception and then clear this array,
            //this will then cause the root references to be cleaned up, so the GC will try and clean this
            //in doing so it will possibly get into large clean up time issues, due to fragmentation problems
            //fragmentation will occur because contiguous chunks are likely not to be found
            int growthFactor = hf;
            arraysize = basesize * 10 * growthFactor;
            try
            {
                for (int i = 0; i < arraysize; i++)
                {
                    int objectToGrow = rnd.Next(1, 7); // creates a number between 1 and 12
                    switch (objectToGrow)
                    {
                        case 1:
                            MyCache1.indexer++;
                            MyCache1.list.Add(MyCache1.indexer, myString);
                            break;
                        case 2:
                            MyCache2.indexer++;
                            MyCache2.list.Add(MyCache2.indexer, myString);
                            break;
                        case 3:
                            MyCache3.indexer++;
                            MyCache3.list.Add(MyCache3.indexer, myString);
                            break;
                        case 4:
                            MyCache4.indexer++;
                            MyCache4.list.Add(MyCache4.indexer, myString);
                            break;
                        case 5:
                            MyCache5.indexer++;
                            MyCache5.list.Add(MyCache5.indexer, myString);
                            break;
                        case 6:
                            MyCache6.indexer++;
                            MyCache6.list.Add(MyCache6.indexer, myString);
                            break;
                        case 7:
                            MyCache7.indexer++;
                            MyCache7.list.Add(MyCache7.indexer, myString);
                            break;
                    }
                }
            }
            catch (OutOfMemoryException)
            {
                MyCache1.indexer = 0;
                MyCache1.list.Clear();//this should auto induce a GC run at some point (Gen 2) --for long lived object

                MyCache2.indexer = 0;
                MyCache2.list.Clear();//this should auto induce a GC run at some point (Gen 2) --for long lived object

                MyCache3.indexer = 0;
                MyCache3.list.Clear();//this should auto induce a GC run at some point (Gen 2) --for long lived object

                MyCache4.indexer = 0;
                MyCache4.list.Clear();//this should auto induce a GC run at some point (Gen 2) --for long lived object

                MyCache5.indexer = 0;
                MyCache5.list.Clear();//this should auto induce a GC run at some point (Gen 2) --for long lived object

                MyCache6.indexer = 0;
                MyCache6.list.Clear();//this should auto induce a GC run at some point (Gen 2) --for long lived object

                MyCache7.indexer = 0;
                MyCache7.list.Clear();//this should auto induce a GC run at some point (Gen 2) --for long lived object
            }
        }
    }

    public class Worker
    {
        // This method will be called when the thread is started.
        public void DoWork()
        {
            while (true)
            {
                int b = 10 * 10;
            }
        }

        public void RequestStop()
        {
            _shouldStop = true;
        }
        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.
        private volatile bool _shouldStop;
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
        public bool consumeheap { get; set; }
        public int heapfactor { get; set; }
        public bool consumecpu { get; set; }
        public int consumeheapfactor { get; set; }
        public int cpufactor { get; set; }
    }

    public static class SimulationDL
    {
        public static Sim sim;

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