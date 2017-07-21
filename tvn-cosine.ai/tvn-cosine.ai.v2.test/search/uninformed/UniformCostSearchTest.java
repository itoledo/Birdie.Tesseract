namespace aima.test.unit.search.uninformed;

using aima.core.environment.map2d.GoAction;
using aima.core.environment.map2d.SimplifiedRoadMapOfPartOfRomania;
using aima.core.environment.support.ProblemFactory;
using aima.core.search.api.Problem;
using aima.core.search.api.SearchForActionsFunction;
using aima.core.search.basic.uninformed.UniformCostSearch;
using aima.extra.search.pqueue.uninformed.GraphPriorityQueueSearch;
using aima.extra.search.pqueue.uninformed.GraphRLPriorityQueueSearch;
using aima.extra.search.pqueue.uninformed.TreePriorityQueueSearch;
using aima.extra.search.pqueue.uninformed.UniformCostQueueSearch;

using org.junit.Assert;
using org.junit.Test;
using org.junit.runner.RunWith;
using org.junit.runners.Parameterized;

using java.util.Arrays;
using java.util.Collection;
using java.util.List;

/**
 * @author manthan.
 */
@RunWith(Parameterized.class)
public class UniformCostSearchTest {
    @Parameterized.Parameters(name = "{index}: {0}")
    public static Collection<Object[]> implementations() {
        return Arrays.asList(new Object[][]{
        	{"core.UniformCostSearch"}, 
        	{"extra.UniformCostQueueSearch.graph"}, 
         	{"extra.UniformCostQueueSearch.graphrl"}, 
        	{"extra.UniformCostQueueSearch.tree"}
        });
    }

    @Parameterized.Parameter
    public string searchFunctionName;

    public <A, S> List<A> searchForActions(Problem<A, S> problem) {
        SearchForActionsFunction<A, S> searchForActionsFunction;
        if (searchFunctionName.equals("core.UniformCostSearch")) {
        	searchForActionsFunction = new UniformCostSearch<>();
        }
        else if (searchFunctionName.equals("extra.UniformCostQueueSearch.graph")){
        	searchForActionsFunction = new UniformCostQueueSearch<>(new GraphPriorityQueueSearch<>());
        }
        else if (searchFunctionName.equals("extra.UniformCostQueueSearch.graphrl")) {
        	searchForActionsFunction = new UniformCostQueueSearch<>(new GraphRLPriorityQueueSearch<>());
        }
        else if (searchFunctionName.equals("extra.UniformCostQueueSearch.tree")) {
        	searchForActionsFunction = new UniformCostQueueSearch<>(new TreePriorityQueueSearch<>());
        }
        else {
        	throw new UnsupportedOperationException();
        }
        
        return searchForActionsFunction.apply(problem);
    }

    [TestMethod]
    public void testUniformCostSearch() {
        Assert.assertEquals(
                Arrays.asList(new GoAction(SimplifiedRoadMapOfPartOfRomania.SIBIU),
                        new GoAction(SimplifiedRoadMapOfPartOfRomania.RIMNICU_VILCEA),
                        new GoAction(SimplifiedRoadMapOfPartOfRomania.PITESTI),
                        new GoAction(SimplifiedRoadMapOfPartOfRomania.BUCHAREST)),
                searchForActions(ProblemFactory.getSimplifiedRoadMapOfPartOfRomaniaProblem(
                        SimplifiedRoadMapOfPartOfRomania.ARAD, SimplifiedRoadMapOfPartOfRomania.BUCHAREST)));
    }
}