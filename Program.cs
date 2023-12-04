using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

class MainRunner
{
    public static void Main(){
        //PromptUser();
        ExecuteCode();
    }

    public static void PromptUser(){
        Console.WriteLine("--- please enter the class or leave empty for auto selection ---");
        var data = Console.ReadLine();

        if (string.IsNullOrEmpty(data))
        {
            ExecuteCode();
        }else{
            ExecuteCode(data);
        }
    }

    public static void ExecuteCode(){
        // Get the current year and month in the Eastern Time (EST) zone
        DateTime currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        int currentYear = int.Parse(currentDateTime.ToString("yyyy"));

        string namespacePrefix = $"y{currentYear}.Day";

        // Find all classes in the current year's namespace
        List<Type> classesInNamespace = GetClassesInNamespace(namespacePrefix);

        if (classesInNamespace.Count > 0){
            // Find the class with the highest day
            var mostRecentClass = GetMostRecentClass(classesInNamespace);

            if (mostRecentClass != null){
                // Construct the full class name
                string className = mostRecentClass == null ? "" : mostRecentClass.FullName ?? "";

                // Assuming the class and method exist
                var dynamicType = Type.GetType(className);

                if (dynamicType != null){
                    ExecuteMethod(dynamicType, className);
                }else{
                    Console.WriteLine($"Class {className} not found.");
                }
            }else{
                Console.WriteLine("No classes found in the current year's namespace.");
            }
        }else{
            Console.WriteLine($"No classes found for the current year ({currentYear}).");
        }
    }

    public static void ExecuteCode(string data){
        // Assuming the class and method exist
        var dynamicType = Type.GetType(data);

        if (dynamicType != null){
            ExecuteMethod(dynamicType, data);
        }else{
            Console.WriteLine($"Class {data} not found.");
        }
    }

    private static List<Type> GetClassesInNamespace(string namespacePrefix){
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        List<Type> classesInNamespace = new List<Type>();

        foreach (Assembly assembly in assemblies){
            // GetTypes may throw exceptions for certain assemblies, so we need to handle them
            try{
                Type[] types = assembly.GetTypes();

                // Filter types based on the namespace prefix
                IEnumerable<Type> filteredTypes = types.Where(type => type.FullName != null && type.FullName.Contains(namespacePrefix));


                // Add filtered types to the result list                
                foreach (Type filteredType in filteredTypes){
                    classesInNamespace.Add(filteredType);
                }
            }catch (Exception e){
                Console.WriteLine(e.StackTrace);
            }
        }

        return classesInNamespace;
    }

    private static Type? GetMostRecentClass(List<Type> classes){
        // Find the class with the highest day
        Type? mostRecentClass = null;
        int highestDay = -1;

        foreach (Type type in classes){
            string typeName = type != null ? type.FullName ?? "" : "";

            // Extract day from the class name
            int day = WordsToNumber(typeName.Substring(typeName.LastIndexOf("Day") + 3));

            if (day != -1){
                if (day > highestDay){
                    highestDay = day;
                    mostRecentClass = type;
                }
            }
        }

        return mostRecentClass;
    }

    private static void ExecuteMethod(Type dynamicType, string className){
        var instance = Activator.CreateInstance(dynamicType, className.Substring(6));

        // Assuming there is a method with the constructed name
        var method = dynamicType.GetMethod("Execute");

        if (method != null){
            // Invoke the method
            method.Invoke(instance, null);
        }
        else{
            Console.WriteLine($"Method not found in class {className}.");
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

    private static int WordsToNumber(string words){
        // Split the input string into words
        string[] wordArray = words.Split(' ');

        // Create a dictionary to map words to their numerical values
        Dictionary<string, int> wordToNumber = new Dictionary<string, int>{
            {"Zero", 0}, {"One", 1}, {"Two", 2}, {"Three", 3}, {"Four", 4}, {"Five", 5}, {"Six", 6},
            {"Seven", 7}, {"Eight", 8}, {"Nine", 9}, {"Ten", 10}, {"Eleven", 11}, {"Twelve", 12},
            {"Thirteen", 13}, {"Fourteen", 14}, {"Fifteen", 15}, {"Sixteen", 16}, {"Seventeen", 17},
            {"Eighteen", 18}, {"Nineteen", 19}, {"Twenty", 20}, {"Thirty", 30}, {"Forty", 40},
            {"Fifty", 50}, {"Sixty", 60}, {"Seventy", 70}, {"Eighty", 80}, {"Ninety", 90}, {"Hundred", 100},
            {"Thousand", 1000}
        };

        int result = 0;
        int currentNumber = 0;

        foreach (string word in wordArray){
            if (wordToNumber.ContainsKey(word)){
                // Handle cases like "Hundred" and "Thousand"
                if (word == "Hundred" || word == "Thousand")
                {
                    result += currentNumber * wordToNumber[word];
                    currentNumber = 0;
                }
                else
                {
                    // Accumulate the numerical value for the current word
                    currentNumber += wordToNumber[word];
                }
            }else{
                return -1;
            }
        }

        // Add the last accumulated number to the result
        result += currentNumber;

        return result;
    }

}
