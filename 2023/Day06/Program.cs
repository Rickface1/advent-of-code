
namespace y2023;

public class DaySix(string filePath) : main.CalendarCode(filePath){
    public override void Execute(string[] args){
        List<string> input = args.Select(str => str[11..]).ToList();
        Func<(int, long)> ParseFunction = () => {
            return BothParts(input);
        };

        var Result = IterateWithTime(ParseFunction, 10000000, 1000);

        var Data = (ValueTuple<int, long>)(Result["data"] ?? (0,0L));

        Console.WriteLine("--- OVERALL TIME ---");
        Console.WriteLine(Result["time"]);

        Console.WriteLine();

        Console.WriteLine("--- OUTPUT ---");
        Console.WriteLine(Data.Item1);
        Console.WriteLine(Data.Item2);

        Console.WriteLine();

        Func<long> PartOne = () => {
            return FirstPart(input);
        };

        var FirstPartResult = IterateWithTime(PartOne, 1000000, 1000);

        Console.WriteLine("--- PART ONE TIME ---");
        Console.WriteLine(FirstPartResult["time"]);

        Console.WriteLine();

        Func<long> PartTwo = () => {
            return SecondPart(input);
        };

        var SecondPartResult = IterateWithTime(PartTwo, 1000000, 1000);

        Console.WriteLine("--- PART TWO TIME ---");
        Console.WriteLine(SecondPartResult["time"]);

        Console.WriteLine();
    }

    public static (int, long) BothParts(List<string> args){
        ReadOnlySpan<int> Time = args[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray().AsSpan();
        ReadOnlySpan<int> Distance = args[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray().AsSpan();

        int total = 1;

        for(int x = 0; x < Time.Length; x++){
            total *= (int)Optimize(Time[x], Distance[x]);
        }

        return (total, Optimize(long.Parse(args[0].Replace(" ", "")), long.Parse(args[1].Replace(" ", ""))));
    }

    public static long SecondPart(List<string> args){
        return Optimize(long.Parse(args[0].Replace(" ", "")), long.Parse(args[1].Replace(" ", "")));
    }

    public static long FirstPart(List<string> args){        
        ReadOnlySpan<int> Time = args[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray().AsSpan();
        ReadOnlySpan<int> Distance = args[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray().AsSpan();

        long total = 1;

        for(int x = 0; x < Time.Length; x++){
            total *= Optimize(Time[x], Distance[x]);
        }

        return total;
    }

    public static long Optimize(long Time, long Distance){
        double RootTerm = Math.Sqrt(Math.Pow(Time,2) - 4 * Distance);

        double UpperBound = (Time + RootTerm)/2;

        double LowerBound = (Time - RootTerm)/2;

        if (RootTerm % 1 == 0){
            UpperBound -= 1;
            LowerBound += 1;
        }
        
        return (long)Math.Floor(UpperBound) - (long)Math.Ceiling(LowerBound) + 1;
    }
}
        