using KioskApi.Managers;
namespace KioskApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var logger = new LoggerFactory().CreateLogger("stuff");
            new DatabaseManager(configuration, logger).InitializeDatabase();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<ILogger>(x => new LoggerFactory().CreateLogger("stuff"));
            //Add this line
            //services.AddSingleton<IDocumentClient>(x => new DocumentClient(new Uri(Configuration["CosmosDB:URL"]), Configuration["CosmosDB:PrimaryKey"]));
        }
    }
}