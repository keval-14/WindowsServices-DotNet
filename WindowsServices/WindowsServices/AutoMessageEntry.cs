using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace WindowsServices
{
    public class AutoMessageEntry
    {
        private readonly Timer _timer;

        public AutoMessageEntry()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;

        }
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            string[] lines = new string[] { DateTime.Now.ToString()};
            File.AppendAllLines(@"D:\other\WindowsServices\WindowsServiceMsg.txt", lines);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

    }
}
