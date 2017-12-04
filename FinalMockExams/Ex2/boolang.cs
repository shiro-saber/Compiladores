/*-------------------------------------------------------------------

BNF Grammar for the Boolang language:

    Start     ::= Exp
    Exp       ::= AndExp
    Exp       ::= Exp "|" AndExp
    AndExp    ::= SimpleExp
    AndExp    ::= AndExp "&" SimpleExp
    SimpleExp ::= "(" Exp ")"
    SimpleExp ::= "!" SimpleExp
    SimpleExp ::= "0"
    SimpleExp ::= "1"
     
-------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Boolang {
    
    //---------------------------------------------------------------
    class SyntaxError: Exception {}
    
    //---------------------------------------------------------------
    enum Token {
        AND, OR, NOT, PAR_LEFT, PAR_RIGHT, ZERO, ONE, 
        ILLEGAL_CHAR, EOF
    }
                   
    //---------------------------------------------------------------
    class Scanner {
        readonly string input;
        static readonly Regex regex = new Regex(
            @"                             
                (?<And>        [&] )
              | (?<Or>         [|] )
              | (?<Not>        [!] )
              | (?<ParLeft>    [(] )
              | (?<ParRight>   [)] )
              | (?<Zero>       [0] )
              | (?<One>        [1] )
              | (?<WhiteSpace> \s  )
              | (?<Other>      .   )
            ", 
            RegexOptions.IgnorePatternWhitespace 
                | RegexOptions.Compiled                
            );
        static readonly IDictionary<string, Token> regexLabels =
            new Dictionary<string, Token>() {
                {"And", Token.AND},
                {"Or", Token.OR},
                {"Not", Token.NOT},
                {"ParLeft", Token.PAR_LEFT},
                {"ParRight", Token.PAR_RIGHT},
                {"Zero", Token.ZERO},
                {"One", Token.ONE}
            };
        public Scanner(string input) {
            this.input = input;
        }
        public IEnumerable<Token> Start() {
            foreach (Match m in regex.Matches(input)) {
                if (m.Groups["WhiteSpace"].Success) {
                    // Skip spaces.
                } else if (m.Groups["Other"].Success) {
                    yield return Token.ILLEGAL_CHAR;
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
    
    //---------------------------------------------------------------
    class Node: IEnumerable<Node> {
        IList<Node> children = new List<Node>();
        public Node this[int index] {
            get {
                return children[index];
            }
        }
        public void Add(Node node) {
            children.Add(node);
        }
        public IEnumerator<Node> GetEnumerator() {
            return children.GetEnumerator();
        }
        System.Collections.IEnumerator
                System.Collections.IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }
        public override string ToString() {
            return GetType().Name;                                 
        }
        public string ToStringTree() {
            var sb = new StringBuilder();
            TreeTraversal(this, "", sb);
            return sb.ToString();
        }
        static void TreeTraversal(
                Node node, 
                string indent, 
                StringBuilder sb) {
            sb.Append(indent);
            sb.Append(node);
            sb.Append('\n');
            foreach (var child in node.children) {
                TreeTraversal(child, indent + "  ", sb);
            }
        }
    }
    
    //---------------------------------------------------------------
    class Program:   Node {}
    class And:       Node {}
    class Or:        Node {}
    class Not:       Node {}
    class Literal_0: Node {}
    class Literal_1: Node {}
    
    //---------------------------------------------------------------
    class Parser {
        IEnumerator<Token> tokenStream;
        public Parser(IEnumerator<Token> tokenStream) {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }
        public Token CurrentToken {
            get { return tokenStream.Current; }
        }
        public Token Expect(Token category) {
            if (CurrentToken == category) {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            } else {
                throw new SyntaxError();                
            }
        }
        public Node Start() {
            var e = new Program() { Exp() };
            Expect(Token.EOF);
            return e;
        }
        public Node Exp() {            
            var exp1 = AndExp();
            while (CurrentToken == Token.OR) {
                Expect(Token.OR);
                var exp2 = new Or() { exp1, AndExp() };
                exp1 = exp2;
            }
            return exp1;
        }
        public Node AndExp() {
            var exp1 = SimpleExp();
            while (CurrentToken == Token.AND) {
                Expect(Token.AND);
                var exp2 = new And() { exp1, SimpleExp() };                
                exp1 = exp2;
            }
            return exp1;
        }
        public Node SimpleExp() {
            switch (CurrentToken) {                
            case Token.PAR_LEFT:
                Expect(Token.PAR_LEFT);
                var exp = Exp();
                Expect(Token.PAR_RIGHT);
                return exp;
            case Token.NOT:
                Expect(Token.NOT);                                 
                return new Not() { SimpleExp() };            
            case Token.ZERO:
                Expect(Token.ZERO);
                return new Literal_0();
            case Token.ONE:
                Expect(Token.ONE);
                return new Literal_1();
            default:
                throw new SyntaxError();
            }
        }        
    }
    
    //---------------------------------------------------------------
    class CILGenerator {
        public string Visit(Program node) {
            return ".assembly 'boolang' {}\n\n"
                + ".class public 'FinalExam' extends ['mscorlib']'System'.'Object' {\n"
                + "\t.method public static void 'start'() {\n"
                + "\t\t.entrypoint\n"
                + Visit((dynamic) node[0])
                + "\t\tcall void ['mscorlib']'System'.'Console'::'WriteLine'(int32)\n"
                + "\t\tret\n"
                + "\t}\n"
                + "}\n";
        }
        public string Visit(And node) {
            return Visit((dynamic) node[0])
                + Visit((dynamic) node[1])
                + "\t\tand\n";
        }
        public string Visit(Or node) {
            return Visit((dynamic) node[0])
                + Visit((dynamic) node[1])
                + "\t\tor\n";                
        }
        public string Visit(Not node) {
            return Visit((dynamic) node[0]) 
                + "\t\tldc.i4.1\n"
                + "\t\txor\n"; 
        }
        public string Visit(Literal_0 node) {
            return "\t\tldc.i4.0\n";
        }
        public string Visit(Literal_1 node) {
            return "\t\tldc.i4.1\n";
        }        
    }
    
    //---------------------------------------------------------------
    class Driver {
        public static void Main(string[] args) {
            try {
                var p = new Parser(
                    new Scanner(args[0]).Start().GetEnumerator());                
                var ast = p.Start();
                Console.Write(ast.ToStringTree());
                File.WriteAllText(
                    "output.il", 
                    new CILGenerator().Visit((dynamic) ast));
            } catch (SyntaxError) {
                Console.Error.WriteLine("parse error");
                Environment.Exit(1);
            }
        }
    }    
}