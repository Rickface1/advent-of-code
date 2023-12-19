using System.Text;
using advent;
using advent.resources;


namespace advent.y2023;

public class DayTen(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        ExecutePartOne(args);
        PrintLines(2);
        ExecutePartTwo(args);
        PrintLines(2);
        ExecuteBoth(args);
    }

    public static void ExecuteBoth(string[] args) => PrintIterations((long, long)() => ParseBoth(args), 10000, 500);

    public static void ExecutePartOne(string[] args) => PrintIterations(int() => ParsePartOne(args), 10000, 500);

    public static void ExecutePartTwo(string[] args) => PrintIterations(long() => Area.Inside(PrepPartTwo(args)), 10000, 500);

    public static (long, long) ParseBoth(string[] args){
        List<MapIndex> indices = PrepPartTwo(args);
        long length = MapIndex.GetTotalLength(indices)/2L;
        return (length, Area.Shoelace(indices) - length + 1);
    }
    public static List<MapIndex> PrepPartTwo(string[] args){
        List<List<char>> Map = args.Select(data => data.ToCharArray().ToList()).ToList();
        (int, int) StartingIndex = (-1, -1);

        for(int line = 0; line < Map.Count; line++){
            int TempValue = Map[line].IndexOf('S');
            if(TempValue != -1){
                StartingIndex = (line, TempValue);
                break;
            }
        }

        char CurrentSymbol = 'S';
        (int, int) CurrentIndex = (0, 0);
        Direction CurrentDirection = Direction.North;

        if(GetNode(Map[StartingIndex.Item1 + 1][StartingIndex.Item2]).Touching(Direction.North)){
            CurrentSymbol = Map[StartingIndex.Item1 + 1][StartingIndex.Item2];
            CurrentIndex = (StartingIndex.Item1 + 1, StartingIndex.Item2);
            CurrentDirection = Direction.South;
        }else if(GetNode(Map[StartingIndex.Item1][StartingIndex.Item2 + 1]).Touching(Direction.West)){
            CurrentSymbol = Map[StartingIndex.Item1][StartingIndex.Item2 + 1];
            CurrentIndex = (StartingIndex.Item1, StartingIndex.Item2 + 1);
            CurrentDirection = Direction.East;
        }else if(GetNode(Map[StartingIndex.Item1 - 1][StartingIndex.Item2]).Touching(Direction.South)){
            CurrentSymbol = Map[StartingIndex.Item1 - 1][StartingIndex.Item2];
            CurrentIndex = (StartingIndex.Item1 - 1, StartingIndex.Item2);
            CurrentDirection = Direction.North;
        }else if(GetNode(Map[StartingIndex.Item1][StartingIndex.Item2 - 1]).Touching(Direction.East)){
            CurrentSymbol = Map[StartingIndex.Item1][StartingIndex.Item2 - 1];
            CurrentIndex = (StartingIndex.Item1, StartingIndex.Item2 - 1);
            CurrentDirection = Direction.West;
        }

        List<(int, int)> Indexes = [
            StartingIndex
        ];
        List<Direction> directions = [];
        Node CurrentNode = GetNode(CurrentSymbol);

        while(CurrentSymbol != 'S'){
            CurrentNode = GetNode(CurrentSymbol);

            CurrentDirection = CurrentNode.NextPlace(CurrentDirection);

            if(CurrentSymbol == 'L' || CurrentSymbol == '7' || CurrentSymbol == 'F' || CurrentSymbol == 'J'){
                Indexes.Add(CurrentIndex);
            }

            if(CurrentDirection == Direction.North){
                CurrentIndex = (CurrentIndex.Item1 - 1, CurrentIndex.Item2);
            }else if(CurrentDirection == Direction.East){
                CurrentIndex = (CurrentIndex.Item1, CurrentIndex.Item2 + 1);
            }else if(CurrentDirection == Direction.West){
                CurrentIndex = (CurrentIndex.Item1, CurrentIndex.Item2 - 1);
            }else if(CurrentDirection == Direction.South){
                CurrentIndex = (CurrentIndex.Item1 + 1, CurrentIndex.Item2);
            }

            directions.Add(CurrentDirection);

            CurrentSymbol = Map[CurrentIndex.Item1][CurrentIndex.Item2];
        }

        Map[StartingIndex.Item1][StartingIndex.Item2] = GetSymbol(directions[0], Node.Opposite(directions[^1]));

        return Indexes.Select(data => new MapIndex(data)).ToList();
    }

    public static int ParsePartOne(string[] args){
        List<List<char>> Map = args.Select(data => data.ToCharArray().ToList()).ToList();
        (int, int) StartingIndex = (-1, -1);

        for(int line = 0; line < Map.Count; line++){
            int TempValue = Map[line].IndexOf('S');
            if(TempValue != -1){
                StartingIndex = (line, TempValue);
                line = Map.Count;
                break;
            }
        }

        char CurrentSymbol = 'S';
        (int, int) CurrentIndex = (0, 0);
        Direction CurrentDirection = Direction.North;

        if(GetNode(Map[StartingIndex.Item1 + 1][StartingIndex.Item2]).Touching(Direction.North)){
            CurrentSymbol = Map[StartingIndex.Item1 + 1][StartingIndex.Item2];
            CurrentIndex = (StartingIndex.Item1 + 1, StartingIndex.Item2);
            CurrentDirection = Direction.South;
        }else if(GetNode(Map[StartingIndex.Item1][StartingIndex.Item2 + 1]).Touching(Direction.West)){
            CurrentSymbol = Map[StartingIndex.Item1][StartingIndex.Item2 + 1];
            CurrentIndex = (StartingIndex.Item1, StartingIndex.Item2 + 1);
            CurrentDirection = Direction.East;
        }else if(GetNode(Map[StartingIndex.Item1 - 1][StartingIndex.Item2]).Touching(Direction.South)){
            CurrentSymbol = Map[StartingIndex.Item1 - 1][StartingIndex.Item2];
            CurrentIndex = (StartingIndex.Item1 - 1, StartingIndex.Item2);
            CurrentDirection = Direction.North;
        }else if(GetNode(Map[StartingIndex.Item1][StartingIndex.Item2 - 1]).Touching(Direction.East)){
            CurrentSymbol = Map[StartingIndex.Item1][StartingIndex.Item2 - 1];
            CurrentIndex = (StartingIndex.Item1, StartingIndex.Item2 - 1);
            CurrentDirection = Direction.West;
        }

        List<Node> Nodes = [];
        Node CurrentNode = GetNode(CurrentSymbol);
        Direction LastDirection = Direction.North;

        while(CurrentSymbol != 'S'){
            CurrentNode = GetNode(CurrentSymbol);
            Nodes.Add(CurrentNode);

            CurrentDirection = CurrentNode.NextPlace(CurrentDirection);

            if(CurrentDirection == Direction.North){
                CurrentIndex = (CurrentIndex.Item1 - 1, CurrentIndex.Item2);
            }else if(CurrentDirection == Direction.East){
                CurrentIndex = (CurrentIndex.Item1, CurrentIndex.Item2 + 1);
            }else if(CurrentDirection == Direction.West){
                CurrentIndex = (CurrentIndex.Item1, CurrentIndex.Item2 - 1);
            }else if(CurrentDirection == Direction.South){
                CurrentIndex = (CurrentIndex.Item1 + 1, CurrentIndex.Item2);
            }

            CurrentSymbol = Map[CurrentIndex.Item1][CurrentIndex.Item2];
            LastDirection = CurrentDirection;
        }

        return (int)(((double)Nodes.Count / 2) + .5);
    }

    public static Node GetNode(char c){
        if(c == '|')
            return Pipes.Vertical;
        else if(c == '-')
            return Pipes.Horizontal;
        else if(c == 'L')
            return Pipes.TopRight;
        else if(c == '7')
            return Pipes.BottomLeft;
        else if(c == 'F')
            return Pipes.BottomRight;
        else if(c == 'J')
            return Pipes.TopLeft;
        else
            throw new Exception($"NOT VALID NODE: '{c}'");
    }

    public static char GetSymbol(Direction direction1, Direction direction2){
        List<Direction> directions = [
            direction1,
            direction2
        ];

        if(directions.Contains(Direction.North)){
            directions.Remove(Direction.North);
            Direction direction = directions[0];

            if(direction == Direction.East){
                return 'L';
            }else if(direction == Direction.West){
                return 'J';
            }else if(direction == Direction.South){
                return '|';
            }
        }else if(directions.Contains(Direction.South)){
            directions.Remove(Direction.South);
            Direction direction = directions[0];

            if(direction == Direction.East){
                return 'F';
            }else if(direction == Direction.West){
                return '7';
            }
        }else{
            return '-';
        }

        throw new Exception("DIRECTION NOT FOUND");
    }
}

public static class Pipes{
    public static readonly Node Vertical = new(Direction.North, Direction.South);
    public static readonly Node Horizontal = new(Direction.East, Direction.West);
    public static readonly Node TopRight = new(Direction.North, Direction.East);
    public static readonly Node TopLeft = new(Direction.North, Direction.West);
    public static readonly Node BottomLeft = new(Direction.South, Direction.West);
    public static readonly Node BottomRight = new(Direction.South, Direction.East);
}

public class Node(Direction RootOne, Direction RootTwo){
    public Direction RootOne = RootOne;
    public Direction RootTwo = RootTwo;

    public bool Touching(Node node){
        return node.RootOne == RootOne || node.RootTwo == RootTwo || node.RootOne == RootTwo;
    }

    public bool Touching(Direction direction){
        return direction == RootOne || direction == RootTwo;
    }

    public Direction NextPlace(Direction CurrentDirection){
        return CurrentDirection == Opposite(RootOne) ? RootTwo : RootOne;
    }

    public override string ToString(){
        return $"({RootOne}, {RootTwo})";
    }

    public static Direction Opposite(Direction direction){
        if(direction == Direction.North){
            return Direction.South;
        }else if(direction == Direction.East){
            return Direction.West;
        }else if(direction == Direction.South){
            return Direction.North;
        }else if(direction == Direction.West){
            return Direction.East;
        }

        throw new Exception("OPPOSITE FUNCTION ERROR");
    }
}
