using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.robotics.datatypes
{
    /**
     * A {@code RobotException} may be thrown by a class implementing {@link IMclRobot} during any actions invoked on the robot in case something has gone wrong and the localization should be halted.
     * 
     * @author Arno von Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     */
    public class RobotException : Exception
    {
        public RobotException(string message)
            : base(message)
        {

        }
    }
}
