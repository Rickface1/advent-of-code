
namespace y2023;

public class DayFive(string filePath) : main.CalendarCode(filePath){
    public override void Execute(){
        Console.WriteLine(Parse(ReadAllLines().AsSpan()));
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

            //Iterate for all of the ranges of the seeds
            while(Seeds.Count > 0){
                //Get all of the ranges required for each seed
                List<(List<Range>,List<Range>)> Result = MapList.Select(map => map.Split(Seeds.Where(seed => seed != new Range(-1,-1)).ToList())).ToList();
                Seeds.AddRange(Result.SelectMany(result => result.Item2));


                List<Range> tempNewSeedList = Result.SelectMany(result => result.Item1).ToList();
                if (tempNewSeedList.Count > 0) {
                    TempSeedList.AddRange(tempNewSeedList);
                }

                Seeds.RemoveAt(0);
            }


            //Set Seeds for next iteration
            Range Example = new(-1, -1);
            Seeds = TempSeedList.Where(range => !range.Equals(Example)).ToList();
        }   

        //return the lowest seed
        return Seeds.Select(range => range.StartingValue).Min();
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

    public (List<Range> Related, List<Range> Unrelated) Split(List<Range> range){
        List<Range> Related = [];
        List<Range> Unrelated = [];

        Span<Range> SortedRange = [.. range.OrderBy(range => range.StartingValue)];

        long StartingValue = RootRange.StartingValue;

        for(int y = 0; y < range.Count; y++){
            long intersectionStart = Math.Max(StartingValue, range[y].StartingValue);
            long intersectionEnd = Math.Min(RootRange.EndingValue, range[y].EndingValue);

            if(intersectionStart <= intersectionEnd){
                if(range[y].StartingValue < StartingValue){
                    Related.Add(new Range(RootRange.StartingValue, range[y].EndingValue - range[y].StartingValue));
                }


                Related.Add(new Range(Value(intersectionStart), intersectionEnd - intersectionStart));
                //Console.WriteLine(new Range(Value(intersectionStart), intersectionEnd - intersectionStart));

                StartingValue = intersectionEnd;
            }
        }

        long IntersectionStart = Math.Max(StartingValue, range.Last().StartingValue);
        long IntersectionEnd = Math.Min(RootRange.EndingValue, range.Last().EndingValue);

        if(IntersectionStart <= IntersectionEnd){
            if(range.Last().StartingValue < RootRange.EndingValue)
                Unrelated.Add(new Range(range.Last().EndingValue, range.Last().EndingValue - range.Last().StartingValue));
        }

        return (Related, Unrelated);
    }

    public override string ToString(){
        return $"[y2023.Map]\n    [Root Range: {RootRange}]\n    [Destination Range: {DestinationRange}]";
    }
}