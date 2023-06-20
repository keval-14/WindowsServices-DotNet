using Topshelf;

namespace WindowsServices
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

                X.SetServiceName("AutoMessageEntryPerSecond");
                X.SetDisplayName("Auto Message Entry in Text File");
                X.SetDescription("This is Simple Windows Service that add new entry in Text file every second");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}