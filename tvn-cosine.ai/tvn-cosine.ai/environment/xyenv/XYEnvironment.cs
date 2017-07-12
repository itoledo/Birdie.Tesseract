using System.Collections.Generic;
using System.Diagnostics;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.environment.xyenv
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class XYEnvironment : AbstractEnvironment
    {
        private XYEnvironmentState envState;

        //
        // PUBLIC METHODS
        //
        public XYEnvironment(int width, int height)
        {
            Debug.Assert(width > 0);
            Debug.Assert(height > 0);

            envState = new XYEnvironmentState(width, height);
        }

        /** Does nothing (don't ask me why...). */

        public override void executeAction(IAgent a, IAction action)
        {
        }

        public override IPercept getPerceptSeenBy(IAgent anAgent)
        {
            return new DynamicPercept();
        }

        public void addObjectToLocation(IEnvironmentObject eo, XYLocation loc)
        {
            moveObjectToAbsoluteLocation(eo, loc);
        }

        public void moveObjectToAbsoluteLocation(IEnvironmentObject eo, XYLocation loc)
        {
            // Ensure the object is not already at a location
            envState.moveObjectToAbsoluteLocation(eo, loc);

            // Ensure is added to the environment
            AddEnvironmentObject(eo);
        }

        public void moveObject(IEnvironmentObject eo, XYLocation.Direction direction)
        {
            XYLocation presentLocation = envState.getCurrentLocationFor(eo);

            if (null != presentLocation)
            {
                XYLocation locationToMoveTo = presentLocation.LocationAt(direction);
                if (!(isBlocked(locationToMoveTo)))
                {
                    moveObjectToAbsoluteLocation(eo, locationToMoveTo);
                }
            }
        }

        public XYLocation getCurrentLocationFor(IEnvironmentObject eo)
        {
            return envState.getCurrentLocationFor(eo);
        }

        public ISet<IEnvironmentObject> getObjectsAt(XYLocation loc)
        {
            return envState.getObjectsAt(loc);
        }

        public ISet<IEnvironmentObject> getObjectsNear(IAgent agent, int radius)
        {
            return envState.getObjectsNear(agent, radius);
        }

        public bool isBlocked(XYLocation loc)
        {
            foreach (IEnvironmentObject eo in envState.getObjectsAt(loc))
            {
                if (eo is Wall)
                {
                    return true;
                }
            }
            return false;
        }

        public void makePerimeter()
        {
            for (int i = 0; i < envState.width; i++)
            {
                XYLocation loc = new XYLocation(i, 0);
                XYLocation loc2 = new XYLocation(i, envState.height - 1);
                envState.moveObjectToAbsoluteLocation(new Wall(), loc);
                envState.moveObjectToAbsoluteLocation(new Wall(), loc2);
            }

            for (int i = 0; i < envState.height; i++)
            {
                XYLocation loc = new XYLocation(0, i);
                XYLocation loc2 = new XYLocation(envState.width - 1, i);
                envState.moveObjectToAbsoluteLocation(new Wall(), loc);
                envState.moveObjectToAbsoluteLocation(new Wall(), loc2);
            }
        }
    }

    class XYEnvironmentState : IEnvironmentState
    {
        public int width;
        public int height;

        private IDictionary<XYLocation, ISet<IEnvironmentObject>> objsAtLocation = new Dictionary<XYLocation, ISet<IEnvironmentObject>>();

        public XYEnvironmentState(int width, int height)
        {
            this.width = width;
            this.height = height;
            for (int h = 1; h <= height; h++)
            {
                for (int w = 1; w <= width; w++)
                {
                    objsAtLocation.Add(new XYLocation(h, w), new HashSet<IEnvironmentObject>());
                }
            }
        }

        public void moveObjectToAbsoluteLocation(IEnvironmentObject eo, XYLocation loc)
        {
            // Ensure is not already at another location
            foreach (ISet<IEnvironmentObject> eos in objsAtLocation.Values)
            {
                if (eos.Remove(eo))
                {
                    break; // Should only every be at 1 location
                }
            }
            // Add it to the location specified
            getObjectsAt(loc).Add(eo);
        }

        public ISet<IEnvironmentObject> getObjectsAt(XYLocation loc)
        {
            ISet<IEnvironmentObject> objectsAt = objsAtLocation[loc];
            if (null == objectsAt)
            {
                // Always ensure an empty Set is returned
                objectsAt = new HashSet<IEnvironmentObject>();
                objsAtLocation.Add(loc, objectsAt);
            }
            return objectsAt;
        }

        public XYLocation getCurrentLocationFor(IEnvironmentObject eo)
        {
            foreach (XYLocation loc in objsAtLocation.Keys)
            {
                if (objsAtLocation[loc].Contains(eo))
                {
                    return loc;
                }
            }
            return null;
        }

        public ISet<IEnvironmentObject> getObjectsNear(IAgent agent, int radius)
        {
            ISet<IEnvironmentObject> objsNear = new HashSet<IEnvironmentObject>();

            XYLocation agentLocation = getCurrentLocationFor(agent);
            foreach (XYLocation loc in objsAtLocation.Keys)
            {
                if (withinRadius(radius, agentLocation, loc))
                {
                    foreach (var v in objsAtLocation[loc])
                        objsNear.Add(v);
                }
            }
            // Ensure the 'agent' is not included in the Set of
            // objects near
            objsNear.Remove(agent);

            return objsNear;
        }

        public override string ToString()
        {
            return "XYEnvironmentState:" + objsAtLocation.ToString();
        }

        //
        // PRIVATE METHODS
        //
        private bool withinRadius(int radius, XYLocation agentLocation, XYLocation objectLocation)
        {
            int xdifference = agentLocation.X - objectLocation.X;
            int ydifference = agentLocation.Y - objectLocation.Y;
            return System.Math.Sqrt((xdifference * xdifference) + (ydifference * ydifference)) <= radius;
        }
    }
}
