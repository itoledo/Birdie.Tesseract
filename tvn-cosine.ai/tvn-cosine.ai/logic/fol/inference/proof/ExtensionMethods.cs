using System;
using System.Collections.Generic;
using System.Text;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public static class ExtensionMethods
    {
        public static string CustomDictionaryWriterToString(this IDictionary<Variable, Term> dictionary)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            sb.Append("{");
            foreach (var row in dictionary)
            {
                if (first)
                    first = false;
                else
                    sb.Append(", ");

                sb.Append(row.Key);
                sb.Append("=");
                sb.Append(row.Value);
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
