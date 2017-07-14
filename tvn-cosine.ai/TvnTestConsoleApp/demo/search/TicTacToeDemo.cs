using System;
using tvn.cosine.ai.environment.tictactoe;
using tvn.cosine.ai.search.adversarial;
using tvn.cosine.ai.util.datastructure;

namespace TvnTestConsoleApp.demo.search
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
            Console.WriteLine("TIC-TAC-TOE DEMO");
            Console.WriteLine("");
            startMinimaxDemo();
            startAlphaBetaDemo();
        }

        private static void startMinimaxDemo()
        {
            Console.WriteLine("MINI MAX DEMO\n");
            TicTacToeGame game = new TicTacToeGame();
            TicTacToeState currState = game.getInitialState();
            AdversarialSearch<TicTacToeState, XYLocation> search = MinimaxSearch<TicTacToeState, XYLocation, string>.createFor(game);
            while (!(game.isTerminal(currState)))
            {
                Console.WriteLine(game.getPlayer(currState) + "  playing ... ");
                XYLocation action = search.makeDecision(currState);
                currState = game.getResult(currState, action);
                Console.WriteLine(currState);
            }
            Console.WriteLine("MINI MAX DEMO done");
        }

        private static void startAlphaBetaDemo()
        {
            Console.WriteLine("ALPHA BETA DEMO\n");
            TicTacToeGame game = new TicTacToeGame();
            TicTacToeState currState = game.getInitialState();
            AdversarialSearch<TicTacToeState, XYLocation> search = AlphaBetaSearch<TicTacToeState, XYLocation, string>.createFor(game);
            while (!(game.isTerminal(currState)))
            {
                Console.WriteLine(game.getPlayer(currState) + "  playing ... ");
                XYLocation action = search.makeDecision(currState);
                currState = game.getResult(currState, action);
                Console.WriteLine(currState);
            }
            Console.WriteLine("ALPHA BETA DEMO done");
        }
    }

}
