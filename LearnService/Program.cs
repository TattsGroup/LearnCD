using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Serilog;
using Topshelf;
using Topshelf.Nancy;

namespace LearnService
{
    public class SampleModule : NancyModule
    {
        public SampleModule()
        {
            Get["/"] = p => "Hi [insert your name here]";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .MinimumLevel.Debug()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            // This must be set in App.config to be unique for every service!
            // (must not contain spaces or slashes)
            string serviceName = ConfigurationManager.AppSettings["ServiceName"];

            // This must also be unique for every service running on the same server
            int servicePort = 8080;

            if (!int.TryParse(ConfigurationManager.AppSettings["ServicePort"], out servicePort))
                servicePort = 8080;

            var host = HostFactory.New(x =>
            {
                x.UseSerilog();

                x.Service<SampleService>(s =>
                {
                    s.ConstructUsing(settings => new SampleService(serviceName));
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                    s.WithNancyEndpoint(x, c =>
                    {
                        c.AddHost(port: servicePort);
                        c.CreateUrlReservationsOnInstall();
                    });
                });
                x.StartAutomatically();
                x.SetServiceName(serviceName);
                x.RunAsNetworkService();
            });

            host.Run();
        }
    }
}
