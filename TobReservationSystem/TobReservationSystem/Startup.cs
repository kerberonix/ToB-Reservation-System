using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TobReservationSystem.Startup))]
namespace TobReservationSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
