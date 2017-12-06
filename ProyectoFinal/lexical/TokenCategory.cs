/*
  Jaime Margolin A01019332
  Juan carlos Leon A01020200
  Rodrigo Solana A01129839
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
        //
        IF,
        INLINEIF,
        ASSIGN,
        END,
        EOF,
        FALSE,
        BOOL,
        IDENTIFIER,
        INT_LITERAL,
        PARENTHESIS_OPEN,
        PARENTHESIS_CLOSE,
        PRINT,
        THEN,
        TRUE,
        ILLEGAL_CHAR,
        //
        BREAK,
        ELSE,
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
        SEMICOLON,
        COLON,
        // ints
        INT,
        BIN,
        OCT,
        HEX,
        //
        COMMA,
        COMMENT
    }
}
