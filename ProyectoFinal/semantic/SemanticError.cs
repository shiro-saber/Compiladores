/*
  Jaime Margolin A01019332
  Juan carlos Leon A01020200
  Rodrigo Solana A01129839
*/

using System;

namespace Int64 {

    class SemanticError: Exception {

        public SemanticError(string message, Token token):
            base(String.Format(
                "Semantic Error: {0} \n" +
                "at row {1}, column {2}.",
                message,
                token.Row,
                token.Column)) {
        }

        public SemanticError(string message):
    base(String.Format(
        "Semantic Error: {0} \n",
        message)) {
}


    }
}
