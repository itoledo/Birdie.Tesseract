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
    public class TableDrivenAgentProgram : IAgentProgram
    {
        private IQueue<IPercept> percepts = Factory.CreateQueue<IPercept>();

        private Table<IQueue<IPercept>, string, IAction> table;

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
        public TableDrivenAgentProgram(IMap<IQueue<IPercept>, IAction> perceptSequenceActions)
        { 
            IQueue<IQueue<IPercept>> rowHeaders 
                = Factory.CreateQueue<IQueue<IPercept>>(perceptSequenceActions.GetKeys());

            IQueue<string> colHeaders = Factory.CreateFifoQueue<string>();
            colHeaders.Add(ACTION);

            table = new Table<IQueue<IPercept>, string, IAction>(rowHeaders, colHeaders);

            foreach (IQueue<IPercept> row in rowHeaders)
            {
                table.set(row, ACTION, perceptSequenceActions.Get(row));
            }
        }

        //
        // START-AgentProgram

        // function TABLE-DRIVEN-AGENT(percept) returns an action
        public IAction Execute(IPercept percept)
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
        private IAction lookupCurrentAction()
        {
            IAction action = null;

            action = table.get(percepts, ACTION);
            if (null == action)
            {
                action = DynamicAction.NO_OP;
            }

            return action;
        }
    } 
}
