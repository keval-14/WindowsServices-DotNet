#region
//using System;
//using System.IO;
//using System.ServiceProcess;
//using System.Timers;

//namespace WindowsServiceDemo
//{
//    class Program
//    {
//        public const string ServiceName = "MyService";

//        System.Timers.Timer timer = new System.Timers.Timer();
//        public static void Main(string[] args)
//        {
//            Start(args);



//            Stop();


//            //if (Environment.UserInteractive)
//            //{
//            //    // running as console app
//            //    Start(args);

//            //    Console.WriteLine("Press any key to stop...");
//            //    Console.ReadKey(true);

//            //    Stop();
//            //}
//            //else
//            //{
//            //    // running as service
//            //    using (var service = new Service())
//            //    {
//            //        ServiceBase.Run(service);
//            //    }
//            //}
//        }

//        public static void Start(string[] args)
//        {
//            File.AppendAllText(@"D:\other\WindowsServices\WindowsServiceMsg.txt", String.Format("Service started at {0} {1}", DateTime.Now, Environment.NewLine));
//        }

//        public static void Stop()
//        {
//            File.AppendAllText(@"D:\other\WindowsServices\WindowsServiceMsg.txt", String.Format("Service stopped at {0} {1}", DateTime.Now, Environment.NewLine));
//        }
//    }
//}

#endregion

#region
//using System;
//using System.IO;
//using System.ServiceProcess;
//using System.Timers;
//using Timer = System.Timers.Timer;

//namespace WindowsServiceDemo
//{
//    class Program
//    {
//        public const string ServiceName = "MyService";

//        private static Timer timer;

//        public static void Main(string[] args)
//        {
//            Start(args);

//            Console.WriteLine("Press any key to stop...");
//            Console.ReadKey(true);

//            Stop();
//        }

//        public static void Start(string[] args)
//        {
//            // Set the interval for the timer (in milliseconds)
//            int interval = 3000;

//            // Create and configure the timer
//            timer = new Timer(interval);
//            timer.Elapsed += TimerElapsed;
//            timer.AutoReset = true; // Set to true for continuous execution

//            // Start the timer
//            timer.Start();

//            File.AppendAllText(@"D:\other\WindowsServices\WindowsServiceMsg.txt", String.Format("Service started at {0} {1}", DateTime.Now, Environment.NewLine));
//        }

//        public static void Stop()
//        {
//            // Stop and dispose of the timer
//            timer.Stop();
//            timer.Dispose();

//            File.AppendAllText(@"D:\other\WindowsServices\WindowsServiceMsg.txt", String.Format("Service stopped at {0} {1}", DateTime.Now, Environment.NewLine));
//        }

//        private static void TimerElapsed(object sender, ElapsedEventArgs e)
//        {
//            // Logic to execute every second
//            File.AppendAllText(@"D:\other\WindowsServices\WindowsServiceMsg.txt", String.Format("Timer elapsed at {0} {1}", DateTime.Now, Environment.NewLine));
//        }
//    }
//}
#endregion

#region 
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceDemo
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MyService()
            };
            ServiceBase.Run(ServicesToRun);

            //For Debugg only
            //new MyService().callAPI();
        }
    }
}


#endregion
