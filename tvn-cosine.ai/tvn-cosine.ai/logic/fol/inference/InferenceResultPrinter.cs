﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.logic.fol.inference
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class InferenceResultPrinter
    {
        /**
         * Utility method for outputting InferenceResults in a formatted textual
         * representation.
         * 
         * @param ir
         *            an InferenceResult
         * @return a String representation of the InferenceResult.
         */
        public static String printInferenceResult(InferenceResult ir)
        {
            StringBuilder sb = new StringBuilder();

            sb.append("InferenceResult.isTrue=" + ir.isTrue());
            sb.append("\n");
            sb.append("InferenceResult.isPossiblyFalse=" + ir.isPossiblyFalse());
            sb.append("\n");
            sb.append("InferenceResult.isUnknownDueToTimeout="
                    + ir.isUnknownDueToTimeout());
            sb.append("\n");
            sb.append("InferenceResult.isPartialResultDueToTimeout="
                    + ir.isPartialResultDueToTimeout());
            sb.append("\n");
            sb.append("InferenceResult.#Proofs=" + ir.getProofs().size());
            sb.append("\n");
            int proofNo = 0;
            for (Proof p : ir.getProofs())
            {
                proofNo++;
                sb.append("InferenceResult.Proof#" + proofNo + "=\n"
                        + ProofPrinter.printProof(p));
            }

            return sb.toString();
        }
    }
}
