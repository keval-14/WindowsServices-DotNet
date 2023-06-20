using System;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Timers;
namespace SendEmailForBirthdayReminder
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        DateTime scheduleDateTime;
        public Service1()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            WriteLogFile("Service is started");
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            scheduleDateTime = DateTime.Today.AddHours(17).AddMinutes(42);
            var scheduleInterval = scheduleDateTime.Subtract(DateTime.Now).TotalSeconds * 1000;
            if (scheduleInterval < 0)
            {
                scheduleInterval += new TimeSpan(24, 0, 0).TotalSeconds * 1000;
            }
            timer.Interval = scheduleInterval;
            timer.Enabled = true;
        }
        protected override void OnStop()
        {
            WriteLogFile("Service is stopped");
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            if (timer.Interval != 24 * 60 * 60 * 1000)
            {
                timer.Interval = 24 * 60 * 60 * 1000; //Reset the timer    
            }
            string ApiData = new WebClient().DownloadString("http://192.168.1.53:5555/api/SendEmailToUser");
            WriteLogFile($"Web Api called : Api Data {ApiData} ");
        }
        public void WriteLogFile(string message)
        {
            StreamWriter sw = null;
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
            sw.WriteLine($"{DateTime.Now.ToString()} : {message}");
            sw.Flush();
            sw.Close();
        }
    }
}