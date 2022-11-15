using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PPCP07302018.Startup))]
namespace PPCP07302018
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
