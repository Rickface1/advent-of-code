using System.Diagnostics;
using System.Diagnostics.Metrics;

class ColorCombinations{
    public string color;
    public int quantity;

    public ColorCombinations(string color, int quantity){
        this.color = color;
        this.quantity = quantity;
    }
}
class AdventDayTwo{
    static void Main(string[] args){
        string[] list = File.ReadAllLines("input.txt");

        Func<double> bob = () => MainProgram(list);
        double[] time = IterateWithTime(10000, 100, bob);

        Console.WriteLine("Old Way:\n");
        Console.WriteLine("Average Time:");
        Console.WriteLine(time.GetValue(1));
        Console.WriteLine("Values:");
        Console.WriteLine(time.GetValue(0));

        bob = () => NewMainProgram(list);
        time = IterateWithTime(10000, 100, bob);

        Console.WriteLine("\nNew Way:\n");
        Console.WriteLine("Average Time:");
        Console.WriteLine(time.GetValue(1));
        Console.WriteLine("Values:");
        Console.WriteLine(time.GetValue(0));
    }
    public static double[] IterateWithTime(int times, int warmups, Func<double> function){
        for(int x = 0; x < warmups; x++){
            function();
        }

        Stopwatch sw = new();
        sw.Start();

        for(int y = 0; y < times; y++){
            function();
        }

        sw.Stop();

        double timeElapsed = sw.Elapsed.TotalMilliseconds / times;

        double[] arr = [function(), timeElapsed];
        return arr;
    }
    static int MainProgram(string[] list){
        int value = 0;

        Parallel.ForEach(list,input =>{
            int currentVar = Parse(input[(input.IndexOf(':') + 2)..]);
            Interlocked.Add(ref value, currentVar);
        });

        return value;
    }

    public static int NewMainProgram(string[] list){
        int value = 0;

        Parallel.ForEach(list,input =>{
            int currentVar = NewParse(input[(input.IndexOf(':') + 2)..].ToCharArray());
            Interlocked.Add(ref value, currentVar);
        });

        return value;
    }

    public static int NewParse(char[] input){
        int globalGreen = 0, globalRed = 0, globalBlue = 0;
        int green = 0, red = 0, blue = 0;

        for(int y = 0; y < input.Length; y++){
            if(char.IsDigit(input[y])){
                int index = input.AsSpan(y).IndexOf(' ');

                index = index == -1 ? input.Length - 1 : index + y;

                int currentValue = int.Parse(new string(input.AsSpan(y, index - y)));

                char constant = input[index + 1];

                switch(constant){
                    case 'b':
                        blue += currentValue;
                        break;
                    case 'g':
                        green += currentValue;
                        break;
                    case 'r':
                        red += currentValue;
                        break;
                }
                int semicolonIndex = input.AsSpan(y).IndexOf(';');
                semicolonIndex = semicolonIndex < 0 ? int.MaxValue : semicolonIndex + y;

                int commaIndex = input.AsSpan(y).IndexOf(',');
                commaIndex = commaIndex < 0 ? int.MaxValue : commaIndex + y;

                if (!(semicolonIndex == int.MaxValue && commaIndex == int.MaxValue))
                    y = Math.Min(semicolonIndex, commaIndex) - 1;
                else break;
            }else if(input[y] == ';'){
                globalBlue = Math.Max(blue, globalBlue);
                blue = 0;

                globalGreen = Math.Max(green, globalGreen);
                green = 0;

                globalRed = Math.Max(red, globalRed);
                red = 0;
            }
        }

        int returnValue = Math.Max(blue, globalBlue) * Math.Max(green, globalGreen) * Math.Max(red, globalRed);
        return returnValue;
    }

    public static int Parse(string input){
        int globalGreen = 0;
        int globalRed = 0;
        int globalBlue = 0;

        string[] splitStr = input.Split(';');
        foreach(string strGlobal in splitStr){
            string[] strArray = strGlobal.Split(", ");
            int green = 0;
            int blue = 0;
            int red = 0;

            foreach(string str in strArray){
                int counter = 0;
                int localPlaceHolder = 0;
                string value = "";

                while(char.IsWhiteSpace(str[localPlaceHolder + counter])){
                    counter++;
                }

                while(char.IsNumber(str[localPlaceHolder + counter])){
                    value += str[localPlaceHolder + counter];
                    counter++;
                }

                string updatedStr = str.Substring(1 + counter);
                int finalValue = int.Parse(value);

                switch(updatedStr){
                    case "green":
                        green += finalValue;
                        globalGreen = Math.Max(globalGreen, green);
                        break;
                    case "red":
                        red += finalValue;
                        globalRed = Math.Max(globalRed, red);
                        break;
                    case "blue":
                        blue += finalValue;
                        globalBlue = Math.Max(globalBlue, blue);
                        break;
                }
            }
        }

        return globalGreen * globalRed * globalBlue;
    }
}