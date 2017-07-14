using System;
using System.Collections.Generic;
using tvn.cosine.ai.agent;

namespace TvnTestConsoleApp.demo.search
{
    public static class Util
    {
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
