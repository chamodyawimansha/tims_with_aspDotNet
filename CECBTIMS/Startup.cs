using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CECBTIMS.Startup))]
namespace CECBTIMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
