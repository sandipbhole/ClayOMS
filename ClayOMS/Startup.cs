using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JDV.Startup))]
namespace JDV
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
