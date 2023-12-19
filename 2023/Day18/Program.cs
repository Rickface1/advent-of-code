using advent.resources;

namespace advent.y2023;

public class DayEighteen(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        PartOne(args);

        PrintLines(2);

        PartTwo(args);
    }

    public static void PartOne(string[] args){
        long ParseFunction(){
            return Area.TotalArea(GetVectors1(args));
        }

        PrintIterations(ParseFunction, 10000, 500);
    }


    public static void PartTwo(string[] args){
        long ParseFunction(){
            return Area.TotalArea(GetVectors2(args));
        }

        PrintIterations(ParseFunction, 10000, 500);
    }

    public static List<Vector> GetVectors2(string[] args){
        List<Vector> vectors = [];
        MapIndex CurrentIndex = new(0,0);

        for(int x = 0; x < args.Length; x++){
            string color = args[x][args[x].LastIndexOf(' ') ..];
            string HexDistance = color[3.. (color.Length - 2)];
            long HexDirection = long.Parse(color[(color.Length - 2) .. (color.Length - 1)]);

            Direction direction = HexDirection switch{
                0 => Direction.North,
                1 => Direction.East,
                2 => Direction.South,
                3 => Direction.West,
                _ => throw new Exception(),
            };

            long distance = Convert.ToInt64(HexDistance, 16);

            vectors.Add(new Vector(CurrentIndex, direction, distance));
            CurrentIndex = vectors[^1].GetNextPosition();
        }

        if(!CurrentIndex.Equals(new MapIndex(0,0))){
            throw new Exception();
        }

        return vectors;
    }

    public static long GetBorderLength(List<Vector> vectors) => vectors.Select(data => data.distance).Sum();

    public static List<MapIndex> FillIndicies(List<Vector> mapIndices){
        List<MapIndex> Return = [];

        for(int x = 0; x < mapIndices.Count - 1; x++){
            MapIndex index1 = mapIndices[x].index;
            MapIndex index2 = mapIndices[x + 1].index;

            Return.AddRange(MapIndex.FillSpace(index1, index2));
        }

        Return.AddRange(MapIndex.FillSpace(mapIndices[^1].index, mapIndices[0].index));

        return Return;
    }


    public static List<Vector> GetVectors1(string[] args){
        List<Vector> vectors = [];
        MapIndex CurrentIndex = new(0,0);

        for(int x = 0; x < args.Length; x++){
            string[] CurrentLine = args[x].ToString().Split(' ');

            Direction direction = Direction.North;
            switch(CurrentLine[0]){
                case "U":
                    direction = Direction.North;
                    break;
                case "R":
                    direction = Direction.East;
                    break;
                case "D":
                    direction = Direction.South;
                    break;
                case "L":
                    direction = Direction.West;
                    break;
            }

            int distance = int.Parse(CurrentLine[1]);

            vectors.Add(new Vector(CurrentIndex, direction, distance));
            CurrentIndex = vectors[^1].GetNextPosition();
        }

        if(!CurrentIndex.Equals(new MapIndex(0,0))){
            throw new Exception();
        }

        return vectors;
    }
}