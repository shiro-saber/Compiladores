//---------------------------------------------------------
// Jaime margolin a01019332
// juan carlos leon a01020200
// rodrigo solana a01129839
//---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public enum TokenCategory {
    ATOM, EOL, ILLEGAL,
    PAR_OPEN, PAR_CLOSE,
    SQUARE_OPEN, SQUARE_CLOSE,
    ANGLE_OPEN, ANGLE_CLOSE,
    CURLY_OPEN, CURLY_CLOSE,
    COMMA
}

public class Token {

    TokenCategory category;

    public TokenCategory Category {
        get { return category; }
    }

    public Token(TokenCategory category) {
        this.category = category;
    }
}

public class Scanner {

    readonly String input;

    static readonly Regex regex =
        new Regex(
            @"(\w+)|(\s)
            |(\() | (\))
            |(\[) | (\])
            |(\<) | (\>)
            |(\{) | (\})
            |(,)
            |(.)",
            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace
        );

    public Scanner(String input) {
        this.input = input;
    }

    public IEnumerable<Token> Start() {
        foreach (Match m in regex.Matches(input)) {
            if (m.Groups[1].Success) {
                yield return new Token(TokenCategory.ATOM);
            } else if (m.Groups[2].Success) {
                continue;
            } else if(m.Groups[3].Success) {
                yield return new Token(TokenCategory.PAR_OPEN);
            } else if(m.Groups[4].Success) {
                yield return new Token(TokenCategory.PAR_CLOSE);
            } else if(m.Groups[5].Success) {
                yield return new Token(TokenCategory.SQUARE_OPEN);
            } else if(m.Groups[6].Success) {
                yield return new Token(TokenCategory.SQUARE_CLOSE);
            } else if(m.Groups[7].Success) {
                yield return new Token(TokenCategory.ANGLE_OPEN);
            } else if(m.Groups[8].Success) {
                yield return new Token(TokenCategory.ANGLE_CLOSE);
            } else if(m.Groups[9].Success) {
                yield return new Token(TokenCategory.CURLY_OPEN);
            } else if(m.Groups[10].Success) {
                yield return new Token(TokenCategory.CURLY_CLOSE);
            } else if (m.Groups[11].Success) {
                yield return new Token(TokenCategory.COMMA);
            } else if (m.Groups[12].Success) {
                yield return new Token(TokenCategory.ILLEGAL);
            }
        }
        yield return new Token(TokenCategory.EOL);
    }
}

class SyntaxError: Exception {
  public SyntaxError(TokenCategory expectedCategory,
                     Token token):
      base(String.Format(
          "Syntax Error: Expecting {0} but found {1}",
          expectedCategory,
          token.Category)) {
  }

  public SyntaxError(string tokens,
                     Token token):
      base(String.Format(
          "Syntax Error: Expecting {0} but found {1}",
          tokens,
          token.Category)) {
  }
}

public class Parser {

    IEnumerator<Token> tokenStream;

    public Parser(IEnumerator<Token> tokenStream) {
        this.tokenStream = tokenStream;
        this.tokenStream.MoveNext();
    }

    public TokenCategory Current {
        get { return tokenStream.Current.Category; }
    }
    public Token CurrentToken {
        get { return tokenStream.Current; }
    }

    public Token Expect(TokenCategory category) {
        if (Current == category) {
            Token current = tokenStream.Current;
            tokenStream.MoveNext();
            return current;
        } else {
            throw new SyntaxError(category, CurrentToken);
        }
    }

    public void DetectorOpen()
    {
      // Console.WriteLine("--- entre a normal ---");
      switch (Current) {
        case TokenCategory.PAR_OPEN:
          Expect(TokenCategory.PAR_OPEN);
            SharmutaRecursiva();
          Expect(TokenCategory.PAR_CLOSE);
          break;
        case TokenCategory.SQUARE_OPEN:
          Expect(TokenCategory.SQUARE_OPEN);
            SharmutaRecursiva();
          Expect(TokenCategory.SQUARE_CLOSE);
          break;
        case TokenCategory.ANGLE_OPEN:
          Expect(TokenCategory.ANGLE_OPEN);
            SharmutaRecursiva();
          Expect(TokenCategory.ANGLE_CLOSE);
          break;
        case TokenCategory.CURLY_OPEN:
          Expect(TokenCategory.CURLY_OPEN);
            SharmutaRecursiva();
          Expect(TokenCategory.CURLY_CLOSE);
          break;
          default:
          throw new SyntaxError("(,[,<,{", CurrentToken);
    }
    // Console.WriteLine("programa termina");
  }

  public void DetectorOpenComma()
  {
    // Console.WriteLine("+++ entre a detector coma +++");
    // Console.WriteLine($"-----------------{Current}------------------");
    switch (Current) {
      case TokenCategory.PAR_OPEN:
        Expect(TokenCategory.PAR_OPEN);
          SharmutaRecursiva();
        Expect(TokenCategory.PAR_CLOSE);
        while(Current == TokenCategory.COMMA)
        {
          ListContinuer();
        }
        break;
      case TokenCategory.SQUARE_OPEN:
        Expect(TokenCategory.SQUARE_OPEN);
          SharmutaRecursiva();
        Expect(TokenCategory.SQUARE_CLOSE);
        while(Current == TokenCategory.COMMA)
        {
          ListContinuer();
        }
        break;
      case TokenCategory.ANGLE_OPEN:
        Expect(TokenCategory.ANGLE_OPEN);
          SharmutaRecursiva();
        Expect(TokenCategory.ANGLE_CLOSE);
        while(Current == TokenCategory.COMMA)
        {
          ListContinuer();
        }
        break;
      case TokenCategory.CURLY_OPEN:
        Expect(TokenCategory.CURLY_OPEN);
          SharmutaRecursiva();
        Expect(TokenCategory.CURLY_CLOSE);
        while(Current == TokenCategory.COMMA)
        {
          ListContinuer();
        }
        break;
        default:
          throw new SyntaxError("(,[,<,{", CurrentToken);
  }
  // Console.WriteLine("sali de detector de coma");
}

  public void SharmutaRecursiva()
  {
    // Console.WriteLine("++++entre a sahrmuta ++++");
    if(Current == TokenCategory.ATOM)
    {
      AtomContinuer();
    }else if(
      Current == TokenCategory.PAR_OPEN ||
      Current == TokenCategory.SQUARE_OPEN ||
      Current == TokenCategory.ANGLE_OPEN ||
      Current == TokenCategory.CURLY_OPEN
    )
    {
      DetectorOpenComma();
    }
    // Console.WriteLine("sali de sharmuta");
  }

  public void ListContinuer()
  {
    // Console.WriteLine("entre a ListContinuercontinuer");
    Expect(TokenCategory.COMMA);
    DetectorOpenComma();
    // Console.WriteLine("sali de ListContinuercontinuer");
  }

  public void AtomContinuer()
  {
    // Console.WriteLine("entre a AtomContinuer");
    Expect(TokenCategory.ATOM);
    if (Current == TokenCategory.COMMA ) {
      Expect(TokenCategory.COMMA);
      AtomContinuer();
    }
    // Console.WriteLine("sali de continuer");
  }

    public void Mbdle() {

        if(Current == TokenCategory.ATOM)
        {
          Expect(TokenCategory.ATOM);
        }else if(
          Current == TokenCategory.PAR_OPEN ||
          Current == TokenCategory.SQUARE_OPEN ||
          Current == TokenCategory.ANGLE_OPEN ||
          Current == TokenCategory.CURLY_OPEN
        )
        {
          DetectorOpen();
        }
        Expect(TokenCategory.EOL);
    }
}

public class MBDLE {

    public static void Main(String[] args) {
      int[] pito = new int[3] {1,4,7};
      Console.WriteLine($"{pito}");
        try {
            while (true) {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == null) {
                    break;
                }
                var parser = new Parser(new Scanner(line).Start().GetEnumerator());
                parser.Mbdle();
                Console.WriteLine("syntax ok");
            }
        } catch (SyntaxError) {
            // Console.WriteLine(e);
            Console.WriteLine("syntax error");
        }
    }
}
