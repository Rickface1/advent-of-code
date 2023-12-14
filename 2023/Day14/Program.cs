using advent.resources;

namespace advent.y2023;

public class DayFourteen(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        ExecutePartOne(args);
    }
    public static void ExecutePartOne(string[] args){
        Func<int> ParseFunction = () => {
            return ParsePartOne(args);
        };

        var Result = IterateWithTime(ParseFunction, 100, 10);

        Console.WriteLine("--- TIME ---");
        Console.WriteLine(Result["time"]);

        Console.WriteLine();

        Console.WriteLine("--- DATA ---");
        Console.WriteLine(Result["data"]);
    }

    public static int ParsePartOne(string[] args){
        var (CubeRocks, RoundRocks) = GetRocks(args);
        int RowLength = args.Length;
        int ColumnLength = args[0].Length;

        TiltBoard(CubeRocks, RoundRocks, RowLength, ColumnLength, Direction.North);

        /*for(int row = 0; row < RowLength; row++){
            for(int column = 0; column < ColumnLength; column++){
                if(CubeRocks.Contains((row, column))){
                    Console.Write('#');
                }else if(RoundRocks.Contains((row, column))){
                    Console.Write('O');
                }else{
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }*/

        return GetValues(RoundRocks, RowLength);
    }

    public static int GetValues(List<(int, int)> RoundRocks, int Length){
        return RoundRocks.Sum(data => Length - data.Item1);
    }

    public static void TiltBoard(List<(int, int)> CubeRocks, List<(int, int)> RoundRocks, int RowLength, int ColumnLength, Direction direction){
        if(direction == Direction.North){
            //Sort by rows
            RoundRocks.Sort((item1, item2) => item1.Item1.CompareTo(item2.Item1));

            for(int x = 0; x < RoundRocks.Count; x++){
                List<(int, int)> rocks = [];
                rocks.AddRange(CubeRocks.Where(data => data.Item2 == RoundRocks[x].Item2));
                rocks.AddRange(RoundRocks.Where(data => data.Item2 == RoundRocks[x].Item2));

                List<int> CurrentValues = rocks.Select(data => data.Item1)
                    .Where(data => data < RoundRocks[x].Item1)
                    .ToList();


                RoundRocks[x] = (CurrentValues.Count > 0 ? CurrentValues.Max() + 1 : 0, RoundRocks[x].Item2);
            }
        }else if(direction == Direction.East){
            //Sort by columns
            RoundRocks.Sort((item1, item2) => item2.Item2.CompareTo(item1.Item2));

            for(int x = 0; x < RoundRocks.Count; x++){
                List<(int, int)> rocks = [];
                rocks.AddRange(CubeRocks.Where(data => data.Item1 == RoundRocks[x].Item1));
                rocks.AddRange(RoundRocks.Where(data => data.Item1 == RoundRocks[x].Item1));

                List<int> CurrentValues = rocks.Select(data => data.Item2)
                                    .Where(data => data > RoundRocks[x].Item2)
                                    .ToList();


                RoundRocks[x] = (RoundRocks[x].Item1, CurrentValues.Count > 0 ? CurrentValues.Min() - 1 : RowLength - 1);
            }
        }else if(direction == Direction.West){
            //Sort by columns
            RoundRocks.Sort((item1, item2) => item1.Item2.CompareTo(item2.Item2));

            for(int x = 0; x < RoundRocks.Count; x++){
                List<(int, int)> rocks = [];
                rocks.AddRange(CubeRocks.Where(data => data.Item1 == RoundRocks[x].Item1));
                rocks.AddRange(RoundRocks.Where(data => data.Item1 == RoundRocks[x].Item1));

                List<int> CurrentValues = rocks.Select(data => data.Item2)
                                    .Where(data => data < RoundRocks[x].Item2)
                                    .ToList();


                RoundRocks[x] = (RoundRocks[x].Item1, CurrentValues.Count > 0 ? CurrentValues.Max() + 1 : 0);
            }
        }else if(direction == Direction.South){
            //Sort by rows
            RoundRocks.Sort((item1, item2) => item2.Item1.CompareTo(item1.Item1));

            for(int x = 0; x < RoundRocks.Count; x++){
                List<(int, int)> rocks = [];
                rocks.AddRange(CubeRocks.Where(data => data.Item2 == RoundRocks[x].Item2));
                rocks.AddRange(RoundRocks.Where(data => data.Item2 == RoundRocks[x].Item2));

                List<int> CurrentValues = rocks.Select(data => data.Item1)
                                    .Where(data => data > RoundRocks[x].Item1)
                                    .ToList();


                RoundRocks[x] = (CurrentValues.Count > 0 ? CurrentValues.Min() - 1 : ColumnLength - 1, RoundRocks[x].Item2);
            }
        }
    }

    public static (List<(int, int)>, List<(int, int)>) GetRocks(string[] args){
        List<(int, int)> CubeRocks = [];
        List<(int, int)> RoundRocks = [];

        for(int row = 0; row < args.Length; row++){
            for(int column = 0; column < args[row].Length; column++){
                if(args[row][column] == '#'){
                    CubeRocks.Add((row, column));
                }else if(args[row][column] == 'O'){
                    RoundRocks.Add((row, column));
                }
            }
        }

        return (CubeRocks, RoundRocks);
    }
}
        