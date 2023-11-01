namespace KioskApi.Enums;

public enum TimeOfDay {
    Dawn,
    Dusk,
    Day,
    Night,
    Unknown
}

public enum WindType {
    None,
    Light,
    Medium,
    Heavy
}

public enum Intensity {
    Race,
    Workout,
    Long,
    Normal,
    Easy

}

public enum Feel {
    None,
    Cool,
    Warm
}

public static class Enums
{
    public static Feel ConvertStringToFeel(string feel){
        Feel returnVal = Feel.None;
        switch (feel){
            case "cool":
                returnVal = Feel.Cool;
                break;
            case "warm":
                returnVal = Feel.Warm;
                break;
        }
        return returnVal;
    }

    public static Intensity ConvertStringToIntensity(string? intensity){
        Intensity returnVal = Intensity.Normal;
        
        switch (intensity ?? "Easy"){
            case "Easy":
                returnVal = Intensity.Easy;
                break;
            case "Long":
                returnVal = Intensity.Race;
                break;
            case "Race":
                returnVal = Intensity.Long;
                break;
            case "Workout":
                returnVal = Intensity.Workout;
                break;
        }
        return returnVal;
    }
}