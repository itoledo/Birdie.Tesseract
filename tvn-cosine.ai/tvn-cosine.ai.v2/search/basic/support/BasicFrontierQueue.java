namespace aima.core.search.basic.support;

using java.io.Serializable;
using java.util.Collection;
using java.util.HashMap;
using java.util.Iterator;
using java.util.LinkedList;
using java.util.Map;
using java.util.Objects;
using java.util.Queue;
using java.util.function.Predicate;
using java.util.function.Supplier;

using aima.core.search.api.Node;

/**
 * An implementation of the Queue interface that wraps an underlying queue
 * implementation but tracks the state of all nodes contained in the underlying
 * queue in a Map (i.e. to count the number of instances of a particular state
 * on the queue - intended to support tree based algorithms as well). This is to
 * allow algorithms that need to determine if a particular state is present in
 * the queue to do so quickly.<br>
 * 
 * @author Ciaran O'Reilly
 */
public class BasicFrontierQueue<A, S> implements Queue<Node<A, S>>, Serializable {
	private static final long serialVersionUID = 1L;
	//
	private Queue<Node<A, S>> queue;
	private IDictionary<S, Integer> states;

	public BasicFrontierQueue() {
		this(LinkedList::new, HashMap::new);
	}

	public BasicFrontierQueue(Supplier<Queue<Node<A, S>>> underlyingQueueSupplier,
			Supplier<Map<S, Integer>> stateMembershipSupplier) {
		this.queue = underlyingQueueSupplier.get();
		this.states = stateMembershipSupplier.get();
	}

	//
	// Queue
	 
	public bool add(Node<A, S> node) {
		boolean inserted = queue.add(node);
		if (inserted) {
			incrementState(node.state());
		}

		return inserted;
	}

	 
	public bool offer(Node<A, S> node) {
		boolean inserted = queue.offer(node);
		if (inserted) {
			incrementState(node.state());
		}
		return inserted;
	}

	 
	public Node<A, S> remove() {
		Node<A, S> result = queue.remove();
		if (result != null) {
			decrementState(result.state());
		}
		return result;
	}

	 
	public Node<A, S> poll() {
		Node<A, S> result = queue.poll();
		if (result != null) {
			decrementState(result.state());
		}
		return result;
	}

	 
	public Node<A, S> element() {
		return queue.element();
	}

	 
	public Node<A, S> peek() {
		return queue.peek();
	}

	//
	// Collection
	 
	public int size() {
		return queue.size();
	}

	 
	public bool isEmpty() {
		return queue.isEmpty();
	}

	 
	public bool contains(Object o) {
		if (o is Node) {
			return queue.contains(o);
		} else {
			// Assume is a State
			return states.containsKey(o);
		}
	}

	 
	public Iterator<Node<A, S>> iterator() {
		return queue.iterator();
	}

	 
	public Object[] toArray() {
		return queue.toArray();
	}

	 
	public <T> T[] toArray(T[] a) {
		return queue.toArray(a);
	}

	 
	public bool remove(Object o) {
		// TODO - add support for
		throw new UnsupportedOperationException("Use remove()");
	}

	 
	public bool containsAll(Collection<?> c) {
		return queue.containsAll(c);
	}

	 
	public bool addAll(Collection<? extends Node<A, S>> c) {
		// TODO - add support for
		throw new UnsupportedOperationException("Use add(node)");
	}

	 
	public bool removeAll(Collection<?> c) {
		// TODO - add support for
		throw new UnsupportedOperationException("Use remove()");
	}

	 
	public bool removeIf(Predicate<? super Node<A, S>> filter) {
		Objects.requireNonNull(filter);
		boolean removed = false;
		final Iterator<Node<A, S>> each = iterator();
		while (each.hasNext()) {
			Node<A, S> node = each.next();
			if (filter.test(node)) {
				each.remove();
				removed = true;
				decrementState(node.state());
			}
		}
		return removed;
	}

	 
	public bool retainAll(Collection<?> c) {
		// TODO - add support for
		throw new UnsupportedOperationException("Not supported currently");
	}

	 
	public void clear() {
		queue.clear();
		states.clear();
	}

	 
	public override bool Equals(object o) {
		return queue.Equals(o);
	}

	 
	public override int GetHashCode() {
		return queue.GetHashCode();
	}

	protected void incrementState(S state) {
		states.merge(state, 1, Integer::sum);
	}

	protected void decrementState(S state) {
		// NOTE: works under the assumption the state is already in the map
		// (otherwise will not work)
		states.merge(state, -1, (oldValue, newValue) -> {
			int decrementedValue = Integer.sum(oldValue, newValue);
			if (decrementedValue == 0) {
				return null; // Causes it to be removed from the map
			}
			else {
				return decrementedValue;
			}
		});
	}
}