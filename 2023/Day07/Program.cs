
using System;
using System.Data;

namespace y2023;

public class DaySeven(string filePath) : main.CalendarCode(filePath){
    public override void Execute(string[] args){
        

        Func<int> PartOne = () => {
           return FirstPart(args);
        };

        var Result = IterateWithTime(PartOne,10000,100);

        Console.WriteLine("--- TIME ---");
        Console.WriteLine(Result["time"]);

        Console.WriteLine();

        Console.WriteLine("--- DATA ---");
        Console.WriteLine(Result["data"]);
    }

    public static int FirstPart(string[] args){
        ReadOnlySpan<string> LineList = args.AsSpan();
        List<Hand> Hands = [];

        for(int x = 0; x < LineList.Length; x++){
            List<string> CurrentLine = [.. LineList[x].Split(' ')];

            Hands.Add(new Hand(CurrentLine[0], int.Parse(CurrentLine[1])));
        }

        Span<Hand> SortedHands = [.. Hands.OrderBy(data => data.Value).Reverse()];

        int total = 0;
        for(int x  = 0; x < SortedHands.Length; x++){
            total += (SortedHands.Length - x) * SortedHands[x].Bet;
        }

        return total;
    }
}

public class Hand(string hand, int Bet) : IComparable{
    public string hand = hand;
    public int Bet = Bet;
    public int Value = GetValue(hand);

    public int CompareTo(object? obj){
        if(obj != null && obj is Hand hand1){
            return Value.CompareTo(hand1.Value);
        }

        throw new Exception("CompareTo Out Of Range");
    }

    public static int GetValue(string hand){
        int Value = 0;

        char[] HandRepresentation = (hand + "").ToCharArray();
        Dictionary<char, int> Quantities = new(){
            {'2', 0},
            {'3', 0},
            {'4', 0},
            {'5', 0},
            {'6', 0},
            {'7', 0},
            {'8', 0},
            {'9', 0},
            {'T', 0},
            {'J', 0},
            {'Q', 0},
            {'K', 0},
            {'A', 0},
        };

        for(int x = 0; x < HandRepresentation.Length; x++){
            Quantities[HandRepresentation[x]] += 1;
        }

        int JokerCount = Quantities['J'];

        Span<int> HandSpan = Quantities.Where(kv => kv.Value > 0 && kv.Key != 'J').Select(data => data.Value).ToArray();

        if(HandSpan.Contains(5 - JokerCount) || JokerCount == 5){
            Value = 60000000;
        }else if(HandSpan.Contains(4 - JokerCount)){
            Value = 50000000;
        }else if(HandSpan.Contains(3 - JokerCount)){
            if(HandSpan.Length <= 2){
                Value = 40000000;
            }else{
                Value = 30000000;
            }
        }else if(HandSpan.Contains(2 - JokerCount)){
            if(HandSpan.Length <= 3){
                Value = 20000000;
            }else{
                Value = 10000000;
            }
        }


        for(int x = 0; x < HandRepresentation.Length; x++){
            Value += GetCardValue(HandRepresentation[x]) * (int)Math.Pow(15, HandRepresentation.Length - x);
        }

        return Value;
    }

    public static int GetCardValue(char c){
        if(char.IsNumber(c)){
            return int.Parse(c + "");
        }else{
            return c switch
            {
                'T' => 10,
                'J' => 1,
                'Q' => 11,
                'K' => 12,
                'A' => 13,
                _ => throw new ArgumentException($"Invalid card: {c}"),
            };
        }
    }

    public override String ToString(){
        return $"Hand Value: {Value}\nBet Value: {Bet}\n";
    }
}