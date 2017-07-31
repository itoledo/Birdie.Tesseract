using tvn.cosine.collections.api;
using tvn.cosine.ai.environment.nqueens;

namespace tvn_cosine.ai.demo.search.nqueens
{
    public abstract class NQueensDemoBase : SearchDemoBase
    {
        protected const int boardSize = 8;

        protected static void printActions(ICollection<QueenAction> actions)
        {
            foreach (QueenAction action in actions)
            {
                System.Console.WriteLine(action);
            }
        }
    }
}
