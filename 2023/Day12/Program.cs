using advent;

namespace advent.y2023;

public class DayTwelve(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        ParsePartOne(args);
    }

    public static void ParsePartOne(string[] args){
        int total = 0;

        for(int x = 0; x < args.Length; x++){
            total += Combinations([.. args[x].ToCharArray()]);
        }

        Console.WriteLine(total);
    }

    public static int Combinations(List<char> input){
        (List<List<char>> springs, List<int> groups) = GetArrays(input);
        
        return 0;
    }

    public static (List<List<char>>, List<int>) GetArrays(List<char> input){
        List<List<char>> split = [];

        List<char> Map1 = input[.. input.IndexOf(' ')];

        int CurrentIndex = Map1.IndexOf('.');
        while(CurrentIndex != -1){
            split.Add(Map1[.. CurrentIndex]);
            Map1.RemoveRange(0, CurrentIndex + 1);
            CurrentIndex = Map1.IndexOf('.');
        }

        split.Add(Map1);

        List<char> Map2 = input[(input.IndexOf(' ') + 1) ..];
        List<int> Numbers = [];

        CurrentIndex = Map2.IndexOf(',');
        while(CurrentIndex != -1){
            Numbers.Add(int.Parse(new string(Map2[.. CurrentIndex].ToArray())));
            Map2.RemoveRange(0, CurrentIndex + 1);
            CurrentIndex = Map2.IndexOf(',');
        }

        Numbers.Add(int.Parse(new string(Map2.ToArray())));

        return (split, Numbers);
    }
}
        