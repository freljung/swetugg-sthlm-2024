using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CheatSheet.ASPNET.Startup))]
namespace CheatSheet.ASPNET
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
