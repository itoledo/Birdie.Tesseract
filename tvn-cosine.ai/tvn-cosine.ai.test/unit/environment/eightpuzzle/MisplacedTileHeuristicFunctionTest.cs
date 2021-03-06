﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.environment.eightpuzzle;
using tvn.cosine.ai.search.framework; 
using tvn.cosine.ai.util.api;

namespace tvn_cosine.ai.test.unit.environment.eightpuzzle
{
    [TestClass]
    public class MisplacedTileHeuristicFunctionTest
    {

        [TestMethod]
        public void testHeuristicCalculation()
        {
            IToDoubleFunction<Node<EightPuzzleBoard, IAction>> h =
                    EightPuzzleFunctions.createMisplacedTileHeuristicFunction();
            EightPuzzleBoard board = new EightPuzzleBoard(new int[] { 2, 0, 5, 6,
                4, 8, 3, 7, 1 });
            Assert.AreEqual(6.0, h.applyAsDouble(new Node<EightPuzzleBoard, IAction>(board)), 0.001);

            board = new EightPuzzleBoard(new int[] { 6, 2, 5, 3, 4, 8, 0, 7, 1 });
            Assert.AreEqual(5.0, h.applyAsDouble(new Node<EightPuzzleBoard, IAction>(board)), 0.001);

            board = new EightPuzzleBoard(new int[] { 6, 2, 5, 3, 4, 8, 7, 0, 1 });
            Assert.AreEqual(6.0, h.applyAsDouble(new Node<EightPuzzleBoard, IAction>(board)), 0.001);

            board = new EightPuzzleBoard(new int[] { 8, 1, 2, 3, 4, 5, 6, 7, 0 });
            Assert.AreEqual(1.0, h.applyAsDouble(new Node<EightPuzzleBoard, IAction>(board)), 0.001);

            board = new EightPuzzleBoard(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
            Assert.AreEqual(0.0, h.applyAsDouble(new Node<EightPuzzleBoard, IAction>(board)), 0.001);
        }
    }
}
