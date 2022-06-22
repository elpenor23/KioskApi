using Microsoft.Azure.Documents;
using KioskApi.Models;

namespace KioskApi.Managers;
public class IndoorStatusManager
{
    private IConfiguration Configuration { get; }
    private DatabaseManager dbm;
    public IndoorStatusManager(IDocumentClient documentClient, IConfiguration configuration)
    {
        Configuration = configuration;
        dbm = new DatabaseManager(documentClient, configuration, "IndoorStatus");
    }

    public async Task<IndoorStatusData> GetIndoorStatus(){
        var statusData = dbm.GetData<IndoorStatusData>(1);
        var data = statusData.Where( x=> x.Id == "1" ).ToList().FirstOrDefault();

        if (data == null ) {
            
            data = new IndoorStatusData();
        }

        return data;
    }

    public async Task<IndoorStatusData> SaveIndoorStatus(IndoorStatusData status){
        DateTime threeMinutesAgo = DateTime.Now.AddMinutes(-3);
        var ReturnData = new IndoorStatusData();
        var statusData = dbm.GetData<IndoorStatusData>(1);
        var dataExists = true;
        //get cached data
        var data = statusData.Where( x=> x.Id == "1" ).ToList().FirstOrDefault();

        // if the data does not exist or is old we need to refresh the data
        if (data == null ) {
            
            data = new IndoorStatusData();
            dataExists = false;
        }

        data.Data = status.Data;
        data.LastSet = DateTime.Now;
            
        if (dataExists){
            dbm.UpdateData<IndoorStatusData>(data);
        }else{
            dbm.AddData<IndoorStatusData>(data);
        }

        return data;
    }
}