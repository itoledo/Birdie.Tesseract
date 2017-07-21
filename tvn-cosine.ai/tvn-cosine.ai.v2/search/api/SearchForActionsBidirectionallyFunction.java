namespace aima.core.search.api;


using java.util.function.BiFunction;

/**
 * @author manthan
 */
@FunctionalInterface
public interface SearchForActionsBidirectionallyFunction<A, S> extends BiFunction<Problem<A,S>,
    Problem<A, S>, BidirectionalSearchResult<A>> {
}
