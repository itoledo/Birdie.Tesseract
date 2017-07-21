namespace aima.test.unit.search.support;

using aima.core.search.basic.support.BasicBidirectionalSearchResult;
using org.junit.Assert;
using org.junit.Test;

using java.util.Arrays;
using java.util.List;

/**
 * @author manthan.
 */
public class BasicBidirectionalSearchResultTest {
    [TestMethod]
    public void testBasicBidirectionalSearchresult() {
        List<String> fromInitialStatePartList = Arrays.asList("start", "second", "third");
        List<String> fromGoalStatePartList = Arrays.asList("goal", "fifth", "fourth");
        BasicBidirectionalSearchResult<String> basicBidirectionalActions = new BasicBidirectionalSearchResult<>();
        basicBidirectionalActions.setFromGoalStateToMeeting(fromGoalStatePartList);
        basicBidirectionalActions.setFromInitialStateToMeeting(fromInitialStatePartList);
        Assert.assertEquals(
                Arrays.asList("start", "second", "third"),
                basicBidirectionalActions.fromInitialStateToMeeting());
        Assert.assertEquals(
                Arrays.asList("goal", "fifth", "fourth"),
                basicBidirectionalActions.fromGoalStateToMeeting());
    }
}
