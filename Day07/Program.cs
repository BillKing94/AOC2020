using System;
using System.Text.RegularExpressions;

namespace Day07
{
    class Program
    {
        static string[] SplitDoubleNewline(string input) => Regex.Split(input.Trim(), "\\n\\s*\\n");

        static string[] SplitNewLine(string input) => input.Split('\n',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        private static string input = @"

";
        
        static void Main(string[] args)
        {
        }
    }
}