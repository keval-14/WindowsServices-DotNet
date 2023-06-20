using Dapper;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace TransferDataFromEmpToEmpTempTBL
{
    public partial class Service1 : ServiceBase
    {
        public string ConnectionString { get; }
        public string providerName { get; }

        #region Service1()
        public Service1()
        {
            InitializeComponent();
        }

        #endregion

        #region OnStart
        protected override void OnStart(string[] args)
        {
            this.WriteToFile("Simple Service started {0}");
            this.ScheduleService();
        }

        #endregion

        #region OnStop
        protected override void OnStop()
        {
            this.WriteToFile("Simple Service stopped {0}");
            this.Schedular.Dispose();
        }

        #endregion

        #region ScheduleService
        private Timer Schedular;

        public void ScheduleService()
        {
            try
            {
                Schedular = new Timer(new TimerCallback(SchedularCallback));
                string mode = ConfigurationManager.AppSettings["Mode"].ToUpper();
                this.WriteToFile("Simple Service Mode: " + mode + " {0}");

                //Set the Default Time.
                DateTime scheduledTime = DateTime.MinValue;

                if (mode == "DAILY")
                {
                    //Get the Scheduled Time from AppSettings.
                    scheduledTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ScheduledTime"]);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next day.
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                }

                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                this.WriteToFile("Simple Service scheduled to run after: " + schedule + " {0}");

                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                Schedular.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                WriteToFile("Simple Service Error on: {0} " + ex.Message + ex.StackTrace);

                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("SimpleService"))
                {
                    serviceController.Stop();
                }
            }
        }

        #endregion

        #region SchedularCallback
        private void SchedularCallback(object e)
        {
            var message = "";

            var connSTR = "Server=PCI53\\SQL2019;Initial Catalog=SQL-Practice;Persist Security Info=False;User ID=sa;Password=Tatva@123;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;Connection Timeout=60;";
            try
            {
                using (var connection = new SqlConnection(connSTR))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@outPut", dbType: DbType.String, direction: ParameterDirection.Output, size: 200);

                    connection.Execute("SP_InsertOrUpdateIntoEmployeeTempTbl", parameters, commandType: CommandType.StoredProcedure);

                    message = parameters.Get<string>("@outPut");
                }
            }
            catch(Exception ex)
            {
                message = "Connection is not Established with DataBase";
                EventLog.WriteEntry("Error", ex.Message, EventLogEntryType.Warning);
            }

            #region output param ado


            //SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connSTR"]);
            //try
            //{
            //    SqlCommand cmd = new SqlCommand("SP_InsertOrUpdateIntoEmployeeTempTbl", conn);

            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add("@outPut", "");
            //    cmd.Parameters["@outPut"].Direction = ParameterDirection.Output;
            //    conn.Open();
            //    cmd.ExecuteNonQuery();
            //    conn.Close();
            //    //connTrue = "1";
            //    var message = cmd.Parameters["@outPut"].Value;

            //    connTrue = message.ToString();

            //}
            //catch (Exception ex)
            //{
            //    connTrue = "Connection is not Established with DataBase";
            //    EventLog.WriteEntry("Error", ex.Message, EventLogEntryType.Warning);
            //}
            #endregion


            this.WriteToFile("Simple Service Log: {0}");
            this.WriteToFile("Message:" + message);
            this.ScheduleService();
        }

        #endregion

        #region WriteToFile
        private void WriteToFile(string text)
        {
            string path = "C:\\ServiceLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }

        #endregion
    }
}