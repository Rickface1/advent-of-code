using advent.resources;

namespace advent.y2023;

public class DayEighteen(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        PartOne(args);
    }

    public static void PartOne(string[] args){
        List<Vector> Vectors = GetVectors(args);
        Console.WriteLine(Vector.GetBorderLength(Vectors));
        Console.WriteLine(Area.Inside(Vectors));
        Console.WriteLine(Area.TotalArea(Vectors));
    }

    public static int GetBorderLength(List<Vector> vectors) => vectors.Select(data => data.distance).Sum();

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


    public static List<Vector> GetVectors(string[] args){
        List<Vector> vectors = [];
        MapIndex CurrentIndex = new(0,0);

        for(int x = 0; x < args.Length; x++){
            Direction direction = Direction.North;
            switch(args[x][0]){
                case 'U':
                    direction = Direction.North;
                    break;
                case 'R':
                    direction = Direction.East;
                    break;
                case 'D':
                    direction = Direction.South;
                    break;
                case 'L':
                    direction = Direction.West;
                    break;
            }

            int distance = int.Parse(args[x][2].ToString());
            string color = args[x][4..].ToString();

            vectors.Add(new Vector(CurrentIndex, direction, distance, color));
            CurrentIndex = vectors[^1].GetNextPosition();
        }

        return vectors;
    }
}