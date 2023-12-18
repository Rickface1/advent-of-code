namespace advent.resources;
public static class FloodFill{
    public static CharMap FillOutside(CharMap map, char from, char to){
        List<char> start = [.. CalendarCode.PrintTimes(map.GetRowBounds() + 2, to)];

        List<List<char>> NewMap = [
            start
        ];

        for(int x = 0; x < map.GetColumnBounds(); x++){
            NewMap.Add([.. (to + new string(map.map[x].ToArray()) + to)]);
        }

        NewMap.Add(start);

        CharMap Map = new(NewMap);

        List<(int, int)> Checked = [
            (0, 0),
        ];

        Queue<(int, int)> Q = new();
        Q.Enqueue((0, 0));

        while(Q.Count > 0){
            (int, int) n = Q.Dequeue();
            char c = Map.map[n.Item1][n.Item2];

            if(c == to || c == from){
                Map.map[n.Item1][n.Item2] = to;

                if(n.Item1 > 0){
                    if(!Checked.Contains((n.Item1 - 1, n.Item2))){
                        Q.Enqueue((n.Item1 - 1, n.Item2));
                        Checked.Add((n.Item1 - 1, n.Item2));
                    }
                }

                if(n.Item2 > 0){
                    if(!Checked.Contains((n.Item1, n.Item2 - 1))){
                        Q.Enqueue((n.Item1, n.Item2 - 1));
                        Checked.Add((n.Item1, n.Item2 - 1));
                    }
                }

                if(n.Item1 < Map.map.Count - 1){
                    if(!Checked.Contains((n.Item1 + 1, n.Item2))){
                        Q.Enqueue((n.Item1 + 1, n.Item2));
                        Checked.Add((n.Item1 + 1, n.Item2));
                    }
                }

                if(n.Item2 < Map.map[n.Item1].Count - 1){
                    if(!Checked.Contains((n.Item1, n.Item2 + 1))){
                        Q.Enqueue((n.Item1, n.Item2 + 1));
                        Checked.Add((n.Item1, n.Item2 + 1));
                    }
                }
            }
        }

        Map.map.RemoveAt(0);
        Map.map.RemoveAt(Map.map.Count - 1);

        CharMap Return = new(Map.map.Select(data => data[1.. (data.Count - 2)]).ToList());

        return Return;
    }
}