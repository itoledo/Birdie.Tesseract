using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.environment.wumpusworld;
using tvn.cosine.ai.environment.wumpusworld.action;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.problem.api;

namespace tvn_cosine.ai.test.unit.environment.wumpusworld
{
    [TestClass]
    public class WumpusFunctionsTest
    {
        private IActionsFunction<AgentPosition, IAction> actionFn;
        private IResultFunction<AgentPosition, IAction> resultFn;

        [TestInitialize]
        public void setUp()
        {
            WumpusCave completeCave = new WumpusCave(4, 4);

            actionFn = WumpusFunctionFunctions.createActionsFunction(completeCave);
            resultFn = WumpusFunctionFunctions.createResultFunction();
        }

        [TestMethod]
        public void testSuccessors()
        {
            ICollection<AgentPosition> succPositions = CollectionFactory.CreateQueue<AgentPosition>();
            ICollection<AgentPosition.Orientation> succOrientation = CollectionFactory.CreateQueue<AgentPosition.Orientation>();

            // From every position the possible actions are:
            //    - Turn right (change orientation, not position)
            //    - Turn left (change orientation, not position)
            //    - Forward (change position, not orientation)
            AgentPosition P11U = new AgentPosition(1, 1, AgentPosition.Orientation.FACING_NORTH);
            succPositions.Add(new AgentPosition(1, 2, AgentPosition.Orientation.FACING_NORTH));
            succOrientation.Add(AgentPosition.Orientation.FACING_EAST);
            succOrientation.Add(AgentPosition.Orientation.FACING_WEST);
            foreach (IAction a in actionFn.apply(P11U))
            {
                if (a is Forward)
                {
                    Assert.IsTrue(succPositions.Contains(((Forward)a).getToPosition()));
                    Assert.IsTrue(succPositions.Contains(resultFn.apply(P11U, a)));
                }
                else if (a is TurnLeft)
                {
                    Assert.IsTrue(succOrientation.Contains(((TurnLeft)a).getToOrientation()));
                    Assert.AreEqual("[1,1]->FacingWest", resultFn.apply(P11U, a).ToString());
                }
                else if (a is TurnRight)
                {
                    Assert.IsTrue(succOrientation.Contains(((TurnRight)a).getToOrientation()));
                    Assert.AreEqual("[1,1]->FacingEast", resultFn.apply(P11U, a).ToString());
                }
            }


            //If you are in front of a wall forward action is not possible
            AgentPosition P31D = new AgentPosition(3, 1, AgentPosition.Orientation.FACING_SOUTH);
            AgentPosition P41R = new AgentPosition(4, 1, AgentPosition.Orientation.FACING_EAST);
            foreach (IAction a in actionFn.apply(P31D))
            {
                Assert.IsFalse(a is Forward);
            }

            foreach (IAction a in actionFn.apply(P41R))
            {
                Assert.IsFalse(a is Forward);
            }
        }
    } 
}
