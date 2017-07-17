using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.datastructures;

namespace tvn.cosine.ai.agent.impl.aprog
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.7, page 47.<br>
     * <br>
     * 
     * <pre>
     * function TABLE-DRIVEN-AGENT(percept) returns an action
     *   persistent: percepts, a sequence, initially empty
     *               table, a table of actions, indexed by percept sequences, initially fully specified
     *           
     *   append percept to end of percepts
     *   action <- LOOKUP(percepts, table)
     *   return action
     * </pre>
     * 
     * Figure 2.7 The TABLE-DRIVEN-AGENT program is invoked for each new percept and
     * returns an action each time. It retains the complete percept sequence in
     * memory.
     * 
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     * 
     */
    public class TableDrivenAgentProgram : AgentProgram
    {
        private IQueue<Percept> percepts = Factory.CreateFifoQueue<Percept>();

        private Table<IQueue<Percept>, string, Action> table;

        private const string ACTION = "action";

        // persistent: percepts, a sequence, initially empty
        // table, a table of actions, indexed by percept sequences, initially fully
        // specified
        /**
         * Constructs a TableDrivenAgentProgram with a table of actions, indexed by
         * percept sequences.
         * 
         * @param perceptSequenceActions
         *            a table of actions, indexed by percept sequences
         */
        public TableDrivenAgentProgram(IMap<IQueue<Percept>, Action> perceptSequenceActions)
        { 
            IQueue<IQueue<Percept>> rowHeaders 
                = Factory.CreateFifoQueue<IQueue<Percept>>(perceptSequenceActions.GetKeys());

            IQueue<string> colHeaders = Factory.CreateFifoQueue<string>();
            colHeaders.Add(ACTION);

            table = new Table<IQueue<Percept>, string, Action>(rowHeaders, colHeaders);

            foreach (IQueue<Percept> row in rowHeaders)
            {
                table.set(row, ACTION, perceptSequenceActions.Get(row));
            }
        }

        //
        // START-AgentProgram

        // function TABLE-DRIVEN-AGENT(percept) returns an action
        public Action execute(Percept percept)
        {
            // append percept to end of percepts
            percepts.Add(percept);

            // action <- LOOKUP(percepts, table)
            // return action
            return lookupCurrentAction();
        }

        // END-AgentProgram
        //

        //
        // PRIVATE METHODS
        //
        private Action lookupCurrentAction()
        {
            Action action = null;

            action = table.get(percepts, ACTION);
            if (null == action)
            {
                action = NoOpAction.NO_OP;
            }

            return action;
        }
    } 
}
