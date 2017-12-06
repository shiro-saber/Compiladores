/*
  Jaime Margolin A01019332
  Juan carlos Leon A01020200
  Rodrigo Solana A01129839
*/

using System;
using System.Text;
using System.Collections.Generic;

namespace Int64 {

  public class Sharmuta {

    public string name;
    public bool param;
    public int? pos;

    public Sharmuta(string na, bool par, int? po){
      name = na;
      param = par;
      pos = po;
    }

  }

  public class Modishness {
    public string name;
    public int args = 0;
    public bool predef = false;
    public Dictionary<string,Sharmuta> locTable = null;

    public Modishness(string na, int ar=0, bool pr=false, Dictionary<string,Sharmuta> lt = null)
    {
      args = ar;
      name = na;
      predef = pr;
      locTable = lt;
    }
  }

    public class SymbolTable: IEnumerable<KeyValuePair<string, Modishness>> {

        IDictionary<string, Modishness> data = new Dictionary<string, Modishness>();

        //-----------------------------------------------------------
        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append("\n\nSymbol Table\n");
            sb.Append("====================\n");
            sb.Append("|name\t\t|arity\t\t|predefined\t\t\n");

            foreach (var entry in data) {
                sb.Append(String.Format("{0} \t\t|{2} \t\t|{3} ]\n",
                                        entry.Key,
                                        entry.Value.name,
                                        entry.Value.args,
                                        entry.Value.predef));

            if(!entry.Value.predef) {
                       sb.Append("\t\t\t|LOCALS\n");
                       sb.Append("\t\t\t|name\t|param?\t|pos\n");
                       foreach(var zz in (IDictionary<string, Sharmuta>)entry.Value.locTable)
                       {
                               sb.Append(String.Format("\t\t\t|{0}\t|{1}\t{2}\n",
                                                       zz.Key,
                                                       zz.Value.param,
                                                       zz.Value.pos
                                                       ));
                       }
                       sb.Append("\t\t\t____________________________________________\n");
               }
             }
            sb.Append("====================\n");
            return sb.ToString();
        }

        //-----------------------------------------------------------
        public Modishness this[string key] {
            get {
                return data[key];
            }
            set {
                data[key] = value;
            }
        }

        //-----------------------------------------------------------
        public bool Contains(string key) {
            return data.ContainsKey(key);
        }

        //-----------------------------------------------------------
        public IEnumerator<KeyValuePair<string, Modishness>> GetEnumerator() {
            return data.GetEnumerator();
        }

        //-----------------------------------------------------------
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }
    }
}
