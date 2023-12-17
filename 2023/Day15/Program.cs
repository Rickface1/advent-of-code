namespace advent.y2023;

public class DayFifteen(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        string arg = args[0];
        PartOne(arg);

        PrintLines(2);
        
        PartTwo(arg);
    }

    public static void PartOne(string arg){
        Func<int> ParseFunction = () => {
            return HASH.GetValues(arg);
        };
        
        var Result = IterateWithTime(ParseFunction, 10000, 100);

        Console.WriteLine("--- TIME ---");
        Console.WriteLine(Result["time"]);

        Console.WriteLine();

        Console.WriteLine("--- DATA ---");
        Console.WriteLine(Result["data"]);
    }

    public static int ExecutePartTwo(string arg){
        List<List<char>> args = [.. arg.Split(',').Select(data => data.ToCharArray().ToList())];
        Boxes boxes = new();
        for(int x = 0; x < args.Count; x++){
            if(args[x].Contains('=')){
                int index = args[x].IndexOf('=');
                string label = new(args[x][.. index].ToArray());
                int value = int.Parse(args[x][(index + 1) ..].ToArray());

                boxes.Add(label, value);
            }else if(args[x].Contains('-')){
                int index = args[x].IndexOf('-');
                string label = new(args[x][.. index].ToArray());

                boxes.Remove(label);
            }else{
                throw new Exception("NO VALUE FOUND");
            }
        }

        return boxes.GetValues();
    }

    public static void PartTwo(string arg){
        Func<int> ParseFunction = () => {
            return ExecutePartTwo(arg);
        };
        
        var Result = IterateWithTime(ParseFunction, 10000, 100);

        Console.WriteLine("--- TIME ---");
        Console.WriteLine(Result["time"]);

        Console.WriteLine();

        Console.WriteLine("--- DATA ---");
        Console.WriteLine(Result["data"]);
    }
}

public class Boxes(){
    public Dictionary<int, Box> boxes = [];
    public void Add(string label, int value){
        int HashValue = HASH.GetValue(label);
        if(boxes.TryGetValue(HashValue, out Box? box)){
            box.AddItem(new Lense(label, value));
        }else{
            boxes.Add(HashValue, new Box(new Lense(label, value)));
        }
    }

    public void Remove(string label){
        int HashValue = HASH.GetValue(label);
        if(boxes.TryGetValue(HashValue, out Box? value))
            value.RemoveItem(label);
    }

    public int GetValues(){
        return boxes.Select(value => value.Value.GetValue(value.Key)).Sum();
    }
}

public class Box(Lense lense){
    public List<Lense> lenses = [lense];
    public int GetValue(int index){
        int total = 0;
        for(int x = 0; x < lenses.Count; x++){
            total += (1 + index) * lenses[x].value * (x + 1);
        }

        return total;
    }
    public void AddItem(Lense lense){
        int index = lenses.FindIndex(data => data.label == lense.label);
        if(index != -1)
            lenses[index] = lense;
        else
            lenses.Add(lense);
    }

    public void RemoveItem(string str){
        lenses = lenses.Where(data => data.label != str).ToList();
    }
}

public class Lense(string label, int value){
    public string label = label;
    public int value = value;

    public bool Equals(Lense lense){
        return lense.label == label;
    }

    public override int GetHashCode(){
        return HASH.GetValue(label);
    }
}

public static class HASH{
    public static int GetValues(string args){
        return args.Split(',').Select(GetValue).Sum();
    }
    public static int GetValue(string args){
        List<char> line = [.. args.ToCharArray()];
        int CurrentTotal = 0;
        for(int x = 0; x < line.Count; x++){
            CurrentTotal = (17 * (CurrentTotal + line[x])) % 256;
        }

        return CurrentTotal;
    }
}