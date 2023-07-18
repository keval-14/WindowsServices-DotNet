#region OldCode

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//public class Worker : BackgroundService
//{
//    //private readonly ILogger<Worker> _logger;

//    public Worker(/*ILogger<Worker> logger*/)
//    {
//        //_logger = logger;
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {
//            //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

//            string[] lines = new string[] { "Service recalled at: " + DateTime.Now.ToString() };
//            File.AppendAllLines(@"D:\other\WindowsServices\WindowsServiceMsg.txt", lines);

//            await Task.Delay(5000, stoppingToken);
//        }
//    }

//    public override Task StartAsync(CancellationToken cancellationToken)
//    {
//        // Perform startup actions here
//        //_logger.LogInformation("Service started at: {time}", DateTimeOffset.Now);

//        string[] lines = new string[] { "Service started at: " + DateTime.Now.ToString() };
//        File.AppendAllLines(@"D:\other\WindowsServices\WindowsServiceMsg.txt", lines);

//        return base.StartAsync(cancellationToken);
//    }

//    public override Task StopAsync(CancellationToken cancellationToken)
//    {
//        // Perform cleanup or graceful shutdown actions here
//        //_logger.LogInformation("Service stopped at: {time}", DateTimeOffset.Now);

//        string[] lines = new string[] { "Service stopped at: " + DateTime.Now.ToString() };
//        File.AppendAllLines(@"D:\other\WindowsServices\WindowsServiceMsg.txt", lines);

//        return base.StopAsync(cancellationToken);
//    }
//}


//public class Program
//{
//    public static async Task Main(string[] args)
//    {
//        var builder = Host.CreateDefaultBuilder(args)
//            .ConfigureServices((hostContext, services) =>
//            {
//                services.AddHostedService<Worker>();
//            })
//            .ConfigureLogging(logging =>
//            {
//                logging.ClearProviders();
//                logging.AddConsole();
//            });

//        await builder.RunConsoleAsync();
//    }
//}

#endregion

using Topshelf;

namespace WindowsServiceDotnetCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(X =>
            {
                X.Service<AutoMessageEntry>(S =>
                {
                    S.ConstructUsing(AutoMessageEntry => new AutoMessageEntry());
                    S.WhenStarted(AutoMessageEntry => AutoMessageEntry.Start());
                    S.WhenStopped(AutoMessageEntry => AutoMessageEntry.Stop());
                });

                X.RunAsLocalSystem();

                X.SetServiceName("AutoMessageEntryPer3Second");
                X.SetDisplayName("Auto Message Entry in Text File");
                X.SetDescription("This is Simple Windows Service that add new entry in Text file every second");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}