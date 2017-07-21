namespace aima.test.unit.search.uninformed;

using aima.core.environment.map2d.GoAction;
using aima.core.environment.map2d.InState;
using aima.core.environment.support.ProblemFactory;
using aima.core.search.api.BidirectionalSearchResult;
using aima.core.search.api.Problem;
using aima.core.search.api.SearchForActionsBidirectionallyFunction;
using aima.core.util.datastructure.Pair;
using aima.extra.search.uninformed.BidirectionalSearchGW;

using aima.extra.search.uninformed.BidirectionalSearchMRS;
using org.junit.Test;
using org.junit.runner.RunWith;
using org.junit.runners.Parameterized;

using java.util.Arrays;
using java.util.Collection;
using java.util.List;

using static aima.core.environment.map2d.SimplifiedRoadMapOfPartOfRomania.ARAD;
using static aima.core.environment.map2d.SimplifiedRoadMapOfPartOfRomania.BUCHAREST;
using static aima.core.environment.map2d.SimplifiedRoadMapOfPartOfRomania.FAGARAS;
using static aima.core.environment.map2d.SimplifiedRoadMapOfPartOfRomania.ORADEA;
using static aima.core.environment.map2d.SimplifiedRoadMapOfPartOfRomania.SIBIU;
using static aima.core.environment.map2d.SimplifiedRoadMapOfPartOfRomania.TIMISOARA;
using static aima.core.environment.map2d.SimplifiedRoadMapOfPartOfRomania.URZICENI;
using static aima.core.environment.map2d.SimplifiedRoadMapOfPartOfRomania.ZERIND;
using static org.junit.Assert.assertEquals;

/**
 * @author manthan.
 */
@RunWith(Parameterized.class)
public class BidirectionalSearchTest {
    @Parameterized.Parameters(name = "{index}: {0}")
    public static Collection<Object[]> implementations() {
        return Arrays.asList(new Object[][]{{"BidirectionalSearchGW"},{"BidirectionalSearchMRS"}});
    }

    @Parameterized.Parameter
    public string searchFunctionName;

    public <A, S> BidirectionalSearchResult<A> searchForActions(Pair<Problem<A, S>, Problem<A, S>> pair) {
        SearchForActionsBidirectionallyFunction<A, S> search;

        if (searchFunctionName.equals("BidirectionalSearchGW")) {
        	search = new BidirectionalSearchGW<>();
        }
        else if(searchFunctionName.equals("BidirectionalSearchMRS")){
            search = new BidirectionalSearchMRS<>();
        }
        else{
            throw new UnsupportedOperationException("Unsupported searchFunctionName="+searchFunctionName);
        }
        return search.apply(pair.getFirst(), pair.getSecond());
    }

    [TestMethod]
    public void aradToArad() throws Exception {
        Pair<Problem<GoAction, InState>, Problem<GoAction, InState>> pair =
            ProblemFactory.getSimpleBidirectionalSearchProblem(ARAD, ARAD);

        assertEquals(null, searchForActions(pair).fromGoalStateToInitialState());
    }

    [TestMethod]
    public void timisoaraToUrziceni() throws Exception {
        Pair<Problem<GoAction, InState>, Problem<GoAction, InState>> pair =
            ProblemFactory.getSimpleBidirectionalSearchProblem(TIMISOARA, URZICENI);
        final List<GoAction> solution = searchForActions(pair).fromInitialStateToGoalState();
        final List<GoAction> reverse = searchForActions(pair).fromGoalStateToInitialState();

        assertEquals(Arrays.asList(
            new GoAction(ARAD),
            new GoAction(SIBIU),
            new GoAction(FAGARAS),
            new GoAction(BUCHAREST),
            new GoAction(URZICENI)),
            solution);
        assertEquals(Arrays.asList(
            new GoAction(BUCHAREST),
            new GoAction(FAGARAS),
            new GoAction(SIBIU),
            new GoAction(ARAD),
            new GoAction(TIMISOARA)),
            reverse);
    }

    [TestMethod]
    public void zerindToSibiu() throws Exception {
        Pair<Problem<GoAction, InState>, Problem<GoAction, InState>> pair =
            ProblemFactory.getSimpleBidirectionalSearchProblem(ZERIND, SIBIU);
        final List<GoAction> solution = searchForActions(pair).fromInitialStateToGoalState();

        assertEquals(Arrays.asList(
            new GoAction(ORADEA),
            new GoAction(SIBIU)),
            solution);

    }

    [TestMethod]
    public void aradToBucharest() throws Exception {
        Pair<Problem<GoAction, InState>, Problem<GoAction, InState>> pair =
            ProblemFactory.getSimpleBidirectionalSearchProblem(ARAD, BUCHAREST);
        final List<GoAction> solution = searchForActions(pair).fromInitialStateToGoalState();

        assertEquals(Arrays.asList(
            new GoAction(SIBIU),
            new GoAction(FAGARAS),
            new GoAction(BUCHAREST)),
            solution);

    }

    [TestMethod]
    public void aradToSibiu() throws Exception {
        Pair<Problem<GoAction, InState>, Problem<GoAction, InState>> pair =
            ProblemFactory.getSimpleBidirectionalSearchProblem(ARAD, SIBIU);
        final List<GoAction> solution = searchForActions(pair).fromInitialStateToGoalState();
        final List<GoAction> reverse = searchForActions(pair).fromGoalStateToInitialState();

        assertEquals(Arrays.asList(
            new GoAction(SIBIU)),
            solution);

        assertEquals(Arrays.asList(
            new GoAction(ARAD)),
            reverse);

    }

    [TestMethod]
    public void aradToFagras() throws Exception {
        Pair<Problem<GoAction, InState>, Problem<GoAction, InState>> pair =
            ProblemFactory.getSimpleBidirectionalSearchProblem(ARAD, FAGARAS);
        final BidirectionalSearchResult<GoAction> result = searchForActions(pair);
        final List<GoAction> solution = result.fromInitialStateToGoalState();
        final List<GoAction> reverse = result.fromGoalStateToInitialState();

        assertEquals(Arrays.asList(
            new GoAction(SIBIU),
            new GoAction(FAGARAS)),
            solution);

        assertEquals(Arrays.asList(
            new GoAction(SIBIU),
            new GoAction(ARAD)),
            reverse);


    }
}