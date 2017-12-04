//==========================================================
// A01020200
//==========================================================

using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Exam1 {
    public class Problem1 {
        public static void delete_comments(string s){
            Regex fortran = new Regex(
                @"
                (?<notCom> ^[^c|C|\*] .*)
                ",
                RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Multiline
            );
        
            foreach (Match match in fortran.Matches(s)) {
                Console.WriteLine($"{match.Captures[0]}");
            }
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
                delete_comments(lines);
            } catch (FileNotFoundException e) {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}