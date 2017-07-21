namespace aima.test.unit.agent.support
{ 
    public class TestState
    {
        public readonly string position;
        public readonly bool dirty;

        public TestState(string position, bool dirty)
        {
            this.position = position;
            this.dirty = dirty;
        }
    }
}