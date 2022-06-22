namespace KioskApi.Models;

public class PersonsClothing
{
    public PersonsClothing(Person _person){
        this.Person = _person;
        this.Intensity = new List<IntensityClothing>();
    }
    public Person Person { get; set; }
    public List<IntensityClothing> Intensity { get; set; }

}