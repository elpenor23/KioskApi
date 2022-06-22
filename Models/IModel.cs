namespace KioskApi.Models;
public interface IModel 
{
    [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
    string? Id { get; }
}
