namespace tvn_cosine.ai.test.unit.environment.eightpuzzle
{
    public class MisplacedTileHeuristicFunctionTest
    {

        @Test
        public void testHeuristicCalculation()
        {
            ToDoubleFunction<Node<EightPuzzleBoard, Action>> h =
                    EightPuzzleFunctions.createMisplacedTileHeuristicFunction();
            EightPuzzleBoard board = new EightPuzzleBoard(new int[] { 2, 0, 5, 6,
                4, 8, 3, 7, 1 });
            Assert.assertEquals(6.0, h.applyAsDouble(new Node<>(board)), 0.001);

            board = new EightPuzzleBoard(new int[] { 6, 2, 5, 3, 4, 8, 0, 7, 1 });
            Assert.assertEquals(5.0, h.applyAsDouble(new Node<>(board)), 0.001);

            board = new EightPuzzleBoard(new int[] { 6, 2, 5, 3, 4, 8, 7, 0, 1 });
            Assert.assertEquals(6.0, h.applyAsDouble(new Node<>(board)), 0.001);

            board = new EightPuzzleBoard(new int[] { 8, 1, 2, 3, 4, 5, 6, 7, 0 });
            Assert.assertEquals(1.0, h.applyAsDouble(new Node<>(board)), 0.001);

            board = new EightPuzzleBoard(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
            Assert.assertEquals(0.0, h.applyAsDouble(new Node<>(board)), 0.001);
        }
    }
}
