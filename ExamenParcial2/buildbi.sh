bison --defines=mbdle_tokens.h -o mbdle.c mbdle.y
flex -o mbdle_lex.c mbdle_lex.l
gcc -o mbdle2 mbdle.c mbdle_lex.c -lfl
