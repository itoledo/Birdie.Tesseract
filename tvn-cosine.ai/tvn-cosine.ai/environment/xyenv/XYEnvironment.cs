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

        public override void executeAction(Agent a, Action action)
        {
        }

        public override Percept getPerceptSeenBy(Agent anAgent)
        {
            return new DynamicPercept();
        }

        public void addObjectToLocation(EnvironmentObject eo, XYLocation loc)
        {
            moveObjectToAbsoluteLocation(eo, loc);
        }

        public void moveObjectToAbsoluteLocation(EnvironmentObject eo, XYLocation loc)
        {
            // Ensure the object is not already at a location
            envState.moveObjectToAbsoluteLocation(eo, loc);

            // Ensure is added to the environment
            addEnvironmentObject(eo);
        }

        public void moveObject(EnvironmentObject eo, XYLocation.Direction direction)
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

        public XYLocation getCurrentLocationFor(EnvironmentObject eo)
        {
            return envState.getCurrentLocationFor(eo);
        }

        public ISet<EnvironmentObject> getObjectsAt(XYLocation loc)
        {
            return envState.getObjectsAt(loc);
        }

        public ISet<EnvironmentObject> getObjectsNear(Agent agent, int radius)
        {
            return envState.getObjectsNear(agent, radius);
        }

        public bool isBlocked(XYLocation loc)
        {
            foreach (EnvironmentObject eo in envState.getObjectsAt(loc))
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

    class XYEnvironmentState : EnvironmentState
    {
        public int width;
        public int height;

        private IDictionary<XYLocation, ISet<EnvironmentObject>> objsAtLocation = new Dictionary<XYLocation, ISet<EnvironmentObject>>();

        public XYEnvironmentState(int width, int height)
        {
            this.width = width;
            this.height = height;
            for (int h = 1; h <= height; h++)
            {
                for (int w = 1; w <= width; w++)
                {
                    objsAtLocation.Add(new XYLocation(h, w), new HashSet<EnvironmentObject>());
                }
            }
        }

        public void moveObjectToAbsoluteLocation(EnvironmentObject eo, XYLocation loc)
        {
            // Ensure is not already at another location
            foreach (ISet<EnvironmentObject> eos in objsAtLocation.Values)
            {
                if (eos.Remove(eo))
                {
                    break; // Should only every be at 1 location
                }
            }
            // Add it to the location specified
            getObjectsAt(loc).Add(eo);
        }

        public ISet<EnvironmentObject> getObjectsAt(XYLocation loc)
        {
            ISet<EnvironmentObject> objectsAt = objsAtLocation[loc];
            if (null == objectsAt)
            {
                // Always ensure an empty Set is returned
                objectsAt = new HashSet<EnvironmentObject>();
                objsAtLocation.Add(loc, objectsAt);
            }
            return objectsAt;
        }

        public XYLocation getCurrentLocationFor(EnvironmentObject eo)
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

        public ISet<EnvironmentObject> getObjectsNear(Agent agent, int radius)
        {
            ISet<EnvironmentObject> objsNear = new HashSet<EnvironmentObject>();

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
