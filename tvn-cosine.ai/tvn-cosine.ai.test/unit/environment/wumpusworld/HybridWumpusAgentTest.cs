using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.environment.wumpusworld;

namespace tvn_cosine.ai.test.unit.environment.wumpusworld
{
////    [TestClass]
////    public class HybridWumpusAgentTest
////    {
////        [TestMethod]
////        public void testPlanRoute()
////        {
////            HybridWumpusAgent hwa = new HybridWumpusAgent();
////            ISet<Room> allowed = Factory.CreateSet<Room>();
////            allowed.Add(new Room(1, 1));
////            // Should be a NoOp plan as we are already at the goal.
////            Assert.AreEqual(Factory.CreateQueue<Action>(),
////                hwa.planRoute(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST), allowed),

////                allRooms(4)
////        );
////            Assert.AreEqual(Factory.CreateQueue<>(
////                    new TurnLeft(AgentPosition.Orientation.FACING_EAST),
////                    new TurnLeft(AgentPosition.Orientation.FACING_NORTH),
////                    new Forward(new AgentPosition(2, 1, AgentPosition.Orientation.FACING_WEST))
////                ),
////                hwa.planRoute(new AgentPosition(2, 1, AgentPosition.Orientation.FACING_EAST),
////                    Factory.CreateSet<Room>() {{

////                    add(new Room(1,1));
////        }
////    },

////                allRooms(4)
////		));
////		Assert.AreEqual(Factory.CreateQueue<>(
////				new TurnLeft(AgentPosition.Orientation.FACING_EAST),
////				new Forward(new AgentPosition(3, 1, AgentPosition.Orientation.FACING_NORTH)),
////				new Forward(new AgentPosition(3, 2, AgentPosition.Orientation.FACING_NORTH)),
////				new TurnLeft(AgentPosition.Orientation.FACING_NORTH),
////				new Forward(new AgentPosition(3, 3, AgentPosition.Orientation.FACING_WEST)),
////				new Forward(new AgentPosition(2, 3, AgentPosition.Orientation.FACING_WEST)),
////				new TurnLeft(AgentPosition.Orientation.FACING_WEST),
////				new Forward(new AgentPosition(1, 3, AgentPosition.Orientation.FACING_SOUTH)),
////				new Forward(new AgentPosition(1, 2, AgentPosition.Orientation.FACING_SOUTH))
////			), 
////			hwa.planRoute(new AgentPosition(3, 1, AgentPosition.Orientation.FACING_EAST), 
////				Factory.CreateSet<Room>() {{

////                    add(new Room(1,1));
////				}},
////				Factory.CreateSet<Room>() {{

////                    addAll(allRooms(4));

////                    remove(new Room(2, 1));

////                    remove(new Room(2, 2));
////				}}
////	    ));
////	}


     

////    [TestMethod]
////public void testPlanShot()
////{
////    HybridWumpusAgent hwa = new HybridWumpusAgent(4);
////    // Should be just shoot as are facing the Wumpus
////    Assert.AreEqual(Factory.CreateQueue<>(new Shoot()),
////        hwa.planShot(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST),
////            Factory.CreateSet<Room>() {{
////                    add(new Room(3,1));
////}},

////                allRooms(4)
////		));	
////		Assert.AreEqual(Factory.CreateQueue<>(
////				new TurnLeft(AgentPosition.Orientation.FACING_EAST),
////				new Shoot()
////			), 
////			hwa.planShot(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST), 
////				Factory.CreateSet<Room>() {{

////                    add(new Room(1,2));
////				}},

////                allRooms(4)
////		));	
////		Assert.AreEqual(Factory.CreateQueue<>(
////				new Forward(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST)),
////				new TurnLeft(AgentPosition.Orientation.FACING_EAST),
////				new Shoot()
////			), 
////			hwa.planShot(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST), 
////				Factory.CreateSet<Room>() {{

////                    add(new Room(2,2));
////				}},

////                allRooms(4)
////		));	
////	}
	
////	[TestMethod]
////public void testGrabAndClimb()
////{
////    HybridWumpusAgent hwa = new HybridWumpusAgent(2);
////    // The gold is in the first square
////    Action a = hwa.execute(new AgentPercept(true, true, true, false, false));
////    Assert.IsTrue(a is Grab);
////    a = hwa.execute(new AgentPercept(true, true, true, false, false));
////    Assert.IsTrue(a is Climb);
////}

////private static ISet<Room> allRooms(int caveXandYDimensions)
////{
////    ISet<Room> allRooms = Factory.CreateSet<Room>();
////    for (int x = 1; x <= caveXandYDimensions; x++)
////    {
////        for (int y = 1; y <= caveXandYDimensions; y++)
////        {
////            allRooms.Add(new Room(x, y));
////        }
////    }

////    return allRooms;
////}
////}

}
