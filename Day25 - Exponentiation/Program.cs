using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day25
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
        public const string input = @"9717666
                                      20089533";
        
        public const string sample = @"5764801
17807724";
        
        static void Main(string[] args)
        {
            var lines = input.SplitNewLine().ParseLongArray();

            var pubKey1 = lines[0];
            var pubKey2 = lines[1];
            
            // var encrKey = (pubKey1 * (pubKey2 / 7)) % 20201227;
            // Clipboard.Set(encrKey);

            long? loop1 = null;
            long? loop2 = null;

            const int mod = 20201227;

            int value = 1;
            int nLoops = 0;
            while (true)
            {
                nLoops++;
                value = value * 7 % mod;
                if (value == pubKey1)
                {
                    loop1 = nLoops;
                    break;
                }

                if (value == pubKey2)
                {
                    loop2 = nLoops;
                    break;
                }
            }

            long longPowMod(long mantissa, long exponent, long mod)
            {
                long result = 1;
                while (exponent > 0)
                {
                    result = (result * mantissa) % mod;
                    exponent--;
                }

                return result;
            }

            double result;
            if (loop1.HasValue)
            {
                result = longPowMod(pubKey2, loop1.Value, mod);
            }
            else if (loop2.HasValue)
            {
                result = longPowMod(pubKey1, loop2.Value, mod);
            }
            else throw new Exception(":(");
            
            Clipboard.Set(result);
        }
    }
}