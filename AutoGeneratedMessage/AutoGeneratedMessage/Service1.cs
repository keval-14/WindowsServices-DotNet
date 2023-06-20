﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace AutoGeneratedMessage
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteTextToFile("Service started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 10000; //time interval in milliseconds (10Sec) 
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteTextToFile("Service stopped at " + DateTime.Now);
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteTextToFile("Service recalled at " + DateTime.Now);
        }

        public void WriteTextToFile(string Message)
        {
            string checkPath = AppDomain.CurrentDomain.BaseDirectory + "\\LogsFile";
            if (!Directory.Exists(checkPath))
            {
                Directory.CreateDirectory(checkPath);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\LogsFile\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }

            }
        }
    }
}