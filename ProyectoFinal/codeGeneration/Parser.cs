/*
  Jaime Margolin A01019332
  Juan carlos Leon A01020200
  Rodrigo Solana A01129839
*/

using System;
using System.Collections.Generic;

namespace Int64 {

	class Parser {

		static readonly ISet<TokenCategory> firstOfProgram =
		new HashSet<TokenCategory>() {
			TokenCategory.IDENTIFIER,
			TokenCategory.VAR,
			TokenCategory.EOF
		};

		static readonly ISet<TokenCategory> firstOfStatement =
		new HashSet<TokenCategory>() {
			TokenCategory.IDENTIFIER,
			TokenCategory.IF,
			TokenCategory.SWITCH,
			TokenCategory.WHILE,
			TokenCategory.DO,
			TokenCategory.FOR,
			TokenCategory.BREAK,
			TokenCategory.CONTINUE,
			TokenCategory.RETURN,
			TokenCategory.SEMICOLON
		};

		static readonly ISet<TokenCategory> firstOfUnary =
		new HashSet<TokenCategory>() {
			TokenCategory.ADDITION,
			TokenCategory.SUBTRACTION,
			TokenCategory.BIT_NOT,
			TokenCategory.NOT
		};

		static readonly ISet<TokenCategory> firstOfLit =
		new HashSet<TokenCategory>() {
			TokenCategory.TRUE,
			TokenCategory.FALSE,
			TokenCategory.BASE_2,
			TokenCategory.BASE_8,
			TokenCategory.BASE_10,
			TokenCategory.BASE_16,
			TokenCategory.CHARACTER
		};

		static readonly ISet<TokenCategory> firstOfLitAlt =
		new HashSet<TokenCategory>() {
			TokenCategory.TRUE,
			TokenCategory.FALSE,
			TokenCategory.BASE_2,
			TokenCategory.BASE_8,
			TokenCategory.BASE_10,
			TokenCategory.BASE_16,
			TokenCategory.CHARACTER,
			TokenCategory.STRING,
			TokenCategory.CURLY_BRACE_LEFT
		};

		static readonly ISet<TokenCategory> firstOfExprPrimary =
		new HashSet<TokenCategory>() {
			TokenCategory.IDENTIFIER,
			TokenCategory.TRUE,
			TokenCategory.FALSE,
			TokenCategory.BASE_2,
			TokenCategory.BASE_8,
			TokenCategory.BASE_10,
			TokenCategory.BASE_16,
			TokenCategory.CHARACTER,
			TokenCategory.STRING,
			TokenCategory.CURLY_BRACE_LEFT,
			TokenCategory.PARENTHESIS_LEFT
		};

		IEnumerator<Token> tokenStream;

		public Parser(IEnumerator<Token> tokenStream) {
			this.tokenStream = tokenStream;
			this.tokenStream.MoveNext();
		}

		public TokenCategory CurrentToken {
			get { return tokenStream.Current.Category; }
		}

		public Token Expect(TokenCategory category) {
			if (Environment.GetEnvironmentVariable("VERBOSE") == "true") {
				Console.WriteLine("Expecting: " + tokenStream.Current.Lexeme);
				Console.WriteLine(System.Environment.StackTrace);
			}
			if (CurrentToken == category) {
				if (Environment.GetEnvironmentVariable("verbose") == "true") {
					Console.WriteLine("Consuming: " + tokenStream.Current.Lexeme);
				}
				Token current = tokenStream.Current;
				tokenStream.MoveNext();
				return current;
			} else {
				if (Environment.GetEnvironmentVariable("verbose") == "true") {
					Console.WriteLine("Not consuming: " + tokenStream.Current.Lexeme);
				}
				throw new SyntaxError(category, tokenStream.Current);
			}
		}

		// Grammar entry point
		// Returns NProgram
		public Node CProgram() {
			NProgram nProgram = (NProgram)Program();
			Expect(TokenCategory.EOF);
			return nProgram;
		}

		// Returns NProgram
		public Node Program() {
			NProgram nProgram = new NProgram();
			NVarDefList nVarDefList = new NVarDefList();
			NFunDefList nFunDefList = new NFunDefList();
			while (firstOfProgram.Contains(CurrentToken)) {
				if (CurrentToken == TokenCategory.VAR) {
					VarDef(nVarDefList);
				}
				else if (CurrentToken == TokenCategory.IDENTIFIER) {
					FunDef(nFunDefList);
				}
				else if (CurrentToken == TokenCategory.EOF) {
					break;
				}
				else {
					throw new SyntaxError(firstOfProgram, tokenStream.Current);
				}
			}
			if (!firstOfProgram.Contains(CurrentToken)) {
				throw new SyntaxError(new HashSet<TokenCategory>() {
					TokenCategory.IDENTIFIER,
					TokenCategory.VAR,
					TokenCategory.EOF
				}
				, tokenStream.Current);
			}
			nProgram.Add(nVarDefList);
			nProgram.Add(nFunDefList);
			return nProgram;
		}

		// Appends NVarDef to nVarDefList
		public void VarDef(NVarDefList nVarDefList) {
			NVarDef nVarDef = new NVarDef();
			Expect(TokenCategory.VAR);
			nVarDef.AnchorToken = Expect(TokenCategory.IDENTIFIER);
			nVarDefList.Add(nVarDef);
			while (CurrentToken==TokenCategory.COMMA) {
				Expect(TokenCategory.COMMA);
				NVarDef otherNVarDef = new NVarDef();
				otherNVarDef.AnchorToken = Expect(TokenCategory.IDENTIFIER);
				nVarDefList.Add(otherNVarDef);
			}
			Expect(TokenCategory.SEMICOLON);
		}

		// Appends NFunDef to nFunDefList
		public void FunDef(NFunDefList nFunDefList) {
			NFunDef nFunDef = new NFunDef(){
				AnchorToken = Expect(TokenCategory.IDENTIFIER)
			};
			NParameterList nParameterList = new NParameterList();
			Expect(TokenCategory.PARENTHESIS_LEFT);
			if (CurrentToken==TokenCategory.IDENTIFIER) {
				nParameterList.Add(new NParameter(){
					AnchorToken = Expect(TokenCategory.IDENTIFIER)
				});
				while (CurrentToken==TokenCategory.COMMA) {
					Expect(TokenCategory.COMMA);
					nParameterList.Add(new NParameter(){
						AnchorToken = Expect(TokenCategory.IDENTIFIER)
					});
				}
			}
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			NVarDefList nVarDefList = (NVarDefList)VarDefList();
			NStmtList nStmtList = (NStmtList)StmtList();
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			nFunDef.Add(nParameterList);
			nFunDef.Add(nVarDefList);
			nFunDef.Add(nStmtList);
			nFunDefList.Add(nFunDef);
		}

		// Returns NVarDefList
		public Node VarDefList() {
			NVarDefList nVarDefList = new NVarDefList();
			while (CurrentToken == TokenCategory.VAR) {
				VarDef(nVarDefList);
			}
			return nVarDefList;
		}

		// Returns NStmtList
		public Node StmtList() {
			NStmtList nStmtList = new NStmtList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Stmt(nStmtList);
			}
			return nStmtList;
		}

		// Appends NStmt to nStmtList
		public void Stmt(NStmtList nStmtList) {
			switch(CurrentToken) {
				case TokenCategory.IDENTIFIER: {
					Token tokenForAnchor = Expect(TokenCategory.IDENTIFIER);
					if (CurrentToken==TokenCategory.ASSIGN) {
						NAssign nAssign = new NAssign(){
							AnchorToken = tokenForAnchor
						};
						nAssign.Add(Assign());
						nStmtList.Add(nAssign);
					}
					else if (CurrentToken==TokenCategory.PARENTHESIS_LEFT) {
						NFunCall nFunCall = new NFunCall(){
							AnchorToken = tokenForAnchor
						};
						nFunCall.Add(FunCall());
						nStmtList.Add(nFunCall);
					}
					else {
						throw new SyntaxError(new HashSet<TokenCategory>() {
							TokenCategory.ASSIGN,
							TokenCategory.PARENTHESIS_LEFT
						}
						, tokenStream.Current);
					}
					break;
				}
				case TokenCategory.IF: {
					Expect(TokenCategory.IF);
					nStmtList.Add(IfStmt());
					break;
				}
				case TokenCategory.SWITCH: {
					nStmtList.Add(SwitchStmt());
					break;
				}
				case TokenCategory.WHILE: {
					nStmtList.Add(WhileStmt());
					break;
				}
				case TokenCategory.DO: {
					nStmtList.Add(DoWhileStmt());
					break;
				}
				case TokenCategory.FOR: {
					nStmtList.Add(ForStmt());
					break;
				}
				case TokenCategory.BREAK: {
					nStmtList.Add(new NBreak() {
						AnchorToken = Expect(TokenCategory.BREAK)
					});
					Expect(TokenCategory.SEMICOLON);
					break;
				}
				case TokenCategory.CONTINUE: {
					nStmtList.Add(new NContinue() {
						AnchorToken = Expect(TokenCategory.CONTINUE)
					});
					Expect(TokenCategory.SEMICOLON);
					break;
				}
				case TokenCategory.RETURN: {
					Expect(TokenCategory.RETURN);
					nStmtList.Add(new NReturn(){
						Expr()
					});
					Expect(TokenCategory.SEMICOLON);
					break;
				}
				case TokenCategory.SEMICOLON: {
					Expect(TokenCategory.SEMICOLON);
					break;
				}
				default: {
					throw new SyntaxError(firstOfStatement, tokenStream.Current);
				}
			}
		}

		// Returns NAssign
		public Node Assign() {
			Expect(TokenCategory.ASSIGN);
			Node nExpr = Expr();
			Expect(TokenCategory.SEMICOLON);
			return nExpr;
		}

		// Returns NExprList
		public Node FunCall() {
			NExprList nExprList = new NExprList();
			Expect(TokenCategory.PARENTHESIS_LEFT);
			if (firstOfExprPrimary.Contains(CurrentToken) || firstOfUnary.Contains(CurrentToken)) {
				nExprList.Add(Expr());
				while (CurrentToken == TokenCategory.COMMA) {
					Expect(TokenCategory.COMMA);
					nExprList.Add(Expr());
				}
			}
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			return nExprList;
		}

		// Returns NIfStmt
		public Node IfStmt() {
			NIfStmt nIfStmt = new NIfStmt();
			Expect(TokenCategory.PARENTHESIS_LEFT);
			nIfStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			NStmtList nStmtList = new NStmtList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Stmt(nStmtList);
			}
			nIfStmt.Add(nStmtList);
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			while (CurrentToken==TokenCategory.ELSE) {
				Expect(TokenCategory.ELSE);
				if (CurrentToken==TokenCategory.IF) {
					Expect(TokenCategory.IF);
					Expect(TokenCategory.PARENTHESIS_LEFT);
					nIfStmt.Add(Expr());
					Expect(TokenCategory.PARENTHESIS_RIGHT);
					Expect(TokenCategory.CURLY_BRACE_LEFT);
					NStmtList otherNStmtList = new NStmtList();
					while (firstOfStatement.Contains(CurrentToken)) {
						Stmt(otherNStmtList);
					}
					nIfStmt.Add(otherNStmtList);
					Expect(TokenCategory.CURLY_BRACE_RIGHT);
				}
				else if (CurrentToken==TokenCategory.CURLY_BRACE_LEFT) {
					Expect(TokenCategory.CURLY_BRACE_LEFT);
					NStmtList otherNStmtList = new NStmtList();
					while (firstOfStatement.Contains(CurrentToken)) {
						Stmt(otherNStmtList);
					}
					nIfStmt.Add(otherNStmtList);
					Expect(TokenCategory.CURLY_BRACE_RIGHT);
					break;
				}
			}
			return nIfStmt;
		}

		// Return NSwitchStmt
		public Node SwitchStmt() {
			NSwitchStmt nSwitchStmt = new NSwitchStmt();
			Expect(TokenCategory.SWITCH);
			Expect(TokenCategory.PARENTHESIS_LEFT);
			nSwitchStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			NCaseList nCaseList = new NCaseList();
			while (CurrentToken==TokenCategory.CASE) {
				nCaseList.Add(Case());
			}
			nSwitchStmt.Add(nCaseList);
			if (CurrentToken==TokenCategory.DEFAULT) {
				nSwitchStmt.Add(Default());
			}
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			return nSwitchStmt;
		}

		//Returns NCase
		public Node Case() {
			NCase nCase = new NCase();
			Expect(TokenCategory.CASE);
			nCase.Add(Lit());
			while (CurrentToken==TokenCategory.COMMA) {
				Expect(TokenCategory.COMMA);
				nCase.Add(Lit());
			}
			Expect(TokenCategory.COLON);
			NStmtList nStmtList = new NStmtList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Stmt(nStmtList);
			}
			nCase.Add(nStmtList);
			return nCase;
		}

		// Returns NLitInt, NLitBool or NLitChar
		public Node Lit() {
			switch(CurrentToken) {
				case TokenCategory.TRUE: {
					return new NLitBool(){
						AnchorToken = Expect(TokenCategory.TRUE)
					};
				}
				case TokenCategory.FALSE: {
					return new NLitBool(){
						AnchorToken = Expect(TokenCategory.FALSE)
					};
				}
				case TokenCategory.BASE_2: {
					return new NLitInt(){
						AnchorToken = Expect(TokenCategory.BASE_2)
					};
				}
				case TokenCategory.BASE_8: {
					return new NLitInt(){
						AnchorToken = Expect(TokenCategory.BASE_8)
					};
				}
				case TokenCategory.BASE_10: {
					return new NLitInt(){
						AnchorToken = Expect(TokenCategory.BASE_10)
					};
				}
				case TokenCategory.BASE_16: {
					return new NLitInt(){
						AnchorToken = Expect(TokenCategory.BASE_16)
					};
				}
				case TokenCategory.CHARACTER: {
					return new NLitChar(){
						AnchorToken = Expect(TokenCategory.CHARACTER)
					};
				}
				default: {
					throw new SyntaxError(new HashSet<TokenCategory>() {
						TokenCategory.TRUE,
						TokenCategory.FALSE,
						TokenCategory.BASE_2,
						TokenCategory.BASE_8,
						TokenCategory.BASE_10,
						TokenCategory.BASE_16,
						TokenCategory.CHARACTER
					}
					, tokenStream.Current);
				}
			}
		}

		// Returns NStmtList
		public Node Default() {
			NStmtList nStmtList = new NStmtList();
			Expect(TokenCategory.DEFAULT);
			Expect(TokenCategory.COLON);
			while (firstOfStatement.Contains(CurrentToken)) {
				Stmt(nStmtList);
			}
			return nStmtList;
		}

		// Returns NWhileStmt
		public Node WhileStmt() {
			NWhileStmt nWhileStmt = new NWhileStmt();
			Expect(TokenCategory.WHILE);
			Expect(TokenCategory.PARENTHESIS_LEFT);
			nWhileStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			NStmtList nStmtList = new NStmtList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Stmt(nStmtList);
			}
			nWhileStmt.Add(nStmtList);
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			return nWhileStmt;
		}

		// Returns NDoWhileStmt
		public Node DoWhileStmt() {
			NDoWhileStmt nDoWhileStmt = new NDoWhileStmt();
			Expect(TokenCategory.DO);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			NStmtList nStmtList = new NStmtList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Stmt(nStmtList);
			}
			nDoWhileStmt.Add(nStmtList);
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			Expect(TokenCategory.WHILE);
			Expect(TokenCategory.PARENTHESIS_LEFT);
			nDoWhileStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.SEMICOLON);
			return nDoWhileStmt;
		}

		// Returns NForStmt
		public Node ForStmt() {
			Expect(TokenCategory.FOR);
			Expect(TokenCategory.PARENTHESIS_LEFT);
			NForStmt nForStmt = new NForStmt() {
				AnchorToken = Expect(TokenCategory.IDENTIFIER)
			};
			Expect(TokenCategory.IN);
			nForStmt.Add(Expr());
			Expect(TokenCategory.PARENTHESIS_RIGHT);
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			NStmtList nStmtList = new NStmtList();
			while (firstOfStatement.Contains(CurrentToken)) {
				Stmt(nStmtList);
			}
			nForStmt.Add(nStmtList);
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			return nForStmt;
		}

		// Returns NExpr or any of subproduction nodes
		public Node Expr() {
			Node resultingNode = ExprOr();
			if (CurrentToken == TokenCategory.QUESTION_MARK) {
				NExpr tmp = new NExpr();
				tmp.Add(resultingNode);
				Expect(TokenCategory.QUESTION_MARK);
				tmp.Add(Expr());
				Expect(TokenCategory.COLON);
				tmp.Add(Expr());
				resultingNode = tmp;
			}
			return resultingNode;
		}

		// Returns NExprOr or any of subproduction nodes
		public Node ExprOr() {
			Node resultingNode = ExprAnd();
			while (CurrentToken == TokenCategory.OR) {
				NExprOr tmp = new NExprOr();
				tmp.Add(resultingNode);
				resultingNode = tmp;
				Expect(TokenCategory.OR);
				resultingNode.Add(ExprAnd());
			}
			return resultingNode;
		}

		// Returns NExprAnd or any of subproduction nodes
		public Node ExprAnd() {
			Node resultingNode = ExprComp();
			while (CurrentToken == TokenCategory.AND) {
				NExprAnd tmp = new NExprAnd();
				tmp.Add(resultingNode);
				resultingNode = tmp;
				Expect(TokenCategory.AND);
				resultingNode.Add(ExprComp());
			}
			return resultingNode;
		}

		// Returns NExprComp or any of subproduction nodes
		public Node ExprComp() {
			Node resultingNode = ExprRel();
			while (CurrentToken == TokenCategory.NOT_EQUAL || CurrentToken == TokenCategory.EQUAL) {
				NExprComp tmp = new NExprComp();
				tmp.Add(resultingNode);
				resultingNode = tmp;
				switch(CurrentToken) {
					case TokenCategory.NOT_EQUAL: {
						resultingNode.AnchorToken = Expect(TokenCategory.NOT_EQUAL);
						resultingNode.Add(ExprRel());
						break;
					}
					case TokenCategory.EQUAL: {
						resultingNode.AnchorToken = Expect(TokenCategory.EQUAL);
						resultingNode.Add(ExprRel());
						break;
					}
				}
			}
			return resultingNode;
		}

		// Returns NExprRel or any of subproduction nodes
		public Node ExprRel() {
			Node resultingNode = ExprBitOr();
			while (CurrentToken == TokenCategory.LESS_THAN || CurrentToken == TokenCategory.LESS_OR_EQUAL_THAN || CurrentToken == TokenCategory.GREATER_THAN || CurrentToken == TokenCategory.GREATER_OR_EQUAL_THAN) {
				NExprRel tmp = new NExprRel();
				tmp.Add(resultingNode);
				resultingNode = tmp;
				switch(CurrentToken) {
					case TokenCategory.LESS_THAN: {
						resultingNode.AnchorToken = Expect(TokenCategory.LESS_THAN);
						resultingNode.Add(ExprBitOr());
						break;
					}
					case TokenCategory.LESS_OR_EQUAL_THAN: {
						resultingNode.AnchorToken = Expect(TokenCategory.LESS_OR_EQUAL_THAN);
						resultingNode.Add(ExprBitOr());
						break;
					}
					case TokenCategory.GREATER_THAN: {
						resultingNode.AnchorToken = Expect(TokenCategory.GREATER_THAN);
						resultingNode.Add(ExprBitOr());
						break;
					}
					case TokenCategory.GREATER_OR_EQUAL_THAN: {
						resultingNode.AnchorToken = Expect(TokenCategory.GREATER_OR_EQUAL_THAN);
						resultingNode.Add(ExprBitOr());
						break;
					}
				}
			}
			return resultingNode;
		}

		// Returns NExprBitOr or any of subproduction nodes
		public Node ExprBitOr() {
			Node resultingNode =  ExprBitAnd();
			while (CurrentToken == TokenCategory.BIT_OR || CurrentToken == TokenCategory.XOR) {
				NExprBitOr tmp = new NExprBitOr();
				tmp.Add(resultingNode);
				resultingNode = tmp;
				switch(CurrentToken) {
					case TokenCategory.BIT_OR: {
						resultingNode.AnchorToken = Expect(TokenCategory.BIT_OR);
						resultingNode.Add(ExprBitAnd());
						break;
					}
					case TokenCategory.XOR: {
						resultingNode.AnchorToken = Expect(TokenCategory.XOR);
						resultingNode.Add(ExprBitAnd());
						break;
					}
					// Default case would be unreachable
				}
			}
			return resultingNode;
		}

		// Returns NExprBitAnd or any of subproduction nodes
		public Node ExprBitAnd() {
			Node resultingNode =  ExprBitShift();
			while (CurrentToken == TokenCategory.BIT_AND) {
				NExprBitAnd tmp = new NExprBitAnd();
				tmp.Add(resultingNode);
				resultingNode = tmp;
				Expect(TokenCategory.BIT_AND);
				resultingNode.Add(ExprBitShift());
			}
			return resultingNode;
		}

		// Returns NExprBitShift or any of subproduction nodes
		public Node ExprBitShift() {
			Node resultingNode =  ExprAdd();
			while (CurrentToken == TokenCategory.SHIFT_LEFT || CurrentToken == TokenCategory.SHIFT_RIGHT || CurrentToken == TokenCategory.SHIFT_RIGHT_ALT) {
				NExprBitShift tmp = new NExprBitShift();
				tmp.Add(resultingNode);
				resultingNode = tmp;
				switch(CurrentToken) {
					case TokenCategory.SHIFT_LEFT: {
						resultingNode.AnchorToken = Expect(TokenCategory.SHIFT_LEFT);
						resultingNode.Add(ExprAdd());
						break;
					}
					case TokenCategory.SHIFT_RIGHT: {
						resultingNode.AnchorToken = Expect(TokenCategory.SHIFT_RIGHT);
						resultingNode.Add(ExprAdd());
						break;
					}
					case TokenCategory.SHIFT_RIGHT_ALT: {
						resultingNode.AnchorToken = Expect(TokenCategory.SHIFT_RIGHT_ALT);
						resultingNode.Add(ExprAdd());
						break;
					}
					// Default case would be unreachable
				}
			}
			return resultingNode;
		}

		// Returns NExprAdd or any of subproduction nodes
		public Node ExprAdd() {
			Node resultingNode = ExprMul();
			while (CurrentToken==TokenCategory.SUBTRACTION||CurrentToken==TokenCategory.ADDITION) {
				NExprAdd tmp = new NExprAdd();
				tmp.Add(resultingNode);
				resultingNode = tmp;
				switch(CurrentToken) {
					case TokenCategory.SUBTRACTION: {
						resultingNode.AnchorToken = Expect(TokenCategory.SUBTRACTION);
						resultingNode.Add(ExprMul());
						break;
					}
					case TokenCategory.ADDITION: {
						resultingNode.AnchorToken = Expect(TokenCategory.ADDITION);
						resultingNode.Add(ExprMul());
						break;
					}
					// Default case would be unreachable
				}
			}
			return resultingNode;
		}

		// Returns NExprMul or any of subproduction nodes
		public Node ExprMul() {
			Node resultingNode = ExprPow();
			while (CurrentToken == TokenCategory.MULTIPLICATION || CurrentToken == TokenCategory.DIVISION || CurrentToken == TokenCategory.MODULUS) {
				NExprMul tmp = new NExprMul();
				tmp.Add(resultingNode);
				resultingNode = tmp;
				switch(CurrentToken) {
					case TokenCategory.MULTIPLICATION: {
						resultingNode.AnchorToken = Expect(TokenCategory.MULTIPLICATION);
						resultingNode.Add(ExprPow());
						break;
					}
					case TokenCategory.DIVISION: {
						resultingNode.AnchorToken = Expect(TokenCategory.DIVISION);
						resultingNode.Add(ExprPow());
						break;
					}
					case TokenCategory.MODULUS: {
						resultingNode.AnchorToken = Expect(TokenCategory.MODULUS);
						resultingNode.Add(ExprPow());
						break;
					}
					// Default case would be unreachable
				}
			}
			return resultingNode;
		}

		// Returns NExprPow or any of subproduction nodes
		public Node ExprPow() {
			Node resultingNode = ExprUnary();
			if (CurrentToken == TokenCategory.POWER) {
				NExprPow tmp = new NExprPow();
				tmp.Add(resultingNode);
				resultingNode = tmp;
				Expect(TokenCategory.POWER);
				resultingNode.Add(ExprPow());
			}
			return resultingNode;
		}

		// Returns NExprUnary or any of subproduction nodes
		public Node ExprUnary() {
			NExprUnary nExprUnary = new NExprUnary();
			if (firstOfUnary.Contains(CurrentToken)) {
				switch(CurrentToken) {
					case TokenCategory.ADDITION: {
						nExprUnary.AnchorToken = Expect(TokenCategory.ADDITION);
						nExprUnary.Add(ExprUnary());
						break;
					}
					case TokenCategory.SUBTRACTION: {
						nExprUnary.AnchorToken = Expect(TokenCategory.SUBTRACTION);
						nExprUnary.Add(ExprUnary());
						break;
					}
					case TokenCategory.NOT: {
						nExprUnary.AnchorToken = Expect(TokenCategory.NOT);
						nExprUnary.Add(ExprUnary());
						break;
					}
					case TokenCategory.BIT_NOT: {
						nExprUnary.AnchorToken = Expect(TokenCategory.BIT_NOT);
						nExprUnary.Add(ExprUnary());
						break;
					}
					// Default case would be unreachable
				}
			}
			else if (firstOfExprPrimary.Contains(CurrentToken)) {
				return ExprPrimary();
			}
			return nExprUnary;
		}

		// Returns NExprPrimary or any of subproduction nodes
		public Node ExprPrimary() {
			if (CurrentToken == TokenCategory.IDENTIFIER) {
				Token identifier = Expect(TokenCategory.IDENTIFIER);
				if (CurrentToken==TokenCategory.PARENTHESIS_LEFT) {
					NFunCall nFunCall = new NFunCall(){
						AnchorToken = identifier
					};
					nFunCall.Add(FunCall());
					return nFunCall;
				}
				else {
					NExprPrimary nExprPrimary = new NExprPrimary();
					nExprPrimary.AnchorToken = identifier;
					return nExprPrimary;
				}
			}
			else if (firstOfLitAlt.Contains(CurrentToken)) {
				return LitAlt();
			}
			else if (CurrentToken==TokenCategory.PARENTHESIS_LEFT) {
				Expect(TokenCategory.PARENTHESIS_LEFT);
				Node resultingNode = Expr();
				Expect(TokenCategory.PARENTHESIS_RIGHT);
				return resultingNode;
			}
			return new Node();
		}

		// Returns NLitInt, NLitBool, NLitChar, NLitString or NArrayList
		public Node LitAlt() {
			if (firstOfLit.Contains(CurrentToken)) {
				return Lit();
			}
			else if (CurrentToken==TokenCategory.STRING) {
				NLitString nLitString = new NLitString(){
					AnchorToken = Expect(TokenCategory.STRING)
				};
				return nLitString;
			}
			else if (CurrentToken==TokenCategory.CURLY_BRACE_LEFT) {
				return ArrayList();
			}
			else {
				throw new SyntaxError(new HashSet<TokenCategory>() {
					TokenCategory.STRING,
					TokenCategory.CURLY_BRACE_LEFT,
					TokenCategory.TRUE,
					TokenCategory.FALSE,
					TokenCategory.BASE_2,
					TokenCategory.BASE_8,
					TokenCategory.BASE_10,
					TokenCategory.BASE_16,
					TokenCategory.CHARACTER
				}
				, tokenStream.Current);
			}
		}

		// Returns NArrayList
		public Node ArrayList() {
			NArrayList nArrayList = new NArrayList();
			Expect(TokenCategory.CURLY_BRACE_LEFT);
			if (firstOfLit.Contains(CurrentToken)) {
				nArrayList.Add(Lit());
				while (CurrentToken==TokenCategory.COMMA) {
					Expect(TokenCategory.COMMA);
					nArrayList.Add(Lit());
				}
			}
			Expect(TokenCategory.CURLY_BRACE_RIGHT);
			return nArrayList;
		}
	}
}
