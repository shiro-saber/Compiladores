/*
  Jaime Margolin A01019332
  Juan carlos Leon A01020200
  Rodrigo Solana A01129839
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Buttercup {

class Scanner {

readonly string input;

static readonly Regex regex = new Regex(
        @"
               (?<Comment>    [/][/].*  )
              | (?<Comment>    \/\* [\w \W]*? \*\/ )
              | (?<False>      false      )
              | (?<Identifier> [a-zA-Z]+ [a-zA-Z0-9_]* )
              | (?<Char>       ' [\w \d \ ]? '  )
              | (?<Char>       ' \\u[a-fA-F0-9]{6} '  )
              | (?<Char>       ' \\[n t \\ r "" '] '   )
              | (?<String>     "".*""    )
              | (?<EqLess>     \<=       )
              | (?<Less>       \<        )
              | (?<EqMore>     \>=       )
              | (?<More>       \>        )
              | (?<EqCompare>  ==        )
              | (?<Assign>     =         )
              | (?<NotEq>      !=        )
              | (?<Mul>        [*]       )
              | (?<Neg>        [-]       )
              | (?<ParLeft>    [(]       )
              | (?<ParRight>   [)]       )
              | (?<Plus>       [+]       )
              | (?<True>       true      )
              | (?<CurlyOpen>  [{]       )
              | (?<CurlyClose> [}]       )
              | (?<Semicolon>  [;]       )
              | (?<InlineIf>   [?]       )
              | (?<Colon>      [:]       )
              | (?<NomOr>      [||]      )
              | (?<BitOr>      [|]       )
              | (?<NomAnd>     [&&]      )
              | (?<BitAnd>     [&]       )
              | (?<Comma>      ,         )
              | (?<Bin>        [0 1]+ [b B])
              | (?<Oct>        [0-7]+ [o O])
              | (?<Hex>        [0-9a-fA-F]+ [x|X])
              | (?<Int>        \d+       )
              | (?<Newline>    \n        )
              | (?<WhiteSpace> \s        )     # Must go anywhere after Newline.
              | (?<Other>      .         )     # Must be last: match any other character.
            ",
        RegexOptions.IgnorePatternWhitespace
        | RegexOptions.Compiled
        | RegexOptions.Multiline
        );
        static readonly Regex nl = new Regex(
        @"
          (?<Newline>    \n        )
        ",
        RegexOptions.IgnorePatternWhitespace
        | RegexOptions.Compiled
        | RegexOptions.Multiline);

static readonly IDictionary<string, TokenCategory> keywords =
        new Dictionary<string, TokenCategory>() {
        {"break", TokenCategory.BREAK},
        {"else", TokenCategory.ELSE},
        {"return", TokenCategory.RETURN},
        {"case", TokenCategory.CASE},
        {"false", TokenCategory.FALSE},
        {"switch", TokenCategory.SWITCH},
        {"continue", TokenCategory.CONTINUE},
        {"for", TokenCategory.FOR},
        {"true", TokenCategory.TRUE},
        {"default", TokenCategory.DEFAULT},
        {"if", TokenCategory.IF},
        {"while", TokenCategory.WHILE},
        {"do", TokenCategory.DO},
        {"in", TokenCategory.IN},
        {"var", TokenCategory.VAR}
};

static readonly IDictionary<string, TokenCategory> nonKeywords =
        new Dictionary<string, TokenCategory>() {
        {"And", TokenCategory.AND},
        {"Assign", TokenCategory.ASSIGN},
        {"False", TokenCategory.FALSE},
        {"Int", TokenCategory.INT},
        {"Bin", TokenCategory.BIN},
        {"Oct", TokenCategory.OCT},
        {"Hex", TokenCategory.HEX},
        {"Less", TokenCategory.LESS},
        {"Mul", TokenCategory.MUL},
        {"Neg", TokenCategory.NEG},
        {"ParLeft", TokenCategory.PARENTHESIS_OPEN},
        {"ParRight", TokenCategory.PARENTHESIS_CLOSE},
        {"Plus", TokenCategory.PLUS},
        {"True", TokenCategory.TRUE},
        {"CurlyOpen", TokenCategory.CURLY_OPEN},
        {"CurlyClose", TokenCategory.CURLY_CLOSE},
        {"Semicolon", TokenCategory.SEMICOLON},
        {"InlineIf", TokenCategory.INLINEIF},
        {"Colon", TokenCategory.COLON},
        {"NomOr", TokenCategory.NOMOR},
        {"BitOr", TokenCategory.BITOR},
        {"NomAnd", TokenCategory.NOMAND},
        {"BitAnd", TokenCategory.BITAND},
        {"EqCompare", TokenCategory.EQCOMPARE},
        {"NotEq", TokenCategory.NOTEQ},
        {"More", TokenCategory.MORE},
        {"EqMore", TokenCategory.EQMORE},
        {"Comma", TokenCategory.COMMA}
};

public Scanner(string input) {
        this.input = input;
}

public IEnumerable<Token> Start() {

        var row = 1;
        var columnStart = 0;

        Func<Match, TokenCategory, Token> newTok = (m, tc) =>
                                                   new Token(m.Value, tc, row, m.Index - columnStart + 1);

        foreach (Match m in regex.Matches(input)) {

                if (m.Groups["Newline"].Success) {

                        // Found a new line.
                        row++;
                        columnStart = m.Index + m.Length;

                } else if (m.Groups["WhiteSpace"].Success) {
                        // Skip white space and comments.

                } else if (m.Groups["Comment"].Success){
                        foreach(Match mm in nl.Matches(m.Captures[0].ToString())){
                          row++;
                        }
                        yield return newTok(m, TokenCategory.COMMENT);
                }else if (m.Groups["Identifier"].Success) {

                        if (keywords.ContainsKey(m.Value)) {

                                // Matched string is a Buttercup keyword.
                                yield return newTok(m, keywords[m.Value]);

                        } else {

                                // Otherwise it's just a plain identifier.
                                yield return newTok(m, TokenCategory.IDENTIFIER);
                        }

                } else if (m.Groups["Other"].Success) {

                        // Found an illegal character.
                        yield return newTok(m, TokenCategory.ILLEGAL_CHAR);

                } else {

                        // Match must be one of the non keywords.
                        foreach (var name in nonKeywords.Keys) {
                                if (m.Groups[name].Success) {
                                        yield return newTok(m, nonKeywords[name]);
                                        break;
                                }
                        }
                }
        }

        yield return new Token(null,
                               TokenCategory.EOF,
                               row,
                               input.Length - columnStart + 1);
}
}
}
