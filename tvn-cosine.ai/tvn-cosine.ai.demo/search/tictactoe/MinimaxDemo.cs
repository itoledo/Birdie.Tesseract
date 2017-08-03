using tvn.cosine.datastructures;
using tvn.cosine.ai.environment.tictactoe;
using tvn.cosine.ai.search.adversarial;
using tvn.cosine.ai.search.adversarial.api;

namespace tvn_cosine.ai.demo.search.tictactoe
{
    public class MinimaxDemo
    {
        public static void Main(params string[] args)
        {
            System.Console.WriteLine("TIC-TAC-TOE DEMO");
            System.Console.WriteLine("");
            startMinimaxDemo();
        }

        static void startMinimaxDemo()
        {
            System.Console.WriteLine("MINI MAX DEMO\n");
            TicTacToeGame game = new TicTacToeGame();
            TicTacToeState currState = game.GetInitialState();
            IAdversarialSearch<TicTacToeState, XYLocation>
                search = MinimaxSearch<TicTacToeState, XYLocation, string>.createFor(game);
            while (!(game.IsTerminal(currState)))
            {
                System.Console.WriteLine(game.GetPlayer(currState) + "  playing ... ");
                XYLocation action = search.makeDecision(currState);
                currState = game.GetResult(currState, action);
                System.Console.WriteLine(currState);
            }
            System.Console.WriteLine("MINI MAX DEMO done");
        }
    }
}
