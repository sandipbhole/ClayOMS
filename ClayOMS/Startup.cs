using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClayOMS.Startup))]
namespace ClayOMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
