using Microsoft.Owin;
using Owin;
[assembly: OwinStartupAttribute(typeof(InstaOperatorOauthAPIS.Startup))]
namespace InstaOperatorOauthAPIS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}