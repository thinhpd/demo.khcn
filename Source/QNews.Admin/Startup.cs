using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QNews.Admin.Startup))]
namespace QNews.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
