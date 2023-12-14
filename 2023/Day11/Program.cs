using advent;

namespace advent.y2023;

public class DayEleven(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        Func<long> ParseFunction = () => {
            return Parse(args);
        };

        var Result = IterateWithTime(ParseFunction, 100, 10);
        //var Result = IterateOnce(ParseFunction);

        Console.WriteLine("--- TIME ---");
        Console.WriteLine(Result["time"]);

        Console.WriteLine();

        Console.WriteLine("--- DATA ---");
        Console.WriteLine(Result["data"]);
    }

    public long Parse(string[] args){
        List<List<char>> Map = args.Select(data => data.ToCharArray().ToList()).ToList();
        DoubleMap map = new();

        var (EmptyRows, EmptyColumns) = FindEmpty(Map);

        int increment = 1000000;
        
        for(int x = 0; x < Map.Count; x++){
            if(EmptyRows.Contains(x)){
                map.rows.Add(increment);
            }else{
                map.rows.Add(1);
            }

            if(EmptyColumns.Contains(x)){
                map.columns.Add(increment);
            }else{
                map.columns.Add(1);
            }
        }



        List<Galaxy> Galaxies = [];

        for(int line = 0; line < Map.Count; line++){
            for(int column = 0; column < Map[line].Count; column++){
                if(Map[line][column] == '#')
                    Galaxies.Add(new Galaxy(line, column));
            }
        }

        long total = 0;

        for(int outer = 0; outer < Galaxies.Count; outer++){
            for(int inner = outer + 1; inner < Galaxies.Count; inner++){
                total += map.DistanceBetween(Galaxies[outer].GetValues(), Galaxies[inner].GetValues());
            }
        }

        if(total < 0){
            Console.WriteLine("WHY");
        }

        return total;
    }

    public static (List<int> EmptyRows, List<int> EmptyColumns) FindEmpty(List<List<char>> Map){
        List<int> EmptyRows = [];
        
        for(int row = 0; row < Map.Count; row++){
            if(!Map[row].Any(data => data == '#')){
                EmptyRows.Add(row);
            }
        }

        List<int> EmptyColumns = [];

        for(int column = 0; column < Map[0].Count; column++){
            List<char> Column = Map.Select(data => data[column]).ToList();
            if(!Column.Any(data => data == '#')){
                EmptyColumns.Add(column);
            }
        }

        return (EmptyRows, EmptyColumns);
    }
}

public class DoubleMap{
    public List<int> rows;
    public List<int> columns;

    public DoubleMap(){
        rows = [];
        columns = [];
    }

    public DoubleMap(List<int> rows, List<int> columns){
        this.rows = rows;
        this.columns = columns;
    }

    public long DistanceBetween((int, int) index1, (int, int) index2){
        List<int> TempRows = rows.GetRange(Math.Min(index1.Item1, index2.Item1), Math.Abs(index1.Item1 - index2.Item1));
        List<int> TempColumns = columns.GetRange(Math.Min(index1.Item2, index2.Item2), Math.Abs(index1.Item2 - index2.Item2));

        long RowValue = 0;
        long ColumnValue = 0;

        if(TempRows.Count > 0){
            RowValue = TempRows.Sum();
        }
        
        if(TempColumns.Count > 0){
            ColumnValue = TempColumns.Sum();
        }

        return RowValue + ColumnValue;
    }
}

public class Galaxy(int line, int column){
    public readonly int line = line;
    public readonly int column = column;

    public int Distance(Galaxy galaxy){
        return Math.Abs(galaxy.line - line) + Math.Abs(galaxy.column - column);
    }

    public (int, int) GetValues(){
        return (line, column);
    }

    public override string ToString(){
        return $"({line}, {column})";
    }
}