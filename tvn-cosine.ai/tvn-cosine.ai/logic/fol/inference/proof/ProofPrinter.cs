namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofPrinter
    {

        /**
         * Utility method for outputting proofs in a formatted textual
         * representation.
         * 
         * @param proof
         * @return a string representation of the Proof.
         */
        public static string printProof(Proof proof)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Proof, Answer Bindings: ");
            sb.Append(proof.getAnswerBindings());
            sb.Append("\n");

            IQueue<ProofStep> steps = proof.getSteps();

            int maxStepWidth = "Step".length();
            int maxProofWidth = "Proof".length();
            int maxJustificationWidth = "Justification".length();

            // Calculate the maximum width for each column in the proof
            for (ProofStep step : steps)
            {
                string sn = "" + step.getStepNumber();
                if (sn.length() > maxStepWidth)
                {
                    maxStepWidth = sn.length();
                }
                if (step.getProof().length() > maxProofWidth)
                {
                    maxProofWidth = step.getProof().length();
                }
                if (step.getJustification().length() > maxJustificationWidth)
                {
                    maxJustificationWidth = step.getJustification().length();
                }
            }

            // Give a little extra padding
            maxStepWidth += 1;
            maxProofWidth += 1;
            maxJustificationWidth += 1;

            string f = "|%-" + maxStepWidth + "s| %-" + maxProofWidth + "s|%-"
                    + maxJustificationWidth + "s|\n";

            int barWidth = 5 + maxStepWidth + maxProofWidth + maxJustificationWidth;
            StringBuilder bar = new StringBuilder();
            for (int i = 0; i < barWidth; i++)
            {
                bar.Append("-");
            }
            bar.Append("\n");

            sb.Append(bar);
            sb.Append(String.format(f, "Step", "Proof", "Justification"));
            sb.Append(bar);
            for (ProofStep step : steps)
            {
                sb.Append(String.format(f, "" + step.getStepNumber(),
                        step.getProof(), step.getJustification()));
            }
            sb.Append(bar);

            return sb.ToString();
        }
    }
}
