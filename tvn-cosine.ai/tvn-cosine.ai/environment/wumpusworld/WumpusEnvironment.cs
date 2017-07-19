using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.environment.wumpusworld
{
    /**
    * Implements an environment for the Wumpus World.
    * @author Ruediger Lunde
    */
    public class WumpusEnvironment : AbstractEnvironment
    {

    private WumpusCave cave;
    private bool isWumpusAlive = true;
    private bool isGoldGrabbed;
    private IMap<Agent, AgentPosition> agentPositions = Factory.CreateMap<>();
    private ISet<Agent> bumpedAgents = Factory.CreateSet<>();
    private ISet<Agent> agentsHavingArrow = Factory.CreateSet<>();
    private Agent agentJustKillingWumpus;

    public WumpusEnvironment(WumpusCave cave)
    {
        this.cave = cave;
    }

    public WumpusCave getCave()
    {
        return cave;
    }

    public bool isWumpusAlive()
    {
        return isWumpusAlive;
    }

    public bool isGoalGrabbed()
    {
        return isGoldGrabbed;
    }

    public AgentPosition getAgentPosition(Agent agent)
    {
        return agentPositions.Get(agent);
    }

     
    public void addAgent(Agent agent)
    {
        agentPositions.Put(agent, cave.getStart());
        agentsHavingArrow.Add(agent);
        super.addAgent(agent);
    }

     
    public void executeAction(Agent agent, Action action)
    {
        bumpedAgents.Remove(agent);
        if (agent == agentJustKillingWumpus)
            agentJustKillingWumpus = null;
        AgentPosition pos = agentPositions.Get(agent);
        if (action == WumpusAction.FORWARD)
        {
            AgentPosition newPos = cave.moveForward(pos);
            agentPositions.Put(agent, newPos);
            if (newPos.Equals(pos))
            {
                bumpedAgents.Add(agent);
            }
            else if (cave.isPit(newPos.getRoom()) || newPos.getRoom().Equals(cave.getWumpus()) && isWumpusAlive)
                agent.setAlive(false);
        }
        else if (action == WumpusAction.TURN_LEFT)
        {
            agentPositions.Put(agent, cave.turnLeft(pos));
        }
        else if (action == WumpusAction.TURN_RIGHT)
        {
            agentPositions.Put(agent, cave.turnRight(pos));
        }
        else if (action == WumpusAction.GRAB)
        {
            if (!isGoldGrabbed && pos.getRoom().Equals(cave.getGold()))
                isGoldGrabbed = true;
        }
        else if (action == WumpusAction.SHOOT)
        {
            if (agentsHavingArrow.contains(agent) && isAgentFacingWumpus(pos))
            {
                isWumpusAlive = false;
                agentsHavingArrow.Remove(agent);
                agentJustKillingWumpus = agent;
            }
        }
        else if (action == WumpusAction.CLIMB)
        {
            agent.setAlive(false);
        }
    }

    private bool isAgentFacingWumpus(AgentPosition pos)
    {
        Room wumpus = cave.getWumpus();
        switch (pos.getOrientation())
        {
            case FACING_NORTH:
                return pos.getX() == wumpus.getX() && pos.getY() < wumpus.getY();
            case FACING_SOUTH:
                return pos.getX() == wumpus.getX() && pos.getY() > wumpus.getY();
            case FACING_EAST:
                return pos.getY() == wumpus.getY() && pos.getX() < wumpus.getX();
            case FACING_WEST:
                return pos.getY() == wumpus.getY() && pos.getX() > wumpus.getX();
        }
        return false;
    }

     
    public Percept getPerceptSeenBy(Agent anAgent)
    {
        WumpusPercept result = new WumpusPercept();
        AgentPosition pos = agentPositions.Get(anAgent);
        IQueue<Room> adjacentRooms = Arrays.asList(
                new Room(pos.getX() - 1, pos.getY()), new Room(pos.getX() + 1, pos.getY()),
                new Room(pos.getX(), pos.getY() - 1), new Room(pos.getX(), pos.getY() + 1)
        );
        foreach (Room r in adjacentRooms)
        {
            if (r.Equals(cave.getWumpus()))
                result.setStench();
            if (cave.isPit(r))
                result.setBreeze();
        }
        if (pos.getRoom().Equals(cave.getGold()))
            result.setGlitter();
        if (bumpedAgents.contains(anAgent))
            result.setBump();
        if (agentJustKillingWumpus != null)
            result.setScream();
        return result;
    }
}
}
