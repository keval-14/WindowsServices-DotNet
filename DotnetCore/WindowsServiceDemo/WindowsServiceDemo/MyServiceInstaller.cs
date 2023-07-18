//using System.ComponentModel;
//using System.Configuration.Install;
//using System.ServiceProcess;

//namespace WindowsServiceDemo
//{
//    [RunInstaller(true)]
//    public class MyServiceInstaller : Installer
//    {
//        public string ServiceDescription { get; set; }
//        public MyServiceInstaller()
//        {
//            var spi = new ServiceProcessInstaller();
//            var si = new ServiceInstaller();

//            spi.Account = ServiceAccount.LocalSystem;
//            spi.Username = null;
//            spi.Password = null;

//            si.DisplayName = "TransferDataFromEmpToEmpTempTBL";
//            si.ServiceName = "WindowsService.NET";
//            si.Description = ServiceDescription;
//            si.StartType = ServiceStartMode.Automatic;

//            Installers.Add(spi);
//            Installers.Add(si);
//        }
//    }
//}


