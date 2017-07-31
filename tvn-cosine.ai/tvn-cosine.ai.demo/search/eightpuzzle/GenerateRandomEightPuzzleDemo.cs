using tvn.cosine;
using tvn.cosine.api;
using tvn.cosine.ai.environment.eightpuzzle;

namespace tvn_cosine.ai.demo.search.eightpuzzle
{
    public class GenerateRandomEightPuzzleDemo
    {
        public static void Main(params string[] args)
        {
            IRandom r = CommonFactory.CreateRandom();
            EightPuzzleBoard board = new EightPuzzleBoard(new int[] { 0, 1, 2, 3,
                4, 5, 6, 7, 8 });
            for (int i = 0; i < 50;++i)
            {
                int th = r.Next(4);
                if (th == 0)
                {
                    board.moveGapUp();
                }
                if (th == 1)
                {
                    board.moveGapDown();
                }
                if (th == 2)
                {
                    board.moveGapLeft();
                }
                if (th == 3)
                {
                    board.moveGapRight();
                }
            }
            System.Console.WriteLine(board);
        } 
    }
}
