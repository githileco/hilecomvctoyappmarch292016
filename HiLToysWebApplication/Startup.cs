using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HiLToysWebApplication.Startup))]
namespace HiLToysWebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}