using tvn.cosine.ai.search.csp;
using tvn.cosine.ai.search.csp.examples;

namespace tvn_cosine.ai.demo.search.mapcoloring
{
public  class FlexibleBacktrackingSolverMrvDegLcvAc3
    {
        static void Main(params string[] args)
        {
            CSP<Variable, string> csp = new MapCSP();
            CspListenerStepCounter<Variable, string> stepCounter = new CspListenerStepCounter<Variable, string>();
            CspSolver<Variable, string> solver;

            solver = new FlexibleBacktrackingSolver<Variable, string>().setAll();
            solver.addCspListener(stepCounter);
            stepCounter.reset();
            System.Console.WriteLine("Map Coloring (Backtracking + MRV & DEG + LCV + AC3)");
            System.Console.WriteLine(solver.solve(csp));
            System.Console.WriteLine(stepCounter.getResults() + "\n"); 
        }
    }
}
