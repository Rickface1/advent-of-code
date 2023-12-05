
namespace y2023;

public class DayFive(string filePath) : main.CalendarCode(filePath){
    public override void Execute(){
        Console.WriteLine(Parse(ReadAllLines().AsSpan()));
    }

    public static long Parse(Span<string> input){
        ReadOnlySpan<long> InitialSeeds = input[0][7..].Split(' ').Select(seed => long.Parse(seed)).ToArray().AsSpan();
        List<Range> Seeds = [];
        for(int a = 0; a < InitialSeeds.Length; a+= 2){
            Seeds.Add(new Range(InitialSeeds[a], InitialSeeds[a + 1]));
        }

        //List<long> Seeds = input[0][7..].Split(' ').Select(code => long.Parse(code)).ToList();
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

        for(int index = 0; index < indexes.Count - 1; index++){
            int TempIndexHolder = indexes[index + 1];
            ReadOnlySpan<string> strings = input[indexes[index]..TempIndexHolder];

            List<Ranges> RangeList = [];

            for(int line = 1; line < (indexes[index + 1] - indexes[index] - 1); line++){
                ReadOnlySpan<long> CurrentLine = strings[line].Split(' ').Select(long.Parse).ToArray().AsSpan();

                RangeList.Add(new Ranges(new Range(CurrentLine[1], CurrentLine[2]), new Range(CurrentLine[0], CurrentLine[2])));
            }

            List<Range> TempSeedList = [];

            for(int y = 0; y < Seeds.Count; y++){
                List<Range> TempNewSeedList = RangeList.Select(ranges => ranges.Split(Seeds[y])).ToList();

                if(!TempNewSeedList.Where(range => !range.Equals(new Range(-1, -1))).Any()){
                    TempSeedList.Add(Seeds[y]);
                }else{
                    TempSeedList.AddRange(TempNewSeedList);
                }
            }

            Seeds = TempSeedList;
        }

        return Seeds.Where(range => !range.Equals(new Range(-1, -1))).Select(range => range.StartingValue).Min();
    }
}

public class Range(long StartingValue, long Displacement){
    public long StartingValue = StartingValue;
    public long EndingValue = StartingValue + Displacement;
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

    public bool Touching(Range range){
        return (StartingValue >= range.StartingValue && StartingValue <= range.StartingValue + range.Displacement) ||
            (EndingValue >= range.StartingValue && EndingValue <= range.StartingValue + range.Displacement) ||
            (range.StartingValue >= StartingValue && range.StartingValue <= StartingValue + Displacement) ||
            (range.StartingValue + range.Displacement >= StartingValue && range.StartingValue + range.Displacement <= StartingValue + Displacement);
    }
}

public class Ranges(Range RootRange, Range DestinationRange)
{
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

    public Range Split(Range range){
        /*if (RootRange.Touching(range)){
            return new Range(Math.Max(RootRange.StartingValue, range.StartingValue), Math.Min(range.EndingValue,RootRange.EndingValue));
        }*/

        long intersectionStart = Math.Max(RootRange.StartingValue, range.StartingValue);
        long intersectionEnd = Math.Min(RootRange.EndingValue, range.EndingValue);
        long intersectionDisplacement = Math.Min(RootRange.Displacement, range.Displacement);

        if (intersectionStart <= intersectionEnd){
            return new Range(Value(intersectionStart), intersectionEnd - intersectionStart);
            //return new Range(Value(intersectionStart), intersectionEnd - intersectionStart);
        }

        return new Range(-1, -1);
    }

    public override string ToString(){
        return $"[y2023.Ranges]\n    [Root Range: {RootRange}]\n    [Destination Range: {DestinationRange}]";
    }
}