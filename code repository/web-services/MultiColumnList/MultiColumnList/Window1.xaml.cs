using SimulationApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultiColumnList
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        SimulationAction currentSelectedMethod = null;
        Thread workerThread;
        Worker workerObject;
        //bool cpumaxstarted = false;

        public Window1()
        {
            InitializeComponent();
            SimulationApp.SimulationDL.Init();
            this.lbTodoList.ItemsSource = SimulationApp.SimulationDL.sim.methodSimulationMap;

            this.lbTodoList.SelectionChanged += lbTodoList_SelectionChanged;
            this.lbTodoList.SelectedIndex = 0;

            //create a thread for cpu maxing
            workerObject = new Worker();
            workerThread = new Thread(workerObject.DoWork);

            //get facade
            string df = GetFacade();
            if (df == "DataFacade1")
            {
                this.radioFacade1.IsChecked = true;
            }
            if (df == "DataFacade2")
            {
                this.radioFacade2.IsChecked = true;
            }
        }

        void lbTodoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentSelectedMethod = (SimulationAction)e.AddedItems[0];
            this.lblmethod.Content = currentSelectedMethod.methodname;
            this.txtSleeptimer.Text = currentSelectedMethod.sleep.ToString();
            this.chkException.IsChecked = currentSelectedMethod.throwException;
            this.chkUnmanaged.IsChecked = currentSelectedMethod.throwException;
            this.chkCpu.IsChecked = currentSelectedMethod.consumecpu;
            this.heaprate.Text = currentSelectedMethod.consumeheapfactor.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (currentSelectedMethod != null)
            {
                currentSelectedMethod.sleep = int.Parse(this.txtSleeptimer.Text);
                currentSelectedMethod.throwException = this.chkException.IsChecked.HasValue ? this.chkException.IsChecked.Value : false;
                currentSelectedMethod.consumeheap = this.chkUnmanaged.IsChecked.HasValue ? this.chkUnmanaged.IsChecked.Value : false;
                currentSelectedMethod.consumeheapfactor = int.Parse(this.heaprate.Text);
                currentSelectedMethod.consumecpu = this.chkCpu.IsChecked.HasValue ? this.chkCpu.IsChecked.Value : false;
                this.lbTodoList.Items.Refresh();
                SimulationDL.Persisttofile();//this technique is needed so that the actual service can read this and perform simulations
            }
            //persist the data facade state
            string facade = radioFacade1.IsChecked.HasValue ? "DataFacade1" : "DataFacade2";
            if (!radioFacade1.IsChecked.Value)
            {
                facade = radioFacade2.IsChecked.HasValue ? "DataFacade2" : "DataFacade1";
            }

            System.Configuration.AppSettingsReader r = new System.Configuration.AppSettingsReader();
            string path = (string)r.GetValue("DataFaceSimulationsettings", typeof(String));

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(facade);
            }
        }

        //simulate cpu usage increase
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            /*if (!cpumaxstarted)
            {
                // Start the worker thread.
                workerThread.Start();
                cpumaxstarted = true;
                this.btnCpu.Content = "Stop CPU comsumption";
            }
            else
            {
                cpumaxstarted = false;
                this.btnCpu.Content = "Start CPU comsumption";
                workerObject.RequestStop();
            }
             * */
        }

        public string GetFacade()
        {
            
            System.Configuration.AppSettingsReader r = new System.Configuration.AppSettingsReader();
            string path = (string)r.GetValue("DataFaceSimulationsettings", typeof(String));
            //class name to load
            string line;

            if (File.Exists(path))
            {
                // Read the file and display it line by line.
                using (System.IO.StreamReader file = new System.IO.StreamReader(path))
                {
                    line = file.ReadToEnd();
                    file.Close();
                }

                if (line.Trim() == "DataFacade1" || line.Trim() == "DataFacade2")
                {
                    return line;
                }
            }
            return "DataFacade1";//default
        }
    }
}