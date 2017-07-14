using System;
using tvn.cosine.ai.search.csp;
using tvn.cosine.ai.search.csp.examples;
using tvn.cosine.ai.search.csp.listeners;

namespace TvnTestConsoleApp.demo.search.mapcoloring
{
    /**
     * Demonstrates the performance of different constraint solving strategies.
     * The map coloring problem from the textbook is used as CSP.
     * 
     * @author Ruediger Lunde
     */

    public class MapColoringCspDemo
    {
        public static void Main(params string[] args)
        {
            CSP<Variable, string> csp = new MapCSP();
            StepCounter<Variable, string> stepCounter = new StepCounter<Variable, string>();
            CspSolver<Variable, string> solver;

            solver = new MinConflictsSolver<Variable, string>(1000);
            solver.addCspListener(stepCounter);
            stepCounter.reset();
            Console.WriteLine("Map Coloring (Minimum Conflicts)");
            Console.WriteLine(solver.solve(csp));
            Console.WriteLine(stepCounter.getResults() + "\n");

            solver = new FlexibleBacktrackingSolver<Variable, string>().setAll();
            solver.addCspListener(stepCounter);
            stepCounter.reset();
            Console.WriteLine("Map Coloring (Backtracking + MRV & DEG + LCV + AC3)");
            Console.WriteLine(solver.solve(csp));
            Console.WriteLine(stepCounter.getResults() + "\n");

            solver = new FlexibleBacktrackingSolver<Variable, string>();
            solver.addCspListener(stepCounter);
            stepCounter.reset();
            Console.WriteLine("Map Coloring (Backtracking)");
            Console.WriteLine(solver.solve(csp));
            Console.WriteLine(stepCounter.getResults() + "\n");
        }
    }

}
