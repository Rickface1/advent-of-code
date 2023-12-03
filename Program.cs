using System;
using System.Globalization;
using System.Reflection;

class MainRunner {
    public static void Main(){
        ExecuteCode();
        //PromptUser();
    }
    public static void PromptUser(){
        Console.WriteLine("--- please enter the class or leave empty for auto selection ---");
        var data = Console.ReadLine();

        if(data == null || data.Length == 0){
            ExecuteCode();
        }else{
            ExecuteCode(data);
        }
    }
    public static void ExecuteCode() {
        // Get the current year and month in the Eastern Time (EST) zone
        DateTime currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        int currentYear = currentDateTime.Year;

        string fileName = $"Day{NumberToWords(currentDateTime.Day)}";

        // Construct the class name based on the current year
        string className = $"y{currentYear}.{fileName}";

        // Assuming the class and method exist
        var dynamicType = Type.GetType(className);

        if (dynamicType != null) {
            var instance = Activator.CreateInstance(dynamicType,fileName);

            // Assuming there is a method with the constructed name
            var method = dynamicType.GetMethod("Execute");

            if (method != null) {
                // Invoke the method
                method.Invoke(instance, null);
            } else {
                Console.WriteLine($"Method not found in class {className}.");
            }
        } else {
            Console.WriteLine($"Class {className} not found for the current year.");
        }
    }
    public static void ExecuteCode(string data) {
        string fileName = data.Split('.')[1];

        // Assuming the class and method exist
        var dynamicType = Type.GetType(data);

        if (dynamicType != null) {
            var instance = Activator.CreateInstance(dynamicType,fileName);

            // Assuming there is a method with the constructed name
            var method = dynamicType.GetMethod("Execute");

            if (method != null) {
                // Invoke the method
                method.Invoke(instance, null);
            } else {
                Console.WriteLine($"Method not found in class {data}.");
            }
        } else {
            Console.WriteLine($"Class {data} not found for the current year.");
        }
    }
    private static string NumberToWords(int number) {
        if (number == 0)
            return "Zero";

        // Handle numbers up to 20 directly
        if (number <= 20)
            return new[]
            {
                "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", "Twenty"
            }[number - 1];

        // Handle numbers above 20
        if (number < 100)
            return $"{new[] {"Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"}[number / 10 - 2]} {NumberToWords(number % 10)}";

        // Handle numbers above 100
        if (number < 1000)
            return $"{NumberToWords(number / 100)} Hundred {NumberToWords(number % 100)}";

        // Handle larger numbers
        return $"{NumberToWords(number / 1000)} Thousand {NumberToWords(number % 1000)}";
    }
}
