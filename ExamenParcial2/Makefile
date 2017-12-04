all: mbdle1.exe mbdle2

mbdle1.exe: mbdle.cs
	mcs -out:mbdle1.exe mbdle.cs

mbdle2: mbdle_lex.l mbdle.y
	bison --defines=mbdle_tokens.h -o mbdle.c mbdle.y
	flex -o mbdle_lex.c mbdle_lex.l
	gcc -o mbdle2 mbdle.c mbdle_lex.c -lfl

clean:
	rm mbdle1.exe mbdle2 mbdle.c mbdle_lex.c mbdle_tokens.h
