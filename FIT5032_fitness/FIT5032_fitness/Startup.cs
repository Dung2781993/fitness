using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FIT5032_fitness.Startup))]
namespace FIT5032_fitness
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
