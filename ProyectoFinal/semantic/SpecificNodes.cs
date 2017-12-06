/*
   Jaime Margolin A01019332
   Juan carlos Leon A01020200
   Rodrigo Solana A01129839
 */

namespace Int64 {
class NProgram : Node {}

class NVarDef : Node {}

class NFunDef : Node {}

class NVarDefList : Node {}

class NFunDefList : Node {}  //Added for semantics

class NParameterList : Node {}  //Added for semantics

class NParameter : Node {}  //Added for semantics

class NBreak : Node {}  //Added for semantics

class NContinue : Node {}  //Added for semantics

class NReturn : Node {}  //Added for semantics

class NStmtList : Node {}

class NAssign : Node {}

class NFunCall : Node {}

class NIfStmt : Node {}

class NSwitchStmt : Node {}

class NCaseList : Node {}  //Added for semantics

class NCase : Node {}

//class NDefault: Node {} Removed for semantics equivalence to NStmtList

class NLitBool : Node {}

class NLitInt : Node {}

class NLitChar : Node {}

class NWhileStmt : Node {}

class NDoWhileStmt : Node {}

class NForStmt : Node {}

class NExprList : Node {}  //Added for semantics

class NExpr : Node {}  //Added for semantics

class NExprOr : Node {}

class NExprAnd : Node {}

class NExprComp : Node {}

class NExprRel : Node {}

class NExprBitOr : Node {}

class NExprBitAnd : Node {}

class NExprBitShift : Node {}

class NExprAdd : Node {}

class NExprMul : Node {}

class NExprPow : Node {}

class NExprUnary : Node {}

class NExprPrimary : Node {}

class NLitString : Node {}

class NArrayList : Node {}

class Print : Node {}

class NIdentifier : Node {}
}
