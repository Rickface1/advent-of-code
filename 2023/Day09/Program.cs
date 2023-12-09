namespace y2023;

public class DayNine(string filePath) : main.CalendarCode(filePath){
    public override void Execute(string[] args){
        PartOneMain(args);

        PrintLines(2);

        PartTwoMain(args);
    }

    public static void PartOneMain(string[] args){
        Func<int> ParseFunction = () => {
            return PartOneExecute(args);
        };

        var Result = IterateWithTime(ParseFunction,1000,10);
        //var Result = IterateOnce(ParseFunction);

        Console.WriteLine("--- TIME ---");
        Console.WriteLine(Result["time"]);

        Console.WriteLine();

        Console.WriteLine("--- DATA ---");
        Console.WriteLine(Result["data"]);
    }

    public static void PartTwoMain(string[] args){
        Func<int> ParseFunction = () => {
            return PartTwoExecute(args);
        };

        var Result = IterateWithTime(ParseFunction,1000,10);
        //var Result = IterateOnce(ParseFunction);

        Console.WriteLine("--- TIME ---");
        Console.WriteLine(Result["time"]);

        Console.WriteLine();

        Console.WriteLine("--- DATA ---");
        Console.WriteLine(Result["data"]);
    }

    public static int PartTwoExecute(string[] args){
        Span<List<int>> LineList = args.Select(data => data.Split(' ').Select(int.Parse).ToList()).ToArray().AsSpan();

        int total = 0;

        for(int x = 0; x < LineList.Length; x++){
            total += PartTwoParse(LineList[x]);
        }

        return total;
    }

    public static int PartTwoParse(List<int> CurrentLine){
        List<List<int>> AllLines = [
            CurrentLine
        ];

        while(CurrentLine.Any(data => data != 0)){
            List<int> NewLine = [];

            for(int x = 0; x < CurrentLine.Count - 1; x++){
                NewLine.Add(CurrentLine[x + 1] - CurrentLine[x]);
            }

            AllLines.Add(NewLine);

            CurrentLine = NewLine;
        }

        int PreviousData = 0;

        for(int x = AllLines.Count - 1; x >= 0; x--){
            PreviousData = AllLines[x].First() - PreviousData;
        }

        return PreviousData;
    }

    public static int PartOneExecute(string[] args){
        Span<List<int>> LineList = args.Select(data => data.Split(' ').Select(int.Parse).ToList()).ToArray().AsSpan();

        int total = 0;

        for(int x = 0; x < LineList.Length; x++){
            total += PartOneParse(LineList[x]);
        }

        return total;
    }

    public static int PartOneParse(List<int> CurrentLine){
        List<List<int>> AllLines = [
            CurrentLine
        ];

        while(CurrentLine.Any(data => data != 0)){
            List<int> NewLine = [];

            for(int x = 0; x < CurrentLine.Count - 1; x++){
                NewLine.Add(CurrentLine[x + 1] - CurrentLine[x]);
            }

            AllLines.Add(NewLine);

            CurrentLine = NewLine;
        }

        int PreviousData = 0;

        for(int x = AllLines.Count - 1; x >= 0; x--){
            PreviousData += AllLines[x].Last();
        }

        return PreviousData;
    }
}
        