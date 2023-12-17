namespace advent.resources;

public class CharMap{
    public List<List<char>> map;

    public CharMap(List<List<char>> map){
        this.map = map;
    }

    public CharMap(string[] map){
        this.map = [.. map.Select(data => data.ToCharArray().ToList())];
    }

    public char GetValue(MapIndex index){
        return map[index.line][index.column];
    }

    public char GetNextValue(MapIndex index, Direction direction){
        if(direction == Direction.North){
            return map[index.line - 1][index.column];
        }else if(direction == Direction.South){
            return map[index.line + 1][index.column];
        }else if(direction == Direction.East){
            return map[index.line][index.column + 1];
        }else if(direction == Direction.West){
            return map[index.line][index.column - 1];
        }

        throw new Exception($"DIRECTION OUT OF BOUNDS: {direction}");
    }

    public List<char> GetRow(int index){
        return map[index];
    }

    public List<char> GetColumn(int index){
        return map.Select(data => data[index]).ToList();
    }
}