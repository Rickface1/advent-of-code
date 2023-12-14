using System.Collections;
using System.Diagnostics;
using System.Text;

namespace advent;
public abstract class CalendarCode(string filePath){
    public string filePath = "../../../" + DateTime.Now.ToString("yyyy") + "/" + filePath;

    public abstract void Execute(string[] args);
    public static Dictionary<string, object?> IterateWithTime<T>(Func<T> function, int times, int warmups){
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
        T output = function();

        Dictionary<string, object?> NewDictionary = new(){
            ["time"] = timeElapsed,
            ["data"] = output
        };

        return NewDictionary;
    }

    public static Dictionary<string, object?> IterateOnce<T>(Func<T> function){
        Stopwatch sw = new();
        sw.Start();

        T value = function();

        sw.Stop();

        double timeElapsed = sw.Elapsed.TotalMilliseconds;

        Dictionary<string, object?> NewDictionary = new(){
            ["time"] = timeElapsed,
            ["data"] = value
        };
        
        return NewDictionary;
    }

    public string[] ReadAllLines(){
        return File.ReadAllLines(filePath + "/" + "input.txt");
    }

    public string[] ReadAllSampleLines(){
        return File.ReadAllLines(filePath + "/" + "sample.txt");
    }

    public string[] ReadAllLines(string FilePath){
        return File.ReadAllLines(filePath + "/" + FilePath);
    }

    public void WriteLines(string FilePath, string input){
        string path = filePath + "/" + FilePath;
        string contentToWrite = input + Environment.NewLine;

        File.AppendAllText(path, contentToWrite);
    }

    public static void LeftShiftArray<T>(List<T> array, T DefaultValue){
        if (array.Count > 0){
            array.RemoveAt(0); // Remove the first element
            array.Add(DefaultValue); // Add the default value at the end
        }
    }

    public static void PrintLines(int times){
        for(int x = 0; x < times; x++){
            Console.WriteLine();
        }
    }

    public static long LeastCommonMultiple(List<long> inputs){
        return inputs.Aggregate((current, next) => LCM(current, next));
    }

    public static long LCM(long a, long b){
        return a * b / CalculateGCD(a, b);
    }

    public static long GreatestCommonFactor(List<long> numbers){
        long result = numbers[0];

        for (int i = 1; i < numbers.Count; i++){
            result = CalculateGCD(result, numbers[i]);
        }

        return result;
    }

    public static long CalculateGCD(long a, long b){
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    public static int CountIterations<T>(Func<T, bool> Condition, T Value, Func<T, T> ValueFunction){
        int x = 0;

        for(; Condition(Value); x++){
            Value = ValueFunction(Value);
        }

        return x;
    }

    public static double PowerOf(int Root, int Power){
        return Math.Pow(Root, Power);
    }

    public static int Multiply(int Value1, int Value2){
        return Value1 * Value2;
    }

    public static double Multiply(List<int> Values){
        return Values.Aggregate(Multiply);
    }

    public static int Divide(int Dividend, int Divisor){
        return Dividend / Divisor;
    }

    public static int Divide(List<int> Dividends, List<int> Divisors){
        return Divide(Dividends.Aggregate(Add), Divisors.Aggregate(Add));
    }

    public static int Add(int Value1, int Value2){
        return Value1 + Value2;
    }

    public static int Add(List<int> values){
        return values.Aggregate(Add);
    }

    public static double Root(int Value){
        return Math.Sqrt(Value);
    }

    public static string PrintTimes(int Times, string str){
        StringBuilder sb = new();

        for(int x = 0; x < Times; x++){
            sb.Append(str);
        }

        return sb.ToString();
    }

    public static string PrintTimes(int Times, char c){
        StringBuilder sb = new();

        for(int x = 0; x < Times; x++){
            sb.Append(c);
        }

        return sb.ToString();
    }
}