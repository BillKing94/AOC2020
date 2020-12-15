using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day15
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

        public static string[] SplitNewLine(this string input) => input.Split('\n',
            StringSplitOptions.RemoveEmptyEntries).Select(str => str.Trim()).ToArray();

        public static long[] ParseLongArray(this string[] input) => input.Select(str => long.Parse(str)).ToArray();
    }

    // up is -y
    class V2
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

        public static V2 operator +(V2 p, V2 q)
            => new V2(p.X + q.X, p.Y + q.Y);

        public static V2 operator -(V2 p, V2 q)
            => new V2(p.X - q.X, p.Y - q.Y);

        public static V2 operator -(V2 v)
            => new V2(-v.X, -v.Y);

        public static V2 operator *(V2 v, double a)
            => new V2(v.X * a, v.Y * a);

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
        private const string input = "20,9,11,0,1,2";
        private const string sample = "0,3,6";

        static void Main(string[] args)
        {
            var startTime = DateTime.UtcNow;
            var numbers = Parse.ParseLongArray(input.Split(","));

            var spokenTurns = new Dictionary<long, int>();

            int turnNumber = 1;
            long lastNumber = 0;
            while (true)
            {
                if (turnNumber <= numbers.Length)
                {
                    lastNumber = numbers[turnNumber - 1];
                    spokenTurns[lastNumber] = turnNumber;
                }
                else
                {
                    var laterTurn = turnNumber - 1;
                    
                    if (spokenTurns.TryGetValue(lastNumber, out int earlierTurn))
                    {
                        spokenTurns[lastNumber] = laterTurn;
                        var difference = laterTurn - earlierTurn;
                        lastNumber = difference;
                    }
                    else
                    {
                        spokenTurns[lastNumber] = laterTurn;
                        lastNumber = 0;
                    }
                }

                // Console.WriteLine($"{turnNumber}: {lastNumber}");

                // if (turnNumber == 2020)
                if (turnNumber == 30000000)
                {
                    Clipboard.Set(lastNumber);
                    Console.WriteLine(DateTime.UtcNow - startTime);
                    return;
                }
                else
                {
                    turnNumber++;
                }
            }
        }
    }
}