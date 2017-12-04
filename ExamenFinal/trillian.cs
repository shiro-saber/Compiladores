/*
Juan Carlos León Jarquín
A01020200
Legendary Compiler Design Exam
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Trillian {
    public enum TokenCategory {
        FLOTANTE,
        MAXIMUM,
        DUP,
        COMMA,
        BRACKET_OPEN,
        BRACKET_CLOSE,
        ILEGAL,
        EOF
    }

    class SyntaxError : Exception { }

    public class Token {
        TokenCategory category;
        readonly string value;

        public string Value {
            get {
                return value;
            }
        }

        public TokenCategory Category {
            get {
                return category;
            }
        }

        public Token (TokenCategory category, string value) {
            this.category = category;
            this.value = value;
        }

        public override string ToString () {
            return this.category + " : " + this.value;
        }
    }

    // Syntax and Lex Process
    class Scanner {
        readonly string input;
        // El float puede ser positivo y negativo
        // al menos un numero al principio
        // Si hay un punto de menos debe haber un numero despues de el
        // el ? de [-] es para saber si hay un menos
        // el ? de ([.][0-9]+) es para saber si es float o int
        // Los \ son para escapar los caracteres especiales de la regex
        // Los espacios se ignoran
        static readonly Regex regex = new Regex (
            @"
                (?<Flotante>             [-]?[0-9]+([.][0-9]+)?  )
                |(?<Max>                 [!]                     )
                |(?<Dup>                 [*]                     )
                |(?<Comma>               [,]                     )
                |(?<Bracket_open>        [\[]                    )
                |(?<Bracket_close>       [\]]                    )
                |(?<Spaces>              \s                      )
                |(?<Other>               .                       )
            ",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled
        );

        static readonly IDictionary<string, TokenCategory> regexLabels =
            new Dictionary<string, TokenCategory> () { 
                { "Flotante", TokenCategory.FLOTANTE }, 
                { "Max", TokenCategory.MAXIMUM }, 
                { "Dup", TokenCategory.DUP }, 
                { "Comma", TokenCategory.COMMA }, 
                { "Bracket_open", TokenCategory.BRACKET_OPEN }, 
                { "Bracket_close", TokenCategory.BRACKET_CLOSE }
            };

        public Scanner (string input) {
            this.input = input;
        }

        public IEnumerable<Token> Start () {
            foreach (Match m in regex.Matches (input)) {
                if (m.Groups["Spaces"].Success) {
                    // Ignora los espacios
                } else if (m.Groups["Other"].Success) {
                    yield return new Token (TokenCategory.ILEGAL, m.Value);
                } else {
                    foreach (var name in regexLabels.Keys) {
                        if (m.Groups[name].Success) {
                            yield return new Token (regexLabels[name], m.Value);
                            break;
                        }
                    }
                }
            }
            yield return new Token (TokenCategory.EOF, "");
        }
    }

    // AST Tree
    class Node : IEnumerable<Node> {
        IList<Node> children = new List<Node> ();

        public Node this [int index] {
            get {
                return children[index];
            }
        }

        public Token GetToken {
            get;
            set;
        }

        public int GetCount {
            get {
                return children.Count;
            }
        }

        public void Add (Node node) {
            children.Add (node);
        }

        public IEnumerator<Node> GetEnumerator () {
            return children.GetEnumerator ();
        }

        System.Collections.IEnumerator
        System.Collections.IEnumerable.GetEnumerator () {
            throw new NotImplementedException ();
        }

        public override string ToString () {
            return GetType ().Name;
        }

        public string ToStringTree () {
            var builder = new StringBuilder ();
            TreeTrasversal (this, "", builder);
            return builder.ToString ();
        }

        static void TreeTrasversal (Node node, string indent, StringBuilder builder) {
            builder.Append (indent);
            builder.Append (node);
            builder.Append ("\n");

            foreach (var child in node.children) {
                TreeTrasversal (child, indent + "  ", builder);
            }
        }
    }

    /*  
        FLOAT,
        MAX,
        DUP,
        SUM (Not regex but still operation)
    */
    class Programa : Node { }
    class Flotante : Node {
        public override string ToString () {
            // Override the print of the value in the node class
            return this.GetToken.Value;
        }
    }
    class Max : Node { }
    class Dup : Node { }
    class Sum : Node { }
    // Chamba pesada
    class Parser {
        IEnumerator<Token> tokenStream;

        public Parser (IEnumerator<Token> tokenStream) {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext ();
        }

        public TokenCategory CurrentToken {
            get {
                return tokenStream.Current.Category;
            }
        }

        public Token Expect (TokenCategory category) {
            if (CurrentToken == category) {
                // read
                Token current = tokenStream.Current;
                // move
                tokenStream.MoveNext ();
                // return readed
                return current;
            } else {
                Console.WriteLine ("Expect");
                throw new SyntaxError ();
            }
        }

        public Node Inicio () {
            // check all program
            Node p = new Programa ();
            p.Add (ExpMax ());
            // Then finish the program
            Expect (TokenCategory.EOF);
            // return the program path
            return p;
        }

        public Node ExpSum () {
            Node n = new Sum () {
                GetToken = Expect (TokenCategory.BRACKET_OPEN)
            };
            n.Add (ExpMax ());

            while (CurrentToken == TokenCategory.COMMA) {
                Expect (TokenCategory.COMMA);
                n.Add (ExpMax ());
            }
            Expect (TokenCategory.BRACKET_CLOSE);
            return n;
        }

        public Node ExpMax () {
            Node n = ExpSimple ();
            while (CurrentToken == TokenCategory.MAXIMUM) {
                Node n1 = new Max () {
                GetToken = Expect (TokenCategory.MAXIMUM)
                };

                n1.Add (n);
                n1.Add (ExpSimple ());
                n = n1;
            }
            return n;
        }

        public Node ExpSimple () {
            Node nodo = null;
            switch (CurrentToken) {
                case TokenCategory.FLOTANTE:
                nodo = new Flotante () {
                GetToken = Expect (TokenCategory.FLOTANTE)
                    };
                    break;
                case TokenCategory.DUP:
                    nodo = new Dup () {
                        GetToken = Expect (TokenCategory.DUP)
                    };
                    nodo.Add (ExpSimple ());
                    break;
                case TokenCategory.BRACKET_OPEN:
                    nodo = ExpSum ();
                    break;
                default:
                    Console.WriteLine ("Switch");
                    throw new SyntaxError ();
            }
            return nodo;
        }
    }

    class CILGenerator {
        public string Visit (Programa node) {
            return ".assembly 'Trillian' {}\n\n" +
                ".class public 'final_exam' extends " +
                "['mscorlib']'System'.'Object'{\n"
                + "\t.method public static void 'main'() {\n"
                +"\t\t.entrypoint\n"
                + Visit ((dynamic) node[0])
                +"\t\tcall void ['mscorlib']'System'.'Console'::'WriteLine'(float64)\n"
                +"\t\tret\n"
                +"\t}\n"
                +"}\n";
        }

        public string Visit (Max node) {
            var stringBuilder = new StringBuilder ();
            var first = Visit ((dynamic) node[0]);
            var second = Visit ((dynamic) node[1]);
            stringBuilder.Append (first);
            stringBuilder.Append (second);
            stringBuilder.Append ("\t\tcall float64 ['mscorlib']'System'.'Math'::Max(float64, float64)\n");
            return stringBuilder.ToString ();
        }

        public string Visit (Dup node) {
            var stringBuilder = new StringBuilder ();
            var first = Visit ((dynamic) node[0]);
            stringBuilder.Append (first);
            stringBuilder.Append ("\t\tdup\n");
            stringBuilder.Append ("\t\tadd\n");
            return stringBuilder.ToString ();
        }

        public string Visit (Flotante node) {
            return String.Format ("\t\tldc.r8 {0}\n", node.GetToken.Value);
        }

        public string Visit (Sum node) {
            var stringBuilder = new StringBuilder ();
            stringBuilder.Append (Visit ((dynamic) node[0]));
            for (var i = 1; i < node.GetCount; i++) {
                stringBuilder.Append (Visit ((dynamic) node[i]));
                stringBuilder.Append ("\t\tadd\n");
            }
            return stringBuilder.ToString ();
        }
    }
    // Main function
    class Driver {
        public static void Main (string[] args) {
            try {
                var p = new Parser (new Scanner (args[0]).Start ().GetEnumerator ());
                var ast = p.Inicio ();
                Console.WriteLine (ast.ToStringTree ());
                File.WriteAllText (
                    "output.il",
                    new CILGenerator ().Visit ((dynamic) ast));
            } catch (SyntaxError) {
                Console.Error.WriteLine ("parse error");
                Environment.Exit (1);
            }
        }
    }
}
