using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(E_FirmaElect_Demo.Startup))]
namespace E_FirmaElect_Demo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
