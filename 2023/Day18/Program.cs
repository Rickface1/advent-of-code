using System.Drawing;
using advent.resources;

namespace advent.y2023;

public class DayEighteen(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        PartOne(args);
    }

    public static void PartOne(string[] args){
        List<Vector> vectors = GetVectors(args);
        List<MapIndex> MapIndices = GetMapIndices(vectors);
        HashSet<MapIndex> FullIndies = [.. FillIndicies(MapIndices)];
        int area = FindArea([.. FullIndies]);

        Console.WriteLine(area);
    }

    public static int FindArea(List<MapIndex> MapIndices){
        List<List<MapIndex>> Sorted = MapIndices.GroupBy(data => data.line)
                                                .Select(group => group.OrderBy(index => index.column)
                                                    .ToList())
                                                .ToList();
        int total = 0;
        for(int x = 0; x < Sorted.Count; x++){
            bool worked = true;
            int CurrentIndex = 0;
            MapIndex group = new(-1,-1);

            while(worked && CurrentIndex < Sorted[x].Count){
                MapIndex index1 = Sorted[x][CurrentIndex];
                int Position = Sorted[x].FindIndex(data => data.column > index1.column && !Sorted[x].Any(inside => inside.column == data.column + 1));

                if(Position != -1){
                    MapIndex index2 = Sorted[x][Position];

                    if(group.column != -1 && group.line != -1){
                        total += Math.Abs(group.column - index1.column);
                        group = new(-1, -1);
                    }else{
                        group = index2;
                    }

                    CurrentIndex = Position + 1;
                    total += Math.Abs(index2.column - index1.column);
                }else{
                    worked = false;
                }
            }
        }

        return total;
    }

    public static List<MapIndex> FillIndicies(List<MapIndex> mapIndices){
        List<MapIndex> Return = [];

        for(int x = 0; x < mapIndices.Count - 1; x++){
            MapIndex index1 = mapIndices[x];
            MapIndex index2 = mapIndices[x + 1];

            Return.AddRange(MapIndex.FillSpace(index1, index2));
        }

        Return.AddRange(MapIndex.FillSpace(mapIndices[^1], mapIndices[0]));

        return Return;
    }

    public static List<MapIndex> GetMapIndices(List<Vector> vectors){
        List<MapIndex> mapIndices = [
            new MapIndex(0,0)
        ];

        for(int x = 0; x < vectors.Count; x++){
            mapIndices.Add(vectors[x].GetNextPosition(mapIndices[^1]));
        }

        return mapIndices;
    }

    public static List<Vector> GetVectors(string[] args){
        List<Vector> vectors = [];

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

            vectors.Add(new Vector(direction, distance, color));
        }

        return vectors;
    }
}

public class Vector(Direction direction, int distance, string color){
    public Direction direction = direction;
    public int distance = distance;
    public string color = color;

    public MapIndex GetNextPosition(MapIndex index){
        MapIndex Return = index.Clone();

        switch(direction){
            case Direction.North:
                Return.line -= distance;
                break;
            case Direction.East:
                Return.column += distance;
                break;
            case Direction.South:
                Return.line += distance;
                break;
            case Direction.West:
                Return.column -= distance;
                break;
        }

        return Return;
    }
}