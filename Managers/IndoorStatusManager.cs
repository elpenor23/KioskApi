using KioskApi.Models;
namespace KioskApi.Managers;
public class IndoorStatusManager
{
    private DatabaseManager dbm;
    private ILogger logger;
    public IndoorStatusManager( IConfiguration configuration, ILogger log)
    {
        logger = log;
        dbm = new DatabaseManager(configuration, logger);
    }

    public async Task<IndoorStatusData> GetIndoorStatus(){
        var statusData = await dbm.GetIndoorStatusData();
        var data = statusData.Where( x=> x.Id == "1" ).ToList().FirstOrDefault();

        if (data == null ) {
            data = new IndoorStatusData();
        }

        return data;
    }

    public async Task<IndoorStatusData> SaveIndoorStatus(string status){
        DateTime threeMinutesAgo = DateTime.Now.AddMinutes(-3);
        var ReturnData = new IndoorStatusData();
        var statusData = await dbm.GetIndoorStatusData();

        //get cached data
        var data = statusData.Where( x=> x.Id == "1" ).ToList().FirstOrDefault();

        // if the data does not exist or is old we need to refresh the data
        if (data == null ) {
            data = new IndoorStatusData();
        }

        data.Data = status;
        data.LastSet = DateTime.Now;
            
        dbm.AddUpdateData(data);

        return data;
    }
}