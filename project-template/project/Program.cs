using HostedWindowsAppSdk;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedWindowsAppSdk
{
    public static class Program
    {
        [STAThread]
        public static Task Main(string[] args)
        {
            var builder = new WindowsAppSdkHostBuilder<App>();

            builder.ConfigureServices(
                (_, collection) =>
                {
                    collection.AddSingleton<MainWindow>();
                }
            );

            var app = builder.Build();

            return app.StartAsync();
        }
    }
}
