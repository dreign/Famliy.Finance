using Microsoft.Owin;
using Owin;
using Famliy.Finance.Models;

[assembly: OwinStartupAttribute(typeof(Famliy.Finance.Startup))]
namespace Famliy.Finance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
