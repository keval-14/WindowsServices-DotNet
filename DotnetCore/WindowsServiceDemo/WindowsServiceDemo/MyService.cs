//using System.ServiceProcess;

//namespace WindowsServiceDemo
//{
//    class Service : ServiceBase
//    {
//        public Service()
//        {
//            ServiceName = Program.ServiceName;
//        }

//        protected override void OnStart(string[] args)
//        {
//            Program.Start(args);
//        }

//        protected override void OnStop()
//        {
//            Program.Stop();
//        }
//    }
//}

#region
//using System;
//using System.IO;
//using System.ServiceProcess;
//using System.Timers;
//using Timer = System.Timers.Timer;

//namespace WindowsServiceDemo
//{
//    public class MyService : ServiceBase
//    {
//        private Timer timer;

//        public MyService()
//        {
//            ServiceName = "MyService";
//        }

//        protected override void OnStart(string[] args)
//        {
//            timer = new Timer();
//            timer.Interval = 1000; // Set the interval for the timer (in milliseconds)
//            timer.Elapsed += TimerElapsed;
//            timer.AutoReset = true; // Set to true for continuous execution
//            timer.Start();

//            File.AppendAllText(@"D:\other\WindowsServices\WindowsServiceMsg.txt", String.Format("Service started at {0} {1}", DateTime.Now, Environment.NewLine));
//        }

//        protected override void OnStop()
//        {
//            timer.Stop();
//            timer.Dispose();

//            File.AppendAllText(@"D:\other\WindowsServices\WindowsServiceMsg.txt", String.Format("Service stopped at {0} {1}", DateTime.Now, Environment.NewLine));
//        }

//        private void TimerElapsed(object sender, ElapsedEventArgs e)
//        {
//            // Logic to execute every second
//            File.AppendAllText(@"D:\other\WindowsServices\WindowsServiceMsg.txt", String.Format("Timer elapsed at {0} {1}", DateTime.Now, Environment.NewLine));
//        }
//    }
//}
#endregion

using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.ServiceProcess;
using System.Timers;
namespace WindowsServiceDemo
{
    #region Runs every minutes and send emails(if email is already sent then it'll not send again)
    public partial class MyService : ServiceBase
    {
        System.Timers.Timer timer = new System.Timers.Timer();

        /// <summary>
        /// Service1 CTOR
        /// </summary>

        #region Service1
        public MyService()
        {
        }

        #endregion


        /// <summary>
        /// OnStart Method to handle all initialization of service, interval is set to 1 minute
        /// </summary>
        /// <param name="args"></param>

        #region OnStart
        protected override void OnStart(string[] args)
        {
            try
            {
                WriteTextToFile("Service started at " + DateTime.Now);
                timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
                timer.Interval = 5000; //time interval in milliseconds (10Sec) 
                timer.Enabled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(System.DateTime.Now + " " + ex.InnerException.Message);
                throw ex;
            }
        }

        #endregion


        /// <summary>
        /// OnStop method attempt to stop the service 
        /// </summary>

        #region OnStop
        protected override void OnStop()
        {
            try
            {
                WriteTextToFile("Service stopped at " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(System.DateTime.Now + " " + ex.InnerException.Message);
                throw ex;
            }
        }

        #endregion


        /// <summary>
        /// Instantiating the timer with an interval of 1 minute, which fires when the specified time has passed, this method calls callAPI() method...
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>

        #region OnElapsedTime
        public void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            try
            {
                WriteTextToFile("Service recalled at " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
            finally
            {
                timer.Start();
            }
        }

        #endregion


        /// <summary>
        /// WriteTextToFile method for check the log file is exist or not and write logs in that file
        /// </summary>
        /// <param name="Message"></param> 

        #region WriteTextToFile
        public void WriteTextToFile(string Message)
        {
            try
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
            catch (Exception ex)
            {
                OnStop();
            }
        }
        #endregion
    }
    #endregion


    #region Run at Exact time and only once a day and send emails
    //public partial class Service1 : ServiceBase
    //{
    //    Timer timer = new Timer();
    //    DateTime scheduleDateTime;
    //    public Service1()
    //    {
    //        InitializeComponent();
    //    }
    //    protected override void OnStart(string[] args)
    //    {
    //        WriteLogFile("Service is started");
    //        timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
    //        scheduleDateTime = DateTime.Today.AddHours(17).AddMinutes(42);
    //        var scheduleInterval = scheduleDateTime.Subtract(DateTime.Now).TotalSeconds * 1000;
    //        if (scheduleInterval < 0)
    //        {
    //            scheduleInterval += new TimeSpan(24, 0, 0).TotalSeconds * 1000;
    //        }
    //        timer.Interval = scheduleInterval;
    //        timer.Enabled = true;
    //    }
    //    protected override void OnStop()
    //    {
    //        WriteLogFile("Service is stopped");
    //    }
    //    private void OnElapsedTime(object source, ElapsedEventArgs e)
    //    {
    //        if (timer.Interval != 24 * 60 * 60 * 1000)
    //        {
    //            timer.Interval = 24 * 60 * 60 * 1000; //Reset the timer    
    //        }
    //        string ApiData = new WebClient().DownloadString("http://192.168.1.53:5555/api/SendEmailToUser");
    //        WriteLogFile($"Web Api called : Api Data {ApiData} ");
    //    }
    //    public void WriteLogFile(string message)
    //    {
    //        StreamWriter sw = null;
    //        sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
    //        sw.WriteLine($"{DateTime.Now.ToString()} : {message}");
    //        sw.Flush();
    //        sw.Close();
    //    }
    //}

    #endregion
}