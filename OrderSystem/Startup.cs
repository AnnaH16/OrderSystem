using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OrderSystem.Startup))]
namespace OrderSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
