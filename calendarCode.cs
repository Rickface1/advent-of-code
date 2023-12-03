using System.Collections;
using System.Diagnostics;

namespace main;

public abstract class CalendarCode{
    public string filePath;
    public CalendarCode(string filePath){
        this.filePath = "../../../" + DateTime.Now.ToString("yyyy") + "/" + filePath;
    }
    public abstract void Execute();
    public static ArrayList IterateWithTime(int times, int warmups, Func<object> function){
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

        var arr = new ArrayList
        {
            function(),
            timeElapsed
        };
        
        return arr;
    }

    public static ArrayList IterateOnce(Func<object> function){

        Stopwatch sw = new();
        sw.Start();

        function();

        sw.Stop();

        double timeElapsed = sw.Elapsed.TotalMilliseconds;

        var arr = new ArrayList
        {
            function(),
            timeElapsed
        };
        
        return arr;
    }

    public string[] ReadAllLines(){
        return File.ReadAllLines(filePath + "/" + "input.txt");
    }
}