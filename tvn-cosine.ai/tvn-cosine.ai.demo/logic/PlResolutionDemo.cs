namespace tvn_cosine.ai.demo.logic
{
    public class PlResolutionDemo
    {
        private static PLResolution plr = new PLResolution();

        public static void main(String[] args)
        {
            KnowledgeBase kb = new KnowledgeBase();
            String fact = "(B11 => ~P11) & B11)";
            kb.tell(fact);
            System.out.println("\nPlResolutionDemo\n");
            System.out.println("adding " + fact + "to knowldegebase");
            displayResolutionResults(kb, "~B11");
        }

        private static void displayResolutionResults(KnowledgeBase kb, String query)
        {
            PLParser parser = new PLParser();
            System.out.println("Running plResolution of query " + query
                    + " on knowledgeBase  gives " + plr.plResolution(kb, parser.parse(query)));
        }
    }

}
