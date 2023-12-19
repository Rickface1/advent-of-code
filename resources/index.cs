using advent.y2023;

namespace advent.resources;

public class MapIndex{
    public long line;
    public long column;
    public MapIndex(int line, int column){
        this.line = line;
        this.column = column;
    }

    public MapIndex(long line, long column){
        this.line = line;
        this.column = column;
    }

    public MapIndex((int, int) Values){
        this.line = Values.Item1;
        this.column = Values.Item2;
    }

    public long Manhattan(MapIndex index){
        return Math.Abs(line - index.line) + Math.Abs(column - index.column);
    }
    public long Manhattan(int line, int column){
        return Math.Abs(this.line - line) + Math.Abs(this.column - column);
    }

    public double AbsoluteDistance(MapIndex index){
        return Math.Sqrt(Math.Pow(Math.Abs(line - index.line), 2) + Math.Pow(Math.Abs(column - index.column), 2));
    }

    public bool ValidIndex(int RowLength, int ColumnLength){
        return line >= 0 && line < RowLength && column >= 0 && column < ColumnLength;
    }

    public override bool Equals(object? obj){
        if(obj is MapIndex index){
            return index.line == line && index.column == column;
        }else{
            return false;
        }
    }

    public MapIndex Clone(){
        return new MapIndex(line, column);
    }

    public override String ToString(){
        return $"({line},{column})";
    }

    public override int GetHashCode(){
        return HashCode.Combine(line, column);
    }

    public static List<MapIndex> FillSpace(MapIndex index1, MapIndex index2){
        List<MapIndex> Return = [];

        for (long line = Math.Min(index1.line, index2.line); line <= Math.Max(index1.line, index2.line); line++){
            for(long column = Math.Min(index1.column, index2.column); column <= Math.Max(index1.column, index2.column); column++){
                Return.Add(new MapIndex(line, column));
            }
        }

        return Return;
    }

    public void NextValue(Direction direction){
        switch(direction){
            case Direction.North:
                line--;
                break;
            case Direction.East:
                column++;
                break;
            case Direction.South:
                line++;
                break;
            case Direction.West:
                column--;
                break;
        }
    }

    public static long GetTotalLength(List<MapIndex> indices){
        long total = 0;

        for(int x = 0; x < indices.Count; x++){
            total += indices[x].Manhattan(indices[(x + 1) % indices.Count]);
        }

        return total;
    }
}