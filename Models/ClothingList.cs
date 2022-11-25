namespace KioskApi.Models;

public class ClothingList : IModel
{
    public ClothingList() {
        this.Clothing = new List<ClothingItem>();
    }
    public string? Id { get; set; }
    public List<ClothingItem> Clothing { get; set; }
    
}