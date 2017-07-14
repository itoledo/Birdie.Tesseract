using System; 
using tvn.cosine.ai.environment.tictactoe;
using tvn.cosine.ai.search.adversarial;
using tvn.cosine.ai.util.datastructure;

namespace TvnTestConsoleApp.demo.search.tictactoe
{
    class StartMinimaxDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("TIC-TAC-TOE DEMO");
            Console.WriteLine("");
            Console.WriteLine("MINI MAX DEMO\n");

            startMinimaxDemo();
            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void startMinimaxDemo()
        {
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
    }
}
