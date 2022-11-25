
using KioskApi.Models;
using SQLite;

namespace KioskApi.Managers;
public class DatabaseManager
{    
    #region "Database Access"
    readonly string databaseId;
    private ILogger logger;
    public IConfiguration Configuration{get;}
    private SQLiteAsyncConnection database;

    public DatabaseManager(IConfiguration configuration, ILogger log){
        Configuration = configuration;
        databaseId = Configuration["DatabaseId"];
        logger = log;
        database = new SQLiteAsyncConnection(databaseId);
    }

    public async Task<IEnumerable<WeatherItem>> GetWeatherData(int maxResults)
    {
        var returnData = string.Empty;

        try
        {
            var results = await database.QueryAsync<WeatherItem>($"SELECT * FROM WeatherItem");

            return results;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Getting WeatherItem.");
            throw;
        }
    }

    public async Task<IEnumerable<IndoorStatusData>> GetIndoorStatusData()
    {
        var returnData = string.Empty;

        try
        {
            var results = await database.QueryAsync<IndoorStatusData>($"SELECT * FROM IndoorStatusData");

            return results;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Getting IndoorStatusData.");
            throw;
        }
    }

    public async void AddUpdateData<T>(T data)
    {
        try
        {
            await database.RunInTransactionAsync(tran =>
            {
                tran.InsertOrReplace(data);
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Updating Data.");
            throw;
        }
    }

    #endregion

    public void InitializeDatabase()
    {     
        database.CreateTableAsync<WeatherItem>();
        database.CreateTableAsync<IndoorStatusData>();
        database.CreateTableAsync<MoonData>();
    }
}