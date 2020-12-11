using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day10
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
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        public static long[] ParseLongArray(this string[] input) => input.Select(str => long.Parse(str)).ToArray();
    }

    static class Clipboard
    {
        public static void Set(object value)
        {
            Console.WriteLine($"Setting Clipboard: {value}");
            Process.Start("bash", $"-c \"echo {value} | xclip -selection c\"").WaitForExit();
        }
    }
    
    class Program
    {
        private const string input = @"105
                               78
                               37
                               153
                               10
                               175
                               62
                               163
                               87
                               22
                               24
                               92
                               46
                               5
                               115
                               61
                               124
                               128
                               8
                               60
                               17
                               93
                               166
                               29
                               90
                               148
                               113
                               55
                               141
                               134
                               79
                               101
                               49
                               133
                               38
                               53
                               33
                               30
                               66
                               159
                               23
                               132
                               145
                               147
                               121
                               94
                               146
                               21
                               135
                               56
                               176
                               118
                               44
                               138
                               85
                               169
                               111
                               9
                               1
                               83
                               36
                               59
                               140
                               149
                               160
                               43
                               131
                               69
                               2
                               25
                               84
                               39
                               28
                               171
                               172
                               100
                               18
                               15
                               114
                               70
                               86
                               97
                               155
                               152
                               40
                               122
                               77
                               16
                               11
                               170
                               52
                               45
                               139
                               76
                               102
                               63
                               54
                               142
                               14
                               158
                               80
                               154
                               112
                               91
                               108
                               73
                               127
                               123";

        private const string sample = @"28
                                33
                                18
                                42
                                31
                                14
                                46
                                20
                                48
                                47
                                24
                                23
                                49
                                45
                                19
                                38
                                39
                                11
                                1
                                32
                                25
                                35
                                8
                                17
                                7
                                9
                                4
                                2
                                34
                                10
                                3";
        
        static void Main(string[] args)
        {
            var adapters = Parse.ParseLongArray(input.SplitNewLine());
            var joltsSorted = adapters.OrderBy(x => x).ToList();
            // joltsSorted.Insert(0, 0); // outlet (part1 only)
            var finalJolt = joltsSorted.Last() + 3;
            joltsSorted.Add(finalJolt);

            // part1
            // int n1Jumps = 0;
            // int n3Jumps = 0;
            //
            // for (int iAdapter = 0; iAdapter < joltsSorted.Count - 1; iAdapter++)
            // {
            //     var jump = joltsSorted[iAdapter + 1] - joltsSorted[iAdapter];
            //     if (jump == 1) n1Jumps++;
            //     else if (jump == 3) n3Jumps++;
            // }
            //
            // Clipboard.Set(n1Jumps * n3Jumps);

            var nWays = new long[finalJolt + 1];
            for (int iJolt = 0; iJolt < joltsSorted.Count; iJolt++)
            {
                var destJolt = joltsSorted[iJolt];

                if (destJolt <= 3) nWays[destJolt]++;
                
                for (int delta = -3; delta < 0; delta++)
                {
                    var srcJolt = destJolt + delta;

                    if (srcJolt >= 0)
                    {
                        nWays[destJolt] += nWays[srcJolt];
                    }
                }
            }
            
            Clipboard.Set(nWays[finalJolt]);

        }
    }
}