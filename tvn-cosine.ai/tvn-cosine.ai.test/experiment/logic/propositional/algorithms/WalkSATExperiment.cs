namespace tvn_cosine.ai.test.experiment.logic.propositional.algorithms
{
    public class WalkSATExperiment
    {

        private PLParser parser = new PLParser();

        // NOT REALLY A JUNIT TESTCASE BUT written as one to allow easy execution
        @Test
        public void testWalkSat()
        {
            WalkSAT walkSAT = new WalkSAT();
            Model m = walkSAT.walkSAT(ConvertToConjunctionOfClauses.convert(parser.parse("A & B"))
                    .getClauses(), 0.5, 1000);
            if (m == null)
            {
                System.out.println("failure");
            }
            else
            {
                m.print();
            }
        }

        @Test
        public void testWalkSat2()
        {
            WalkSAT walkSAT = new WalkSAT();
            Model m = walkSAT.walkSAT(ConvertToConjunctionOfClauses.convert(parser.parse("A & ~B"))
                    .getClauses(), 0.5, 1000);
            if (m == null)
            {
                System.out.println("failure");
            }
            else
            {
                m.print();
            }
        }

        @Test
        public void testAIMAExample()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("P => Q");
            kb.tell("L & M => P");
            kb.tell("B & L => M");
            kb.tell("A & P => L");
            kb.tell("A & B => L");
            kb.tell("A");
            kb.tell("B");
            WalkSAT walkSAT = new WalkSAT();
            Model m = walkSAT.walkSAT(ConvertToConjunctionOfClauses.convert(kb.asSentence())
                    .getClauses(), 0.5, 1000);
            if (m == null)
            {
                System.out.println("failure");
            }
            else
            {
                m.print();
            }
        }
    }

}
