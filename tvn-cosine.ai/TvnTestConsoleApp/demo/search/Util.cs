using System;
using System.Collections.Generic;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.environment.eightpuzzle;

namespace TvnTestConsoleApp.demo.search
{
    public static class Util
    {
        public const int boardSize = 8;
        public static EightPuzzleBoard boardWithThreeMoveSolution = new EightPuzzleBoard(new int[] { 1, 2, 5, 3, 4, 0, 6, 7, 8 });
        public static EightPuzzleBoard random1 = new EightPuzzleBoard(new int[] { 1, 4, 2, 7, 5, 8, 3, 0, 6 });

        //	public static EightPuzzleBoard extreme = new EightPuzzleBoard(new int[] { 0, 8, 7, 6, 5, 4, 3, 2, 1 });

        public static void printInstrumentation(IDictionary<string, double> properties)
        {
            foreach (var o in properties)
            {
                Console.WriteLine(o.Key + " : " + o.Value);
            }
        }

        public static void printActions<T>(IList<T> actions) where T : IAction
        {
            foreach (var a in actions)
            {
                Console.WriteLine(a);
            }
        }
    }
}
