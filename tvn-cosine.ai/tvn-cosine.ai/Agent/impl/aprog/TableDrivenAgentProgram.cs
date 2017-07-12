using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.agent.impl.aprog
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.7, page 47. 
     *  
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

        private IList<IPercept> percepts = new List<IPercept>();

        private Table<IList<IPercept>, string, IAction> table;

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
        public TableDrivenAgentProgram(IDictionary<IList<IPercept>, IAction> perceptSequenceActions)
        {

            IList<IList<IPercept>> rowHeaders = new List<IList<IPercept>>(perceptSequenceActions.Keys);

            IList<string> colHeaders = new List<string>();
            colHeaders.Add(ACTION);

            table = new Table<IList<IPercept>, string, IAction>(rowHeaders, colHeaders);

            foreach (IList<IPercept> row in rowHeaders)
            {
                table.Set(row, ACTION, perceptSequenceActions[row]);
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

            action = table.Get(percepts, ACTION);
            if (null == action)
            {
                action = NoOpAction.NO_OP;
            }

            return action;
        }
    }
}
