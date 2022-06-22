using KioskApi.Enums;
namespace KioskApi.Models;

public class Person
{
    public string? Id { get; set; }
    public Feel? Feel { get; set; }
    public string? Name { get; set; }
    public string? Color { get; set; }
}