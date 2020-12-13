using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text.RegularExpressions;

namespace Day13
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
        private const string input = @"1007153
                                       29,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,37,x,x,x,x,x,433,x,x,x,x,x,x,x,x,x,x,x,x,13,17,x,x,x,x,19,x,x,x,23,x,x,x,x,x,x,x,977,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,41";

        private const string sample = @"939
                                        7,13,x,x,59,x,31,19";

        private const string sample2 = @"blah
1789,37,47,1889";

        static void Main(string[] args)
        {
            var lines = input.SplitNewLine();

            var numbers = lines[1].Split(",").Select((str, idx) => (
                id: int.TryParse(str, out int t) ? new int?(t) : default(int?),
                idx: idx
            )).Where(t => t.id != null).Select(t => (id: t.id.Value, offsetTime: t.idx)).ToArray();


            long interval1 = numbers[0].id;
            long sum1 = 0;

            Console.WriteLine(long.MaxValue);

            for (int iN = 1; iN < numbers.Length; iN++)
            {
                long interval2 = numbers[iN].id;
                var offset = numbers[iN].offsetTime;

                long sum2 = 0;

                long? initialAlignment = null;

                while (true)
                {
                    unchecked
                    {
                        if (sum1 < 0 || sum2 < 0) throw new Exception("uh oh");

                        if (sum2 - sum1 >= offset)
                        {
                            sum1 += interval1;
                        }
                        else
                        {
                            // took about 90 minutes to write this line...............
                            long increment = (long)(Math.Ceiling((double)(offset + sum1 - sum2) / interval2)) * interval2;
                            sum2 += increment;
                        }
                    }

                    if (sum2 - sum1 == offset)
                    {
                        Console.WriteLine($"{iN}: Correct offset occurs at {sum1}");
                        if (initialAlignment.HasValue)
                        {
                            long alignmentInterval = sum1 - initialAlignment.Value;

                            sum1 = initialAlignment.Value;
                            interval1 = alignmentInterval;

                            break;
                        }
                        else
                        {
                            initialAlignment = sum1;
                            if (iN == numbers.Length - 1)
                            {
                                Clipboard.Set(sum1);
                                return;
                            }
                        }
                    }
                }
            }

            // part1 
            // var earliestTime = int.Parse(lines[0]);
            // var busses = lines[1].Split(",").Where(str => str != "x").Select(str => int.Parse(str.Trim())).ToArray();
            //
            // var timeLeft = busses.Select(bTime => (id: bTime, timeToWait: bTime - (earliestTime % bTime))).ToArray();
            // var ideal = timeLeft.OrderBy(b => b.timeToWait).First();
            //
            // Clipboard.Set(ideal.id * ideal.timeToWait);
        }
    }
}