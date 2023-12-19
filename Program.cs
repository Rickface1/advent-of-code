using System;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using advent;

namespace advent;


class MainRunner
{
    public static void Main(){
        PromptUser();
    }

    public static void PromptUser(){
        var LastInput = "";

        while(LastInput != "exit"){
            Console.WriteLine("--- Insert Command ---");
            string Temp = Console.ReadLine() ?? LastInput;
            if(Temp == "")
                Temp = LastInput;
            LastInput = Temp;
            List<string> input = LastInput.Split(' ').ToList();

            Console.WriteLine();

            if(LastInput == "gen next day"){
                GenerateNextDay();
            }else if(input.Count > 0 && input[0] == "run"){
                if(input.Count > 1){
                    if(input[1] == "sample")
                        if(input.Count < 3 || input[2] == "" || input[2] == null)
                            ExecuteSampleCode();
                        else
                            ExecuteSampleCode(LastInput);
                    else if(input[1] == "input")
                        ExecuteCode();
                    else if(input.Count > 2)
                        ExecuteCode(input[1], input[2]);
                    else
                        ExecuteCode(input[1]);
                }
            }

            Console.WriteLine();
        }
    }

    public static Task<string> GetInput(string year, string CurrentDay){
        string url = $"https://adventofcode.com/{year}/day/{CurrentDay}/input";
        string cookie = "53616c7465645f5f5021ed0d30c7fe2fd9f5767504093b7960141e7b3b8b4b7a93cf2df6c8274e697cafc0f764aa65679cee39abfb3214ea9667c964321a2ed9";

        Console.WriteLine(url);

        using var handler = new HttpClientHandler{
            CookieContainer = new CookieContainer(),
            UseCookies = true
        };

        handler.CookieContainer.Add(new Uri("https://adventofcode.com/"), new Cookie("session", cookie));
        
        using HttpClient client = new(handler);

        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

        try{
            // Send a GET request to the URL
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Check if the request was successful
            if (response.IsSuccessStatusCode){
                // Read the content as a string
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);

                Console.WriteLine("Finished Printing Content");
                
                // Output the content
                return Task.FromResult(content);
            }else{
                Console.WriteLine($"Failed to retrieve content. Status code: {response.StatusCode}");
            }
        }catch (Exception ex){
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return Task.FromResult("");
    }

    public static void GenerateNextDay(){
        // Get the current year and month in the Eastern Time (EST) zone
        DateTime currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        int currentYear = int.Parse(currentDateTime.ToString("yyyy"));

        string namespacePrefix = $"y{currentYear}.Day";

        // Find all classes in the current year's namespace
        List<Type> classesInNamespace = GetClassesInNamespace(namespacePrefix);

        // Find the class with the highest day
        var mostRecentClass = GetMostRecentClass(classesInNamespace);

        // Determine the next day
        string nextDay = (mostRecentClass?.Name != null)
            ? new Func<string>(() => {
                int number = OtherWordsToNumber(mostRecentClass.Name.Substring(mostRecentClass.Name.LastIndexOf("Day") + 3)) + 1;
                return number > 0 && number < 10 ? '0' + number.ToString() : number.ToString();
            })()
            : "00";


        // Construct the next day's folder name
        string nextDayFolder = $"Day{nextDay}";

        // Create the folder for the next day
        string folderPath = Path.Combine(Directory.GetCurrentDirectory() + "../../../../" + currentYear + "/", nextDayFolder);
        Directory.CreateDirectory(folderPath);

        // Generate Program.cs
        string programCode = GenerateProgramCode(namespacePrefix.Substring(0,5), nextDayFolder.Substring(3));
        File.WriteAllText(Path.Combine(folderPath, "Program.cs"), programCode);

        // Generate input.txt
        Task<string> getInputTask = GetInput(currentYear.ToString(), int.Parse(nextDay).ToString());
        string ReturnValue = getInputTask.Result; // Use .Result to await synchronously
        
        Console.WriteLine(ReturnValue);
        Console.WriteLine("Finished Printing Return");
        File.WriteAllText(Path.Combine(folderPath, "input.txt"), ReturnValue);

        //Generate empty sample.txt
        File.WriteAllText(Path.Combine(folderPath, "sample.txt"), null);


        Console.WriteLine($"Next day folder created: {nextDayFolder}");
    }

    private static string GenerateProgramCode(string namespacePrefix, string nextDayFolder){
        return 
$@"namespace advent.{namespacePrefix};

public class Day{NumberToWords(int.Parse(nextDayFolder))}(string filePath) : CalendarCode(filePath){{
    public override void Execute(string[] args){{
        args.ToList().ForEach(data => Console.WriteLine(data));
    }}
}}
        ";
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
                    ExecuteMethod(dynamicType, className, "input");
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

    public static void ExecuteSampleCode(){
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
                    ExecuteMethod(dynamicType, className, "sample");
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

    public static void ExecuteSampleCode(string data){
        // Assuming the class and method exist
        var dynamicType = Type.GetType(data);

        if (dynamicType != null){
            ExecuteMethod(dynamicType, data, "sample");
        }else{
            Console.WriteLine($"Class {data} not found.");
        }
    }

    public static void ExecuteCode(string data){
        // Assuming the class and method exist
        var dynamicType = Type.GetType("advent." + data);

        if (dynamicType != null){
            ExecuteMethod(dynamicType, data, "input");
        }else{
            Console.WriteLine($"Class {data} not found.");
        }
    }

    public static void ExecuteCode(string data, string input){
        // Assuming the class and method exist
        var dynamicType = Type.GetType("advent." + data);

        if (dynamicType != null){
            ExecuteMethod(dynamicType, data, input);
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
        string highestDay = "-1";

        foreach (Type type in classes){
            string typeName = type != null ? type.FullName ?? "" : "";

            // Extract day from the class name
            string day = WordsToNumber(typeName.Substring(typeName.LastIndexOf("Day") + 3));

            if (day != "-1"){
                if (int.Parse(day) > int.Parse(highestDay)){
                    highestDay = day;
                    mostRecentClass = type;
                }
            }
        }

        return mostRecentClass;
    }

    private static void ExecuteMethod(Type dynamicType, string className, string input){
        className = className.Split('.')[^1][3 ..];

        var instance = Activator.CreateInstance(dynamicType, "Day" + WordsToNumber(className));

        // Assuming there is a method with the constructed name
        var method = dynamicType.GetMethod("Execute");

        if (method != null){
            // Invoke the method
            var CalendarObject = (CalendarCode?)instance ?? null;
            if(CalendarObject != null){
                string[] args = $"{input}.txt" == "sample.txt" ? CalendarObject.ReadAllSampleLines() : CalendarObject.ReadAllLines();
                method.Invoke(instance, [args]);
            }
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

    private static int RecursionWordsToNumber(string words){
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

    private static string WordsToNumber(string words){
        int ReturnVal = RecursionWordsToNumber(words);
        return ReturnVal < 10 && ReturnVal  > 0 ? '0' + ReturnVal.ToString() : ReturnVal.ToString();
    }

    private static int OtherWordsToNumber(string words){
        return RecursionWordsToNumber(words);
    }
}
