using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SeedPlant.Startup))]

[assembly: OwinStartupAttribute(typeof(SeedPlant.Startup))]
namespace SeedPlant
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {

            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            hubConfiguration.EnableJavaScriptProxies = true;

            app.MapSignalR("/signalr", hubConfiguration);

            ConfigureAuth(app);
        }
    }
}
