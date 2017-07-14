using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.mdp;
using tvn.cosine.ai.probability.mdp.search;

namespace TvnTestConsoleApp.demo.probability
{
    class ValueIterationDemo
    {
        public static void Main(params string[] args)
        {
            valueIterationDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void valueIterationDemo()
        {

            Console.WriteLine("DEMO: Value Iteration");
            Console.WriteLine("=====================");
            Console.WriteLine("Figure 17.3");
            Console.WriteLine("-----------");

            CellWorld<double> cw = CellWorldFactory.CreateCellWorldForFig17_1();
            MarkovDecisionProcess<Cell<double>, CellWorldAction> mdp = MDPFactory.createMDPForFigure17_3(cw);
            ValueIteration<Cell<double>, CellWorldAction> vi = new ValueIteration<Cell<double>, CellWorldAction>(1.0);

            IDictionary<Cell<double>, double> U = vi.valueIteration(mdp, 0.0001);

            Console.WriteLine("(1,1) = " + U[cw.getCellAt(1, 1)]);
            Console.WriteLine("(1,2) = " + U[cw.getCellAt(1, 2)]);
            Console.WriteLine("(1,3) = " + U[cw.getCellAt(1, 3)]);

            Console.WriteLine("(2,1) = " + U[cw.getCellAt(2, 1)]);
            Console.WriteLine("(2,3) = " + U[cw.getCellAt(2, 3)]);

            Console.WriteLine("(3,1) = " + U[cw.getCellAt(3, 1)]);
            Console.WriteLine("(3,2) = " + U[cw.getCellAt(3, 2)]);
            Console.WriteLine("(3,3) = " + U[cw.getCellAt(3, 3)]);

            Console.WriteLine("(4,1) = " + U[cw.getCellAt(4, 1)]);
            Console.WriteLine("(4,2) = " + U[cw.getCellAt(4, 2)]);
            Console.WriteLine("(4,3) = " + U[cw.getCellAt(4, 3)]);

            Console.WriteLine("=========================");
        }
    }
}
