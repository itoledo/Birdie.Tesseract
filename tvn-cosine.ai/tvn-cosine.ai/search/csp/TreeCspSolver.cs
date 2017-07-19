namespace tvn.cosine.ai.search.csp
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Ed.): Figure 6.11, Page
     * 224.<br>
     * <br>
     * <p>
     * <pre>
     * <code>
     * function TREE-CSP-SOLVER(csp) returns a solution, or failure
     * 		inputs: csp, a CSP with components X, D, C
     * 		n &larr; number of variables in X
     * 		assignment &larr; an empty assignment
     * 		root  &larr; any variable in X
     * 		X &larr; TOPOLOGICALSORT(X, root )
     * 		for j = n down to 2 do
     * 			MAKE-ARC-CONSISTENT(PARENT(Xj),Xj )
     * 			if it cannot be made consistent then return failure
     * 		for i = 1 to n do
     * 			assignment[Xi] &larr; any consistent value from Di
     * 			if there is no consistent value then return failure (*)
     * 		return assignment
     * </code>
     *
     * <pre>
     *
     * Figure 6.11 The TREE-CSP-SOLVER algorithm for solving tree-structured CSPs.
     * If the CSP has a solution, we will find it in linear time; if not, we will
     * detect a contradiction. Comment to (*) (RL): If no empty domain was found in the
     * previous loop, this will only happen if n == 1.
     *
     * @author Ruediger Lunde
     * @author Anurag Rai
     */
    public class TreeCspSolver<VAR : Variable, VAL> : CspSolver<VAR, VAL> {

    private bool useRandom;

    public TreeCspSolver<VAR, VAL> useRandom(bool b)
    {
        useRandom = b;
        return this;
    }

     
    public Optional<Assignment<VAR, VAL>> solve(CSP<VAR, VAL> csp)
    {

        Assignment<VAR, VAL> assignment = new Assignment<>();
        // Select a root from the List of Variables
        VAR root = useRandom ? Util.selectRandomlyFromList(csp.getVariables()) : csp.getVariables().Get(0);
        // Sort the variables in topological order
        IQueue<VAR> orderedVars = Factory.CreateQueue<>();
        IMap<VAR, Constraint<VAR, VAL>> parentConstraints = Factory.CreateMap<>();
        topologicalSort(csp, root, orderedVars, parentConstraints);
        if (csp.getDomain(root).isEmpty())
            return Optional.empty(); // CSP has no solution! (needed if orderedVars.size() == 1)

        // Establish arc consistency from top to bottom (starting at the bottom).
        csp = csp.copyDomains(); // do not change the original CSP!
        for (int i = orderedVars.size() - 1; i > 0; i--)
        {
            VAR var = orderedVars.Get(i);
            Constraint<VAR, VAL> constraint = parentConstraints.Get(var);
            VAR parent = csp.getNeighbor(var, constraint);
            if (makeArcConsistent(parent, var, constraint, csp))
            {
                fireStateChanged(csp, null, parent);
                if (csp.getDomain(parent).isEmpty())
                    return Optional.empty(); // CSP has no solution!
            }
        }

        // Assign values to variables from top to bottom.
        for (int i = 0; i < orderedVars.size(); i++)
        {
            VAR var = orderedVars.Get(i);
            for (VAL value : csp.getDomain(var))
            {
                assignment.Add(var, value);
                if (assignment.isConsistent(csp.getConstraints(var)))
                {
                    fireStateChanged(csp, assignment, var);
                    break;
                }
            }
        }
        return Optional.of(assignment);
    }

    /**
     * Computes an explicit representation of the tree structure and a total order which is consistent with the
     * parent-child relations. If the provided CSP has not the required properties (CSP contains only binary
     * constraints, constraint graph is tree-structured and connected), an exception is thrown.
     *
     * @param csp               A CSP
     * @param root              A root variable
     * @param orderedVars       The computed total order (initially empty)
     * @param parentConstraints The tree structure, maps a variable to the constraint representing the arc to the parent
     *                          variable (initially empty)
     */
    private void topologicalSort(CSP<VAR, VAL> csp, VAR root, IQueue<VAR> orderedVars,
                                 IMap<VAR, Constraint<VAR, VAL>> parentConstraints)
    {
        orderedVars.Add(root);
        parentConstraints.Put(root, null);
        int currParentIdx = -1;
        while (currParentIdx < orderedVars.size() - 1)
        {
            currParentIdx++;
            VAR currParent = orderedVars.Get(currParentIdx);
            int arcsPointingUpwards = 0;
            for (Constraint<VAR, VAL> constraint : csp.getConstraints(currParent))
            {
                VAR neighbor = csp.getNeighbor(currParent, constraint);
                if (neighbor == null)
                    throw new IllegalArgumentException("Constraint " + constraint + " is not binary.");
                if (parentConstraints.ContainsKey(neighbor))
                { // faster than orderedVars.contains(neighbor)!
                    arcsPointingUpwards++;
                    if (arcsPointingUpwards > 1)
                        throw new IllegalArgumentException("CSP is not tree-structured.");
                }
                else
                {
                    orderedVars.Add(neighbor);
                    parentConstraints.Put(neighbor, constraint);
                }
            }
        }
        if (orderedVars.size() < csp.getVariables().size())
            throw new IllegalArgumentException("Constraint graph is not connected.");
    }

    /**
     * Establishes arc-consistency for (xi, xj).
     * @return value true if the domain of xi was reduced.
     */
    private bool makeArcConsistent(VAR xi, VAR xj, Constraint<VAR, VAL> constraint, CSP<VAR, VAL> csp)
    {
        Domain<VAL> currDomain = csp.getDomain(xi);
        IQueue<VAL> newValues = Factory.CreateQueue<>(currDomain.size());
        Assignment<VAR, VAL> assignment = new Assignment<>();
        for (VAL vi : currDomain)
        {
            assignment.Add(xi, vi);
            for (VAL vj : csp.getDomain(xj))
            {
                assignment.Add(xj, vj);
                if (constraint.isSatisfiedWith(assignment))
                {
                    newValues.Add(vi);
                    break;
                }
            }
        }
        if (newValues.size() < currDomain.size())
        {
            csp.setDomain(xi, new Domain<>(newValues));
            return true;
        }
        return false;
    }
}

}
