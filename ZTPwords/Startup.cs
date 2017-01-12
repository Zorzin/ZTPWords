using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZTPwords.Startup))]
namespace ZTPwords
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
