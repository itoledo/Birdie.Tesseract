using tvn.cosine.ai.search.csp;
using tvn.cosine.ai.search.csp.examples;

namespace tvn_cosine.ai.demo.search
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
            CspListenerStepCounter<Variable, string> stepCounter = new CspListenerStepCounter<Variable, string>();
            CspSolver<Variable, string> solver;

            solver = new MinConflictsSolver<Variable, string>(1000);
            solver.addCspListener(stepCounter);
            stepCounter.reset();
            System.Console.WriteLine("Map Coloring (Minimum Conflicts)");
            System.Console.WriteLine(solver.solve(csp));
            System.Console.WriteLine(stepCounter.getResults() + "\n");

            solver = new FlexibleBacktrackingSolver<Variable, string>().setAll();
            solver.addCspListener(stepCounter);
            stepCounter.reset();
            System.Console.WriteLine("Map Coloring (Backtracking + MRV & DEG + LCV + AC3)");
            System.Console.WriteLine(solver.solve(csp));
            System.Console.WriteLine(stepCounter.getResults() + "\n");

            solver = new FlexibleBacktrackingSolver<Variable, string>();
            solver.addCspListener(stepCounter);
            stepCounter.reset();
            System.Console.WriteLine("Map Coloring (Backtracking)");
            System.Console.WriteLine(solver.solve(csp));
            System.Console.WriteLine(stepCounter.getResults() + "\n");
        }
    }

}
