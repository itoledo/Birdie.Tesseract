 namespace aima.core.logic.fol.inference;

import aima.core.logic.fol.inference.proof.Proof;
import aima.core.logic.fol.inference.proof.ProofPrinter;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class InferenceResultPrinter {
	/**
	 * Utility method for outputting InferenceResults in a formatted textual
	 * representation.
	 * 
	 * @param ir
	 *            an InferenceResult
	 * @return a string representation of the InferenceResult.
	 */
	public static string printInferenceResult(InferenceResult ir) {
		StringBuilder sb = new StringBuilder();

		sb.Append("InferenceResult.isTrue=" + ir.isTrue());
		sb.Append("\n");
		sb.Append("InferenceResult.isPossiblyFalse=" + ir.isPossiblyFalse());
		sb.Append("\n");
		sb.Append("InferenceResult.isUnknownDueToTimeout="
				+ ir.isUnknownDueToTimeout());
		sb.Append("\n");
		sb.Append("InferenceResult.isPartialResultDueToTimeout="
				+ ir.isPartialResultDueToTimeout());
		sb.Append("\n");
		sb.Append("InferenceResult.#Proofs=" + ir.getProofs().Count);
		sb.Append("\n");
		int proofNo = 0;
		for (Proof p : ir.getProofs()) {
			proofNo++;
			sb.Append("InferenceResult.Proof#" + proofNo + "=\n"
					+ ProofPrinter.printProof(p));
		}

		return sb.ToString();
	}
}