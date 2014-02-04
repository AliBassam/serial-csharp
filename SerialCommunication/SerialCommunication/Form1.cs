using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        SerialPort serial;

        public Form1()
        {
            InitializeComponent();

            //Get Available Serial Ports
            String[] ports = SerialPort.GetPortNames();

            comboBox1.Items.Add("Select Serial Port");

            //Fill ComboBox with Available Ports
            foreach (String port in ports)
            {
                comboBox1.Items.Add(port);
            }

            //Make First Item (Select Serial Port) Selected
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0)
            {
                //If a COM port is Selected
                serial = new SerialPort(comboBox1.SelectedItem.ToString());

                //Open the Port
                serial.Open();

                //Clear READ/WRITE buffers
                serial.DiscardInBuffer();
                serial.DiscardOutBuffer();

                //Declare Data Receiving Event
                serial.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            String ReceivedData = sp.ReadExisting();
            SetText(ReceivedData.ToString());
        }

         delegate void SetTextCallback(String ReceivedData);

         private void SetText(String ReceivedData)
         {
             if (this.textBox1.InvokeRequired)
             {
                 SetTextCallback d = new SetTextCallback(SetText);
                 this.Invoke(d, new object[] { ReceivedData });
             }
             else
             {
                 this.textBox1.Text += ReceivedData + "\r\n";
             }
         }

         private void button2_Click(object sender, EventArgs e)
         {
             serial.WriteLine(textBox2.Text);
         }
    }
}


