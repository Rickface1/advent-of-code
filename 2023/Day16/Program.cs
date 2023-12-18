using System.Diagnostics;
using advent.resources;

namespace advent.y2023;

public class DaySixteen(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        PartOne(args);

        PrintLines(1);

        PartTwo(args);
    }

    public static void PartTwo(string[] args){
        Func<int> ParseFunction = () => {
            return ExecutePartTwo(args);
        };

        PrintOneIteration(ParseFunction);
    }

    public static void PartOne(string[] args){
        Func<int> ParseFunction = () => {
            return ExecutePartOne(args);
        };

        PrintIterations(ParseFunction, 500, 10);
    }

    public static int ExecutePartOne(string[] args){
        return TestData(args, new(new(-1, 0), Direction.East));
    }

    public static int ExecutePartTwo(string[] args){
        BeamMap map = new(args);
        List<Beam> sides = map.GetSides();
        List<int> Max = [];

        object LockObject = new();

        Parallel.For(0, sides.Count, x =>{
            int result = TestData(args, sides[x]);

            lock(LockObject){
                Max.Add(result);
            }
        });

        return Max.Max();
    }

    public static int TestData(string[] args, Beam start){
        BeamMap map = new(args);
        Queue<Beam> lazers = [];
        HashSet<Beam> elapsed = new(new BeamEquality());
        List<Beam> backup = [];

        lazers.Enqueue(start);

        for(int y = 0; lazers.Count > 0; y++){
            Beam beam = lazers.Dequeue();
            List<Beam> beams = map.GetNext(beam);

            for(int x = 0; x < beams.Count; x++){
                if(!backup.Any(data => data.Equals(beams[x]))){
                    backup.Add(beams[x].Clone());
                    lazers.Enqueue(beams[x]);
                }
            }

        }
        return map.energized.Count;
    }
}

public class BeamMap : CharMap{
    public HashSet<MapIndex> energized;

    public BeamMap(string[] map) : base(map){
        energized = [];
    }
    public BeamMap(List<List<char>> map) : base(map){
        energized = [];
    }

    public List<Beam> GetSides(){
        int RowLength = GetRowBounds();
        int ColumnLength = GetColumnBounds();


        List<Beam> sides = [
            new Beam(new(0, -1), Direction.East),
            new Beam(new(-1, 0), Direction.South),
            new Beam(new(-1, ColumnLength - 1), Direction.South),
            new Beam(new(0, ColumnLength), Direction.West),
            new Beam(new(RowLength - 1, -1), Direction.East),
            new Beam(new(RowLength, 0), Direction.North),
            new Beam(new(RowLength, ColumnLength - 1), Direction.North),
            new Beam(new(RowLength - 1, ColumnLength), Direction.West)
        ];

        for(int x = 1; x < ColumnLength - 2; x++){
            sides.Add(new Beam(new(-1, x), Direction.South));
            sides.Add(new Beam(new(RowLength, x), Direction.North));
        }

        for(int x = 1; x < RowLength - 2; x++){
            sides.Add(new Beam(new(x, -1), Direction.East));
            sides.Add(new Beam(new(x, ColumnLength), Direction.North));
        }

        return sides;
    }
    
    public List<Beam> GetNext(Beam beam){
        beam.FollowThrough();

        if(beam.index.ValidIndex(map.Count, map[0].Count)){
            char next = GetCurrentValue(beam.index);
            energized.Add(beam.index.Clone());


            if(next == '.'){
                return [beam];
            }else if(next == '/' || next == '\\'){
                beam.Bounce(next);
                return [beam];
            }else if(next == '|'){
                if(beam.direction == Direction.East || beam.direction == Direction.West){
                    return [
                        new Beam(new MapIndex(beam.index.line, beam.index.column), Direction.North), 
                        new Beam(new MapIndex(beam.index.line, beam.index.column), Direction.South),
                    ];
                }else{
                    return [beam];
                }
            }else if(next == '-'){
                if(beam.direction == Direction.North || beam.direction == Direction.South){
                    return [
                        new Beam(new MapIndex(beam.index.line, beam.index.column), Direction.East), 
                        new Beam(new MapIndex(beam.index.line, beam.index.column), Direction.West),
                    ];
                }else{
                    return [beam];
                }
            }else{
                throw new Exception($"Char not recognized: {next}");
            }
        }else{
            return [];
        }
    }

    public char GetCurrentValue(MapIndex index){
        return map[index.line][index.column];
    }
}

public class Beam(MapIndex index, Direction direction){
    public MapIndex index = index;
    public Direction direction = direction;

    public void FollowThrough(){
        if(direction == Direction.North){
            index.line--;
        }else if(direction == Direction.East){
            index.column++;
        }else if(direction == Direction.West){
            index.column--;
        }else if(direction == Direction.South){
            index.line++;
        }else{
            throw new Exception($"Direction not recognized: {direction}");
        }
    }

    public void Bounce(char bounce){
        if(bounce == '\\'){
            if(direction == Direction.North){
                direction = Direction.West;
            }else if(direction == Direction.South){
                direction = Direction.East;
            }else if(direction == Direction.East){
                direction = Direction.South;
            }else if(direction == Direction.West){
                direction = Direction.North;
            }else{
                throw new Exception($"Direction not recognized: {direction}");
            }
        }else if(bounce == '/'){
            if(direction == Direction.North){
                direction = Direction.East;
            }else if(direction == Direction.South){
                direction = Direction.West;
            }else if(direction == Direction.East){
                direction = Direction.North;
            }else if(direction == Direction.West){
                direction = Direction.South;
            }else{
                throw new Exception($"Direction not recognized: {direction}");
            }
        }else{
            throw new Exception($"Char not recognized: {bounce}");
        }
        
    }

    public bool Equals(Beam beam){
        return index.Equals(beam.index) && direction.Equals(beam.direction);
    }

    public override bool Equals(object? obj){
        if (obj is Beam otherBeam){
            return index.Equals(otherBeam.index) && direction.Equals(otherBeam.direction);
        }
        return false;
    }

    public new static bool Equals(object? obj1, object? obj2){
        if(obj1 is Beam beam1 && obj2 is Beam beam2){
            return beam1.Equals(beam2);
        }

        return false;
    }

    public Beam Clone(){
        return new Beam(index.Clone(), direction);
    }

    public override String ToString(){
        return $"index: {index}, direction: {direction}";
    }

    public override int GetHashCode(){
        return HashCode.Combine(index, direction);
    }
}

public class BeamEquality : IEqualityComparer<Beam>{
    public bool Equals(Beam? beam1, Beam? beam2){
        if(beam1 != null && beam2 != null){
            return beam1.Equals(beam2);
        }

        return false;
    }

    public int GetHashCode(Beam beam){
        return HashCode.Combine(beam.index.GetHashCode(), beam.direction);
    }
}