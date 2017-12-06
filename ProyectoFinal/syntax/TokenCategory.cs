/*
  Jaime Margolin A01019332
  Juan carlos Leon A01020200
  Rodrido Solana A01129839
*/

namespace Buttercup {

    enum TokenCategory {
        // operators
        AND,
        PLUS,
        MUL,
        NEG,
        NOMOR,
        BITOR,
        NOMAND,
        BITAND,
        EQCOMPARE,
        NOTEQ,
        LESS,
        EQLESS,
        MORE,
        EQMORE,
        MOD,
        DIV,
        NOTT,
        BINNOTT,
        POWER,
        SHIFTLEFT,
        SHIFTRIGHT,
        TRIPLESHIFT,
        // if stuff
        IF,
        ELSE,
        ELSEIF,
        //
        INLINEIF,
        ASSIGN,
        END,
        EOF,
        IDENTIFIER,
        INT_LITERAL,
        PARENTHESIS_OPEN,
        PARENTHESIS_CLOSE,
        PRINT,
        THEN,
        ILLEGAL_CHAR,
        //
        BREAK,
        RETURN,
        CASE,
        SWITCH,
        CONTINUE,
        FOR,
        DEFAULT,
        WHILE,
        DO,
        IN,
        VAR,
        CURLY_OPEN,
        CURLY_CLOSE,
        COLON,
        // bools
        TRUE,
        FALSE,
        //types
        // ints
        INT,
        BIN,
        OCT,
        HEX,
        // others
        BOOL,
        CHAR,
        STRING,
        //
        SEMICOLON,
        COMMA,
        COMMENT
    }
}
