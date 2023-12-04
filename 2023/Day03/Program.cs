namespace y2023;

public class DayThree(string filePath) : main.CalendarCode(filePath){
    public override void Execute(){
        string[] lines = ReadAllLines();
        Func<(int total,int aggregate)> program = () => {
            return StartProgram(lines);
        };

        var returnVal = IterateWithTime<(int total, int aggregate)>(program, 10, 1);

        var resultTuple = (ValueTuple<int, int>)(0,0);

        if (returnVal != null && returnVal["data"] is ValueTuple<int, int>){
            resultTuple = (ValueTuple<int, int>)(returnVal["data"] ?? (0,0));
        }
        int total = resultTuple.Item1;
        int aggregate = resultTuple.Item2;

        Console.WriteLine("---Old Way---");

        Console.WriteLine($"Time: {(returnVal == null ? 0 : returnVal["time"])}");
        Console.WriteLine($"Part One: {total}");
        Console.WriteLine($"Part Two: {aggregate}");

        program = () => {
            return NewMain(lines);
        };

        returnVal = IterateWithTime<(int total, int aggregate)>(program, 1000, 100);

        if (returnVal != null && returnVal["data"] is ValueTuple<int, int>){
            resultTuple = (ValueTuple<int, int>)(returnVal["data"] ?? (0,0));
        }
        total = resultTuple.Item1;
        aggregate = resultTuple.Item2;

        Console.WriteLine("---New Way---");

        Console.WriteLine($"Time: {(returnVal == null ? 0 : returnVal["time"])}");
        Console.WriteLine($"Part One: {total}");
        Console.WriteLine($"Part Two: {aggregate}");
    }
    public static (int total, int aggregate) NewMain(string[] lines){
        List<List<Span>> LineList = [];

        LineList.Add(GetSpans(lines[0]));
        LineList.Add(GetSpans(lines[1]));
        int total = GetValues(LineList.GetRange(0,2), lines[0]);


        for(int y = 1; y < lines.Length - 1; y++){
            LineList.Add(GetSpans(lines[y + 1]));
            total += GetValues(LineList.GetRange(y - 1, 3), lines[y]);
        }


        total += GetValues(LineList.GetRange(lines.Length - 2, 2), lines[lines.Length - 1]);

        List<Span> flattenedList = [];

        foreach (var innerList in LineList){
            flattenedList.AddRange(innerList);
        }
        
        List<Span> relevantSpans = flattenedList.Where(span => span.rep == '*' && span.touchingThings.Count >= 2).ToList();
        int aggregate = relevantSpans.Sum(span => span.touchingThings.Aggregate((x, y) => x * y));


        return (total,aggregate);
    }

    public static List<Span> GetSpans(string line){
        List<Span> SpanList = [];
        for(int x = 0; x < line.Length; x++){
            char c = line[x];
            if(IsSymbol(c)){
                SpanList.Add(new Span(x, c));
            }
        }
        return SpanList;
    }

    public static int GetValues(List<List<Span>> SpanList, string line){
        int total = 0;
        // Assuming there's an object for locking shared resources
        object lockObject = new object();

        /*Parallel.For(0, line.Length, x => {
            char c = line[x];

            if (char.IsNumber(c)) {
                string data = "" + line[x];
                int starting = x;

                while (x + 1 < line.Length && char.IsNumber(line[x + 1])) {
                    data += line[x + 1];
                    x++;
                }

                // Use lock to make the code thread-safe
                lock (lockObject) {
                    foreach (Span span in SpanList.SelectMany(list => list)) {
                        if (span.Touching(starting, x)) {
                            span.touchingThings.Add(int.Parse(data));
                            total += int.Parse(data);
                            break;
                        }
                    }
                }
            }
        });*/

        for(int x = 0; x < line.Length; x++){
            char c = line[x];

            if(char.IsNumber(c)){
                string data = "" + line[x];
                int starting = x;

                while(x + 1 < line.Length && char.IsNumber(line[x + 1])){
                    data += line[x + 1];
                    x++;
                }

                foreach(Span span in SpanList.SelectMany(list => list)){
                    if(span.Touching(starting, x)){
                        span.touchingThings.Add(int.Parse(data));
                        total += int.Parse(data);
                        break;
                    }
                }
            }
        }
        return total;
    }

    public static (int total, int aggregate) StartProgram(string[] lines){
        HashSet<Span> spanList = [];

        for(int y = 0; y < lines.Length; y++){
            char[] line = lines[y].ToCharArray();
            for(int x = 0; x < line.Length; x++){
                char c = line[x];
                if(IsSymbol(c)){
                    spanList.Add(new Span(x, y, c));
                }
            }
        }

        int total = 0;

        for(int y = 0; y < lines.Length; y++){
            char[] line = lines[y].ToCharArray();
            for(int x = 0; x < line.Length; x++){
                char c = line[x];

                if(char.IsNumber(c)){
                    string data = "" + line[x];
                    int starting = x;

                    while(x + 1 < line.Length && char.IsNumber(line[x + 1])){
                        data += line[x + 1];
                        x++;
                    }

                    foreach(Span span in spanList.Where(span => Math.Abs(span.line - y) <= 1)){
                        if(span.Touching(starting, x, y)){
                            span.touchingThings.Add(int.Parse(data));
                            total += int.Parse(data);
                            break;
                        }
                    }
                }
            }
        }

        List<Span> relevantSpans = spanList.Where(span => span.rep == '*' && span.touchingThings.Count >= 2).ToList();
        int aggregate = relevantSpans.Sum(span => span.touchingThings.Aggregate((x, y) => x * y));

        return (total, aggregate);
    }
    
    public static bool IsSymbol(char c){
        if(char.IsNumber(c)){
            return false;
        }else if(c == '.'){
            return false;
        }else{
            return true;
        }
    }
}

public class Span{
    public int index;
    public int line;
    public List<int> touchingThings;
    public char rep;

    public Span(int index, int line, char c){
        this.index = index;
        this.line = line;
        this.touchingThings = [];
        this.rep = c;
    }

    public Span(int index, char c){
        this.index = index;
        this.touchingThings = [];
        this.rep = c;
    }

    public bool Touching(int startIndex, int endIndex, int line){
        int difference = Math.Abs(line - this.line);
        if(difference <= 1){
            return (startIndex - 1) <= index && index <= (endIndex + 1);
        }
        return false;
    }

    public bool Touching(int startIndex, int endIndex){
        return (startIndex - 1) <= index && index <= (endIndex + 1);
    }

    public override String ToString(){
        return $"Index: {index}\nLine: {line}\n";
    }
}