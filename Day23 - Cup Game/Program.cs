using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace Day23
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
        private const int input = 853192647;
        private const int sample = 389125467;

        static void Main(string[] args)
        {
            // from part 1 when i was using lists and arrays for everything
            // int findIdx(IList<int> arr, int value)
            // {
            //     for (int i = 0; i < arr.Count; i++)
            //     {
            //         if (arr[i] == value) return i;
            //     }
            //
            //     throw new Exception();
            // }

            var cups = new LinkedList<int>(input.ToString().Select(c => int.Parse(c.ToString())));
            var max = cups.Max();

            while (cups.Count < 1_000_000)
            {
                max++;
                cups.AddLast(max);
            }
            
            var labelToNode = new LinkedListNode<int>[max + 1];
            for (var node = cups.First; node != null; node = node.Next)
            {
                labelToNode[node.Value] = node;
            }

            var currentCup = cups.First;

            LinkedListNode<int> doMove(LinkedList<int> cups, LinkedListNode<int> currentCup)
            {
                // var currentIdx = findIdx(cups, currentCup);
                var cupsToRemove = new List<LinkedListNode<int>>();
                var toRemove = currentCup;
                for (int iToRemove = 1; iToRemove <= 3; iToRemove++)
                {
                    toRemove = toRemove.Next;
                    if (toRemove == null) toRemove = cups.First;
                    cupsToRemove.Add(toRemove);
                }

                var destinationCupVal = (currentCup.Value - 1);
                if (destinationCupVal < 1) destinationCupVal = max;

                while (cupsToRemove.Any(c => c.Value == destinationCupVal))
                {
                    destinationCupVal = destinationCupVal - 1;
                    if (destinationCupVal < 1) destinationCupVal = max;
                }

                foreach (var node in cupsToRemove)
                {
                    cups.Remove(node);
                }

                var destinationCup = labelToNode[destinationCupVal];
                foreach (var removedNode in cupsToRemove)
                {
                    destinationCup = cups.AddAfter(destinationCup, removedNode.Value);
                    labelToNode[removedNode.Value] = destinationCup;
                }

                var newCurrentCup = currentCup.Next;
                if (newCurrentCup == null) newCurrentCup = cups.First;

                return newCurrentCup;
            }

            string printCups()
            {
                var sb = new StringBuilder();
                var cup = labelToNode[1].Next;
                while (true)
                {
                    if (cup != null)
                    {
                        sb.Append(cup.Value);
                        cup = cup.Next;
                    }
                    else break;
                }

                cup = cups.First;
                while (cup != labelToNode[1])
                {
                    sb.Append(cup.Value);
                    cup = cup.Next;
                }

                return sb.ToString();
            }


            for (int turn = 0; turn < 10_000_000; turn++)
            // for (int turn = 0; turn < 10; turn++)
            {
                currentCup = doMove(cups, currentCup);
                // Console.WriteLine(printCups());
                if (turn % 10000 == 0) Console.WriteLine(turn);
            }

            // Clipboard.Set(printCups(cups));

            var cupA = labelToNode[1].Next;
            if (cupA == null) cupA = cups.First;
            var cupB = cupA.Next;
            if (cupB == null) cupB = cups.First;

            long result = (long) cupA.Value * cupB.Value;
            Clipboard.Set(result);

        }
    }
}