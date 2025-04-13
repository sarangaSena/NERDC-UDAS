
/*
 Name       :	UDAS (Universal Data Acquisition module for Automation & BMS Industries)
 Software   :   IP Assigning Tool for DAQ unit
 Created    :	4/19/2016 9:45:37 AM
 Author     :	S.M.S Saranga Senarathna
*/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Advantech.Adam;
using Advantech.Common;
using Advantech.Protocol;
using System.Windows.Forms;


namespace Modbus_IP_assigning_Tool
{
    public partial class Form1 : Form
    {

        private ComPort ComPort;
        public Form1()
        {
            InitializeComponent();
            cmbProgPort.SelectedIndex = 0;
        }

        private void ViewError(String msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            ComPort = new ComPort(cmbProgPort.SelectedIndex + 1);

            if (ComPort.OpenComPort())
            {
                if (!ComPort.SetComPortState(Baudrate.Baud_9600, Databits.Eight, Parity.None, Stopbits.One))
                {
                    ViewError("Error Assigning Com Port Settings");
                    return;
                }

                if (!ComPort.SetComPortTimeout(5000, 5000, 0, 5000, 0))
                {
                    ViewError("Error Assigning Com Port Timeout Settings");
                    return;
                }
                btnClose.Enabled = true;
                btnRead.Enabled = true;
                btnWrite.Enabled = true;
                btnOpen.Enabled = false;
            }
            else
                ViewError("Error opening Serial Port");
        }


        private void btnClose_Click(object sender, EventArgs e)
        {

            if (!ComPort.CloseComPort())
            {
                ViewError("Error Closing Serial Port");
            }
            else
            {
                btnOpen.Enabled = true;
                btnClose.Enabled = false;
                btnRead.Enabled = false;
                btnWrite.Enabled = false;
            }



        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            String RStr;
            ComPort.SetPurge((int)Purge.RxClear + (int)Purge.TxClear); // purge the port before sending
            if (ComPort.Send("r " + "\r") > 0) // snding command
            {
                if (ComPort.Recv(out RStr) > 0)	// receiving command
                {
                    DevData Data = new DevData();
                    Data = JsonConvert.DeserializeObject<DevData>(RStr);
                    txtIp_1.Text = (Data.ip1).ToString();
                    txtIp_2.Text = (Data.ip2).ToString();
                    txtIp_3.Text = (Data.ip3).ToString();
                    txtIp_4.Text = (Data.ip4).ToString();
                    txtPort.Text = (Data.Port).ToString();
                 

                    // cmbDevParity.SelectedIndex = Data.Perity;
                }
                else
                    ViewError("Fail to receive");
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            DevData data = new DevData();
            data.ip1 = Convert.ToInt32(txtIp_1.Text);
            data.ip2 = Convert.ToInt32(txtIp_2.Text);
            data.ip3 = Convert.ToInt32(txtIp_3.Text);
            data.ip4 = Convert.ToInt32(txtIp_4.Text);
            data.Port = Convert.ToInt32(txtPort.Text);
            //data.ip2 = cmbip2.SelectedIndex;
            //data.ip3 = cmbip3.SelectedIndex;
            //data.ip4 = cmbip4.SelectedIndex;
            //data.Port = cmbport.SelectedIndex;
            //data.BaudRate = cmbBaudRate.SelectedIndex;
            //data.Perity = cmbDevParity.SelectedIndex;
            String JSONStr = JsonConvert.SerializeObject(data);
            ComPort.SetPurge((int)Purge.RxClear + (int)Purge.TxClear); // purge the port before sending
            String Sval ="w " + JSONStr + "\r";

            if (ComPort.Send(Sval) > 0)
            {
                String RecVal;
                if (ComPort.Recv(out RecVal) > 0)
                {

                    if (RecVal.Trim() == JSONStr.Trim())
                    {
                        MessageBox.Show("Successfuly Writed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ViewError("Error Writing Values");
                    }
                }
            }
        }

        private void FiletoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.ActiveForm.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About frmAbout = new About();
            frmAbout.ShowDialog();
        }
    }
}
