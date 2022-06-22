using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace KioskApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //Add this line
            services.AddSingleton<IDocumentClient>(x => new DocumentClient(new Uri(Configuration["CosmosDB:URL"]), Configuration["CosmosDB:PrimaryKey"]));
        }
    }
}