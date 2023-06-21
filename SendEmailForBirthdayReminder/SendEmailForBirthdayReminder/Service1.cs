using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.ServiceProcess;
using System.Timers;
namespace SendEmailForBirthdayReminder
{
    #region Runs every minutes and send emails(if email is already sent then it'll not send again)
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();

        /// <summary>
        /// Service1 CTOR
        /// </summary>

        #region Service1
        public Service1()
        {
            InitializeComponent();
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
                timer.Interval = 60000; //time interval in milliseconds (10Sec) 
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
                callAPI();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
        }

        #endregion


        /// <summary>
        /// callAPI Method for debugging the service and this method is directly called from program.cs when service is started in debuggind mode
        /// </summary>

        #region callAPI
        public void callAPI()
        {
            try
            {
                WriteTextToFile("Service recalled at " + DateTime.Now);
                string URLForAPICall = ConfigurationManager.AppSettings["URLForAPICall"];
                //Get API Responce by using HTTPClient

                //HttpClient client  = new HttpClient();
                //string ApiRes = client.GetStringAsync(URLForAPICall).Result;


                //Get API Responce by using WebClient
                string ApiData = new WebClient().DownloadString(URLForAPICall);
                WriteTextToFile($"Web Api called : {ApiData} ");
            }
            catch (Exception ex)
            {
                WriteTextToFile($"Error : {ex.InnerException.Message}");
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