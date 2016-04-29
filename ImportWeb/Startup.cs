using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImportWeb.Startup))]
namespace ImportWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
