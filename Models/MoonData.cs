namespace KioskApi.Models;

public class MoonData
{
    public MoonData(){}
    public MoonData(int index, string name, string icon, string length)
    {
        this.PhaseIndex = index;
        this.PhaseName = name;
        this.PhaseIcon = icon;
        this.DayLength = length;
    }
    public int? PhaseIndex { get; set; }
    public string? PhaseName { get; set; }
    public string? PhaseIcon { get; set; }
    public string? DayLength { get; set; }
}