/*
 Name       :	UDAS (Universal Data Acquisition module for Automation & BMS Industries)
 Software   :   IP Assigning Tool for DAQ unit
 Created    :	4/19/2016 9:45:37 AM
 Author     :	S.M.S Saranga Senarathna
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Modbus_IP_assigning_Tool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
