using advent;

namespace advent.y2023;

public class DayFour(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        Func<(int,int)> ParseFunction = () => {
            return StartParse(args.AsSpan());
        };
        var result = IterateWithTime<(int,int)>(ParseFunction, 50000,500);

        var ParseResult = (ValueTuple<int, int>)(result["data"] ?? (0,0));

        Console.WriteLine("---AVG. TIME ELAPSED---");
        Console.WriteLine(result["time"]);

        Console.WriteLine();

        Console.WriteLine("---PART 1 OUTPUT---");
        Console.WriteLine(ParseResult.Item1);

        Console.WriteLine();

        Console.WriteLine("--- PART 2 OUTPUT---");
        Console.WriteLine(ParseResult.Item2);
    }
    public static (int, int) StartParse(ReadOnlySpan<string> lines){
        int PartOneTotal = 0;
        int PartTwoTotal = 0;

        int CurrentNumberOfCards = 0;
        List<int> NextCards = [
            1,1,1,1,1,1,1,1,1,1
        ];

        for(int line = 0; line < lines.Length; line++){
            PartTwoTotal += CurrentNumberOfCards = NextCards[0];
            LeftShiftArray<int>(NextCards, 1);

            ReadOnlySpan<char> DumpLine = lines[line].ToCharArray().AsSpan();

            ReadOnlySpan<char> CurrentLine = DumpLine[10..];

            List<int> CurrentTickets = [];
            List<int> CurrentWinners = [];

            for(int character = 0; character < 30; character++){
                CurrentTickets.Add(int.Parse(CurrentLine[character..(2 + character)]));
                character += 2;
            }

            for(int character = 32; character < 106; character++){
                CurrentWinners.Add(int.Parse(CurrentLine[character..(2 + character)]));
                character += 2;
            }

            

            int Intersections = Enumerable.Intersect(CurrentTickets, CurrentWinners).Count();

            if(Intersections > 0)
                PartOneTotal += (int)Math.Pow(2, Intersections - 1);
            for(int i = 0; i < Intersections; i++)
                NextCards[i] += CurrentNumberOfCards;
        }

        return (PartOneTotal, PartTwoTotal);
    }
}
        