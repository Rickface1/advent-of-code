namespace y2023;

public class DayThree(string filePath) : main.CalendarCode(filePath){
    public override void Execute(){
        string[] lines = ReadAllLines();
        Func<object> program = () => {
            return StartProgram(lines);
        };
        //var returnVal = IterateWithTime(program,100,10);
        var returnVal = IterateOnce(program);

        Console.WriteLine(returnVal[0]);
        Console.WriteLine(returnVal[1]);
    }
    public static int StartProgram(string[] lines){
        List<Span> spanList = [];
        for(int y = 0; y < lines.Length; y++){
            char[] line = lines[y].ToCharArray();
            for(int x = 0; x < line.Length; x++){
                char c = line[x];
                if(IsSymbol(c)){
                    spanList.Add(new Span(x, y));
                }
            }
        }

        int total = 0;
        int stopInt = lines.Length;

        for(int y = 0; y < /*lines.Length*/stopInt; y++){
            char[] line = lines[y].ToCharArray();
            for(int x = 0; x < line.Length; x++){
                char c = line[x];

                if(char.IsNumber(c)){
                    string data = "";
                    int starting = x;

                    while(x < line.Length && char.IsNumber(line[x])){
                        data += line[x];
                        x++;
                    }

                   foreach(Span span in spanList){
                        if(span.Touching(starting, x, y)){
                            if(y == stopInt - 1){
                                Console.WriteLine($"Starting: {starting} Ending: {x} Line: {y}");
                                Console.WriteLine(data);
                                Console.WriteLine();
                            }
                            if(int.Parse(data).ToString() != data){
                                Console.WriteLine("HERE IS THE ISSUE");
                            }
                            total += int.Parse(data);
                            break;
                        }
                    }
                }
            }
        }

        return total;
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

public class Span(int index, int line){
    public int index = index;
    public int line = line;

    public bool Touching(int startIndex, int endIndex, int line){
        int difference = Math.Abs(line - this.line);
        if(difference <= 1){
            if((startIndex - 1) <= index && index <= (endIndex + 1)){
                return true;
            }
        }
        return false;
    }
    public override String ToString(){
        return $"Index: {index}\nLine: {line}\n";
    }
}