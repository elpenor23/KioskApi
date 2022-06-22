namespace KioskApi.Models;

public class BodyPart : IModel
{
    [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
    public string? Id { get; set; }
}