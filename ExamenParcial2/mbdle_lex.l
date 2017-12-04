/*---------------------------------------------------------
  jaime margolin a01019332
  juan carlos leon a01020200
  rodrigo solana a01129839
-----------------------------------------------------------*/

%{

#include "mbdle_tokens.h"

%}

%%

[a-zA-Z0-9_]+  { return ATOM; }
[ \t]          { /* ignore whitespace */ }
\n             { return EOL; }
[(]            { return PAR_OPEN; }
[)]            { return PAR_CLOSE; }
[\[]           { return SQUARE_OPEN; }
[\]]           { return SQUARE_CLOSE; }
[<]            { return ANGLE_OPEN; }
[>]            { return ANGLE_CLOSE; }
[{]            { return CURLY_OPEN; }
[}]            { return CURLY_CLOSE; }
[,]            { return COMMA; }
.              { return ILLEGAL; }

%%
