using System.Collections.Generic;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.agent.impl.aprog
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.7, page 47. <para />
    ///  
    /// Figure 2.7 The TABLE-DRIVEN-AGENT program is invoked for each new percept and
    /// returns an action each time. It retains the complete percept sequence in
    /// memory. 
    /// </summary>
    public class TableDrivenAgentProgram : IAgentProgram
    {
        private const string ACTION = "action";

        /// <summary>
        /// a sequence, initially empty
        /// </summary>
        private readonly IList<IPercept> percepts;
        /// <summary>
        /// a table of actions, indexed by percept sequences, initially fully specified
        /// </summary>
        private readonly Table<IList<IPercept>, string, IAction> table;
         
        /// <summary>
        /// Constructs a TableDrivenAgentProgram with a table of actions, indexed by percept sequences.
        /// </summary>
        /// <param name="perceptSequenceActions">a table of actions, indexed by percept sequences</param>
        public TableDrivenAgentProgram(IDictionary<IList<IPercept>, IAction> perceptSequenceActions)
        {
            percepts = new List<IPercept>();

            IList<IList<IPercept>> rowHeaders = new List<IList<IPercept>>(perceptSequenceActions.Keys); 
            IList<string> colHeaders = new List<string>();
            colHeaders.Add(ACTION);

            table = new Table<IList<IPercept>, string, IAction>(rowHeaders, colHeaders);

            foreach (IList<IPercept> row in rowHeaders)
            {
                table.Set(row, ACTION, perceptSequenceActions[row]);
            }
        }
         
        /// <summary>
        /// TABLE-DRIVEN-AGENT(percept)
        /// </summary>
        /// <param name="percept"></param>
        /// <returns>an action</returns>
        public IAction Execute(IPercept percept)
        {
            // append percept to end of percepts
            percepts.Add(percept);

            // action <- LOOKUP(percepts, table)
            // return action
            return lookupCurrentAction();
        }
         
        private IAction lookupCurrentAction()
        {
            IAction action = null;

            action = table.Get(percepts, ACTION);
            if (null == action)
            {
                action = DynamicAction.NO_OP;
            }

            return action;
        }
    }
}
