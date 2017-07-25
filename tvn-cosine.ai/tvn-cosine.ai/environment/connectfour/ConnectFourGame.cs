using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.search.adversarial;

namespace tvn.cosine.ai.environment.connectfour
{
    /**
     * Provides an implementation of the ConnectFour game which can be used for
     * experiments with the Minimax algorithm.
     * 
     * @author Ruediger Lunde
     * 
     */
    public class ConnectFourGame : Game<ConnectFourState, int, string>
    {
        private string[] players = new string[] { "red", "yellow" };
        private ConnectFourState initialState = new ConnectFourState(6, 7);

        public   ConnectFourState getInitialState()
        {
            return initialState;
        }

        public   string[] getPlayers()
        {
            return players;
        }

        public   string getPlayer(ConnectFourState state)
        {
            return getPlayer(state.getPlayerToMove());
        }

        /**
         * Returns the player corresponding to the specified player number. For
         * efficiency reasons, <code>ConnectFourState</code>s use numbers
         * instead of strings to identify players.
         */
        public string getPlayer(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                    return players[0];
                case 2:
                    return players[1];
            }
            return null;
        }

        /**
         * Returns the player number corresponding to the specified player. For
         * efficiency reasons, <code>ConnectFourState</code>s use numbers instead of
         * strings to identify players.
         */
        public int getPlayerNum(string player)
        {
            for (int i = 0; i < players.Length;++i)
                if (players[i].Equals(player))
                    return i + 1;
            throw new IllegalArgumentException("Wrong player number.");
        }

        public   ICollection<int> getActions(ConnectFourState state)
        {
            ICollection<int> result = CollectionFactory.CreateQueue<int>();
            for (int i = 0; i < state.getCols();++i)
                if (state.getPlayerNum(0, i) == 0)
                    result.Add(i);
            return result;
        }

        public   ConnectFourState getResult(ConnectFourState state, int action)
        {
            ConnectFourState result = state.Clone();
            result.dropDisk(action);
            return result;
        }

        public   bool isTerminal(ConnectFourState state)
        {
            return state.getUtility() != -1;
        }

        public   double getUtility(ConnectFourState state, string player)
        {
            double result = state.getUtility();
            if (result != -1)
            {
                if (player.Equals(players[1]))
                    result = 1 - result;
            }
            else
            {
                throw new IllegalArgumentException("State is not terminal.");
            }
            return result;
        }
    }
}
