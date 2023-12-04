
namespace y2023;

public class DayFour(string filePath) : main.CalendarCode(filePath){
    public override void Execute(){
        string[] args = ReadAllLines();
        Func<(int,int)> ParseFunction = () => {
            return StartParse(args);
        };
        var result = IterateWithTime<(int,int)>(ParseFunction, 1000,50);

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
    public static (int, int) StartParse(string[] lines){
        int PartOneTotal = 0;
        int PartTwoTotal = 0;

        int CurrentNumberOfCards = 0;
        List<int> NextCards = [
            1,1,1,1,1,1,1,1,1,1
        ];

        for(int line = 0; line < lines.Length; line++){
            PartTwoTotal += CurrentNumberOfCards = NextCards[0];
            LeftShiftArray<int>(ref NextCards, 1);

            char[] CurrentLine = lines[line][(lines[line].IndexOf(':') + 2)..].ToCharArray();
            int BarIndex = Array.IndexOf(CurrentLine,'|');

            List<int> CurrentTickets = [];
            List<int> CurrentWinners = [];

            for(int character = 0; character < CurrentLine.Length; character++){
                char CurrentChar = CurrentLine[character];
                if(char.IsNumber(CurrentChar)){
                    int endIndex = Array.IndexOf(CurrentLine[character..], ' ');
                    endIndex = endIndex == -1 ? CurrentLine.Length : endIndex + character;
                    if(character < BarIndex){
                        CurrentTickets.Add(int.Parse(CurrentLine.AsSpan()[character..endIndex]));
                    }else{
                        CurrentWinners.Add(int.Parse(CurrentLine.AsSpan()[character..endIndex]));
                    }
                    character = endIndex;
                }
            }

            int Intersections = Enumerable.Intersect(CurrentTickets, CurrentWinners).Count();

            if(Intersections > 0)
                PartOneTotal += (int)Math.Pow(2, (Intersections - 1));
            for(int i = 0; i < Intersections; i++){
                NextCards[i] += CurrentNumberOfCards;
            }
            
        }

        return (PartOneTotal, PartTwoTotal);
    }
}
        