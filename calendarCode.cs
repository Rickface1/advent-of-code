using System.Collections;
using System.Diagnostics;

namespace main;
/// <summary>
/// Example Implementation of the CalendarCode class
///
/// Namespace of file must be y{insert year}, for example y2023
/// Name of inheriting class must be Day{insert day}, for example DayOne
/// Must implement method "Execute".
/// 
/// </summary>
/// <param name="filePath"></param>
public abstract class CalendarCode{
    public string filePath;
    public CalendarCode(string filePath){
        this.filePath = "../../../" + DateTime.Now.ToString("yyyy") + "/" + filePath;
    }
    /// <summary>
    /// Method that is ran when the main program is compiled.
    /// returns void.
    /// </summary>
    public abstract void Execute();
    public static ArrayList IterateWithTime<T>(Func<T> function, int times, int warmups){
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

        var arr = new ArrayList
        {
            output,
            timeElapsed
        };
        
        return arr;
    }

    public static ArrayList IterateOnce<T>(Func<T> function){
        Stopwatch sw = new();
        sw.Start();

        T value = function();

        sw.Stop();

        double timeElapsed = sw.Elapsed.TotalMilliseconds;

        ArrayList arr =
        [
            value,
            timeElapsed
        ];
        
        return arr;
    }

    public string[] ReadAllLines(){
        return File.ReadAllLines(filePath + "/" + "input.txt");
    }
}