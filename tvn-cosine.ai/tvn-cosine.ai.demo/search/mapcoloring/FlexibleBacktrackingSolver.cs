using tvn.cosine.ai.search.csp;
using tvn.cosine.ai.search.csp.examples;

namespace tvn_cosine.ai.demo.search.mapcoloring
{
    public class FlexibleBacktrackingSolver
    {
        static void Main(params string[] args)
        {
            CSP<Variable, string> csp = new MapCSP();
            CspListenerStepCounter<Variable, string> stepCounter = new CspListenerStepCounter<Variable, string>();
            CspSolver<Variable, string> solver;

            solver = new FlexibleBacktrackingSolver<Variable, string>();
            solver.addCspListener(stepCounter);
            stepCounter.reset();
            System.Console.WriteLine("Map Coloring (Backtracking)");
            System.Console.WriteLine(solver.solve(csp));
            System.Console.WriteLine(stepCounter.getResults() + "\n");
        }
    }
}
