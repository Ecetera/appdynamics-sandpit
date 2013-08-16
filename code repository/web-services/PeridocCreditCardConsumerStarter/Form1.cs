using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeriodicCreditCardConsumerStarter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("CreditCardServiceConsumer.exe",
                string.Format("{0} {1} {2} {3}",
                this.textBoxIP.Text,
                this.textBoxPort.Text,
                this.timerange1.Text,
                this.timerange2.Text));
        }

        private void validation()
        {
            System.Net.IPAddress ipaddress;
            int port;
            int t1;
            int t2;

            if (System.Net.IPAddress.TryParse(this.textBoxIP.Text, out ipaddress) && int.TryParse(this.textBoxPort.Text, out port)
                && int.TryParse(this.timerange1.Text, out t1) && int.TryParse(this.timerange2.Text, out t2))
            {

                this.btnStart.Enabled = (t1 < t2);
            }
            else
            {
                this.btnStart.Enabled = false;
            }
        }

        private void textBoxIP_Leave(object sender, EventArgs e)
        {
            validation();
        }

        private void textBoxPort_Leave(object sender, EventArgs e)
        {
            validation();
        }

        private void timerange1_Leave(object sender, EventArgs e)
        {
            validation();
        }

        private void timerange2_Leave(object sender, EventArgs e)
        {
            validation();
        }
    }
}
