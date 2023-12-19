namespace advent.resources;

public class CharMap{
    public List<List<char>> map;

    public CharMap(List<List<char>> map){
        this.map = map;
    }

    public CharMap(string[] map){
        this.map = [.. map.Select(data => data.ToCharArray().ToList())];
    }

    public int GetRowBounds(){
        return map.Count;
    }

    public int GetColumnBounds(){
        return map[0].Count;
    }

    public char GetValue(MapIndex index){
        int line = (int)index.line;
        int column = (int)index.column;
        return map[line][column];
    }

    public char GetNextValue(MapIndex index, Direction direction){
        int line = (int)index.line;
        int column = (int)index.column;


        if(direction == Direction.North){
            return map[line - 1][column];
        }else if(direction == Direction.South){
            return map[line + 1][column];
        }else if(direction == Direction.East){
            return map[line][column + 1];
        }else if(direction == Direction.West){
            return map[line][column - 1];
        }

        throw new Exception($"DIRECTION OUT OF BOUNDS: {direction}");
    }

    public List<char> GetRow(int index){
        return map[index];
    }

    public List<char> GetColumn(int index){
        return map.Select(data => data[index]).ToList();
    }

    public CharMap Clone(){
        return new CharMap(map.Select(data => new string(data.ToArray())).ToArray());
    }
}