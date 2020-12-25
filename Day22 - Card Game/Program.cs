using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Day22
{
    static class RegexExtensions
    {
        public static Match AssertMatch(this Regex regex, string input)
        {
            var match = regex.Match(input);
            return match.Success
                ? match
                : throw new Exception($"Regex does not match. Input: {input}");
        }
    }

    static class Parse
    {
        public static string[] SplitDoubleNewline(this string input) => Regex.Split(input.Trim(), "\\n\\s*\\n");

        public static string[] SplitNewLine(this string input) =>
            input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(str => str.Trim()).ToArray();

        public static long[] ParseLongArray(this string[] input) => input.Select(str => long.Parse(str)).ToArray();
    }

    // up is -y
    struct V2
    {
        public double X;
        public double Y;

        public V2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double Magnitude => Math.Sqrt(this.X * this.X + this.Y * this.Y);
        public double Manhattan => Math.Abs(this.X) + Math.Abs(this.Y);

        public V2 Unit => this * (1 / this.Magnitude);

        public static V2 Up => new V2(0, -1);
        public static V2 Down => new V2(0, 1);
        public static V2 Right => new V2(1, 0);
        public static V2 Left => new V2(-1, 0);

        public static V2 operator +(V2 p, V2 q)
            => new V2(p.X + q.X, p.Y + q.Y);

        public static V2 operator -(V2 p, V2 q)
            => new V2(p.X - q.X, p.Y - q.Y);

        public static V2 operator -(V2 v)
            => new V2(-v.X, -v.Y);

        public static V2 operator *(V2 v, double a)
            => new V2(v.X * a, v.Y * a);

        public static bool operator ==(V2 p, V2 q)
            => p.X == q.X && p.Y == q.Y;

        public static bool operator !=(V2 p, V2 q)
            => !(p == q);

        public override bool Equals(object? obj)
            => (obj is V2 other && this == other);

        public V2 RotateCCW() => new V2(x: this.Y, y: -this.X);
        public V2 RotateCW() => new V2(x: -this.Y, y: this.X);

        public V2 RotateCCW(int times, V2 pivot)
        {
            if (times >= 0)
            {
                var result = this - pivot;
                while (times > 0)
                {
                    result = result.RotateCCW();
                    times--;
                }

                return result + pivot;
            }
            else return RotateCW(-times, pivot);
        }

        public V2 RotateCW(int times, V2 pivot)
        {
            if (times >= 0)
            {
                var result = this - pivot;
                while (times > 0)
                {
                    result = result.RotateCW();
                    times--;
                }

                return result + pivot;
            }
            else return RotateCCW(-times, pivot);
        }

        public V2 RotateCCW(V2 pivot) => this.RotateCCW(1, pivot);
        public V2 RotateCW(V2 pivot) => this.RotateCW(1, pivot);
        public V2 RotateCCW(int times) => this.RotateCCW(times, new V2(0, 0));
        public V2 RotateCW(int times) => this.RotateCW(times, new V2(0, 0));

        public override string ToString() => $"({this.X}, {this.Y})";
    }

    struct V3
    {
        public double X;
        public double Y;
        public double Z;

        public V3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double Magnitude => Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        public double Manhattan => Math.Abs(this.X) + Math.Abs(this.Y) + Math.Abs(this.Z);

        public V3 Unit => this * (1 / this.Magnitude);

        public static V3 operator +(V3 p, V3 q)
            => new V3(p.X + q.X, p.Y + q.Y, p.Z + q.Z);

        public static V3 operator -(V3 p, V3 q)
            => new V3(p.X - q.X, p.Y - q.Y, p.Z - q.Z);

        public static V3 operator -(V3 v)
            => new V3(-v.X, -v.Y, -v.Z);

        public static V3 operator *(V3 v, double a)
            => new V3(v.X * a, v.Y * a, v.Z * a);

        public override bool Equals(object? obj)
            => (obj is V3 other) && other.X == this.X && other.Y == this.Y && other.Z == this.Z;

        public override int GetHashCode()
            => this.X.GetHashCode() * this.Y.GetHashCode() * this.Z.GetHashCode();

        public override string ToString() => $"({this.X}, {this.Y}, {this.Z})";
    }

    struct V4
    {
        public double X;
        public double Y;
        public double Z;
        public double W;

        public V4(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public double Magnitude => Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
        public double Manhattan => Math.Abs(this.X) + Math.Abs(this.Y) + Math.Abs(this.Z) + Math.Abs(this.W);

        public V4 Unit => this * (1 / this.Magnitude);

        public static V4 operator +(V4 p, V4 q)
            => new V4(p.X + q.X, p.Y + q.Y, p.Z + q.Z, p.W + q.W);

        public static V4 operator -(V4 p, V4 q)
            => new V4(p.X - q.X, p.Y - q.Y, p.Z - q.Z, p.W - q.W);

        public static V4 operator -(V4 v)
            => new V4(-v.X, -v.Y, -v.Z, -v.W);

        public static V4 operator *(V4 v, double a)
            => new V4(v.X * a, v.Y * a, v.Z * a, v.W * a);

        public override bool Equals(object? obj)
            => (obj is V4 other) && other.X == this.X && other.Y == this.Y && other.Z == this.Z && other.W == this.W;

        public override int GetHashCode()
            => this.X.GetHashCode() * this.Y.GetHashCode() * this.Z.GetHashCode() * this.W.GetHashCode();

        public override string ToString() => $"({this.X}, {this.Y}, {this.Z}, {this.W})";
    }

    static class Clipboard
    {
        public static void Set(object value)
        {
            Console.WriteLine($"Setting Clipboard: {value}");
            Process.Start("bash", $"-c \"echo {value} | xclip -selection c\"")?.WaitForExit();
        }
    }

    class Program
    {
        private const string input = @"Player 1:
                               23
                               32
                               46
                               47
                               27
                               35
                               1
                               16
                               37
                               50
                               15
                               11
                               14
                               31
                               4
                               38
                               21
                               39
                               26
                               22
                               3
                               2
                               8
                               45
                               19
                               
                               Player 2:
                               13
                               20
                               12
                               28
                               9
                               10
                               30
                               25
                               18
                               36
                               48
                               41
                               29
                               24
                               49
                               33
                               44
                               40
                               6
                               34
                               7
                               43
                               42
                               17
                               5";

        private const string sample = @"Player 1:
                                9
                                2
                                6
                                3
                                1
                                
                                Player 2:
                                5
                                8
                                4
                                7
                                10";

        static void Main(string[] args)
        {
            var lines = input.SplitNewLine();

            var deck1 = new Queue<int>(lines.Skip(1).TakeWhile(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => int.Parse(l)));
            var deck2 = new Queue<int>(lines.SkipWhile(l => !string.IsNullOrWhiteSpace(l)).Skip(2)
                .Select(l => int.Parse(l)));

            (int winnerNumber, Queue<int> winningDeck) playGame(IEnumerable<int> d1, IEnumerable<int> d2)
            {
                var deck1 = new Queue<int>(d1);
                var deck2 = new Queue<int>(d2);

                HashSet<string> seenLayouts = new HashSet<string>();
                string layoutsToString() => $"{string.Join("-", deck1)}:{string.Join("-", deck2)}";


                int? winner = null;
                {
                    while (deck1.Count > 0 && deck2.Count > 0)
                    {
                        if (!seenLayouts.Add(layoutsToString()))
                        {
                            winner = 1;
                            break;
                        }

                        var card1 = deck1.Dequeue();
                        var card2 = deck2.Dequeue();

                        Queue<int> roundWinnerDeck;
                        int winningCard;
                        int losingCard;

                        if (deck1.Count >= card1 && deck2.Count >= card2)
                        {
                            var (winnerNumber, _) = playGame(deck1.Take(card1), deck2.Take(card2));
                            
                            (roundWinnerDeck, winningCard, losingCard) = (winnerNumber == 1)
                                ? (deck1, card1, card2)
                                : (deck2, card2, card1);
                        }
                        else
                        {
                            if (card1 > card2)
                            {
                                roundWinnerDeck = deck1;
                                winningCard = card1;
                                losingCard = card2;
                            }
                            else if (card2 > card1)
                            {
                                roundWinnerDeck = deck2;
                                winningCard = card2;
                                losingCard = card1;
                            }
                            else throw new Exception(":(");
                        }
                        
                        roundWinnerDeck.Enqueue(winningCard);
                        roundWinnerDeck.Enqueue(losingCard);
                    }

                    if (winner == default)
                        winner = (deck1.Count > 0)
                            ? 1
                            : 2;
                }

                return (winner.Value, (winner == 1) ? deck1 : deck2);
            }

            var (_, winningDeck) = playGame(deck1, deck2);

            int score = 0;
            while (winningDeck.Count > 0)
            {
                var nCardsLeft = winningDeck.Count;
                var card = winningDeck.Dequeue();

                score += nCardsLeft * card;
            }

            Clipboard.Set(score);
        }
    }
}