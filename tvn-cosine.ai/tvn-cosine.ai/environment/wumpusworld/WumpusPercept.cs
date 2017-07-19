using System.Text;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.environment.wumpusworld
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 237.<br>
     * <br>
     * The agent has five sensors, each of which gives a single bit of information:
     * <ul>
     * <li>In the square containing the wumpus and in the directly (not diagonally)
     * adjacent squares, the agent will perceive a Stench.</li>
     * <li>In the squares directly adjacent to a pit, the agent will perceive a
     * Breeze.</li>
     * <li>In the square where the gold is, the agent will perceive a Glitter.</li>
     * <li>When an agent walks into a wall, it will perceive a Bump.</li>
     * <li>When the wumpus is killed, it emits a woeful Scream that can be perceived
     * anywhere in the cave.</li>
     * </ul>
     * 
     * @author Federico Baron
     * @author Alessandro Daniele
     * @author Ciaran O'Reilly
     * @author Ruediger Lunde
     */
    public class WumpusPercept : Percept
    { 
        private bool stench;
        private bool breeze;
        private bool glitter;
        private bool bump;
        private bool scream;

        public WumpusPercept setStench()
        {
            stench = true;
            return this;
        }

        public WumpusPercept setBreeze()
        {
            breeze = true;
            return this;
        }

        public WumpusPercept setGlitter()
        {
            glitter = true;
            return this;
        }

        public WumpusPercept setBump()
        {
            bump = true;
            return this;
        }

        public WumpusPercept setScream()
        {
            scream = true;
            return this;
        }

        public bool isStench()
        {
            return stench;
        }

        public bool isBreeze()
        {
            return breeze;
        }

        public bool isGlitter()
        {
            return glitter;
        }

        public bool isBump()
        {
            return bump;
        }

        public bool isScream()
        {
            return scream;
        }

         
    public override string ToString()
        {
            StringBuilder result = new StringBuilder("{");
            if (stench)
                result.Append("Stench, ");
            if (breeze)
                result.Append("Breeze, ");
            if (glitter)
                result.Append("Glitter, ");
            if (bump)
                result.Append("Bump, ");
            if (scream)
                result.Append("Scream, ");
            if (result.length() > 1)
                result.delete(result.length() - 2, result.length());
            result.Append("}");
            return result.ToString();
        }
    }
}
