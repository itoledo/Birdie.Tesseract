using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.search.csp;

namespace tvn_cosine.ai.test.unit.search.csp
{
    [TestClass]
    public class AssignmentTest
    {
        private static readonly Variable X = new Variable("x");
        private static readonly Variable Y = new Variable("y");

        private ICollection<Variable> variables;
        private Assignment<Variable, string> assignment;

        [TestInitialize]
        public void setUp()
        {
            variables = CollectionFactory.CreateQueue<Variable>();
            variables.Add(X);
            variables.Add(Y);
            assignment = new Assignment<Variable, string>();
        }

        [TestMethod]
        public void testAssignmentCompletion()
        {
            Assert.IsFalse(assignment.isComplete(variables));
            assignment.add(X, "Ravi");
            Assert.IsFalse(assignment.isComplete(variables));
            assignment.add(Y, "AIMA");
            Assert.IsTrue(assignment.isComplete(variables));
            assignment.remove(X);
            Assert.IsFalse(assignment.isComplete(variables));
        }

        // [TestMethod]
        // public void testAssignmentDefaultVariableSelection() {
        // Assert.AreEqual(X, assignment.selectFirstUnassignedVariable(csp));
        // assignment.Add(X, "Ravi");
        // Assert.AreEqual(Y, assignment.selectFirstUnassignedVariable(csp));
        // assignment.Add(Y, "AIMA");
        // Assert.AreEqual(null, assignment.selectFirstUnassignedVariable(csp));
        // }
    }
}
