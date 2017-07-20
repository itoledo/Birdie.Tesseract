using tvn.cosine.ai.common.datastructures;
using tvn.cosine.ai.environment.tictactoe;
using tvn.cosine.ai.search.adversarial;

namespace tvn_cosine.ai.demo.search
{
    /**
     * Applies Minimax search and alpha-beta pruning to find optimal moves for the
     * Tic-tac-toe game.
     * 
     * @author Ruediger Lunde
     */
    public class TicTacToeDemo
    {
        public static void Main(params string[] args)
        {
            System.Console.WriteLine("TIC-TAC-TOE DEMO");
            System.Console.WriteLine("");
            startMinimaxDemo();
            startAlphaBetaDemo();
        }

        private static void startMinimaxDemo()
        {
            System.Console.WriteLine("MINI MAX DEMO\n");
            TicTacToeGame game = new TicTacToeGame();
            TicTacToeState currState = game.getInitialState();
            AdversarialSearch<TicTacToeState, XYLocation> search = MinimaxSearch<TicTacToeState, XYLocation, string>
                    .createFor(game);
            while (!(game.isTerminal(currState)))
            {
                System.Console.WriteLine(game.getPlayer(currState) + "  playing ... ");
                XYLocation action = search.makeDecision(currState);
                currState = game.getResult(currState, action);
                System.Console.WriteLine(currState);
            }
            System.Console.WriteLine("MINI MAX DEMO done");
        }

        private static void startAlphaBetaDemo()
        {
            System.Console.WriteLine("ALPHA BETA DEMO\n");
            TicTacToeGame game = new TicTacToeGame();
            TicTacToeState currState = game.getInitialState();
            AdversarialSearch<TicTacToeState, XYLocation> search = AlphaBetaSearch<TicTacToeState, XYLocation, string>
                    .createFor(game);
            while (!(game.isTerminal(currState)))
            {
                System.Console.WriteLine(game.getPlayer(currState) + "  playing ... ");
                XYLocation action = search.makeDecision(currState);
                currState = game.getResult(currState, action);
                System.Console.WriteLine(currState);
            }
            System.Console.WriteLine("ALPHA BETA DEMO done");
        }
    }

}
