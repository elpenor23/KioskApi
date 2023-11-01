namespace KioskApi.Models;

public class IndoorStatusData : IModel
{
    public IndoorStatusData(){
        this.Id = "1";
        this.Data = "XX";
        this.LastSet = DateTime.Now;
    }
    public IndoorStatusData(string data = "XX", DateTime? lastSet = null)
    {
        if (lastSet == null) {lastSet = DateTime.Now;}

        this.Id = "1";
        this.Data = data;
        this.LastSet = lastSet.Value;
    }

    public string? Id { get; }
    public string? Data { get; set; }
    public DateTime? LastSet { get; set; }
    public string LastSetString
    {
        get
        {
            return (LastSet ?? DateTime.Now).ToString("yyyy-MM-ddTHH:mm:ss");
        }
    }
}