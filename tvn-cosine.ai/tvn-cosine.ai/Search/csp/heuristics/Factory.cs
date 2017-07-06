namespace tvn.cosine.ai.search.csp.heuristics
{
    /// <summary>
    /// Defines variable and value selection heuristics for CSP backtracking strategies.
    /// </summary>
    public static class Factory
    {
        public static VariableSelection<VAR, VAL> mrv<VAR, VAL>()
            where VAR : Variable
        {
            return new MrvHeuristic<VAR, VAL>();
        }

        public static VariableSelection<VAR, VAL> deg<VAR, VAL>()
            where VAR : Variable
        {
            return new DegHeuristic<VAR, VAL>();
        }

        public static VariableSelection<VAR, VAL> mrvDeg<VAR, VAL>()
            where VAR : Variable
        {
            return new MrvDegHeuristic<VAR, VAL>();
        }

        public static ValueSelection<VAR, VAL> lcv<VAR, VAL>()
            where VAR : Variable
        {
            return new LcvHeuristic<VAR, VAL>();
        }
    }
}
