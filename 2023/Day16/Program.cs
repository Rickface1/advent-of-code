using advent.resources;

namespace advent.y2023;

public class DaySixteen(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        PartOne(args);
    }

    public static void PartOne(string[] args){
        Console.WriteLine(ExecutePartOne(args));
    }

    public static int ExecutePartOne(string[] args){
        BeamMap map = new(args);
        Queue<Beam> lazers = [];
        HashSet<Beam> elapsed = [];

        lazers.Enqueue(new Beam(new MapIndex(0,0), Direction.East));
        map.energized.Add(new MapIndex(0,0));

        while(lazers.Count > 0){
            List<Beam> beams = map.GetNext(lazers.Dequeue());


            for(int x = 0; x < beams.Count; x++){
                if(elapsed.Add(beams[x])){
                    lazers.Enqueue(beams[x]);
                }
            }
        }

        for(int x = 0; x < map.map.Count; x++){
            for(int y = 0; y < map.map[x].Count; y++){
                if(map.energized.Any(data => data.line == x && data.column == y)){
                    Console.Write('#');
                }else{
                    Console.Write('.');
                }
            }
            Console.WriteLine();
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
    
    public List<Beam> GetNext(Beam beam){
        beam.FollowThrough();
        if(beam.index.ValidIndex(map.Count, map[0].Count)){
            char next = GetCurrentValue(beam.index);
            energized.Add(beam.index);


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
            index.line -= 1;
        }else if(direction == Direction.East){
            index.column += 1;
        }else if(direction == Direction.West){
            index.column -= 1;
        }else if(direction == Direction.South){
            index.line += 1;
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

    public override bool Equals(object? obj){
        if (obj is Beam otherBeam){
            return index.Equals(otherBeam.index) && direction == otherBeam.direction;
        }
        return false;
    }

    public override String ToString(){
        return $"index: {index}, direction: {direction}";
    }

    public override int GetHashCode(){
        return HashCode.Combine(index.GetHashCode(), direction);
    }
}