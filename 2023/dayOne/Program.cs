namespace y2023;

using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection.Metadata.Ecma335;

class Pair{
    public String str;
    public int value;
    public String name;

    public Pair(string str, int value){
        this.str = str;
        this.value = value;
        this.name = StrRepresentation();
    }

    public String StrRepresentation(){
        return int.Parse(str) switch
        {
            1 => "one",
            2 => "two",
            3 => "three",
            4 => "four",
            5 => "five",
            6 => "six",
            7 => "seven",
            8 => "eight",
            9 => "nine",
            _ => "",
        };
    }
}
public class DayOne(string filePath) : main.CalendarCode(filePath){
    private static List<Pair> pairs = [
        new Pair("1",1),
        new Pair("2",2),
        new Pair("3",3),
        new Pair("4",4),
        new Pair("5",5),
        new Pair("6",6),
        new Pair("7",7),
        new Pair("8",8),
        new Pair("9",9)
    ];
    
    public override
     void Execute(){
        string[] list = ReadAllLines();
        Func<object> bob = () => MainProgram(list);
        ArrayList time = IterateWithTime(bob,10000,100);
        Console.WriteLine("Average Time:");
        Console.WriteLine(time[1]);
        Console.WriteLine("Values:");
        Console.WriteLine(time[0]);
    }

    public static int MainProgram(String[] list){
        int counter = 0;

       Parallel.ForEach(list, var =>{
            int currentVar = Parse(var);
            Interlocked.Add(ref counter, currentVar);
        });
        return counter;
    }

    public static int Parse(string var){
        int startValue = 0;
        int endValue = 0;

        int startIndex = int.MaxValue;
        int endIndex = int.MinValue;

        foreach (Pair p in pairs){
            int firstIndex = var.IndexOf(p.str);

            int otherIndex = var.IndexOf(p.name);

            bool firstBool = firstIndex == -1 ? new Func<bool>(() => { firstIndex = int.MaxValue; return false; })() : true;
            bool otherBool = otherIndex == -1 ? new Func<bool>(() => { otherIndex = int.MaxValue; return false; })() : true;

            if (firstBool && otherBool){
                int minValue = Math.Min(firstIndex, otherIndex);
                if (minValue < startIndex){
                    startValue = p.value;
                    startIndex = minValue;
                }

                int maxValue;
                bool met = false;

                int lastIndex = var.LastIndexOf(p.str);
                lastIndex = lastIndex == -1 ? new Func<int>(() => {
                    met = true;
                    return int.MinValue;
                })() : lastIndex;
                
                int otherLastIndex = var.LastIndexOf(p.name);
                if(otherLastIndex == -1){
                    if(met == true){
                        continue;
                    }else{
                        otherLastIndex = int.MinValue;
                    }
                }

                maxValue = Math.Max(lastIndex, otherLastIndex);
                if (maxValue > endIndex){
                    endValue = p.value;
                    endIndex = maxValue;
                }
            }else if(firstBool){
                if (firstIndex < startIndex){
                    startValue = p.value;
                    startIndex = firstIndex;
                }

                int lastIndex = var.LastIndexOf(p.str);

                if (lastIndex > endIndex){
                    endValue = p.value;
                    endIndex = lastIndex;
                }
            }else if (otherBool){
                if (otherIndex < startIndex){
                    startValue = p.value;
                    startIndex = otherIndex;
                }
                
                int otherLastIndex = var.LastIndexOf(p.name);

                if (otherLastIndex > endIndex){
                    endValue = p.value;
                    endIndex = otherLastIndex;
                }
            }
        }

        return int.Parse(startValue.ToString() + endValue.ToString());
    }
}