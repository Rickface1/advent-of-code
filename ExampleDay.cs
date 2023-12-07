namespace y2023;

/// <summary>
/// Example Implementation of the CalendarCode class
///
/// Namespace of file must be y{insert year}, for example y2023
/// Name of inheriting class must be Day{insert day}, for example DayOne
/// Must implement method "Execute".
/// </summary>
/// <param name="filePath"></param>
public class DayNumber(string filePath) : main.CalendarCode(filePath){
    public override void Execute(string[] args){
        Console.WriteLine("Example Thing");
    }
}