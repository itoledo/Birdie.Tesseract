namespace aima.test.unit.agent.support
{
    public class TestPercept
    {
        public readonly string location;
        public readonly bool floorIsDirty;

        public TestPercept(string location, bool floorIsDirty)
        {
            this.location = location;
            this.floorIsDirty = floorIsDirty;
        }
    }
}