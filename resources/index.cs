namespace advent.resources;

public class MapIndex(int line, int column){
    public int line = line;
    public int column = column;
    public int Manhattan(MapIndex index){
        return Math.Abs(line - index.line) + Math.Abs(column - index.column);
    }
    public int Manhattan(int line, int column){
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

        for (int line = Math.Min(index1.line, index2.line); line <= Math.Max(index1.line, index2.line); line++){
            for(int column = Math.Min(index1.column, index2.column); column <= Math.Max(index1.column, index2.column); column++){
                Return.Add(new MapIndex(line, column));
            }
        }

        return Return;
    }
}