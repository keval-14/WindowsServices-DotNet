using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace WindowsServiceDotnetCore
{
    public class AutoMessageEntry
    {
        private readonly Timer _timer;

        public AutoMessageEntry()
        {
            _timer = new Timer(3000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;

        }
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            string[] lines = new string[] { "Service recalled at: " + DateTime.Now.ToString() };
            File.AppendAllLines(@"D:\other\WindowsServices\WindowsServiceMsg.txt", lines);
        }

        public void Start()
        {
            string[] lines = new string[] { "Service started at: " + DateTime.Now.ToString() };
            File.AppendAllLines(@"D:\other\WindowsServices\WindowsServiceMsg.txt", lines);

            _timer.Start();
        }

        public void Stop()
        {
            string[] lines = new string[] { "Service stopped at: " + DateTime.Now.ToString() };
            File.AppendAllLines(@"D:\other\WindowsServices\WindowsServiceMsg.txt", lines);

            _timer.Stop();
        }

    }
}
