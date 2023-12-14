using advent;

namespace advent.y2023;

public class DayNine(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        BothParts(args);

        //PrintLines(2);

        //PartOneMain(args);

        //PrintLines(2);

        //PartTwoMain(args);
    }

    public static void BothParts(string[] args){
        Func<(int, int)> ParseFunction = () => {
            return BothPartMain(args);
        };

        var Result = IterateWithTime(ParseFunction,1000,10);
        (int, int) data = ((int, int))(Result["data"] ?? (0, 0));

        Console.WriteLine("--- TIME ---");
        Console.WriteLine(Result["time"]);

        Console.WriteLine();

        Console.WriteLine("--- DATA ---");

        Console.WriteLine();
        
        Console.WriteLine("--- PART ONE ---");
        Console.WriteLine(data.Item1);

        Console.WriteLine();

        Console.WriteLine("--- PART TWO ---");
        Console.WriteLine(data.Item2);
    }

    public static (int, int) BothPartMain(string[] args){
        List<List<int>> LineList = args.Select(data => data.Split(' ').Select(int.Parse).ToList()).ToList();

        int Pt1Total = 0;
        int Pt2Total = 0;

        Parallel.For(0, LineList.Count, x => {
            (int, int) Result = ParseBoth(LineList[x]);
            Interlocked.Add(ref Pt1Total, Result.Item1);
            Interlocked.Add(ref Pt2Total, Result.Item2);
        });

        return (Pt1Total, Pt2Total);
    }

    public static (int, int) ParseBoth(List<int> CurrentLine){
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

        int Pt2Data = 0;

        for(int x = AllLines.Count - 1; x >= 0; x--){
            Pt2Data = AllLines[x].First() - Pt2Data;
        }

        int Pt1Data = 0;

        for(int x = AllLines.Count - 1; x >= 0; x--){
            Pt1Data += AllLines[x].Last();
        }

        return (Pt1Data, Pt2Data);
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
        