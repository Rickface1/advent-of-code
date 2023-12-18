namespace advent.y2023;

public class DaySeventeen(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        args.ToList().ForEach(data => Console.WriteLine(data));
    }
}
        