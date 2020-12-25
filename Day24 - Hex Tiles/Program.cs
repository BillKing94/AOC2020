using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day24
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
        private const string input = @"wwwnwsenwnwnwnwnwnewnewsewnwnwnww
                               nenwnwnewnweewsenwenwnwwswwnwswsw
                               seswswseneeeswswnwnwswseswnwswswswswsw
                               swenenwneswswswswsenwswswnwenenwseswsw
                               wneneenwseeeewneeseneeeneeenenene
                               wwwwenwwwwwnwnewwnwswwwwwse
                               neneneneneneeneswneeneenwneeneneenesw
                               seneseswseseswswseseswswseswsewsesesesw
                               enweneeeeeseeeesw
                               wwwsewswwwwswwenwneew
                               sesweenwseeneeeswneeseeeseswenwsese
                               seswswwswswswswseswswnwswswene
                               swwnwwwwwwswnwwwseswwwwwseww
                               wwnewwsewwwwswwneseweewwnw
                               wswswseneswswseswseneswseswseswswseswnesw
                               nenwnwneneseneeneswnewenweeswneeese
                               wwenwwwwnwwwnewsewwnwwwswwnw
                               newnwswneswswswneeeneenenee
                               swwsewswswswnwswswswnewewseswswswsw
                               eseneeenwseesweesw
                               wnenenenwnenenwnenwnwnenwnwsesenwnesene
                               eswneeeeeeeeneeeneenenewnee
                               enwenweeweewesesesweswswnweew
                               eseeseseseeswsewswww
                               wseeneewnwswswenweee
                               swwneswswwwwwswwswwwswwseswww
                               wesesesewsweseenwnesewneneswseswsw
                               nwswsesenwswnenenwenenwne
                               neeneseenenweneneneeeweeneeeswe
                               wwnwnwwwwsenwnwwnwwwnwwnwnwnw
                               eswnewswwnwnwwnwnwnwenwwsenewenww
                               wswsenwsewswseeneseseswswswneseswsw
                               sewswewwwnwwwnwwneewnwnwnwswnw
                               wsweswswwnwwsweswswswswnwswswswswswswe
                               swneseswswswswswsesenwsesw
                               neeeesewseenweeesweseeeeeee
                               nenenenenwnenenwneneneneeswswnwnenenwnwnw
                               nwwwewnwwnwwwwnwwnewnwwwwsenw
                               senwseeeesenwseeeseenwsweseseswnee
                               seseseeesenwseseeseeseseesesese
                               nwsesesesenwseseeseseseswewseswswswsw
                               wwwwnwwwwwnwnenwnwnwewnwwswww
                               neneneneswneswneneeeenwneneneeenene
                               seswnwseswsenenwnewseneswsenwsenwseenwswse
                               neeswwewwwnwneswnwwswswnw
                               eswwwswwnwwswneswwwsweswwweww
                               nwswwsewneeeneswneeeeeneeneneesw
                               eeswewwneseneneeneeeneseeeneneee
                               seseewseeseneweeeweesewsenewe
                               swseswwswneswswswsewsweswnwswseswwe
                               nwnwenwnewnwsenwsewwnwwnwnwwwnewnw
                               nenwsesesenwwnwsewenwwwwweswwwwe
                               nesenweseeeseeswsewsewswwseeeese
                               sewwswseseseneseswnwesenwseesenwenw
                               weseseseesesesesesenwseseseseesesese
                               swwnwnwnewnwnwwnwwwew
                               nwnwnwnwnenwswnwnwnwnwnwwnwnwenwsesesew
                               swsewwneswwnwswseswwneswswsewnewnew
                               wwwwnwwswwnwnwewwwwwwnwwnw
                               seseseseseeseseeseewnweseswnwesesese
                               nwwnenwnwwnenenwnwnwnwneneenenenwnenwse
                               nwwsenwnwnewnwwwnwwnwnwnwwnwnwwsenww
                               sesewsesenweseseseseseseswsesese
                               seewneswwwwnwwwseswwnwewnwnwnw
                               neneneneneneswneneneswswe
                               wwwwwwwwsenwwwwwwwwww
                               wnwwwnwwnwsewnwwwwwwswe
                               nwswnwnwewnwsenwneneneneswnwenene
                               senweweneseeweeeseneeeweeenw
                               wnwnwwwwenwwwswnwnwewswnwnewwne
                               eewnewswnenwswwswwsweeseswnwwnw
                               wsenenenwnenenwnwnenenenwnenenenenenenw
                               swseswwswwswnwswswseneseeesw
                               eeenwnweeeseeeeeeeeeswee
                               seseneewsesesenwseseeseswsesesesese
                               swswwswsweswswwswswsw
                               seseseseseweeeeeeseeseswneeenw
                               nenwnenwneseneswnenwnenwsene
                               neneneneneneneswenenenenene
                               nwnwnwswnwnwenwneneneneeswnwnwnwwnwne
                               wswnwneneenenwneswnwneswnenenwswsenwnwe
                               nwnwnwswnwnwnwsenwnwnewnwsenwnwnwenwswnw
                               seneneneneenenweenenenenenenenenenew
                               nwwswnwnwnwwnwnwnwnwnwwsenwnwsenwenw
                               seseseseswsenesesesesesesesesese
                               enwwwwenwswwswnewwswwsesww
                               wnwsenwnewenewswswswseewwwwswswswe
                               seswswnwswwwswswenwneesenwswswswnwesw
                               sesenwnwswsesesesesenewsesesenwsesesee
                               wseswnwswseseseeneeswsesesesesewsesene
                               swwseneswnwwwswnwwewwnewweewsww
                               neswseenwseneneenesenenenewnenewswnee
                               eeseeseweeeseeeeseesesesee
                               esesewseseseseneseswsenweseeeesese
                               wwnwnwwwwwwwnwnwnwsenewnwse
                               seenweseseseseeeseswnweeseseeenee
                               swswwswwwwwwwwwwnewwwwnenew
                               nenenenenenenwnesenenenewnenenenenenwne
                               enesweeeeenweeneeenwsww
                               eeneeeeeeneeeseeeeeeewswe
                               sweeeeeeeeeeeneeneee
                               wnwnwwwsenenwwwnwnwwnwnesesenwnewsw
                               neneeenweeseneeswneeweesenwneee
                               seneeeseseswnesewswnwneswswnwsewwnwnwsw
                               nwneeenwnenenenwnwnenenenwnwnewnewne
                               neswnwsweesewswswnwwneswswwswneswwsene
                               seeseseeeenwswseeenesesesenwsesewse
                               nwnwnwnwnenwnwnenwnwnwenwnenwenewwse
                               enwnwnwnwnwnwnwnwnwswnwnwswnwswnwenwnwnwne
                               nwwwneneswswneeewnwwnwswswseeseseee
                               nenwnwswnwnwsewneneenwnwwnw
                               senwweweswnwneeneseeeswnweseswse
                               wnwwnwnenwnenwsesenwnwwnwnwswnwswnwnwnenw
                               wwnwnwsewnwnwwnwesweswnewwwswne
                               nenenewneswneneswneeweswneenwnesenw
                               eswnwwwsweseswwswswwwnwswswnwswsenw
                               eeeswsenwseeweeeeeeseeeeee
                               nwwwnwsewwnwwnwwwwnewwsewwnwwne
                               seseseesesesesesesenwseseseseseseseswsw
                               nwnwnwswsenwnenenenenwnwnwnwnwnwwnwne
                               nwsesewesenwnwnwnwswsewenwswnwnenwwe
                               newneseneneseswnwnwswnwewswnwnwseesw
                               eeeeeeneweeeneeneswneeseene
                               nwnwwnwnwnwnwwwswnwnwnwnwwewnwwswe
                               swswswseswneseseseswswseseswswswnwsesesesw
                               eeseneneseweneneeeneeneenwneewne
                               eseeswseeeeseeneseeeenwseeeese
                               sesesenwsesesesewsesenesesesesesesesesesese
                               senwseseseseseseseseseseseseseseswsesenwsw
                               neeneneswnenenenenenenenweneneneneswne
                               swswswseswwswswswswnwswswswswwseswswnwsw
                               neeeeeeseneeneswwneeeeeneeee
                               enweesweneneswweneeewenewenee
                               sewwswswwseneswwwsenwwswwwnewwww
                               wewwwenwseswnwwwwwwewnwnwnw
                               nwwenwsewswwwsw
                               neneneeneneneseneneweseenenweenenene
                               nwswswneswswwnwesenwsweswswswsw
                               swenwnwnwnwnwnwnenwsenwnwwnenwnenw
                               swseseneseseseseneswwsewneseewswnwsene
                               wneeneneswnwneneewneneneeneenenee
                               eenwneeweeeeeeesweeeee
                               seneeswnenweweweeweneeswseee
                               swseseesewswswsenwswswswswswswsw
                               wswwswwwswwswwwwnewwwwsewnesw
                               nwnenwsewnenenenwnenwnwnwne
                               neenwneneneneneseneneneenewnenenwnenesw
                               enwnwwnwnwesenwswnenwnwnwenwnwnwswnw
                               swnenwswsweseeswswswseseswwnwswswseseswsw
                               nwswnesweswswsewswseswnwswswswswneseswesw
                               swneseneswswwsenwswswswsw
                               eeeeseeeeeeeweeeeeneee
                               wnwwnwswewwnwwwwwwwseww
                               neenenwswnenweneeeeseneseee
                               neneweneeeneneneswnenweweeseene
                               neeswnesenenwnwnenenwswnenenwswwnesenw
                               senenwnenenenenenwnene
                               sewnwneseeweswseeseenesesenweswwwne
                               swswnewseswswswswswwseseswswseneswswswsesw
                               nwnwswenwnwnwnwnwnwnwnwnwnwsewnwnwnwnenw
                               eseneswswnwswnweneeeeeeeeeeee
                               nenwnenwnwnenwnenwnwnwneswnwnenwe
                               senenenwsweswsweseenwnwwswnwswswswnwwe
                               nweneeneseneswwneswneneenewneeneene
                               seeseswnesenwnwenwnwnweneswwwnenwnwwnw
                               eseseeseeeseesesenesenweeseswsesese
                               swswsesenwswswswsenwswneswswseneneswsew
                               nenwnwnwseewnwwnwnwsewnew
                               swswseswswswneweswswswwswswswswswswswswse
                               wswsweswswswwsweswswwnwswswswswww
                               swnewwsenwwswwnewnwwsewwswnwnwne
                               wnwwwwnwwwsewnenwneenwswsewwnwww
                               nenweeeeeneeeesweee
                               sesesenwswswneswenwswwswwneswnesw
                               swswsewswwswswswenewswsww
                               seswseesesesesenesese
                               nweeseseneswwewseswseneseeeeeese
                               swnwswswseswwsweswwwswwwwewwnw
                               wnwnwnwnwneswnwnwnwnwwnwnwnenwnwsenwnwnwnw
                               nesenenwwneneswnenwnwsewneenwnwnwnwnw
                               eweeneneeseneeenweneeeneseenene
                               nwwnwnenwsewnesewwewwnewwnwese
                               eseeseseeeeseseenweseeseseeesw
                               wwnwwnwwewnwnwnwnwwnwwwnwsww
                               wnenweewsesewww
                               nwseseseseseseweeseeseseseseseesese
                               seseswwseswswsesweswseseswswswsesw
                               wswswsweswswwswswswwswswwsw
                               eeeneenweneeeeeeeewseseene
                               neneneenwseseswswenewsenenenwwsw
                               nwnwsenwnwsenwnwwnwnwnwnwnwnwnwnwnwnwnw
                               eeneseseneenenwewneeeeeneene
                               wwswswswwwwswswswwswwnwsewsw
                               senewseseewnwswseseseswseseswseswsesese
                               seswseswnwnwweseneseswsesesweseswsew
                               nenwnwseswseeweewnwnwneswnwswwnwsww
                               nwnwnenwnwnwnenenenenwnwnenesenwsenenenwnw
                               sewnwneswsewwswnwwseewnenenweswne
                               nwseswnwswsesenwnwnwwswnenewseswsewe
                               newnweseswsesenwwnwwnwnwnwnwnee
                               swwnweswwneeswnwswenwnwswswwswwse
                               sewwnwswenwwwwsewnwwseswneeswneswe
                               wwwwwwwnwwnesewwwwwnwnwww
                               seenwwswenwswnweeeeeeseeewsese
                               wswnwwwwwsewnewwswsewswwseenw
                               nenenenwwenwnwnenenenwnwnenesenwnwwse
                               nwnwnwnwnwnwwnwenwnwnwnwnwnwsenwnwnwnwnw
                               seseswseswnwseseseseseseseseswnwsesesesw
                               wnwwwwsenenwsenwwnenwwnwwwwenwnw
                               nwnewseseswsewnwnenwswnwseenesewnwnenw
                               neswsewwwwweneswwwweswwwnenw
                               nweswneneeneeeeneneneneswneneeneee
                               wswneneseeswswsewsenweseseswenwnwsw
                               sewswnwnwnwnenwnwnwnwswsewwwnwnenwwne
                               swswwswswswwswnenwswswsweswsenwweseswse
                               neseswwwnwnwnewnwseswnwwwenwnwnwe
                               swwswswswseswneswswwswswswwswnwswsesw
                               wnwnenwneswwnwwnwnwwsenwwnwwwwnw
                               wwnewsenwwnwwnwwwnwnwenwwnwww
                               swseswnweewwswnewswwenesenewswnw
                               eneeseenwsesweeneenweenwwesese
                               sesesewswwswswswswseeseseseseseswewe
                               nenwenwnwwnwnesenenwnwnenwswnwnenenwe
                               seenweeeseseseesenwseswseswsew
                               nenwnenesenwwnwnenenwnwnenwnwnenenenwnw
                               nwswseewswnwneswswneseseswsewseswnwse
                               nweneswwnwnwwwewsenwwwwwnesww
                               neeswswswwwwnwsewneswswswsewwwwww
                               wswseswneswesewneneesesenwseenwenw
                               wewwswewwnwwsewwwwwnwnwwwne
                               ewseneesesesesenwseesesweseseseee
                               nwenesweeeeeneneeneneneeneeee
                               nwswseswsesesewswseseswseseneseswse
                               wnwnewneneneswewneneneeneneenenenene
                               nwnwnwnwnwnwnwnwnwnwnwnwwnwenw
                               wwswswwnwwwwswswwweswwwwsww
                               seseseseewseseswnwneeeseseesenesese
                               neenwnwnesenwswnenweenwsewnwwswwnwse
                               swswseswnwswseswswswseseswseswnwsesweswswsw
                               eseeeseenenwenesweneweeenwwse
                               nwwwwwwwnewwwnwsewwwewwwsw
                               swswseswseeswswseswswswswsenenwwseswsw
                               nwneswseneneseeseseeswseswsenwseseswsese
                               nenwwwwwwwwseseweswwwwwww
                               wsesenwwseeseneseneneswwseseeseswsese
                               swswswneswswswwwwwwswswswswnwsewswse
                               eesewsesewneswseneeseenweeswese
                               swsenwsenwswneswswswseswseneseseswsesenwse
                               wwwswnwsweswwesewwwwwwswwwe
                               eswswnwwwwnwweenwwwwwwnwwnww
                               seweeeneeneeeeenwsweneneewswsee
                               nwswnwnwnwwnwnwnwsenwnwnenwnwnwnwwnwnw
                               sweswwseseswsesesw
                               nwwnwenwswwnwnewwswsewnwwwewwwne
                               nwnwnwnwwwnwnwnewnwnwnwnwnwwsenwnww
                               wswswswneswseswswnwweewswwswswwswsw
                               neneneeneswnenwnenenesenewneeneeene
                               neswseseseneseeewewneseseseseseswnw
                               sesweswnenenwneneeneneneneenenwnwsenene
                               nwwenenenwseewwseseswnwwwwwenewe
                               nwnenwnwnenenenenesewswnenwneenw
                               neeneneneneenewseneneneenene
                               wwwswwesenwswneswswswwwswneswswswsw
                               sewwwwwwwwwwwne
                               seseeeeeseseseseweneseseswsesewsenw
                               seeseeeseewseeneseseeesesesesee
                               wswwswwwwswweswsw
                               nwswnwnenwnwneswnenwnwnwnenwnwnwnenwnwne
                               wswnenwnwnenwnenwswnenwsenenenenenwnwenwne
                               enweeeeeeeeseeeswewweeee
                               seeeesweseeeeseweseneenesenwsese
                               nenwnwswnwnwwswwsewnwnwnwnewnwwwnw
                               sesesweneweseswseseseswnwsesesesesesese
                               senewwwswswwnewwnewsewnwwenwsenww
                               wwwsenwnwwnwnwnwnwwnwwnwwnwsewnw
                               swswswswswseswswswswnewnwwnwswwwswe
                               seeeneeseesenweseeeewseeseswnwsw
                               wwswnewewsewnwweswwwwswswww
                               nwnwwnwnenwnwswnwenwnwnwnwnenwnwnwnwnwnw
                               neneeneneeneeenweewneneswne
                               nesenewenenenenwnwnenenesenewsenesewe
                               seneeseeswesesesesenweeewese
                               sesesesesesesesenwesesesesw
                               wnwwnwwsewwwsenenw
                               neneeneeeneneeeeenwesweeeee
                               eeeseneneneeneeneneeneewewee
                               seseseseseseswsesenwsesesesesesese
                               wsweswswnewswwsweswwswwswswwnwswne
                               wswseswwwnwswswesewswneswnwwwnee
                               eseseseswneseseeseseseswseneswnwsewsesww
                               nwnwsenwnenwnwnenenwswwnwswwnwneeee
                               neneeeneneewwneeneneneeswseneneeee
                               nenwewnwwwnwnwsenwse
                               nwnwnwnwnwnwnwnwswwnesenwnenw
                               nwweseseseseseeeseseseeeenesesese
                               nenwnenwnwnewswnenenenwnesenenenenenesenw
                               seneseseseseseseeseseseseswew
                               neseeneenenenenenenenenewneenenwene
                               nwnwnwnenenenwnwnenesenenenenwnenewnwne
                               swseswswseswnewswwswswseneswneswenwsw
                               eeneeseenwesweeneweeseenwene
                               wswseseseseneseseseseswwseneswesesenenw
                               sesesesesesewseseseseseseseeseneswsesew
                               nweswnwnwesesweseswswnwnweseenwsene
                               seneseeseeeeseweseeeseseesesese
                               wneeneeneweneseneneneesenenenenene
                               seseesesesewwseseseseeesesenwsesese
                               neneweeseneewnwseneneeswneneeeneene
                               neswenwswseseswswneswnwenenwseswsenwnw
                               swswnwseseseseswswsweswnwsenwswnwsesesese
                               wnesenwwwweswwwwwwwswwwww
                               eswnwwswweweeeswnweswnenweseww
                               wwnwwweseswnwwnweseeneswswe
                               wnwnewwnewwwwwwnwsewwwwwsww
                               neswnenenwneneneneneesenewneneeeenene
                               wneewnewsenwsenwnewswesenwswwnewsw
                               nwsesesewwnenwnwe
                               nwnenwnwnwwnewenwsenenwneesenwnwnewse
                               seneseswseseseswseswsewnese
                               swseswswswseeswswswswnwsesesesw
                               newnwseeenwnwwswnenenwnwnenwnw
                               swwwnwswswswsewwswewwswwswwwnesw
                               nwnenwnwnwseseneenwnwnenenwnwnwnwsewsw
                               seeseseseseseseeswnwsesesesesesesenw
                               sewenewnwneneewnwnwnwnwnenenwnwnenwnw
                               neneneesenenwnwwneneneneneneenenwneswnw
                               wwwwswswswswewwenwnwwswwwswsew
                               senwwwwwwwnwwwwwwwwwweww
                               sesesesesesesesesenwsesesese
                               nwwnwnwnwnwnwnwnwwnwnwwnwsenwnwnww
                               nenwnwnwnwsenwnwnesewnenwnenenwnwnwnwnenw
                               nwneneeeeeeeeeeneeeeseswee
                               swseswseswswswswswswswseswnwsweswswswsw
                               swswswnwswswswswswseswswswswneswswswswse
                               seseewsesewwnwnesenenenenenenewnenesew
                               neswseswnwseswswswesesweneswnwseswnwsew
                               senesewnwswwseseseseseeeneeseeese
                               nwsweenwenwnwnwwnwnwwnwnwnwnwwnwswsw
                               seseswswswnenwseswneeswswswsweseseswnw
                               seseswseswswseseneseewseswswseswswsewse
                               swswswwsesewswswswswwswnwwwwneswswnesw
                               eeeeeeenweseeesw
                               eeeneeeneeweseeeee
                               nwnenewneneneenwsw
                               neeneswenwweeswewwseenenw
                               nwswnwewwnwnwnwwsenwnwnwnwwnwnwnwsenww
                               enewenwseneneswnwnesewsewnenenwneseswne
                               seneswswseseswswesenwsesw
                               nwnwnwwwnwnwwnwnewnwsenwwnwnwnwewnw
                               nwnwnwswswwnwsenwnenwenwnwsenwnwnenwnw
                               nwseneswnenwwnwnenenenwnenwnenene
                               wswswswswwwwswwswsenwwswswswsw
                               wwwnwnwwenwwsenwwwwewwwnwww
                               eseeneswseenwwseeeeswsesenwswwe
                               neseeswnenwwnewsesesewsweneseeswsese
                               seswswwseswswswsenwseseseseseeseswseswnw
                               swnwswwnweweewwwswwwwswesww
                               nenwsenwenwnwnwnwnwnwnwnwwnwnwnwnwnwnw
                               swswseswneswswswsenwseswswswnwneseewswsw
                               neneweenenenesenenene
                               swneswswswswswswswswswsw
                               nenenenenenenwnenenesewnenwnwnenenesenene
                               eneeseeeeeeeeeeseeewesee
                               seeseeseseeneseseeeseseseswese
                               seseswenwswnwseswswswswseseeswswseseseswse
                               nwwnwwnwnwwnwnwnwseewnwnwnwnwnw
                               seneswswswwswswseneswswseswswswseseswsese
                               nenenwneneneenwnwwnenwnwseneswnenenwnw
                               seseewsenwneeneswenwnwneswenenenese
                               nwnwsenenenwnwnwnwswnwnenwnwnenenwnenwne
                               nwnwnwwenwswnwnwnwnenwnwnwwnwwnwwwsww
                               swswnwseseseneswneneesewseenwnwsesew
                               neneenwnewneswnenwnenenenwne
                               wswswneseseswseswsesewswswseseseseseese
                               nwnwnwnenwnwnwwnwsewswnww
                               wwewnwwwnwwwwnwwwnwnwnwwwsw
                               swsesewsewnenesenesesesesesenweswsese
                               newnenwswnwneneweswsenenwenenwnenwese
                               swswswswswwswwswswswsewswswnwswswswwe
                               ewswneswnwwswwwwwsewwwwwswwww
                               wwwwwwswwnwnwwwwweewwwww
                               wswseneswnwneswnwnenenenwwsenwnenwnese
                               nwnwswneswnwnenwwnenwnwenwnwnwnwwnwese
                               neenenenenewneneneneneenene
                               weewneswseneneseneswnenww
                               sesesesesesweseseeeseesewseewwenw
                               nwnwnwnwnenwnwnwwwnwsenwnwwnweswnwnw
                               nwnwnenwwwsenwnwnwnwnwnwnenwnenwnwnwnesene
                               swseseseseswseseseseseswsesenwnwsesesesese
                               wwwnwwnwnwnwewwnwwwewswnwww
                               seswseenwseseeeweee
                               swswswnenwseswnwseeseswneseseseneesesww
                               neeneenwswesweneneswswneeneeswene
                               nwswswweesewwwnenwwewwwneww
                               neeeeeewseseseeeseseee
                               sesenesesesweseseeswseswseswwseswnewnww
                               newenwsewseweseseseseseswseseenesesee
                               seseeseewenwwseeseseseewneesesewse
                               nenenenesenwneeneswneneneswneeeneneee
                               sesesesewseseswseseseseneswseseseswsese
                               newnwnenenwnweneneeswnenwnwsenwnwnenene
                               swswswseswswseswswswneseswnwswseseswesw
                               nwneswswnwnwenwnwnwnwnwnwne
                               wnewswwsewwenwwwwnwwnwwwnew
                               swsenweseseeeenwseseeeseseneesewse
                               wwenwwnwnwwnwseewnwwewsewnw
                               nenenwnwneseswnenwnenwnenwnenenwnenenwnene
                               seneswswswswnwswswswswswswseswswswswswse
                               nwweswwwswseswwwwnewwwwwswwswsw
                               wwneswnwnwnwenwwsewwnwnenwnwwnwsenesw
                               sesewnwnwnwswsenewwnenwnwnenwnwnwnwnwnenw
                               swnwswswswwswswswswneswneseswswswswswsw
                               wswwneswswnewswswsesesesesesenweswsw
                               neseseswseseneswwswsesewseseseseswswsese
                               swewneeswswswswsenw
                               esenwnenwswsewswswseswseswnwseswswswswse
                               swewswneeeseseseeeeeweeenenwe
                               nwwnenenwwneseneseneneewseneneneeswne
                               nenewnenwenwneweswnwne
                               eeeeeeeeeseseewweseesenesene
                               swnwswnwenwewwwswnwswnwwnenenesesw
                               seenesweswsesweeeenewneneenewesw
                               seeseeweseseseseseseseseseseseewsese
                               swswswneseneswswswwweswseswseswswswsw
                               sweeeeeeeeeneeneneeswene
                               eswwsenwswswswseswswswswswswwswnwneswsw
                               sesesesweseeseneeseseseseseseseseswene
                               enweseseeeseeseseseneeesweeese
                               wwswwwwwswwewwnenewwwswwww
                               seewnewenwneseeeweeesweeeseene
                               seneseneneneenenewneenenenenewnenene
                               wnweseswswnwnweswnwenwswseeswswswswnw
                               nwneneneeneneswneneneneneneneneneneswnenene
                               nwnwnenwnwnwsenwnwswnenwnwnwnwenwnwnese
                               nenenwneneneneneeneeneeneneneeneswne
                               nenwneeseenenewsesweneenenenwneeene
                               neneneeneeseewneeneewwe
                               wenwsesweseeeenwesewenweesesese
                               nwnwwnenwnwnwsenwsenwenenwnenwnwnwnwnwnw
                               swesesewseswswseswswswseneneseseswnwsesw
                               nwnenwnwnwnwswnwnwnwsenwnenwnwswnwwnwne
                               eswwswseswsewenwnewwnewseswneswwnwsw
                               swseswsesesenwseseseseswsese
                               esweswnwewsenwsewse
                               wsesewwwwwewnenewnwwswwwneww
                               swseseswwswswseseswswneseswswsesewswneswsw
                               swswswswseswseswswswswnwnwswswswseswnesw
                               nenenesenenenenenenenenenenenenwneneneswne
                               eseswesewnwseeseneesweewseseenwese
                               swwswswswnwseswswswswwswsweswswswswswsw
                               neseeenwnweswenwsewseseneswwswnwsww
                               swswwswneneswswswswnewswseswswsenw
                               senwnenenweneneeneswneeneneeseeswnew
                               nenwnwnenwnwnenwswsenwnenenenenwnwwnwe
                               nwwnwnwwnwnwwnwnwnwnwnwnwenwwnwnw
                               swswswseswswswswswnwwseswenwswseswsweswe
                               wnwwwenwsewswwwwswnewwwnwwww
                               nwwwnwnewsewwwwnwnwwwnwnw
                               nwswweswewswswnesw
                               sewsweseswswswswnwsweswswswswswswswse
                               ewneneseeseneeeeeneeneenwnwnee
                               seswswswwseswswseswswswswswneswsese
                               eeeneeseeeeeseseswesese
                               neswsenwwenwnwsenwswnenenenenw
                               eswewswwneenenenenwsenwsewnwenewnw
                               wwswwswwenwswwewswswwwwwwsw
                               eenenenwwnesweeenenewseswenesene
                               seswneneneseswwenwnweneneneenenenene
                               wenwwnwswwwnwwwnwnenwnwwnwswwe
                               nenenenenesenwneweneenenenenenenesene
                               wwwwwwwwswwswwwnwwswswesww
                               wwsesenwnwnwnwwnwnwnwnwnwnwnwnwsewnw
                               senewnwenenenenenewneneneneneneneswneene
                               sesewnwneneneneneneneneewneeneswnw
                               swwswswswswswswnesw
                               swweneswnwwnwwneswseswswesewwswse
                               neneesenewnewneneenenewnenenenenene
                               nwnwswneswseswnweee
                               wenewwwseseswnewwwwnwwswswswwnw
                               swsewnenwenwsenenwse
                               nenenenenwneneneenenwnesenenenenenesenene
                               newwwwnwsewwwwwswswwnesewsww
                               wnwenwwnwwswnwwnwwwwswnwnwenwnw
                               weswswwwewsweswswwswwwwswww
                               seneeeseenwsesenwswsweesenwseeeewe
                               swnwenenenenenenwsenenwnwnenesenenenwnwne
                               nwnwnweswenwnwnwnwnwnwwnwswwne
                               swswwswswswseswswswswsweswnwswswswesw
                               nwnenwnwnwnwneswnwnenenenenenewsenenwnenw
                               nwsesenenenenenenenenenenenenenenenenwsenew
                               eseeeeeeeeeeeeweene
                               senwnwnwwenweswnenwnwneewnwnwwww
                               swnwsenwswneneneenweswseweneneneee
                               weeeewseseeeeseeeseseseseee
                               wnwenwnwenwwneswnwwnwneneeswnwenw
                               nwsenwnwnwwwnewwnwseneseswnwnwwnenwnw
                               nwnenenwsenwnenenenwnenenenewnwnwneew
                               senwnenwnenwnwnwnwnenwnenenwwsenenwnwnw
                               enwwwnwnenenewneneswenenesewenene
                               nenweneneseeenenwenwnenewneswseneswe
                               wneseneneneenwneneneneneswnenenenenenenene
                               neneneeseeneeneweneeneneenene
                               eenesweeneneneneneneneneeenene
                               esweseeneeeeneeneneeeeewnwse
                               sesesweswseseseswseswnwseswseseseswsese
                               nwnenwnwwsenwnwnwnwnw
                               nwenenweneneswsesweeneneewneeene
                               ewneseseeswseeneeeseew
                               swswwswswnwswswswswwswseswswswnewswewsw
                               sesenweseeseeeneneeseweswsesesesw
                               nwsenwseeswswsewwsenenwwnwenenwswwene
                               swswnwwseeneswwnwnew
                               swswswseswseswswnwseswswswseeenwswswsese
                               swnwneswnwwsweswseswwwnwswwswswewse
                               nwnwnwnenwewnwnwnwnwnwneneswnwnwnw
                               seseseswsesesweswseswswseneswwseswswsese
                               swwweswwneswswwswwwseesenwnewse
                               wswwnwwsewnewswwwneswnewweswswswne
                               eseenewneneswsesenwneneeneneswswnenewsw
                               seswesenwnwnwneswswnewwnenwneseneenew
                               wsweswswswswswswneswswwwswneswswwswwse
                               swneneneenewneneneenwwsewneeeneswne
                               nwneswnenwwswnenwsenwnwnwneenwseenenenwsw
                               seseswseswseseseneswswsweswswswsewsesw
                               ewneneseeneeneneswnw
                               eeseswnwseseeeeeseseswnweseeseee
                               wwewseswwwswswwwnenewswswwwswsw
                               nwewwenwswnenwseswwnwwwnewnwnwswe
                               seswseswsesesweswseswsesenwsesesesesese
                               swswswwwswwswewswswnewwewswwwsw
                               swnesesenwewwseseeswnewnenwwenese
                               swnwwseswenwsenwneseseseseseseenwsesw
                               nesenewsenwnweswwwwnwneswnwwsenenwne
                               ewnwnwnwnesenwnenwnwwswnwsenenwenwnw
                               esesewneseseswsenesenwwseswsesesenenw
                               seneswwwnwenwseswnwwwwwswswwene
                               eswwnewnenenwnwnwnenwnwnenwsenwneene
                               wneseseesenwnwseneneneswswwwwnwswenese
                               nesenwnwwnesenesenenwneswnwneswnwnwnenene
                               senesesesewseseseseseseseseseseesesese
                               neweeeeneenenwswenesweeeswneee
                               swneneenenenewnenenenwwnenwsweenenene
                               nwwenwwwseswswwneswwsenewenwnwnw
                               nenenenenwsewneseneswneswnenenwne
                               swswswwswsweswswswswswswsw
                               nenenwsenwnwnenwnwnenenenwnenenenwneswnw
                               neneswnwnwnwnwnenwnenwenwnwnwnesenwnenw
                               nwnwswswswewswswswswswswswseswswswswsw
                               weeeeeeenee
                               swnwswswsenwseneswseeseswswneseseswswnwse
                               swswwseseswswneswseswswneswseseneswswsw
                               weesweneseweeenesewesweneesee
                               seswswswseseseseseseseneseswsesenwesese
                               eweneseeeseneewnwneeeneneenese
                               swwnenwswwnwnwnwnwswwswenweenwnww
                               nwwwwwswwswwwwwswswswwswswwe
                               eneenwneseseeneneeenwseneswneenew
                               sesesewneswnwnenewneseeneswnwnenweew
                               eswswswsenwnwnwnewswnenwseswnenwnenee
                               enwwwesweeeeeeeeseeseeeeee
                               eseeeeeeeseeeneseswwseeeeee
                               nwnenenenenenwnenenenenwneswsenwse
                               weseseseeseseseseseseseeeenesewse
                               neeneeeweeesweeswenweeswnwenw
                               nwwwneswswwswswewewwseswswswww
                               nenewnenenwnwnwnwnwnenwnwnenenesenwnwnw
                               esesesenesewseseneseseswsese
                               sesweswneneneneswwnewnwnenweenenenwsw
                               nwneneswnwswswwnewneneneseseenwsewnese
                               wneseewnwnwenwnesenwwseseswwnewnwe
                               senwwsweeesenwse
                               wnewwnewsewwnwseswwwswnewwwsw
                               nwnwwnwnenwnwnwnenwnwwnwsenenwsenwsenw
                               enwwwwnwwnwnwwwnww
                               wwnwnwswewneenw
                               wnewewswwwwnwswwswswseswswwnesw
                               wneenweweneeweseeeeseeeseee
                               eeswswweeeneseeeneenwswseseenw
                               nenenenwnwnwnwnwnwswnwnewnwnenwnwnwenwne
                               nwswswsenwseswnesewnwswswswswseswneswsw
                               seeweeeeeseseseseeneseseseseswse
                               sesewneseswneseeseswseesewwswsesesw
                               nweneswsesenewweseswwneseneeseswswe
                               senwseseswsesesesenwesenwsesesewesesesw
                               eneneneeenwnwnwnwseseweneswse
                               neswneswnenenewswnesenwnenenenenwnwnesw
                               neswswseswswseswswseweswswseswswswswsesww
                               neeeeeweneesw
                               eneneeeeneswnene
                               swswswswneswswswswsewswswswnwseswneswswsw
                               wnwesweneneseseneweeesewswwwnwe
                               wnwewwwnwnwwnwwenwsewswnwswww
                               neeswnenwnenwsenwnenewweswsenenwnwswnw
                               nwswnwnwwnwnwnenwnwenw
                               swneswswswswwswswswswswswswwswswsw
                               swnwwnenwneswneenenenenenenwseeeesesw
                               wnwnwnwnwnwnwnwnenwnwnwnwnwswwswnwnenwnw
                               seswswnenwseswnwsesweseseseseswseneesww
                               seewnwnwswneweswwwenewwewsenwse
                               swswseswswswswnwswweswseswseneseswswsw
                               neneneeeneeseeswsweeeewneeeenw
                               swseswseswswswneneseswneswnwne
                               neeeneeneneneneneswnwenene
                               swsewewnwnwnenewnewnwwseewseesese
                               nwnwnwewnenwenwewnwnwnwnwnwswnwnwnwnw
                               nwwweeeeseswseseseseesenweseewe
                               sesesewswsweseseswseswswseseswswneenwnw
                               nenenenenenenwneneneseswneneneswne";

        private const string sample = @"sesenwnenenewseeswwswswwnenewsewsw
                                        neeenesenwnwwswnenewnwwsewnenwseswesw
                                        seswneswswsenwwnwse
                                        nwnwneseeswswnenewneswwnewseswneseene
                                        swweswneswnenwsewnwneneseenw
                                        eesenwseswswnenwswnwnwsewwnwsene
                                        sewnenenenesenwsewnenwwwse
                                        wenwwweseeeweswwwnwwe
                                        wsweesenenewnwwnwsenewsenwwsesesenwne
                                        neeswseenwwswnwswswnw
                                        nenwswwsewswnenenewsenwsenwnesesenew
                                        enewnwewneswsewnwswenweswnenwsenwsw
                                        sweneswneswneneenwnewenewwneswswnese
                                        swwesenesewenwneswnwwneseswwne
                                        enesenwswwswneneswsenwnewswseenwsese
                                        wnwnesenesenenwwnenwsewesewsesesew
                                        nenewswnwewswnenesenwnesewesw
                                        eneswnwswnwsenenwnwnwwseeswneewsenese
                                        neswnwewnwnwseenwseesewsenwsweewe
                                        wseweeenwnesenwwwswnew";

        enum Direction
        {
            e,
            se,
            sw,
            w,
            nw,
            ne
        }

        static void Main(string[] args)
        {
            // Using cube-based coordinate system from here: https://www.redblobgames.com/grids/hexagons/
            
            V3 Move(V3 v, Direction direction)
                => direction switch
                {
                    Direction.e => new V3(v.X + 1, v.Y, v.Z - 1),
                    Direction.w => new V3(v.X - 1, v.Y, v.Z + 1),
                    Direction.ne => new V3(v.X + 1, v.Y - 1, v.Z),
                    Direction.nw => new V3(v.X, v.Y - 1, v.Z + 1),
                    Direction.se => new V3(v.X, v.Y + 1, v.Z - 1),
                    Direction.sw => new V3(v.X-1, v.Y + 1, v.Z)
                };

            HashSet<V3> blackTiles = new HashSet<V3>();

            void flip(V3 tile)
            {
                if (!blackTiles.Add(tile)) blackTiles.Remove(tile);
            }

            var lines = input.SplitNewLine();
            foreach (var line in lines)
            {
                V3 position = new V3(0, 0, 0);
                
                string left = line;
                while (left.Length > 0)
                {
                    Direction instr;
                    if (Enum.TryParse<Direction>(left.Substring(0, 1), out var parsed))
                    {
                        instr = parsed;
                        left = left.Substring(1);
                    }
                    else if (Enum.TryParse<Direction>(left.Substring(0, 2), out var parsed2))
                    {
                        instr = parsed2;
                        left = left.Substring(2);
                    }
                    else throw new Exception(":(");

                    position = Move(position, instr);
                }
                
                flip(position);
            }

            // part 2 only
            for (int nDays = 0; nDays < 100; nDays++)
            {
                var nBlackNeighboringTiles = new Dictionary<V3, int>();
                foreach (var blackTile in blackTiles)
                {
                    foreach (var direction in Enum.GetValues<Direction>())
                    {
                        var neighbor = Move(blackTile, direction);

                        if (nBlackNeighboringTiles.TryGetValue(neighbor, out var nNeighbors))
                        {
                            nBlackNeighboringTiles[neighbor] = nNeighbors + 1;
                        }
                        else
                        {
                            nBlackNeighboringTiles.Add(neighbor, 1);
                        }
                    }
                }

                var newBlackTiles = new HashSet<V3>();

                foreach (var blackTile in blackTiles)
                {
                    var nNeighbors = nBlackNeighboringTiles.GetValueOrDefault(blackTile, 0);
                    if (!(nNeighbors == 0 || nNeighbors > 2))
                    {
                        newBlackTiles.Add(blackTile);
                    }
                }

                foreach (var (tile, nNeighbors) in nBlackNeighboringTiles.Where((kvp) => !blackTiles.Contains(kvp.Key)))
                {
                    if (nNeighbors == 2) newBlackTiles.Add(tile);
                }

                blackTiles = newBlackTiles;
            }
            
            var result = blackTiles.Count;
            Clipboard.Set(result);


        }
    }
}