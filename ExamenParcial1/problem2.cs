//==========================================================
// A01020200
//==========================================================

using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Exam1 {
    public class Problem2 {
        static string hex2bin(Match m){
            string hex = m.ToString();
            string bin = "#";
            hex = hex.Trim(new Char[] {'#', 'x'});
            hex = hex.ToUpper();

            int tmp = Convert.ToInt32(hex,16);

            bin = bin + tmp.ToString();
            return bin;
        }

        public static void replace_hex(string s){
            Regex hexa = new Regex(
                @"
                (?<Hex> [#][x][\S][^;]*)
                ",
                RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Multiline
            );
        
            string bin = hexa.Replace(s, new MatchEvaluator(Problem2.hex2bin));
            Console.WriteLine($"{bin}");
        }

        public static void Main(String[] args) {
            // Your code goes here.
            if(args.Length != 1){
                Console.Error.WriteLine("Error!, Please specify the input file.");
                Environment.Exit(1);
            } 

            try{
                var file = args[0];
                /* Read the content of the file */
                var lines = File.ReadAllText(file);
                replace_hex(lines);
            } catch (FileNotFoundException e) {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}