﻿using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
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
        private bool _isWumpusAlive = true;
        private bool isGoldGrabbed;
        private IMap<IAgent, AgentPosition> agentPositions = Factory.CreateMap<IAgent, AgentPosition>();
        private ISet<IAgent> bumpedAgents = Factory.CreateSet<IAgent>();
        private ISet<IAgent> agentsHavingArrow = Factory.CreateSet<IAgent>();
        private IAgent agentJustKillingWumpus;

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
            return _isWumpusAlive;
        }

        public bool isGoalGrabbed()
        {
            return isGoldGrabbed;
        }

        public AgentPosition getAgentPosition(IAgent agent)
        {
            return agentPositions.Get(agent);
        }


        public override void AddAgent(IAgent agent)
        {
            agentPositions.Put(agent, cave.getStart());
            agentsHavingArrow.Add(agent);
            base.AddAgent(agent);
        }


        public override void executeAction(IAgent agent, IAction action)
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
                else if (cave.isPit(newPos.getRoom()) || newPos.getRoom().Equals(cave.getWumpus()) && _isWumpusAlive)
                    agent.SetAlive(false);
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
                if (agentsHavingArrow.Contains(agent) && isAgentFacingWumpus(pos))
                {
                    _isWumpusAlive = false;
                    agentsHavingArrow.Remove(agent);
                    agentJustKillingWumpus = agent;
                }
            }
            else if (action == WumpusAction.CLIMB)
            {
                agent.SetAlive(false);
            }
        }

        private bool isAgentFacingWumpus(AgentPosition pos)
        {
            Room wumpus = cave.getWumpus();

            if (pos.getOrientation().Equals(AgentPosition.Orientation.FACING_NORTH))
            {
                return pos.getX() == wumpus.getX() && pos.getY() < wumpus.getY();
            }
            else if (pos.getOrientation().Equals(AgentPosition.Orientation.FACING_SOUTH))
            {
                return pos.getX() == wumpus.getX() && pos.getY() > wumpus.getY();
            }
            else if (pos.getOrientation().Equals(AgentPosition.Orientation.FACING_EAST))
            {
                return pos.getY() == wumpus.getY() && pos.getX() < wumpus.getX();
            }
            else if (pos.getOrientation().Equals(AgentPosition.Orientation.FACING_WEST))
            {
                return pos.getY() == wumpus.getY() && pos.getX() > wumpus.getX();
            }
            return false;
        }


        public override IPercept getPerceptSeenBy(IAgent anAgent)
        {
            WumpusPercept result = new WumpusPercept();
            AgentPosition pos = agentPositions.Get(anAgent);
            IQueue<Room> adjacentRooms = Factory.CreateQueue<Room>(new[] {
                    new Room(pos.getX() - 1, pos.getY()), new Room(pos.getX() + 1, pos.getY()),
                    new Room(pos.getX(), pos.getY() - 1), new Room(pos.getX(), pos.getY() + 1)
            });
            foreach (Room r in adjacentRooms)
            {
                if (r.Equals(cave.getWumpus()))
                    result.setStench();
                if (cave.isPit(r))
                    result.setBreeze();
            }
            if (pos.getRoom().Equals(cave.getGold()))
                result.setGlitter();
            if (bumpedAgents.Contains(anAgent))
                result.setBump();
            if (agentJustKillingWumpus != null)
                result.setScream();
            return result;
        }
    }
}
