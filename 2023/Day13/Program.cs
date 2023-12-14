using advent;

namespace advent.y2023;

public class DayThirteen(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        ExecutePartOne(args);
    }

    public static void ExecutePartOne(string[] args){
        List<string[]> arguments = [];

        int CurrentIndex = args.ToList().IndexOf("");
        for(int x = 0; x < args.Length; x++){
            if(CurrentIndex != -1){
                arguments.Add(args[.. CurrentIndex]);
                args = args[(CurrentIndex + 1) ..];

                x = CurrentIndex;

                CurrentIndex = args.ToList().IndexOf("");
            }else{
                break;
            }
        }

        arguments.Add(args);

        long total = 0;
        for(int x = 0; x < arguments.Count; x++){
            //if(ParsePartOne(arguments[x]) == 0) throw new Exception(x.ToString());

            total += ParsePartOne(arguments[x]);
        }

        Console.WriteLine(total);
    }

    public static int ParsePartOne(string[] args){
        List<string> rows = [.. args];

        List<string> columns = [];
        for(int column = 0; column < args[0].Length; column++){
            columns.Add(new string(args.Select(data => data[column]).ToArray()));
        }

        return GetReflection<string>(rows, columns);
    }

    public static int GetReflection<T>(List<T> rows, List<T> columns){
        //values1 is *100
        int Index = 0;

        for(int x = 1; x < rows.Count; x++){
            if(IsValid(rows, x)){
                Index = x;
            }
        }

        if(Index == 0){
            for(int x = 1; x < columns.Count; x++){
                if(IsValid(columns, x)){
                    Index = x;
                }
            }
        }

        return ClearSmudge<T>(rows, columns, Index);
    }

    public static int ClearSmudge<T>(List<T> rows, List<T> columns, int Index){
        //TODO
        return 0;
    }

    public static bool IsValid<T>(List<T> values, int Index){
        int Iterations = Math.Min(Index, values.Count - Index);
        T? CurrentValue = default;

        for(int x = 0; x < Iterations; x++){
            if((CurrentValue = values[Index + x]) == null || !CurrentValue.Equals(values[Index - x - 1])){
                return false;
            }
        }

        return true;
    }
}

public enum Line{
    Horizontal,
    Vertical,
}