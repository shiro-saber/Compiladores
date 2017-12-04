using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Ppt {
    enum Token {
        PIEDRA,
        PAPEL,
        TIJERAS,
        MAS,
        MENOS,
        PARENTESIS_OPEN,
        PARENTESIS_CLOSE,
        ILEGAL,
        EOF
    }

    class SyntaxError : Exception { }

    // Lexical and Syntax Analysis
    class Scanner {
        readonly string input;

        // Sinaxis del lenguaje
        static readonly Regex regex = new Regex (
            @"
                (?<Piedra>                 piedra  )
                |(?<Papel>                 papel   )
                |(?<Tijeras>               tijeras )
                |(?<Mas>                   [+]     )
                |(?<Menos>                 [-]     )
                |(?<Parentesis_open>       [(]     )
                |(?<Parentesis_close>      [)]     )
                |(?<Spaces>                \s      )
                |(?<Otro>                  .       )
            ",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled
        );

        // El match de la regex con el Token
        static readonly IDictionary<string, Token> regexLabels =
            new Dictionary<string, Token> () { 
                { "Piedra", Token.PIEDRA }, 
                { "Papel", Token.PAPEL }, 
                { "Tijeras", Token.TIJERAS }, 
                { "Mas", Token.MAS }, 
                { "Menos", Token.MENOS }, 
                { "Parentesis_open", Token.PARENTESIS_OPEN }, 
                { "Parentesis_close", Token.PARENTESIS_CLOSE }
            };

        // Scanner 
        public Scanner (string input) {
            this.input = input;
        }

        public IEnumerable<Token> Start () {
            foreach (Match m in regex.Matches (input)) {
                if (m.Groups["Spaces"].Success) {
                    // Ignora el espacio
                } else if (m.Groups["Otro"].Success) {
                    yield return Token.ILEGAL;
                } else {
                    foreach (var name in regexLabels.Keys) {
                        if (m.Groups[name].Success) {
                            yield return regexLabels[name];
                            break;
                        }
                    }
                }
            }
            yield return Token.EOF;
        }
    }

    // AST Tree and semantics
    class Node : IEnumerable<Node> {
        // Lista de los hijos
        IList<Node> children = new List<Node> ();

        // Current Node
        public Node this [int index] {
            get {
                return children[index];
            }
        }

        // Agregar nodos a la lista
        public void Add (Node node) {
            children.Add (node);
        }

        // Get all childrens in list
        public IEnumerator<Node> GetEnumerator () {
            return children.GetEnumerator ();
        }

        // Exception
        System.Collections.IEnumerator
        System.Collections.IEnumerable.GetEnumerator () {
            throw new NotImplementedException ();
        }

        // Convert to string 
        public override string ToString () {
            return GetType ().Name;
        }

        // Get the tree nad convert it into string
        public string ToStringTree () {
            var builder = new StringBuilder ();
            TreeTrasversal (this, "", builder);
            return builder.ToString ();
        }

        // Build the Tree
        static void TreeTrasversal (Node node, string indent, StringBuilder builder) {
            builder.Append (indent);
            builder.Append (node);
            builder.Append ("\n");

            foreach (var child in node.children) {
                TreeTrasversal (child, indent + "    ", builder);
            }
        }
    }

    /* Inside nodes classes */
    class Programa : Node { }
    class Mas : Node { }
    class Menos : Node { }
    class Piedra : Node { }
    class Papel : Node { }
    class Tijeras : Node { }
    /* End of nodes classes */

    class Parser {
        IEnumerator<Token> tokenStream;

        public Parser (IEnumerator<Token> tokenStream) {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext ();
        }

        public Token CurrentToken {
            get {
                return tokenStream.Current;
            }
        }

        public Token Expect (Token category) {
            if (CurrentToken == category) {
                Token current = tokenStream.Current;
                tokenStream.MoveNext ();
                return current;
            } else {
                throw new SyntaxError ();
            }
        }

        public Node Inicio () {
            var p = new Programa () { Exp () };
            Expect (Token.EOF);
            return p;
        }

        public Node Exp () {
            var exp = ExpMas ();
            if (CurrentToken == Token.MENOS) {
                Expect (Token.MENOS);
                exp = new Menos () { exp, Exp () };
            }
            return exp;
        }

        public Node ExpMas () {
            var exp1 = ExpSimple ();
            while (CurrentToken == Token.MAS) {
                Expect (Token.MAS);
                var exp2 = new Mas () { exp1, ExpSimple () };
                exp1 = exp2;
            }
            return exp1;
        }

        public Node ExpSimple () {
            switch (CurrentToken) {
                case Token.PIEDRA:
                    Expect (Token.PIEDRA);
                    return new Piedra ();
                case Token.PAPEL:
                    Expect (Token.PAPEL);
                    return new Papel ();
                case Token.TIJERAS:
                    Expect (Token.TIJERAS);
                    return new Tijeras ();
                case Token.PARENTESIS_OPEN:
                    Expect (Token.PARENTESIS_OPEN);
                    var exp = Exp ();
                    Expect (Token.PARENTESIS_CLOSE);
                    return exp;
                default:
                    throw new SyntaxError ();
            }
        }
    }

    class CILGenerator {
        public string Visit (Programa node) {
            return ".assembly 'ppt' {}\n\n" +
                ".assembly extern 'pptlib' {}\n\n" +
                ".class public 'salida' extends ['mscorlib']'System'.'Object' {\n"
                + "\t.method public static void 'inicio'() {\n"
                + "\t\t.entrypoint\n"
                + Visit ((dynamic) node[0])
                + "\t\tcall void ['mscorlib']'System'.'Console'::'WriteLine'(string)\n"
                + "\t\tret\n"
                + "\t}\n" +
                "}\n";
        }
        public string Visit (Mas node) {
            return Visit ((dynamic) node[0]) +
                Visit ((dynamic) node[1]) +
                "\t\tcall string class ['pptlib']'ppt'.'Runtime'::'mas'(string, string)\n";
        }
        public string Visit (Menos node) {
            return Visit ((dynamic) node[0]) +
                Visit ((dynamic) node[1]) +
                "\t\tcall string class ['pptlib']'ppt'.'Runtime'::'menos'(string, string)\n";
        }
        public string Visit (Piedra node) {
            return "\t\tldstr \"piedra\"\n";
        }
        public string Visit (Papel node) {
            return "\t\tldstr \"papel\"\n";
        }
        public string Visit (Tijeras node) {
            return "\t\tldstr \"tijeras\"\n";
        }
    }

    //---------------------------------------------------------------
    class Driver {
        public static void Main (string[] args) {
            try {
                var p = new Parser (
                    new Scanner (args[0]).Start ().GetEnumerator ());
                var ast = p.Inicio ();
                Console.WriteLine (ast.ToStringTree ());
                File.WriteAllText (
                    "salida.il",
                    new CILGenerator ().Visit ((dynamic) ast));
            } catch (SyntaxError) {
                Console.Error.WriteLine ("parse error");
                Environment.Exit (1);
            }
        }
    }
}