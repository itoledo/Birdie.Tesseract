using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.util.datastructure;

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

        private List<Percept> percepts = new List<Percept>();

        private Table<List<Percept>, string, Action> table;

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
        public TableDrivenAgentProgram(IDictionary<List<Percept>, Action> perceptSequenceActions)
        {

            List<List<Percept>> rowHeaders = new List<List<Percept>>(
                    perceptSequenceActions.Keys);

            List<string> colHeaders = new List<string>();
            colHeaders.Add(ACTION);

            table = new Table<List<Percept>, string, Action>(rowHeaders, colHeaders);

            foreach (List<Percept> row in rowHeaders)
            {
                table.Set(row, ACTION, perceptSequenceActions[row]);
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

            action = table.Get(percepts, ACTION);
            if (null == action)
            {
                action = NoOpAction.NO_OP;
            }

            return action;
        }
    } 
}
