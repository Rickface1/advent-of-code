using advent;

namespace advent.y2023;

public class DayFive(string filePath) : CalendarCode(filePath){
    public override void Execute(string[] args){
        Console.WriteLine(Parse(args.AsSpan()));
    }

    public static long Parse(Span<string> input){

        //Split First Line into Seeds
        ReadOnlySpan<long> InitialSeeds = input[0][7..].Split(' ').Select(seed => long.Parse(seed)).ToArray().AsSpan();
        List<Range> Seeds = [];
        for(int a = 0; a < InitialSeeds.Length; a+= 2){
            Seeds.Add(new Range(InitialSeeds[a], InitialSeeds[a + 1]));
        }

        //Indexes of all sections
        List<int> indexes = [
            input.IndexOf("seed-to-soil map:"),
            input.IndexOf("soil-to-fertilizer map:"),
            input.IndexOf("fertilizer-to-water map:"),
            input.IndexOf("water-to-light map:"),
            input.IndexOf("light-to-temperature map:"),
            input.IndexOf("temperature-to-humidity map:"),
            input.IndexOf("humidity-to-location map:"),
            input.Length
        ];

        //loop through all indexes
        for(int index = 0; index < indexes.Count - 1; index++){

            //Get all strings from start of section to start of next
            int TempIndexHolder = indexes[index + 1];
            ReadOnlySpan<string> strings = input[indexes[index]..TempIndexHolder];

            List<Map> MapList = [];

            //Split each line into map
            for(int line = 1; line < (indexes[index + 1] - indexes[index] - 1); line++){
                ReadOnlySpan<long> CurrentLine = strings[line].Split(' ').Select(long.Parse).ToArray().AsSpan();

                MapList.Add(new Map(new Range(CurrentLine[1], CurrentLine[2]), new Range(CurrentLine[0], CurrentLine[2])));
            }

            List<Range> TempSeedList = [];

            Console.WriteLine("Starting loop");

            Seeds = Break(Seeds, MapList);
        }   

        //return the lowest seed
        return Seeds.Select(range => range.StartingValue).Min();
    }

    public static List<Range> Break(List<Range> SeedList, List<Map> MapList){
        List<Range> TempSeedList = [];
        MapList = [.. MapList.OrderBy(data => data.RootRange.StartingValue)];
        SeedList = [.. SeedList.OrderBy(data => data.StartingValue)];

        for(int x = 0; x < SeedList.Count; x++){
            List<Range> ValidRanges = SeedList.SelectMany(data => data.Contained(MapList.Select(data => data.RootRange).ToList())).ToList();
            //ValidRanges.AddRange(ValidRanges.Select(data => SeedList.Select(seed => seed.Contained(data))));
        }

        return [];
    }
}

public class Range(long StartingValue, long Displacement){
    //start value of range
    public long StartingValue = StartingValue;
    //end value of range
    public long EndingValue = StartingValue + Displacement;
    //number of values in the range
    public long Displacement = Displacement;

    public override string ToString(){
        return $"Starting: {StartingValue} Ending: {EndingValue} Displacement: {Displacement}";
    }

    public override bool Equals(object? obj) {
        if (obj is Range other){
            return StartingValue == other.StartingValue && Displacement == other.Displacement;
        }

        return false;
    }

    public bool Contained(Range value){
        return StartingValue <= value.StartingValue && value.EndingValue <= EndingValue;
    }

    public Range Overlaps(Range value){
        return new Range(Math.Max(StartingValue, value.StartingValue), Math.Max(EndingValue, value.EndingValue) - Math.Max(StartingValue, value.StartingValue));
    }

    public List<Range> Contained(List<Range> value){
        return value.Where(data => data.Contained(this)).Select(data => data.Overlaps(this)).ToList();
    }

    public List<Range> FillInValues(List<Range> ranges){
        return [];
    }

    public override int GetHashCode(){
        return HashCode.Combine(StartingValue, Displacement);
    }
}

public class Map(Range RootRange, Range DestinationRange){
    //Range to translate from
    public Range RootRange = RootRange;
    public Range DestinationRange = DestinationRange;

    public bool Contained(long value){
        return RootRange.StartingValue <= value && value <= RootRange.EndingValue;
    }

    public bool Contained(Range value){
        return RootRange.StartingValue <= value.StartingValue && value.EndingValue <= RootRange.EndingValue;
    }

    public long Value(long value){
        return value - RootRange.StartingValue + DestinationRange.StartingValue;
    }

    public long Value(Range value){
        return value.StartingValue - RootRange.StartingValue + DestinationRange.StartingValue;
    }

    public override string ToString(){
        return $"[y2023.Map]\n    [Root Range: {RootRange}]\n    [Destination Range: {DestinationRange}]";
    }
}