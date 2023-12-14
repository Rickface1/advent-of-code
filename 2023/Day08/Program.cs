
using System.Diagnostics;
using advent;

namespace advent.y2023;

public class DayEight(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        ExecutePartOne(args);
        PrintLines(3);

        ExecutePartTwo(args);
        PrintLines(3);

        ExecutePartTwoParallel(args);
    }

    public static void ExecutePartOne(string[] args){
        Func<int> ParseFunction = () => {
            return ParsePartOne(args);
        };
        var Result = IterateWithTime(ParseFunction,1000,10);
        //var Result = IterateOnce(ParseFunction);

        Console.WriteLine("--- TIME ---");
        Console.WriteLine(Result["time"]);

        Console.WriteLine();

        Console.WriteLine("--- DATA ---");
        Console.WriteLine(Result["data"]);
    }

    public static void ExecutePartTwo(string[] args){
        Func<long> ParseFunction = () => {
            return ParsePartTwo(args);
        };
        var Result = IterateWithTime(ParseFunction,1000,10);
        //var Result = IterateOnce(ParseFunction);

        Console.WriteLine("--- TIME ---");
        Console.WriteLine(Result["time"]);

        Console.WriteLine();

        Console.WriteLine("--- DATA ---");
        Console.WriteLine(Result["data"]);
    }

    public static void ExecutePartTwoParallel(string[] args){
        Func<long> ParseFunction = () => {
            return ParsePartTwoParallel(args);
        };
        var Result = IterateWithTime(ParseFunction,1000,10);
        //var Result = IterateOnce(ParseFunction);

        Console.WriteLine("--- TIME ---");
        Console.WriteLine(Result["time"]);

        Console.WriteLine();

        Console.WriteLine("--- DATA ---");
        Console.WriteLine(Result["data"]);
    }

    public static long ParsePartTwo(string[] args){
        ReadOnlySpan<char> Directions = [.. args[0].ToCharArray()];
        Dictionary<string, LeftRight> Dictionary = [];
        List<string> StartingValues = [];

        for(int x = 2; x < args.Length; x++){
            Dictionary.Add(args[x][..3], new LeftRight(args[x][7..10], args[x][12..15]));
            if(args[x][2] == 'A'){
                StartingValues.Add(args[x][..3]);
            }
        }

        List<long> LCM = [];

        //BULK OF TIME
        for(int x = 0; x < StartingValues.Count; x++){
            LCM.Add(PartTwoFindIterations(Directions, Dictionary, StartingValues[x]));
        }

        return LeastCommonMultiple(LCM);
    }

    public static long ParsePartTwoParallel(string[] args){
        List<char> Directions = [.. args[0].ToCharArray()];
        Dictionary<string, LeftRight> Dictionary = [];
        List<string> StartingValues = [];

        for(int x = 2; x < args.Length; x++){
            Dictionary.Add(args[x][..3], new LeftRight(args[x][7..10], args[x][12..15]));
            if(args[x][2] == 'A'){
                StartingValues.Add(args[x][..3]);
            }
        }

        List<long> LCM = [];
        object LockedObject = new();

        //BULK OF TIME
        Parallel.For(0, StartingValues.Count, x => {
            var Result = PartTwoFindIterations(Directions, Dictionary, StartingValues[x]);
            lock(LockedObject){
                LCM.Add(Result);
            }
        });

        return LeastCommonMultiple(LCM);
    }

    public static int PartTwoFindIterations(List<char> Directions, Dictionary<string, LeftRight> Dictionary, string CurrentPosition){
        int Iterations = 0;
        int CurrentDirection = 0;

        while(CurrentPosition.Last() != 'Z'){
            CurrentPosition = Dictionary[CurrentPosition].GetValue(Directions[CurrentDirection]);
            CurrentDirection = CurrentDirection < Directions.Count - 1 ? CurrentDirection + 1 : 0;
            Iterations++;
        }
        return Iterations;    
    }


    public static int PartTwoFindIterations(ReadOnlySpan<char> Directions, Dictionary<string, LeftRight> Dictionary, string CurrentPosition){
        int Iterations = 0;
        int CurrentDirection = 0;

        while(CurrentPosition.Last() != 'Z'){
            CurrentPosition = Dictionary[CurrentPosition].GetValue(Directions[CurrentDirection]);
            CurrentDirection = CurrentDirection < Directions.Length - 1 ? CurrentDirection + 1 : 0;
            Iterations++;
        }
        return Iterations;    
    }

    public static int ParsePartOne(string[] args){
        Span<char> Directions = [.. args[0].ToCharArray()];
        Dictionary<string, LeftRight> Dictionary = [];
        Directions = [.. args[0].ToCharArray()];

        for(int x = 2; x < args.Length; x++){
            Dictionary.Add(args[x][..3], new LeftRight(args[x][7..10], args[x][12..15]));
        }
        return FindIterations(Directions, Dictionary);
    }

    public static int FindIterations(Span<char> Directions, Dictionary<string, LeftRight> Dictionary){
        string CurrentPosition = "AAA";
        int CurrentDirection = 0;
        int Iterations = 0;

        while(CurrentPosition != "ZZZ"){
            CurrentPosition = Dictionary[CurrentPosition].GetValue(Directions[CurrentDirection]);
            CurrentDirection = CurrentDirection < Directions.Length - 1 ? CurrentDirection + 1 : 0;
            Iterations++;
        }
        return Iterations;
    }
}

public class LeftRight(string left, string right){
    public string left = left;
    public string right = right;

    public string GetValue(char d){
        if(d == 'L'){
            return left;
        }else{
            return right;
        }
    }

    public override string ToString(){
        return $"({left}, {right})";
    }
}