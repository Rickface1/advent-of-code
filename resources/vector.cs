namespace advent.resources;
public class Vector(MapIndex index, Direction direction, long distance){
    public MapIndex index = index;
    public Direction direction = direction;
    public long distance = distance;

    public MapIndex GetNextPosition(){
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

    public static long GetBorderLength(List<Vector> vectors) => vectors.Select(data => data.distance).Sum();
}