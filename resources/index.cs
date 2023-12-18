namespace advent.resources;

public class MapIndex(int line, int column){
    public int line = line;
    public int column = column;
    public int Cityblock(MapIndex index){
        return Math.Abs(line - index.line) + Math.Abs(column - index.line);
    }
    public int Cityblock(int line, int column){
        return Math.Abs(this.line - line) + Math.Abs(this.column - line);
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
}