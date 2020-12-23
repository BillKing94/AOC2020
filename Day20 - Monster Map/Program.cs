using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace Day20
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
        private const string input = @"Tile 2609:
#....#.#..
.####...#.
#..#.##.##
#..#......
#.##..#...
##.#..#..#
#...##...#
..#.##...#
#....#....
..#..####.

Tile 3347:
.#.####...
.#........
.##......#
.......##.
#.#.....#.
#.####...#
#..##.#.#.
#...#....#
.##.....#.
###.##.##.

Tile 2801:
#####.....
....#.....
#.##....#.
#......###
....#....#
..#.##....
.#.#.#..#.
..#.#..#.#
..........
.####.####

Tile 2851:
..#....#.#
..#####...
.#..##.#.#
##..#.####
#####.##..
..#.#.....
##.#.....#
###......#
#.....#.#.
..#.###.#.

Tile 2297:
.##.##.###
##...#.#.#
...##..#..
..#.###.#.
##...#...#
....#.#..#
#..##.#..#
##.#.#..##
#...#..#..
##.##...#.

Tile 1237:
#....#.#..
..#.###...
..##.#..##
#..#..##.#
#..#.###..
...##.#...
#...##.#..
#...##..##
...#.#..#.
..#.####..

Tile 1931:
.#..#####.
.#..#..#.#
..###..##.
....#.#.#.
##..#.####
###..#..##
.....#...#
###.......
.##.#.#...
.#..######

Tile 3989:
..#..##..#
#....#....
...##....#
..#..#...#
##........
#.........
#........#
..##..#.#.
.#####.###
#####.###.

Tile 1129:
#..##..#.#
#.......#.
#.......##
#.........
...#.###..
#.#.###...
##.#.#.#..
##...###.#
#..#####.#
#.####..#.

Tile 3491:
##..###.#.
#.........
...###..#.
#.#####..#
####...###
...###...#
..#.##.#..
#.#.##...#
...#..#..#
###.#.####

Tile 1607:
...##.####
#.#.....##
#.....#...
#..##.#.#.
........#.
#....###.#
.....#..##
#..#....#.
##.......#
...#.#####

Tile 3307:
..##..###.
..#.##..##
###.......
#..#....##
#...#...##
#.........
#####.....
#.##..#...
#...#....#
...####.##

Tile 3931:
.##....#.#
##.#..#..#
##..###..#
#.#.##....
#...##..#.
##.##.....
..##....#.
#....##.#.
.#..#.#.#.
..#.####..

Tile 2011:
...###.###
.#.#......
.#.......#
..#..#....
##.#.#....
#.#.#...##
#...#####.
#...#.#...
#..#......
..###.#..#

Tile 1033:
..##.##.##
#..#..##.#
.....#.##.
........#.
.....#...#
.##....###
#..#..#..#
##..###..#
#....#...#
####.##.#.

Tile 3761:
##.....###
#...##.#.#
....#.#..#
###.#..#..
#.####.#.#
..#.###..#
........##
.###..#...
..........
.##.#.####

Tile 1277:
#.###...##
#..#......
..#.......
.....#....
#.....#..#
#....#..##
##..#...#.
#....#...#
#....#.#.#
.###.##..#

Tile 1487:
##.....##.
#.#.......
.####..#..
#...#..#..
##...#...#
##.....#..
##.#.#....
#.....#.##
....#.....
.....#..#.

Tile 3617:
..#.##.#.#
.....###..
#.#......#
..#.#..#..
....#.#.##
#...#.#...
###..##...
..#..##..#
#........#
.#.##.#..#

Tile 3121:
###..#...#
..#......#
#.#.......
#..#......
...#.#...#
.....#.#..
#......#..
.#.##..###
.........#
.##.#####.

Tile 2017:
.##..#....
.#.##...##
...#..#..#
#.#.......
#.##...#.#
#.....#..#
........#.
#...#...#.
......#..#
#.###..###

Tile 3119:
##...#.###
##....#..#
#........#
.#..#...##
.....#....
#.#.......
##........
..#...#...
..###.....
.#.#.#.###

Tile 1933:
...##.#...
#..#......
.....#..##
##..#.....
#.#..##...
....#.#...
#......#..
.##.......
#..#......
#.####.#.#

Tile 1597:
.#.##.#.#.
....#####.
#..##...#.
........#.
....#.###.
.....#....
#.#..##...
......#.##
...#....##
...##.#.##

Tile 2467:
#.#.#.####
......###.
.##..#...#
.....#...#
#......##.
......#..#
#....###..
...#..#...
...#.#..##
.#.####...

Tile 1427:
##.###.#.#
#.......#.
...#.#.#.#
##...#.#.#
...#.##..#
.##...#...
##.#.#....
.....#.#.#
..#......#
....#....#

Tile 2351:
.#.#..###.
#.##..####
#...#.....
#........#
#....#...#
..#.....#.
#..#####..
..#.##..#.
.........#
#..#.#.#..

Tile 2243:
.....#.#..
#.##..#..#
.#.......#
...#......
##.#.....#
....#.#..#
##...#...#
#...#.....
.#....#.##
##.#.#.#..

Tile 3163:
.#.....#.#
.#.....###
##.....#.#
..###..#.#
.#........
###.#.##.#
..#..##.#.
.....#..#.
#.....#..#
..###.....

Tile 2281:
##.#..#.#.
###....#.#
#....#..#.
.##....##.
#..#...###
#........#
...##.....
...#.....#
.##.####.#
..#.###.##

Tile 2213:
...####..#
...#..#...
#....###.#
..#..#..##
###.#..#..
....##...#
##.......#
.#..##.#.#
.#..#####.
.##...#...

Tile 1097:
#.##...#.#
.....#.#.#
....#..#.#
...#......
#......###
#.........
.#..#..#..
.#.......#
.......##.
...#...#..

Tile 3821:
.#....####
....#.#..#
.#...#.#.#
...#..##.#
#..#..#...
#.#..#.#.#
#.#.####..
##...##..#
#......##.
#..#.##.#.

Tile 2699:
#..#..#.#.
#......#.#
#........#
...#..#..#
....###...
#.....##.#
...#...#..
....#...#.
#.....#..#
###.#...#.

Tile 1231:
..##..#.#.
..##.#.#.#
.....#...#
..#.#....#
#..#....#.
.##..##...
...#..#.##
#..#......
#.#......#
###.######

Tile 3631:
#..#...##.
#...#..#..
.#....#..#
...##.....
.....#....
.........#
..#.......
.#.###...#
...#..#..#
.#..###...

Tile 1913:
..#.##...#
#....#....
.#..##...#
#.........
#......#..
####......
#.###.....
##........
.#..#.....
##.#..##.#

Tile 1013:
.....##.#.
##.###....
.###.#...#
#........#
..........
..........
#.#....###
....##....
###.#.####
##..###...

Tile 1181:
####..#.#.
#.#.#..#..
.....###.#
##..#..#.#
#.#.....#.
#.##.#.#..
##....###.
..##.#..#.
#...#.#..#
.#.##.#.##

Tile 2671:
...##.#..#
.#...#..##
#.#.......
#.......##
..#......#
#.##..#...
#....##..#
#..##....#
.....#.#..
.#.#....##

Tile 1721:
#...###.##
#....####.
..#....#.#
#....#....
#..###....
##..#..#.#
.##.###...
#.......##
#..#......
##..###..#

Tile 1877:
.###...#..
....#....#
#..#.....#
......#.#.
..#...##.#
#.#..##..#
#......###
....#.#.#.
#...#....#
.##.####..

Tile 1889:
..##.#.###
..#...#.#.
..#.##..##
#....#..##
#........#
##.#.#....
..##.....#
..##..#...
.#.#..#...
..####.#.#

Tile 1091:
..##...###
.....#....
.#.#.##..#
#..#...#.#
##.##...#.
#.......#.
.......#..
#.#..#.#.#
...#.#...#
..#...###.

Tile 2161:
###...#.#.
..#.....#.
###.......
..#.#.#...
..####..#.
##....##.#
......##.#
...#.#...#
.#..#.#..#
#.#...#.#.

Tile 3253:
....###.##
#......#.#
#...#.#..#
..#..#....
...#.....#
#...##....
..##...#..
..#.......
#....#...#
..#.......

Tile 3079:
#..#.#.#.#
#.#.......
#..#..#.#.
#.#.......
...##.####
...##.....
....#...##
...###.#.#
#.#..#...#
#.#.##.###

Tile 1201:
#.#..##.#.
........##
#..#..##..
..###....#
.##......#
.....#...#
#........#
#..#...###
#....#.#.#
.#..#..##.

Tile 3797:
#.#.#..#.#
##.#..#.#.
..#.#...##
.###..#...
#..#......
#..##..#.#
##.###...#
.....##...
#..###.#..
##.##.#...

Tile 2311:
......#.#.
##.....##.
.#.#.....#
#.#...#..#
....#.###.
....#...#.
#.#....##.
###.#..#.#
##.#..#..#
....#..##.

Tile 1187:
.#..##.#.#
#........#
#.#.#....#
.##.#..#..
##....##..
##.....#..
.#.#..#..#
.#......##
#.#......#
.#######..

Tile 3259:
#.#...##.#
...##.....
......#...
#..####...
#...#.....
###......#
..#..#...#
##........
#..#..##..
...###.#.#

Tile 3019:
#...##.###
.##......#
#..#....##
##.#.....#
#.#..#...#
###......#
...#.#.##.
#.......##
.#..##...#
..####....

Tile 1979:
.#####.#..
...###.#..
#.###...#.
#....##.#.
.#..#...#.
...#.....#
...#.#....
##..#.#...
..#...#.##
##.###..##

Tile 2287:
##.####..#
#..#......
#.........
..#...##..
#.##.#.#..
....#.#...
##....#..#
..#.#....#
....#...##
##..#..###

Tile 2437:
.#..#..#..
.#.#.#...#
#..#....##
#..##..###
.#..##.#.#
.....##...
.##..#..##
#.#.#.#..#
#......#.#
#...###.##

Tile 1571:
#.##.#...#
#.###.#..#
#...#..#..
..#..#..##
.###..#..#
###.#.#..#
#..###....
...#....##
.#..#.#...
#.##.####.

Tile 2833:
.##.##....
...#.#....
.#.##..##.
#.....#..#
#.....##..
..##...#.#
###....#.#
..........
#.........
..#.#.#.##

Tile 2861:
.#..#.....
..#......#
..#..#..#.
.#......##
###..#.#.#
#.......#.
.#....#...
....##...#
#...#..#..
#.#..#.###

Tile 3361:
...#..#.#.
....#.....
#...#....#
#.##...##.
##.......#
#........#
.#.....#..
#......###
.#.#.....#
##.#...##.

Tile 3929:
..####.###
#.##....#.
##........
...#......
#.....##.#
...##.##..
..#......#
.##.#.....
##.#...#..
.##.######

Tile 2711:
###..###..
.##...####
.......##.
.##.....#.
.#....#...
#.....#...
.###....##
...#.....#
#..#...###
##..###...

Tile 2789:
#.####...#
#.......#.
##....#.#.
#..###...#
#.#..#....
....#####.
##..#..#..
#...#....#
#.#...#...
.#....#..#

Tile 3511:
.#.#......
#.###..#.#
#..#.....#
#....#...#
#....####.
........##
..#...#.##
.#.....#.#
.##...#...
.#....##..

Tile 2417:
..##.#....
#.#..#....
....#.#..#
#.####.##.
..#..#..#.
##.##.....
....#....#
#.........
..#..#...#
....#..#..

Tile 1453:
...######.
#.......##
.##....###
.##..#...#
.......#..
#.....#...
##.....#..
##..##...#
..#......#
.##.#..###

Tile 3727:
#.#...#.#.
#.#.#.....
#####.#.##
#.##...#.#
#....#.##.
#.......#.
..#...#...
......#.##
##....#.##
#..###.##.

Tile 1061:
..#.#..#.#
.......#.#
##....##.#
.#.....#..
#.##...#.#
.....#..#.
...#......
#.....#..#
##....#..#
.#.###..##

Tile 2389:
#.#.##.#..
..#......#
#......###
...#.#....
.#........
......#.#.
...##..###
....##...#
...###....
###.###.##

Tile 2113:
##.#..###.
###.#....#
..#......#
#...##...#
...#...#..
#..#..#.#.
.##.......
.##.#..##.
.#.......#
###.#.#.#.

Tile 1741:
##..#...#.
#.....#...
...###...#
#..#...##.
....###...
#...###...
.....#.#..
#..##..#.#
#..#..##.#
#.#.##....

Tile 1283:
##....#.#.
.#..#.##..
##..#.....
.....#...#
#.........
..#..##...
#..#....##
...#.#####
...#....##
#..#.#..#.

Tile 1901:
#######.##
#.........
###..#...#
##.##.#..#
.....#..#.
#....#...#
.##.....#.
........##
...#####..
#.##.##.##

Tile 2357:
...#......
#..#.##...
...##.##..
#...##..##
#...##.#.#
#.##.....#
.....##...
..#...#...
.....##.##
...#.#....

Tile 3529:
.##..#.##.
..#.#..##.
#.#.....##
#..##...#.
##...###..
....#..###
#.#.....#.
.....##..#
.#.......#
######..##

Tile 1039:
...##.#.#.
#......#..
#.......##
##.#.##..#
......#..#
#....##..#
##.##.#...
#...#...##
...#..#..#
####.#.#..

Tile 2963:
..#.##...#
#.........
..##.....#
...#..#.#.
#..#.....#
..#..#....
....#.....
#...##.#.#
##.##..#.#
####.###.#

Tile 2557:
##.#..##..
....#....#
....#.....
##.##.#...
..#.##..##
#..#....##
#..#.#..#.
#.......##
#.........
.###.###..

Tile 1523:
#.#.###..#
#.#...#...
.........#
#...#.#..#
..#...#..#
#..#.##...
..###.#...
#....###..
#.#....#.#
#.##..#.#.

Tile 1789:
.##....###
##..#.....
..#.#..#.#
##.#.#..##
...#.##..#
#..#......
#..#.#.#..
##..#.....
#####.#..#
#..#....#.

Tile 3049:
....#.##.#
#.#.....#.
#......##.
#.##.....#
..#.#....#
#........#
#.#...#..#
#.#...#..#
#..#..#..#
.#.#.#..##

Tile 3719:
..#...##..
##..#....#
...#....##
#..#.##...
.##.#..#..
#...#####.
#.##.#..##
..#.##...#
#.....##.#
.##.#..##.

Tile 2069:
#.#.#.#...
#...##....
##..#.....
.#.#...#..
#.......#.
.#...#..#.
.#..#.....
###.....#.
#.#...##.#
#.#.#...##

Tile 2083:
.#.#######
...#..#.##
..###..#..
....#.....
#....####.
......#..#
##........
#...####.#
..#..##...
#.#..##.##

Tile 3671:
#.....###.
#....#....
..#.......
..#.#....#
....#..#.#
..##....##
#...#.##.#
##.#..##..
#..#......
.##.#.####

Tile 1103:
...#.##...
#..##.##.#
...##....#
#.....#..#
.##..#.###
##.......#
.........#
....#..#.#
##.....#.#
#.#..#..##

Tile 3643:
#..####.##
..#...#.##
.#.##..#..
..#.#.....
#....#..#.
..##..#...
#.#....###
..###.#..#
#..##.#..#
#..##.#..#

Tile 3499:
.##.#...#.
#..#####..
...#..##..
...#.....#
#.#....#..
#.##..#..#
#......#..
##..#..#..
##...#####
.##.###...

Tile 2503:
..#####..#
.......##.
#....#....
#.##...#..
..#......#
.##.#....#
###.#....#
#....##...
......#..#
###..#....

Tile 2729:
.###.##..#
#.....#...
#.#....##.
#....##..#
#.........
#..#....##
##..##.##.
.#......#.
#.#.##....
##.....###

Tile 2221:
.#.###.#.#
#...#....#
#....##...
......#..#
...#..#...
.....##..#
..#.##....
.##..##.##
....####.#
#####.....

Tile 2129:
#..###..##
#..#.#.##.
.#.#.....#
.#..##...#
#..#.#.#..
...#......
.#...###.#
....#.#...
.#.....#.#
..##.##..#

Tile 1217:
#######.##
#.......##
......##..
##.##.#.##
.........#
#....#..##
##..#....#
#.....#..#
#...##....
.######.#.

Tile 3967:
##...#..##
#.....#...
..####...#
##....####
...#..#...
.....#...#
#.....#...
##....#...
.#...##.##
...#.#####

Tile 2339:
..#..###..
#....#..#.
#.......##
.....#..#.
..#....#.#
.#..#.##..
#..#..#...
#...#....#
##.#..#..#
.#####....

Tile 1579:
#.####.#..
.......###
#........#
.#......##
#..##....#
..#.#....#
...#..#..#
#.#......#
..#.##.##.
...##.#..#

Tile 2683:
...#####.#
##.##.##..
..#...##.#
.#..#.....
......#...
....#.#..#
#...#.#...
###.##....
..##.....#
..###.#.##

Tile 1993:
#.##.##...
....#....#
...#.#...#
..#...##..
#..####...
###.#.##..
##.......#
#...##....
..#..#....
######..#.

Tile 2777:
###.#.#..#
.###.....#
#.#.....#.
.#.#.##.##
#.##....##
##..#.#..#
.#...#..#.
#..##..#..
#.##..#.#.
##.#.####.

Tile 2081:
.#.#.....#
##.#...###
#....#....
#.#..#..##
.........#
##.##..#.#
#.##....##
#.#...#..#
....##...#
####.##.#.

Tile 1009:
###.#.....
#..#.#.#..
..##...#.#
..##......
#...#..#..
##.#..#...
..##..#...
#...#..##.
##.#...###
#.##.#...#

Tile 1543:
#.#.#..#..
#....#...#
.....##.##
....#...##
.....#..#.
#.....#.##
#....##.##
#..##.#...
..#####..#
#....####.

Tile 3877:
..##.#.##.
...##.#...
#....#....
.##....#.#
##......#.
#...###..#
#......#.#
.#......#.
....#.#...
#.#.#.####

Tile 2857:
..#.#.###.
...#...#.#
........##
##...#..#.
.#.....#.#
...#......
#.........
#.#......#
#..#......
...##.###.

Tile 1481:
##.###.#.#
.#........
#.....#.#.
####..#..#
...#....##
#.##.#....
......#...
#.####....
#.......#.
##...#...#

Tile 1583:
.#.#.#.#.#
..#.......
.....#.#..
##...#.#..
..#.......
...#......
##........
#........#
..#......#
#..####.#.

Tile 1861:
##.##..#..
.....#....
#..#..##..
#.....###.
#..#.....#
...#..##..
##...#..#.
..........
......##..
###...####

Tile 2099:
..##....##
..#.#.#...
#########.
..###..#..
...##.#.##
....#.#...
.#.###.#..
....##...#
#..###.#..
###.##..#.

Tile 2819:
.##.......
....#.....
....####..
.#...###..
#.#..#....
#####....#
.#...#.#.#
#..#.#....
#...#..###
.#.#.....#

Tile 3803:
.....#.###
#..##.....
....#.....
#...###.#.
...#..#.#.
..........
#.......##
#...#.###.
#........#
#.##.....#

Tile 2803:
....#.#...
....#.....
....#.#..#
#.#..#....
.#.##..###
#..##.#.#.
..##..#...
##.##.....
...#.....#
##.#....##

Tile 1747:
#..#..#.#.
#...#...#.
#..#.....#
.#.#.##.#.
###.##..#.
...#....##
.........#
.........#
#......###
#.#.##..##

Tile 2137:
......#...
#..##...##
#..###....
#.##..#.##
....#....#
#..#...##.
....###..#
#......#.#
..#.....##
..###.##..

Tile 1847:
.###.#.###
......#..#
##.##...#.
#.....##..
##..#....#
.#.....#.#
...##.#...
.#..#...##
#...#.##..
.#.#.#...#

Tile 2381:
..#.#..#..
..#####...
.##.#.....
..........
.....#....
##.#...###
#.##..#...
##.#####.#
..##...##.
...###.#..

Tile 3659:
#.#.#.##..
##.##....#
#.##......
##.......#
#.#.#.....
##...#...#
#.#.#....#
#..#.##..#
##.##..#..
.#.####.##

Tile 2207:
....##.#..
.#..#.#...
#....#...#
#.#.#..###
#....#...#
##.#.####.
.####.##.#
##..####.#
#.##.#..#.
###.###..#

Tile 2879:
##.....##.
.#.#.#...#
#.....#..#
#....##.##
..#.......
#.#...#...
.#....##..
#.....#.#.
.##..#..#.
#.#######.

Tile 1321:
.....##...
#.#...##.#
#...##.##.
#...#.#..#
##..#.#..#
...#......
#.####..#.
#.........
..#.....##
########.#

Tile 3331:
#.##..####
#..#.#.#..
..#....##.
#..#.##..#
.......#..
#..#......
..#.#.#.##
#....####.
#..##.#..#
...#.##.##

Tile 3623:
###..###..
.##.#.##..
..###.....
#.#.#...##
##.#..#.##
.##.#.....
...#......
.#..#..#.#
##....####
.#.#.#...#

Tile 3637:
..##.##.##
.##.......
........#.
......##..
.#..#.##..
#....###.#
....#..#.#
.#.#.#..##
....###..#
....###.#.

Tile 2677:
.#.#.#.#..
#........#
....#.##.#
....######
##.#.##...
...#.#..#.
##......#.
..#.......
..####....
....####..

Tile 3299:
#...#..#..
....#.....
##.###..#.
#.........
#...##.#..
#...#..#.#
.#....#..#
##......#.
..#.#..#.#
#...###.#.

Tile 2647:
#..#.#.#..
###.##..##
..#......#
....#.....
....#.#..#
#.#..#..##
.#..#.###.
......##.#
##......#.
#.#..#.##.

Tile 3943:
###.######
..#.......
##..##..#.
..##..#..#
..#..##.##
.#..#..##.
.....##.##
#.##...##.
.#..####..
.#..#.#..#

Tile 2593:
#.#.#....#
.....#..#.
#...#.....
#.#..###.#
#...##....
##.#.#....
..#.#.#.##
.##.#.....
#.#......#
..##...#.#

Tile 3181:
....######
..........
.##....#.#
...#....##
.#.##.#.##
###....##.
...##...##
.#....#..#
....#.#..#
..#.#...##

Tile 3697:
#.#.##....
.#....##..
.#.#......
#....##.#.
#....###.#
...#.#.#..
.##.#.#..#
#...#....#
..#.#...#.
.##.###...

Tile 1213:
..#.###.#.
##.#...#.#
#.###.....
..##...#..
..#...#...
.....##.#.
#.#....#..
..........
.##...#.#.
#######.#.

Tile 2053:
.#.###..#.
#.#.#....#
.........#
..##...#.#
..#..#...#
##..#.####
#.##.#.#..
.##.#.##.#
#...#.##.#
#...#.....

Tile 2549:
#.##..#..#
#.#....#.#
....#.##.#
.#..#....#
..##....#.
##...##...
.#.....##.
..#....###
.......##.
#...##....

Tile 1451:
....##.###
..#....###
.##..##.#.
..........
......#.##
#....##.#.
#..#..#.#.
.....#..#.
.##....##.
.###.#.#.#

Tile 1613:
#.#.#.###.
.####..#.#
....#...##
##......##
#..#......
.##.#.....
.###....##
......#..#
#....#.##.
...######.

Tile 3301:
##.#.###..
.#...##...
#.##...#.#
........#.
#.#......#
.###..#.##
#.#.#..##.
#.........
......####
.###.#.##.

Tile 1787:
###...####
#.##..##.#
...##.#.#.
#.........
.......#.#
.#..#...##
.##.#..#..
#...#....#
#..#.....#
.#...##.#.

Tile 1051:
.........#
...#...#.#
#...###...
....#.....
.#.......#
.#.......#
...##.##..
###.##.#..
##..##....
..#...#.#.

Tile 3041:
#.#.....#.
.##....#..
.#....#.#.
.####...#.
#...##..#.
..##......
.#.#.#...#
.....##.##
.#.#.###..
.#.#.#.#.#

Tile 3691:
####.#.###
#.####..#.
#.###.....
...#.#.#.#
..##....##
.#.....#.#
###...#..#
.#.....###
#.####..##
...#####.#

Tile 3319:
..##.#.##.
..#.....##
#......###
##..##.#..
......#.#.
...####...
#.........
#.........
##....#...
##....###.

Tile 1381:
##.#.##...
.#....#...
#..##.#..#
..#.#.....
....##....
##.###....
#....##..#
.#....##.#
..#..#...#
#.#.....##

Tile 3673:
#.....##.#
###..#....
...#.....#
##...#...#
##...#...#
.#........
.....#...#
#.#..#....
#..#.....#
####.##...

Tile 1493:
###..#..#.
#..#......
.........#
....#.....
..###.....
.#.#.#...#
#..###.###
.##...#..#
#....#####
#.#..#.#..

Tile 1327:
#..#####..
....##..##
...#...##.
......##..
...#....#.
..#.#.....
..#.#.....
##....#...
.##....#..
.####.###. ";

        private const string sample = @"Tile 2311:
                                        ..##.#..#.
                                        ##..#.....
                                        #...##..#.
                                        ####.#...#
                                        ##.##.###.
                                        ##...#.###
                                        .#.#.#..##
                                        ..#....#..
                                        ###...#.#.
                                        ..###..###
                                        
                                        Tile 1951:
                                        #.##...##.
                                        #.####...#
                                        .....#..##
                                        #...######
                                        .##.#....#
                                        .###.#####
                                        ###.##.##.
                                        .###....#.
                                        ..#.#..#.#
                                        #...##.#..
                                        
                                        Tile 1171:
                                        ####...##.
                                        #..##.#..#
                                        ##.#..#.#.
                                        .###.####.
                                        ..###.####
                                        .##....##.
                                        .#...####.
                                        #.##.####.
                                        ####..#...
                                        .....##...
                                        
                                        Tile 1427:
                                        ###.##.#..
                                        .#..#.##..
                                        .#.##.#..#
                                        #.#.#.##.#
                                        ....#...##
                                        ...##..##.
                                        ...#.#####
                                        .#.####.#.
                                        ..#..###.#
                                        ..##.#..#.
                                        
                                        Tile 1489:
                                        ##.#.#....
                                        ..##...#..
                                        .##..##...
                                        ..#...#...
                                        #####...#.
                                        #..#.#.#.#
                                        ...#.#.#..
                                        ##.#...##.
                                        ..##.##.##
                                        ###.##.#..
                                        
                                        Tile 2473:
                                        #....####.
                                        #..#.##...
                                        #.##..#...
                                        ######.#.#
                                        .#...#.#.#
                                        .#########
                                        .###.#..#.
                                        ########.#
                                        ##...##.#.
                                        ..###.#.#.
                                        
                                        Tile 2971:
                                        ..#.#....#
                                        #...###...
                                        #.#.###...
                                        ##.##..#..
                                        .#####..##
                                        .#..####.#
                                        #..#.#..#.
                                        ..####.###
                                        ..#.#.###.
                                        ...#.#.#.#
                                        
                                        Tile 2729:
                                        ...#.#.#.#
                                        ####.#....
                                        ..#.#.....
                                        ....#..#.#
                                        .##..##.#.
                                        .#.####...
                                        ####.#.#..
                                        ##.####...
                                        ##..#.##..
                                        #.##...##.
                                        
                                        Tile 3079:
                                        #.#.#####.
                                        .#..######
                                        ..#.......
                                        ######....
                                        ####.#..#.
                                        .#...#.##.
                                        #.#####.##
                                        ..#.###...
                                        ..#.......
                                        ..#.###...";

        class Tile
        {
            public string[] Content;

            public void RotateCW()
            {
                var newContent = new char[this.Content[0].Length, Content.Length];

                for (int y = 0; y < this.Content.Length; y++)
                {
                    var line = this.Content[y];
                    for (int x = 0; x < line.Length; x++)
                    {
                        var c = this.Content[y][x];
                        var newX = Content.Length - 1 - y;
                        var newY = x;
                        newContent[newX, newY] = c;
                    }
                }

                var newContentJagged = new string[this.Content.Length];
                for (int y = 0; y < newContent.GetLength(1); y++)
                {
                    var sb = new StringBuilder();
                    for (int x = 0; x < newContent.GetLength(0); x++)
                    {
                        sb.Append(newContent[x, y]);
                    }

                    newContentJagged[y] = sb.ToString();
                }

                this.Content = newContentJagged;
            }

            public void FlipV()
            {
                this.Content = this.Content.Reverse().ToArray();
            }

            public void FlipH()
            {
                this.Content = this.Content.Select(row => new string(row.Reverse().ToArray())).ToArray();
            }

            // all edges are returned as if following in a clockwise order.
            public IEnumerable<(string side, V2 direction)> GetBasicSides()
            {
                yield return (this.Content[0], V2.Up);
                yield return (new string(this.Content[^1].Reverse().ToArray()), V2.Down);

                yield return (new string(this.Content.Select(line => line[0]).Reverse().ToArray()), V2.Left);
                yield return (new string(this.Content.Select(line => line[^1]).ToArray()), V2.Right);
            }

            // !flipped = clockwise; flipped = counterclockwise
            public IEnumerable<(string side, V2 direction, bool flipped)> GetPermutedSides()
            {
                foreach (var (side, direction) in this.GetBasicSides())
                {
                    yield return (side, direction, false);
                    yield return (new string(side.Reverse().ToArray()), direction, true);
                }
            }

            public override string ToString()
                => string.Join('\n', this.Content);
        }

        public static void Main()
        {
            var lines = input.SplitNewLine();
            var tiles = new Dictionary<int, Tile>();

            var tileIdPtn = new Regex(@"Tile (?<id>\d+):$", RegexOptions.Compiled);
            int currentId = 0;
            var currentLines = new List<string>();
            foreach (var line in lines)
            {
                var match = tileIdPtn.Match(line);
                if (match.Success)
                {
                    if (currentLines.Any())
                    {
                        tiles.Add(
                            currentId,
                            new Tile
                            {
                                Content = currentLines.ToArray()
                            }
                        );
                    }

                    currentId = int.Parse(match.Groups["id"].Value);
                    currentLines.Clear();
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(line)) currentLines.Add(line);
                }
            }

            tiles.Add(
                currentId,
                new Tile
                {
                    Content = currentLines.ToArray()
                }
            );

            var tileIdToPossiblePairs = new Dictionary<int, HashSet<int>>();

            // Filling tileIdToPossiblePairs with all neighbors for each tile ID.
            foreach (var (id1, tile1) in tiles)
            foreach (var (id2, tile2) in tiles)
            {
                if (id1 != id2)
                {
                    foreach (var (side1, _, _) in tile1.GetPermutedSides())
                    foreach (var (side2, _, _) in tile2.GetPermutedSides())
                    {
                        if (side1 == side2)
                        {
                            if (tileIdToPossiblePairs.TryGetValue(id1, out var possiblePars))
                            {
                                possiblePars.Add(id2);
                            }
                            else tileIdToPossiblePairs[id1] = new HashSet<int> {id2};

                            goto nextTile;
                        }
                    }
                }

                nextTile: ;
            }

            var corners = tileIdToPossiblePairs.Where(kvp => kvp.Value.Count == 2).ToArray();

            if (corners.Length == 4)
            {
                // part1
                // var result = corners.Aggregate(1L, (soFar, kvp) => soFar * kvp.Key);
                // Clipboard.Set(result);

                // Pick a random corner, which will be our (0, 0), and a random neighbor to that corner.

                var cornerIds = new HashSet<int>(corners.Select(kvp => kvp.Key));

                var (initialCornerId, initialCornerPairTiles) = corners[0];
                var position = new V2(0, 0);

                var tileToWalkToward = initialCornerPairTiles.First();
                var nextInitialCorner = initialCornerPairTiles.Last();

                // Find the common edge between the initial corner and its neighbor, and reorient those tiles so that
                // the initial corner is on the left.
                foreach (var s1 in tiles[initialCornerId].GetPermutedSides())
                foreach (var s2 in tiles[tileToWalkToward].GetPermutedSides())
                {
                    if (s1.side == s2.side)
                    {
                        var direction1 = s1.direction;
                        var direction2 = s2.direction;

                        while (direction1 != V2.Right)
                        {
                            direction1 = direction1.RotateCW();
                            tiles[initialCornerId].RotateCW();
                        }

                        while (direction2 != V2.Left)
                        {
                            direction2 = direction2.RotateCW();
                            tiles[tileToWalkToward].RotateCW();
                        }

                        if (s1.flipped)
                        {
                            tiles[initialCornerId].FlipV();
                        }

                        // Opposing edge should be flipped.
                        if (!s2.flipped)
                        {
                            tiles[tileToWalkToward].FlipV();
                        }

                        goto matchFound;
                    }
                }

                throw new Exception("no match");

                matchFound: ;

                // Check the other neighbor of our initial corner. If it is *above* the corner, then we need to
                // vertically flip everything so that our corner is in the top-left.
                foreach (var s1 in tiles[initialCornerId].GetBasicSides())
                foreach (var s2 in tiles[nextInitialCorner].GetPermutedSides())
                {
                    if (s1.side == s2.side)
                    {
                        if (s1.direction == V2.Down) break;
                        else if (s1.direction == V2.Up)
                        {
                            tiles[initialCornerId].FlipV();
                            tiles[tileToWalkToward].FlipV();
                            break;
                        }
                        else throw new Exception("Weird direction");
                    }
                }

                // Map from tile position to tile ID
                var tileMap = new Dictionary<V2, int>();
                tileMap[new V2(0, 0)] = initialCornerId;

                // We're going to walk from our corner in the direction of our chosen neighbor until we reach another corner.

                var currentTileId = tileToWalkToward;
                var currentPosition = new V2(1, 0);
                tileMap[currentPosition] = currentTileId;

                var cornerTileIds = corners.Select(kvp => kvp.Key).Except(new[] {initialCornerId}).ToHashSet();
                while (true)
                {
                    if (cornerTileIds.Contains(currentTileId))
                    {
                        break;
                    }
                    else
                    {
                        var nextEdge = tiles[currentTileId].GetBasicSides().Single(s => s.direction == V2.Right).side;
                        foreach (var tile in tileIdToPossiblePairs[currentTileId])
                        {
                            foreach (var side in tiles[tile].GetPermutedSides())
                            {
                                if (nextEdge == side.side)
                                {
                                    var nextTileDirection = side.direction;
                                    while (nextTileDirection != V2.Left)
                                    {
                                        nextTileDirection = nextTileDirection.RotateCW();
                                        tiles[tile].RotateCW();
                                    }

                                    if (!side.flipped)
                                    {
                                        tiles[tile].FlipV();
                                    }

                                    currentTileId = tile;
                                    currentPosition = new V2(currentPosition.X + 1, currentPosition.Y);
                                    tileMap[currentPosition] = currentTileId;
                                    goto moveToNextTile;
                                }
                            }
                        }
                    }

                    throw new Exception("match issue");

                    moveToNextTile: ;
                }

                // Now we have 1 full row with the correct orientation. Repeat process for remaining rows.
                var currentStartTile = initialCornerId;

                while (true)
                {
                    // move to leftmost tile in next row
                    currentPosition = new V2(0, currentPosition.Y + 1);
                    var edgeToNextStartTile =
                        tiles[currentStartTile].GetBasicSides().Single(s => s.direction == V2.Down);

                    foreach (var neighbor in tileIdToPossiblePairs[currentStartTile])
                    foreach (var neighborEdge in tiles[neighbor].GetPermutedSides())
                    {
                        if (neighborEdge.side == edgeToNextStartTile.side)
                        {
                            var neighborDirection = neighborEdge.direction;
                            while (neighborDirection != V2.Up)
                            {
                                neighborDirection = neighborDirection.RotateCW();
                                tiles[neighbor].RotateCW();
                            }

                            if (!neighborEdge.flipped)
                            {
                                tiles[neighbor].FlipH();
                            }

                            currentStartTile = neighbor;

                            goto nextStartFound;
                        }
                    }

                    break;

                    nextStartFound: ;
                    tileMap[currentPosition] = currentStartTile;

                    var currentRowTile = currentStartTile;

                    // fill out current row
                    while (true)
                    {
                        var edgeToNextRowTile = tiles[currentRowTile].GetBasicSides()
                            .Single(s => s.direction == V2.Right);

                        foreach (var neighbor in tileIdToPossiblePairs[currentRowTile])
                        foreach (var neighborEdge in tiles[neighbor].GetPermutedSides())
                        {
                            if (neighborEdge.side == edgeToNextRowTile.side)
                            {
                                var neighborDirection = neighborEdge.direction;
                                while (neighborDirection != V2.Left)
                                {
                                    neighborDirection = neighborDirection.RotateCW();
                                    tiles[neighbor].RotateCW();
                                }

                                if (!neighborEdge.flipped)
                                {
                                    tiles[neighbor].FlipV();
                                }

                                currentRowTile = neighbor;
                                currentPosition = new V2(currentPosition.X + 1, currentPosition.Y);
                                tileMap[currentPosition] = currentRowTile;
                                goto continueRow;
                            }
                        }

                        break;

                        continueRow: ;
                    }
                }

                // Copy tile contents into final map.
                var tileMaxX = (int) tileMap.Max(kvp => kvp.Key.X);
                var tileMaxY = (int) tileMap.Max(kvp => kvp.Key.Y);
                const int tileWidthWithBorders = 10;
                const int tileHeightWithBorders = 10;
                const int tileWidth = tileWidthWithBorders - 2;
                const int tileHeight = tileHeightWithBorders - 2;

                var finalMap = new char[(tileMaxX + 1) * tileWidth, (tileMaxY + 1) * tileHeight];

                for (int tileX = 0; tileX <= tileMaxX; tileX++)
                for (int tileY = 0; tileY <= tileMaxY; tileY++)
                {
                    var tilePos = new V2(tileX, tileY);

                    var tileId = tileMap[new V2(tileX, tileY)];
                    var tile = tiles[tileId];

                    for (int subTileY = 0; subTileY < tile.Content.Length - 2; subTileY++)
                    {
                        var line = tile.Content[subTileY + 1];
                        for (int subTileX = 0; subTileX < line.Length - 2; subTileX++)
                        {
                            char c = line[subTileX + 1];

                            var globalPos = tilePos * 8 + new V2(subTileX, subTileY);
                            finalMap[(int) globalPos.X, (int) globalPos.Y] = c;
                        }
                    }
                }

                var finalJagged = new string[finalMap.GetLength(1)];

                for (int y = 0; y < finalMap.GetLength(1); y++)
                {
                    var sb = new StringBuilder();
                    for (int x = 0; x < finalMap.GetLength(0); x++)
                    {
                        char c = finalMap[x, y];
                        sb.Append(c);
                    }

                    finalJagged[y] = sb.ToString();
                }

                // This lets us reuse rotation/flip operations
                var bigTile = new Tile {Content = finalJagged};

                var seaMonsterPtn = @"
                  # 
#    ##    ##    ###
 #  #  #  #  #  #   
".Trim('\n').Split('\n');

                var seaMonsterPositions = new List<V2>();
                for (int y = 0; y < seaMonsterPtn.Length; y++)
                {
                    var line = seaMonsterPtn[y];
                    for (int x = 0; x < line.Length; x++)
                    {
                        if (line[x] == '#')
                        {
                            seaMonsterPositions.Add(new V2(x, y));
                        }
                    }
                }

                // Storing the coords that correspond to monster parts on the big map
                var seaMonsterParts = new HashSet<V2>();

                var seaMonsterWidth = seaMonsterPositions.Max(v => v.X);
                var seaMonsterHeight = seaMonsterPositions.Max(v => v.Y);

                // Scanning the map for monsters in each orientation.
                for (int iFlipped = 0; iFlipped < 2; iFlipped++)
                {
                    for (int iDirection = 0; iDirection < 4; iDirection++)
                    {
                        Console.WriteLine(bigTile);
                        Console.WriteLine();

                        for (int y = 0; y < bigTile.Content.Length - seaMonsterHeight; y++)
                        for (int x = 0; x < bigTile.Content[0].Length - seaMonsterWidth; x++)
                        {
                            foreach (var monsterTestPos in seaMonsterPositions)
                            {
                                if (bigTile.Content[y + (int) monsterTestPos.Y][x + (int) monsterTestPos.X] != '#')
                                {
                                    goto tryNextPosition;
                                }
                            }

                            foreach (var monsterTestPos in seaMonsterPositions)
                            {
                                seaMonsterParts.Add(new V2(x + monsterTestPos.X, y + monsterTestPos.Y));
                            }

                            tryNextPosition: ;
                        }

                        bigTile.RotateCW();
                    }

                    bigTile.FlipV();
                }

                var totalHash = bigTile.Content.Sum(row => row.Count(c => c == '#'));
                var result = totalHash - seaMonsterParts.Count;

                Clipboard.Set(result);
            }
            else
            {
                throw new Exception("More than 4 corners.");
            }
        }
    }
}