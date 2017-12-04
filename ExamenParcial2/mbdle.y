//---------------------------------------------------------
// Jaime margolin a01019332
// juan carlos leon a01020200
// rodrigo solana a01129839
//---------------------------------------------------------

%{

#include <stdio.h>
#include <stdarg.h>

int yylex(void);
void yyerror(char *s, ...);

%}

%union {
    int ival;
}

/* declare tokens */
%token ATOM ILLEGAL EOL PAR_OPEN PAR_CLOSE SQUARE_OPEN SQUARE_CLOSE ANGLE_OPEN ANGLE_CLOSE CURLY_OPEN CURLY_CLOSE COMMA

%%

mbdle:
    /* nothing */ { }                              /* Matches at beginning of input */
    | mbdle atini   EOL { printf("syntax ok\n> "); } /* EOL is end of an expression */
    | mbdle prog  EOL { printf("syntax ok\n> "); }
;

prog:
PAR_OPEN prog2 PAR_CLOSE
| SQUARE_OPEN prog2 SQUARE_CLOSE
| ANGLE_OPEN prog2 ANGLE_CLOSE
| CURLY_OPEN prog2 CURLY_CLOSE
| PAR_OPEN PAR_CLOSE
| SQUARE_OPEN SQUARE_CLOSE
| ANGLE_OPEN ANGLE_CLOSE
| CURLY_OPEN CURLY_CLOSE
;

prog2:
at
| PAR_OPEN prog2 PAR_CLOSE com2
| SQUARE_OPEN prog2 SQUARE_CLOSE com2
| ANGLE_OPEN prog2 ANGLE_CLOSE com2
| CURLY_OPEN prog2 CURLY_CLOSE com2
| PAR_OPEN PAR_CLOSE com2
| SQUARE_OPEN SQUARE_CLOSE com2
| ANGLE_OPEN ANGLE_CLOSE com2
| CURLY_OPEN CURLY_CLOSE com2
;

atini:
 ATOM
;

at:
ATOM
| ATOM atcom
;

atcom:
COMMA at
;


com2:
{}
| COMMA prog2
;

%%

int main(int argc, char **argv) {
    printf("> ");
    yyparse();
    return 0;
}

void yyerror(char *s, ...) {
    va_list ap;
    va_start(ap, s);
    vfprintf(stderr, s, ap);
    fprintf(stderr, "\n");
}
