/*
   Jaime Margolin A01019332
   Juan carlos Leon A01020200
   Rodrido Solana A01129839
 */

using System;
using System.Collections.Generic;

namespace Buttercup {

class Parser {

static readonly ISet<TokenCategory> firstOfDeclaration =
        new HashSet<TokenCategory>() {
        TokenCategory.VAR
};

static readonly ISet<TokenCategory> firstOfStatement =
        new HashSet<TokenCategory>() {
        TokenCategory.IDENTIFIER,
        TokenCategory.PRINT,
        TokenCategory.IF,
        TokenCategory.SWITCH,
        TokenCategory.WHILE,
        TokenCategory.DO,
        TokenCategory.FOR
};

static readonly ISet<TokenCategory> firstOfOperator =
        new HashSet<TokenCategory>() {
        TokenCategory.AND,
        TokenCategory.LESS,
        TokenCategory.PLUS,
        TokenCategory.MUL,
        TokenCategory.NEG,
        TokenCategory.NOMOR,
        TokenCategory.BITOR,
        TokenCategory.NOMAND,
        TokenCategory.BITAND,
        TokenCategory.EQCOMPARE,
        TokenCategory.NOTEQ,
        TokenCategory.EQLESS,
        TokenCategory.MORE,
        TokenCategory.EQMORE,
        TokenCategory.POWER,
        TokenCategory.SHIFTLEFT,
        TokenCategory.SHIFTRIGHT,
        TokenCategory.TRIPLESHIFT,
        TokenCategory.DIV,
        TokenCategory.MOD,
        TokenCategory.NOTT,
        TokenCategory.BINNOTT,
        TokenCategory.INLINEIF
};

static readonly ISet<TokenCategory> firstOfSimpleExpression =
        new HashSet<TokenCategory>() {
        TokenCategory.IDENTIFIER,
        TokenCategory.INT_LITERAL,
        TokenCategory.TRUE,
        TokenCategory.FALSE,
        TokenCategory.PARENTHESIS_OPEN,
        TokenCategory.NEG
};

/*Lo necesario para SWITCH statement*/



static readonly ISet<TokenCategory> switchSimpleExpression=
        new HashSet<TokenCategory>() {
        TokenCategory.CASE
};


static readonly ISet<TokenCategory> litSimple =
        new HashSet<TokenCategory>() {
        TokenCategory.INT_LITERAL,
        TokenCategory.CHAR,
        TokenCategory.TRUE,
        TokenCategory.FALSE

};

IEnumerator<Token> tokenStream;

public Parser(IEnumerator<Token> tokenStream) {
        this.tokenStream = tokenStream;
        this.tokenStream.MoveNext();
}

public TokenCategory CurrentToken {
        get { return tokenStream.Current.Category; }
}
public Token CurrentToken2 {
        get { return tokenStream.Current; }
}

public Token Expect(TokenCategory category) {
        Console.WriteLine(CurrentToken2);
        if (CurrentToken == category) {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
        } else {
                throw new SyntaxError(category, tokenStream.Current);
        }
}

public void Program() {

        Comentamela();
        while (firstOfDeclaration.Contains(CurrentToken)) {
                Declaration();
                Comentamela();
        }

        while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
                Comentamela();
        }
        Expect(TokenCategory.EOF);
}
public void Comentamela()
{

        if (CurrentToken == TokenCategory.COMMENT) {
                Expect(TokenCategory.COMMENT);
        }
        if (CurrentToken == TokenCategory.COMMENT) {
                Comentamela();
        }
}

public void Finisher(){
        Expect(TokenCategory.SEMICOLON);
}

public void Declaration() {
        Console.WriteLine("Declaration");
        Comentamela();
        switch (CurrentToken) {
        case TokenCategory.VAR:
                Vareamela();
                break;
        case TokenCategory.COMMENT:
                Expect(TokenCategory.COMMENT);
                break;

        default:
                break;
        }
        Console.WriteLine("sali de Declaration");
}

public void Arreglamela(){
  Console.WriteLine("Arreglamela");
  Expect(TokenCategory.CURLY_OPEN);
  if(CurrentToken != TokenCategory.CURLY_CLOSE)
  {
    ArreglamelaContinuer();
  }
  Expect(TokenCategory.CURLY_CLOSE);
  Finisher();
  Console.WriteLine("sali de Arreglamela");
}

public void ArreglamelaContinuer()
{
  Console.WriteLine("ArreglamelaContinuer");
  SimpleExpression();
  if(CurrentToken == TokenCategory.COMMA)
  {
    Expect(TokenCategory.COMMA);
    ArreglamelaContinuer();
  }
  Console.WriteLine("sali de ArreglamelaContinuer");
}

public void Vareamela(){
        Console.WriteLine("Vareamela");
        Expect(TokenCategory.VAR);
        Expect(TokenCategory.IDENTIFIER);
        if(CurrentToken == TokenCategory.COMMA)
        {
                Expect(TokenCategory.COMMA);
                DeclarationContinuer();
        }else if(CurrentToken == TokenCategory.IDENTIFIER) {
                throw new SyntaxError(TokenCategory.COMMA, tokenStream.Current);
        }
        Finisher();
        Console.WriteLine("sali de Vareamela");
}

public void DeclarationContinuer()
{
        Console.WriteLine("DeclarationContinuer");
        Comentamela();
        Expect(TokenCategory.IDENTIFIER);
        Comentamela();
        if(CurrentToken == TokenCategory.COMMA)
        {
                Expect(TokenCategory.COMMA);
                DeclarationContinuer();
        }
        Console.WriteLine("sali de DeclarationContinuer");
}

public void ArgumentContinuer()
{
        Console.WriteLine("ArgumentContinuer");
        // SimpleExpression();
        Expression();

        if(CurrentToken == TokenCategory.PARENTHESIS_OPEN){
          Funcionamela();
        }
        if(CurrentToken == TokenCategory.COMMA)
        {
                Expect(TokenCategory.COMMA);
                ArgumentContinuer();
        }
        Console.WriteLine("sali de ArgumentContinuer");
}

public void Identificamela()
{
        Console.WriteLine("Identificamela");
        Expect(TokenCategory.IDENTIFIER);
        switch(CurrentToken)
        {
        case TokenCategory.PARENTHESIS_OPEN:
                Funcionamela();
                break;
        case TokenCategory.ASSIGN:
                Assignment();
                break;
        case TokenCategory.AND:
        case TokenCategory.LESS:
        case TokenCategory.PLUS:
        case TokenCategory.MUL:
        case TokenCategory.NEG:
        case TokenCategory.NOMOR:
        case TokenCategory.BITOR:
        case TokenCategory.NOMAND:
        case TokenCategory.BITAND:
        case TokenCategory.EQCOMPARE:
        case TokenCategory.NOTEQ:
        case TokenCategory.EQLESS:
        case TokenCategory.MORE:
        case TokenCategory.EQMORE:
        case TokenCategory.MOD:
        case TokenCategory.DIV:
        case TokenCategory.NOTT:
        case TokenCategory.BINNOTT:
        case TokenCategory.POWER:
        case TokenCategory.SHIFTLEFT:
        case TokenCategory.SHIFTRIGHT:
        case TokenCategory.TRIPLESHIFT:
                Operator();
                Expression();
                //Finisher();
                break;
        default:
                break;
        }
        Console.WriteLine("sali de Identificamela");
}

public void Breakeamela(){
  Console.WriteLine("Breakeamela");
  if(CurrentToken == TokenCategory.BREAK){
        Expect(TokenCategory.BREAK);
        Expect(TokenCategory.SEMICOLON);
  }
  Console.WriteLine("sali de Breakeamela");
}

public void Continuamela(){
  Console.WriteLine("Continuamela");
  if(CurrentToken == TokenCategory.CONTINUE){
        Expect(TokenCategory.CONTINUE);
        Expect(TokenCategory.SEMICOLON);
  }
  Console.WriteLine("sali de Continuamela");
}

public void Returneamela(){
  Console.WriteLine("Returneamela");
  if(CurrentToken == TokenCategory.RETURN){
        Expect(TokenCategory.RETURN);
        if(CurrentToken != TokenCategory.SEMICOLON){
          Expression();
        }
        Expect(TokenCategory.SEMICOLON);
  }
  Console.WriteLine("sali de Returneamela");
}

public void Funcionamela()
{
        Console.WriteLine("Funcionamela");
        Expect(TokenCategory.PARENTHESIS_OPEN);
        Comentamela();
        if (CurrentToken != TokenCategory.PARENTHESIS_CLOSE)
        {
                ArgumentContinuer();

        }
        Expect(TokenCategory.PARENTHESIS_CLOSE);
        if (CurrentToken == TokenCategory.SEMICOLON)
        {
                Expect(TokenCategory.SEMICOLON);
                return;
        }
        if(CurrentToken == TokenCategory.CURLY_OPEN){
          Expect(TokenCategory.CURLY_OPEN);
          while(CurrentToken != TokenCategory.CURLY_CLOSE)
          {
                  Declaration();
                  Statement();
                  Returneamela();
                  Breakeamela();
                  Continuamela();
          }
          Returneamela();
          Breakeamela();
          Continuamela();
          Expect(TokenCategory.CURLY_CLOSE);
        }
        Console.WriteLine("sali de Funcionamela");
}

public void Statement() {
        Console.WriteLine("Statement");
        Comentamela();
        switch (CurrentToken) {
        case TokenCategory.IDENTIFIER:
                Identificamela();
                break;

        case TokenCategory.PRINT:
                Print();
                break;

        case TokenCategory.IF:
                If();
                break;

        case TokenCategory.SWITCH:
                Switcheamela();
                break;


        case TokenCategory.WHILE:
                Whileamela();
                break;


        case TokenCategory.DO:
                DoWhileamela();
                break;


        case TokenCategory.FOR:
                Foreamesto();
                break;

        case TokenCategory.COMMENT:
                Expect(TokenCategory.COMMENT);
                break;

        default:
                // throw new SyntaxError(firstOfStatement, tokenStream.Current);
                break;
        }
        Comentamela();
        Console.WriteLine("sali de statement");
}


public void caseList() {
  Console.WriteLine("caseList");
        Comentamela();
        switch (CurrentToken) {

        case TokenCategory.IDENTIFIER:
                Identificamela();
                break;


        default:
                // throw new SyntaxError(firstOfStatement, tokenStream.Current);
                break;
        }
        Comentamela();
        Console.WriteLine("sali de case list");
}

public void Type() {
        switch (CurrentToken) {

        case TokenCategory.INT:
                Expect(TokenCategory.INT);
                break;

        case TokenCategory.BOOL:
                Expect(TokenCategory.BOOL);
                break;

        default:
                throw new SyntaxError(firstOfDeclaration,
                                      tokenStream.Current);
        }
}

public void Assignment() {
        Console.WriteLine("Assignment");
        Expect(TokenCategory.ASSIGN);
        // if(CurrentToken == TokenCategory.IDENTIFIER){
        //   Identificamela();
        // }else if (CurrentToken == TokenCategory.SEMICOLON) {
        //   Finisher();
        //   return;
        // }else {
        //   Expression();
        // }

        while(CurrentToken != TokenCategory.SEMICOLON)
        {
          if(CurrentToken == TokenCategory.IDENTIFIER){
            Identificamela();
          }else if(CurrentToken == TokenCategory.SEMICOLON) {
            Finisher();
            return;
          }
          else {
            Console.WriteLine("-------pene");
            Expression();
            Console.WriteLine("vagina -----------------");
          }
        }

        if (CurrentToken == TokenCategory.SEMICOLON) {
          Finisher();
        }
        Console.WriteLine("sali de Assignment");
}

public void Print() {
        Expect(TokenCategory.PRINT);
        Expression();
}

public void If() {
        Console.WriteLine("entre a if");
        Expect(TokenCategory.IF);
        Expect(TokenCategory.PARENTHESIS_OPEN);
        Expression();
        KUZ:
        if(CurrentToken == TokenCategory.PARENTHESIS_OPEN)
        {
          Funcionamela();
        }
        if(CurrentToken == TokenCategory.PARENTHESIS_OPEN)
        {
          Comentamela();
          goto KUZ;
        }
        while(CurrentToken != TokenCategory.PARENTHESIS_CLOSE)
        {
          Expression();
        }
        Expect(TokenCategory.PARENTHESIS_CLOSE);
        Expect(TokenCategory.CURLY_OPEN);
        Comentamela();
        while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
        }
        Returneamela();
        Breakeamela();
        Continuamela();
        Expect(TokenCategory.CURLY_CLOSE);
        if(CurrentToken == TokenCategory.ELSEIF) {
                Comentamela();
                RecursiveameEnElIf();
        }
        if (CurrentToken == TokenCategory.ELSE) {
                Expect(TokenCategory.ELSE);
                Expect(TokenCategory.CURLY_OPEN);
                Comentamela();
                while (firstOfStatement.Contains(CurrentToken)) {
                  Comentamela();
                        Statement();
                }
                Returneamela();
                Breakeamela();
                Continuamela();
                Expect(TokenCategory.CURLY_CLOSE);
        }
        Console.WriteLine("sali de if");
}

public void RecursiveameEnElIf()
{
        Console.WriteLine("RecursiveameEnElIf");
        Expect(TokenCategory.ELSEIF);
        Expect(TokenCategory.PARENTHESIS_OPEN);
        DeclarationContinuer();
        Expect(TokenCategory.PARENTHESIS_CLOSE);
        Expect(TokenCategory.CURLY_OPEN);
        Comentamela();
        while (firstOfStatement.Contains(CurrentToken)) {
          Comentamela();
                Statement();
        }
        Returneamela();
        Breakeamela();
        Continuamela();
        Expect(TokenCategory.CURLY_CLOSE);
        if(CurrentToken == TokenCategory.ELSEIF)
        {
                RecursiveameEnElIf();
        }
        Console.WriteLine("sali de RecursiveameEnElIf");
}

public void Inlinemeamela()
{
  Console.WriteLine("Inlinemeamela");
  Expect(TokenCategory.INLINEIF);
  Expression();
  Expect(TokenCategory.COLON);
  Expression();
  Console.WriteLine("sali de Inlinemeamela");
}

public void Switcheamela() {
        Console.WriteLine("Switcheamela");
        Expect(TokenCategory.SWITCH);
        Expect(TokenCategory.PARENTHESIS_OPEN);
        Expect(TokenCategory.IDENTIFIER);
        if(CurrentToken != TokenCategory.PARENTHESIS_OPEN){
          Expression();
        }else{
          Funcionamela();
        }
        Expect(TokenCategory.PARENTHESIS_CLOSE);
        Expect(TokenCategory.CURLY_OPEN);
        Comentamela();

        while (switchSimpleExpression.Contains(CurrentToken)) {

                Expect(TokenCategory.CASE);
                BEGIN:
                switchExpresion();
                if(CurrentToken == TokenCategory.COMMA)
                {
                        Expect(TokenCategory.COMMA);
                        goto BEGIN;
                }
                Expect(TokenCategory.COLON);
                while (firstOfStatement.Contains(CurrentToken)) {
                        Statement();
                }
                Returneamela();
                Breakeamela();
                Continuamela();
        }

        Expect(TokenCategory.DEFAULT);
        Expect(TokenCategory.COLON);

        while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
        }

        Returneamela();
        Breakeamela();
        Continuamela();
        Expect(TokenCategory.CURLY_CLOSE);
        Console.WriteLine("sali de Switcheamela");
}

public void Whileamela() {
        Console.WriteLine("Whileamela");
        Expect(TokenCategory.WHILE);
        Expect(TokenCategory.PARENTHESIS_OPEN);
        Expression();
        Expect(TokenCategory.PARENTHESIS_CLOSE);
        Expect(TokenCategory.CURLY_OPEN);

        while (CurrentToken != TokenCategory.CURLY_CLOSE) {
                Comentamela();
                Declaration();
                Statement();
        }
        Returneamela();
        Breakeamela();
        Continuamela();
        Expect(TokenCategory.CURLY_CLOSE);
        Console.WriteLine("sali de Whileamela");
}


public void DoWhileamela() {
        Console.WriteLine("DoWhileamela");
        Expect(TokenCategory.DO);
        Expect(TokenCategory.CURLY_OPEN);
        while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
        }
        Returneamela();
        Breakeamela();
        Continuamela();
        Expect(TokenCategory.CURLY_CLOSE);
        Expect(TokenCategory.WHILE);
        Expect(TokenCategory.PARENTHESIS_OPEN);
        Expression();
        Expect(TokenCategory.PARENTHESIS_CLOSE);
        Expect(TokenCategory.SEMICOLON);
        Console.WriteLine("sali de DoWhileamela");
}


public void Foreamesto() {
        Console.WriteLine("Foreamesto");
        Expect(TokenCategory.FOR);
        Expect(TokenCategory.PARENTHESIS_OPEN);
        Expect(TokenCategory.IDENTIFIER);
        Expect(TokenCategory.IN);

        Identificamela();

        Expect(TokenCategory.PARENTHESIS_CLOSE);
        Expect(TokenCategory.CURLY_OPEN);
        while (firstOfStatement.Contains(CurrentToken)) {
          Comentamela();
                Statement();
        }
        Returneamela();
        Breakeamela();
        Continuamela();
        Expect(TokenCategory.CURLY_CLOSE);
        Console.WriteLine("sali de Foreamesto");
}


public void Expression() {
        Console.WriteLine("Expression");
        Comentamela();
        SimpleExpression();
        while (firstOfOperator.Contains(CurrentToken)) {
                Operator();
                SimpleExpression();
        }
        Console.WriteLine("sali de Expression");
}

public void switchExpresion() {
  Console.WriteLine("switchExpresion");
        switch (CurrentToken) {

        case TokenCategory.INT:
                Expect(TokenCategory.INT);
                break;

        case TokenCategory.CHAR:
                Expect(TokenCategory.CHAR);
                break;


        case TokenCategory.TRUE:
                Expect(TokenCategory.TRUE);
                break;

        case TokenCategory.FALSE:
                Expect(TokenCategory.FALSE);
                break;

        default:
                throw new SyntaxError(litSimple,
                                      tokenStream.Current);
        }
        Console.WriteLine("sali de switchExpresion");
}

public void SimpleExpression() {
        Console.WriteLine("SimpleExpression");
        switch (CurrentToken) {

        case TokenCategory.IDENTIFIER:
                Expect(TokenCategory.IDENTIFIER);
                break;

        case TokenCategory.INT:
                Expect(TokenCategory.INT);
                break;

        case TokenCategory.BIN:
                Expect(TokenCategory.BIN);
                break;

        case TokenCategory.CHAR:
                Expect(TokenCategory.CHAR);
                break;

        case TokenCategory.STRING:
                Expect(TokenCategory.STRING);
                break;

        case TokenCategory.OCT:
                Expect(TokenCategory.OCT);
                break;

        case TokenCategory.HEX:
                Expect(TokenCategory.HEX);
                break;

        case TokenCategory.TRUE:
                Expect(TokenCategory.TRUE);
                break;

        case TokenCategory.FALSE:
                Expect(TokenCategory.FALSE);
                break;

        case TokenCategory.PARENTHESIS_OPEN:
                Expect(TokenCategory.PARENTHESIS_OPEN);
                Expression();
                Expect(TokenCategory.PARENTHESIS_CLOSE);
                break;

        case TokenCategory.CURLY_OPEN:
                //Expect(TokenCategory.CURLY_OPEN);
                Arreglamela();
                //Expect(TokenCategory.CURLY_CLOSE);
                break;

        case TokenCategory.NEG:
                Expect(TokenCategory.NEG);
                SimpleExpression();
                break;

        case TokenCategory.PLUS:
                Expect(TokenCategory.PLUS);
                SimpleExpression();
                break;

        case TokenCategory.POWER:
                Expect(TokenCategory.POWER);
                SimpleExpression();
                break;

        case TokenCategory.BINNOTT:
                Expect(TokenCategory.BINNOTT);
                SimpleExpression();
                break;

        case TokenCategory.INLINEIF:
                Inlinemeamela();
                break;

        case TokenCategory.NOTT:
              Expect(TokenCategory.NOTT);
              switch(CurrentToken){
                case TokenCategory.TRUE:
                    Expect(TokenCategory.TRUE);
                    break;

                case TokenCategory.FALSE:
                    Expect(TokenCategory.FALSE);
                    break;

                case TokenCategory.IDENTIFIER:
                    Expect(TokenCategory.IDENTIFIER);
                    break;

                case TokenCategory.NOTT:
                      Expect(TokenCategory.NOTT);
                      break;

                default:
                    throw new SyntaxError(TokenCategory.IDENTIFIER,
                                       tokenStream.Current);
              }
            break;

        default:
            if (firstOfOperator.Contains(CurrentToken)) {
              Operator();
            }else{
                throw new SyntaxError(firstOfSimpleExpression,
                                      tokenStream.Current);
                }
                break;
        }
        Console.WriteLine("sali de SimpleExpression");
}

public void Operator() {
        Console.WriteLine("Operator");
        switch (CurrentToken) {

        case TokenCategory.AND:
                Expect(TokenCategory.AND);
                break;

        case TokenCategory.INLINEIF:
                Inlinemeamela();
                break;
        case TokenCategory.LESS:
                Expect(TokenCategory.LESS);
                break;

        case  TokenCategory.PLUS:
                Expect(TokenCategory.PLUS);
                break;

        case TokenCategory.MUL:
                Expect(TokenCategory.MUL);
                break;

        case TokenCategory.NEG:
                Expect(TokenCategory.NEG);
                break;

        case TokenCategory.NOMOR:
                Expect(TokenCategory.NOMOR);
                break;

        case TokenCategory.BITOR:
                Expect(TokenCategory.BITOR);
                break;

        case TokenCategory.NOMAND:
                Expect(TokenCategory.NOMAND);
                break;

        case TokenCategory.BITAND:
                Expect(TokenCategory.BITAND);
                break;

        case TokenCategory.EQCOMPARE:
                Expect(TokenCategory.EQCOMPARE);
                break;

        case TokenCategory.NOTEQ:
                Expect(TokenCategory.NOTEQ);
                break;

        case TokenCategory.MORE:
                Expect(TokenCategory.MORE);
                break;

        case TokenCategory.EQMORE:
                Expect(TokenCategory.EQMORE);
                break;

        case TokenCategory.EQLESS:
                Expect(TokenCategory.EQLESS);
                break;

        case TokenCategory.MOD:
                Expect(TokenCategory.MOD);
                break;

        case TokenCategory.DIV:
              Expect(TokenCategory.DIV);
              break;

        case TokenCategory.BINNOTT:
              Expect(TokenCategory.BINNOTT);
              break;

        case TokenCategory.NOTT:
              Expect(TokenCategory.NOTT);
              switch(CurrentToken){
                case TokenCategory.TRUE:
                    Expect(TokenCategory.TRUE);
                    break;

                case TokenCategory.FALSE:
                    Expect(TokenCategory.FALSE);
                    break;

                case TokenCategory.IDENTIFIER:
                    Expect(TokenCategory.IDENTIFIER);
                    break;

                case TokenCategory.NOTT:
                      Expect(TokenCategory.NOTT);
                      break;

                default:
                    throw new SyntaxError(TokenCategory.IDENTIFIER,
                                       tokenStream.Current);
              }
              break;

        case TokenCategory.POWER:
              Expect(TokenCategory.POWER);
              break;

        case TokenCategory.SHIFTLEFT:
              Expect(TokenCategory.SHIFTLEFT);
              break;

        case TokenCategory.SHIFTRIGHT:
              Expect(TokenCategory.SHIFTRIGHT);
              break;

        case TokenCategory.TRIPLESHIFT:
              Expect(TokenCategory.TRIPLESHIFT);
              break;

        default:
                throw new SyntaxError(firstOfOperator,
                                      tokenStream.Current);
        }
        Console.WriteLine("sali de oper");
}
}
}
