namespace aima.core.search.basic.support;

using aima.core.search.api.BidirectionalSearchResult;

using java.util.List;

/**
 * @author manthan.
 */
public class BasicBidirectionalSearchResult<A> implements BidirectionalSearchResult<A> {

    private List<A> fromInitialStateToMeeting;
    private List<A> fromGoalStateToMeeting;
    private List<A> fromInitialStateToGoalState;
    private List<A> fromGoalStateToInitialState;

    public BasicBidirectionalSearchResult() {
    }

     
    public List<A> fromInitialStateToMeeting() {
        return fromInitialStateToMeeting;
    }

     
    public List<A> fromGoalStateToMeeting() {
        return fromGoalStateToMeeting;
    }

     
    public List<A> fromInitialStateToGoalState() {
        return fromInitialStateToGoalState;
    }

     
    public List<A> fromGoalStateToInitialState() {
        return fromGoalStateToInitialState;
    }

    public void setFromInitialStateToMeeting(List<A> fromInitialStateToMeeting) {
        this.fromInitialStateToMeeting = fromInitialStateToMeeting;
    }

    public void setFromGoalStateToMeeting(List<A> fromGoalStateToMeeting) {
        this.fromGoalStateToMeeting = fromGoalStateToMeeting;
    }

    public void setFromInitialStateToGoalState(List<A> fromInitialStateToGoalState) {
        this.fromInitialStateToGoalState = fromInitialStateToGoalState;
    }

    public void setFromGoalStateToInitialState(List<A> fromGoalStateToInitialState) {
        this.fromGoalStateToInitialState = fromGoalStateToInitialState;
    }
}
