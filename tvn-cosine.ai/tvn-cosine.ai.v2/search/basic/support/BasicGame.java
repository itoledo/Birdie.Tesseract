namespace aima.core.search.basic.support;

using java.util.List;

using aima.core.search.api.ActionsFunction;
using aima.core.search.api.Game;
using aima.core.search.api.InitialStateFunction;
using aima.core.search.api.PlayerFunction;
using aima.core.search.api.ResultFunction;
using aima.core.search.api.TerminalStateUtilityFunction;
using aima.core.search.api.TerminalTestPredicate;

/**
 * Basic implementation of the Game interface.
 * 
 * @author Ciaran O'Reilly
 */
public class BasicGame<S, A, P> implements Game<S, A, P> {
	private InitialStateFunction<S> initialStateFn;
	private PlayerFunction<S, P> playerFn;
	private ActionsFunction<A, S> actionsFn;
	private ResultFunction<A, S> resultFn;
	private TerminalTestPredicate<S> terminalTestPredicate;
	private TerminalStateUtilityFunction<S, P> utilityFn;

	public BasicGame(InitialStateFunction<S> initialStateFn, PlayerFunction<S, P> playerFn,
			ActionsFunction<A, S> actionsFn, ResultFunction<A, S> resultFn,
			TerminalTestPredicate<S> terminalTestPredicate, TerminalStateUtilityFunction<S, P> utilityFn) {
		this.initialStateFn = initialStateFn;
		this.playerFn = playerFn;
		this.actionsFn = actionsFn;
		this.resultFn = resultFn;
		this.terminalTestPredicate = terminalTestPredicate;
		this.utilityFn = utilityFn;
	}
	
	 
	public S initialState() {
		return initialStateFn.initialState();
	}

	 
	public P player(S state) {
		return playerFn.player(state);
	}

	 
	public List<A> actions(S s) {
		return actionsFn.actions(s);
	}

	 
	public S result(S s, A a) {
		return resultFn.result(s, a);
	}

	 
	public bool isTerminalState(S state) {
		return terminalTestPredicate.isTerminalState(state);
	}

	 
	public double utility(S state, P player) {
		return utilityFn.utility(state, player);
	}
}
