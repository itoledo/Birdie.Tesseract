using System.Text;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.logic.propositional.inference;
using tvn.cosine.ai.logic.propositional.kb;
using tvn.cosine.ai.logic.propositional.parsing.ast;
using tvn.cosine.ai.search.framework;

namespace tvn.cosine.ai.environment.wumpusworld
{
    /**
     * A Knowledge base tailored to the Wumpus World environment.
     *
     * @author Ciaran O'Reilly
     * @author Ruediger Lunde
     * @author Federico Baron
     * @author Alessandro Daniele
     */
    public class WumpusKnowledgeBase : KnowledgeBase
    {
        public static readonly string LOCATION = "L";
        public static readonly string LOCATION_VISITED = "LV"; // tuning...
        public static readonly string BREEZE = "B";
        public static readonly string STENCH = "S";
        public static readonly string PIT = "P";
        public static readonly string WUMPUS = "W";
        public static readonly string WUMPUS_ALIVE = "WumpusAlive";
        public static readonly string HAVE_ARROW = "HaveArrow";
        public static readonly string FACING_NORTH = AgentPosition.Orientation.FACING_NORTH.getSymbol();
        public static readonly string FACING_SOUTH = AgentPosition.Orientation.FACING_SOUTH.getSymbol();
        public static readonly string FACING_EAST = AgentPosition.Orientation.FACING_EAST.getSymbol();
        public static readonly string FACING_WEST = AgentPosition.Orientation.FACING_WEST.getSymbol();
        public static readonly string PERCEPT_STENCH = "Stench";
        public static readonly string PERCEPT_BREEZE = "Breeze";
        public static readonly string PERCEPT_GLITTER = "Glitter";
        public static readonly string PERCEPT_BUMP = "Bump";
        public static readonly string PERCEPT_SCREAM = "Scream";
        public static readonly string ACTION_FORWARD = WumpusAction.FORWARD.getSymbol();
        public static readonly string ACTION_SHOOT = WumpusAction.SHOOT.getSymbol();
        public static readonly string ACTION_TURN_LEFT = WumpusAction.TURN_LEFT.getSymbol();
        public static readonly string ACTION_TURN_RIGHT = WumpusAction.TURN_RIGHT.getSymbol();
        public static readonly string OK_TO_MOVE_INTO = "OK";

        private int caveXDimension;
        private int caveYDimension;
        private AgentPosition start;
        private DPLL dpll;
        private bool _disableNavSentences;
        private long reasoningTime; // in milliseconds

        public WumpusKnowledgeBase(int caveXDim, int caveYDim)
            : this(caveXDim, caveYDim, new OptimizedDPLL())
        { }

        public WumpusKnowledgeBase(int caveXDim, int caveYDim, DPLL dpll)
            : this(caveXDim, caveYDim, new AgentPosition(1, 1, AgentPosition.Orientation.FACING_NORTH), dpll)
        { }

        /**
         * Create a Knowledge Base that contains the atemporal "wumpus physics".
         *
         * @param dpll     the SAT solver implementation to use for answering 'ask' queries.
         * @param caveXDim x dimensions of the wumpus world's cave.
         * @param caveYDim y dimensions of the wumpus world's cave.
         */
        public WumpusKnowledgeBase(int caveXDim, int caveYDim, AgentPosition start, DPLL dpll)
        {
            this.start = start;
            this.dpll = dpll;
            caveXDimension = caveXDim;
            caveYDimension = caveYDim;
            tellAtemporalPhysicsSentences();
        }

        public int getCaveXDimension()
        {
            return caveXDimension;
        }

        public int getCaveYDimension()
        {
            return caveYDimension;
        }

        /**
         * Disables creation of computational expensive temporal navigation sentences.
         */
        public void disableNavSentences()
        {
            _disableNavSentences = true;
        }

        public AgentPosition askCurrentPosition(int t)
        {
            int locX = -1, locY = -1;
            for (int x = 1; x <= getCaveXDimension() && locX == -1; x++)
            {
                for (int y = 1; y <= getCaveYDimension() && locY == -1; y++)
                {
                    if (ask(newSymbol(LOCATION, t, x, y)))
                    {
                        locX = x;
                        locY = y;
                    }
                }
            }
            if (locX == -1 || locY == -1)
                throw new IllegalStateException("Inconsistent KB, unable to determine current room position.");

            AgentPosition current = null;
            if (ask(newSymbol(FACING_NORTH, t)))
                current = new AgentPosition(locX, locY, AgentPosition.Orientation.FACING_NORTH);
            else if (ask(newSymbol(FACING_SOUTH, t)))
                current = new AgentPosition(locX, locY, AgentPosition.Orientation.FACING_SOUTH);
            else if (ask(newSymbol(FACING_EAST, t)))
                current = new AgentPosition(locX, locY, AgentPosition.Orientation.FACING_EAST);
            else if (ask(newSymbol(FACING_WEST, t)))
                current = new AgentPosition(locX, locY, AgentPosition.Orientation.FACING_WEST);
            else
                throw new IllegalStateException("Inconsistent KB, unable to determine current room orientation.");
            return current;
        }

        // safe <- {[x, y] : ASK(KB, OK<sup>t</sup><sub>x,y</sub>) = true}
        public ISet<Room> askSafeRooms(int t)
        {
            ISet<Room> safe = Factory.CreateSet<Room>();
            for (int x = 1; x <= getCaveXDimension(); x++)
            {
                for (int y = 1; y <= getCaveYDimension(); y++)
                {
                    // Correction: Already visited rooms are safe! This is important because not all pits
                    // can be located by percept. Not-unsafe plan execution adds knowledge about pit and wumpus
                    // locations by surviving dangerous moves but this knowledge is not covered by OK_TO_MOVE_INTO.
                    if (ask(new ComplexSentence(newSymbol(LOCATION_VISITED, x, y),
                            Connective.OR, newSymbol(OK_TO_MOVE_INTO, t, x, y))))
                    {
                        safe.Add(new Room(x, y));
                    }
                }
            }
            return safe;
        }

        // safe <- {[x, y] : ASK(KB, OK<sup>t</sup><sub>x,y</sub>) = true}
        // Optimization: In this version, the agent can provide information about already visited rooms.
        // There is no need to check again.
        public ISet<Room> askSafeRooms(int t, ISet<Room> visited)
        {
            ISet<Room> safe = Factory.CreateSet<Room>();
            for (int x = 1; x <= getCaveXDimension(); x++)
            {
                for (int y = 1; y <= getCaveYDimension(); y++)
                {
                    Room r = new Room(x, y);
                    if (visited.Contains(r) || ask(newSymbol(OK_TO_MOVE_INTO, t, x, y)))
                        safe.Add(new Room(x, y));
                }
            }
            return safe;
        }

        // not_unsafe <- {[x, y] : ASK(KB, ~OK<sup>t</sup><sub>x,y</sub>) = false}
        public ISet<Room> askNotUnsafeRooms(int t)
        {
            return askNotUnsafeRooms(t, Factory.CreateSet<Room>());
        }

        // not_unsafe <- {[x, y] : ASK(KB, ~OK<sup>t</sup><sub>x,y</sub>) = false}
        // Optimization: In this version, the agent can provide information about already visited rooms.
        // There is no need to check again.
        public ISet<Room> askNotUnsafeRooms(int t, ISet<Room> visited)
        {
            ISet<Room> notUnsafe = Factory.CreateSet<Room>();
            for (int x = 1; x <= getCaveXDimension(); x++)
            {
                for (int y = 1; y <= getCaveYDimension(); y++)
                {
                    Room r = new Room(x, y);
                    if (visited.Contains(r) || !ask(new ComplexSentence
                            (Connective.NOT, newSymbol(OK_TO_MOVE_INTO, t, x, y))))
                        notUnsafe.Add(r);
                }
            }
            return notUnsafe;
        }

        public bool askGlitter(int t)
        {
            return ask(newSymbol(PERCEPT_GLITTER, t));
        }

        public bool askHaveArrow(int t)
        {
            return ask(newSymbol(HAVE_ARROW, t));
        }

        // possible_wumpus <- {[x, y] : ASK(KB, ~W<sub>x,y</sub>) = false}
        public ISet<Room> askPossibleWumpusRooms(int t)
        {
            ISet<Room> possible = Factory.CreateSet<Room>();
            for (int x = 1; x <= getCaveXDimension(); x++)
                for (int y = 1; y <= getCaveYDimension(); y++)
                    if (!ask(new ComplexSentence(Connective.NOT, newSymbol(WUMPUS, x, y))))
                        possible.Add(new Room(x, y));
            return possible;
        }

        // unvisited <- {[x, y] : ASK(KB, L<sup>t'</sup><sub>x,y</sub>) = false for all t' &le; t}
        public ISet<Room> askUnvisitedRooms(int t)
        {
            ISet<Room> unvisited = Factory.CreateSet<Room>();

            for (int x = 1; x <= getCaveXDimension(); x++)
            {
                for (int y = 1; y <= getCaveYDimension(); y++)
                {
                    if (!ask(newSymbol(LOCATION_VISITED, x, y)))
                        unvisited.Add(new Room(x, y)); // i.e. is false for all t' <= t

                    //				way to slow: (try it out!)
                    //				for (int tPrime = 0; tPrime <= t; tPrime++) {
                    //					if (ask(newSymbol(LOCATION, tPrime, x, y)))
                    //						break; // i.e. is not false for all t' <= t
                    //					if (tPrime == t)
                    //						unvisited.Add(new Room(x, y)); // i.e. is false for all t' <= t
                    //				}
                }
            }
            return unvisited;
        }

        public bool ask(Sentence query)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            bool result = dpll.isEntailed(this, query);
            sw.Stop();
            reasoningTime += sw.ElapsedMilliseconds;
            return result;
        }

        /**
         * Add to KB sentences that describe the perception p
         * (only about the current time).
         *
         * @param p    perception that must be added to KB
         * @param time current time
         */
        public void makePerceptSentence(WumpusPercept p, int time)
        {
            if (p.isStench())
                tell(newSymbol(PERCEPT_STENCH, time));
            else
                tell(new ComplexSentence(Connective.NOT, newSymbol(PERCEPT_STENCH, time)));

            if (p.isBreeze())
                tell(newSymbol(PERCEPT_BREEZE, time));
            else
                tell(new ComplexSentence(Connective.NOT, newSymbol(PERCEPT_BREEZE, time)));

            if (p.isGlitter())
                tell(newSymbol(PERCEPT_GLITTER, time));
            else
                tell(new ComplexSentence(Connective.NOT, newSymbol(PERCEPT_GLITTER, time)));

            if (p.isBump())
                tell(newSymbol(PERCEPT_BUMP, time));
            else
                tell(new ComplexSentence(Connective.NOT, newSymbol(PERCEPT_BUMP, time)));

            if (p.isScream())
                tell(newSymbol(PERCEPT_SCREAM, time));
            else
                tell(new ComplexSentence(Connective.NOT, newSymbol(PERCEPT_SCREAM, time)));
        }

        /**
         * Add to KB sentences that describe the action a
         *
         * @param a    action that must be added to KB
         * @param time current time
         */
        public void makeActionSentence(WumpusAction a, int time)
        {
            foreach (WumpusAction action in WumpusAction.values())
            {
                if (action.Equals(a))
                    tell(newSymbol(action.getSymbol(), time));
                else
                    tell(new ComplexSentence(Connective.NOT, newSymbol(action.getSymbol(), time)));
            }
        }

        /**
         * TELL the KB the atemporal "physics" sentences (used to initialize the KB).
         */
        protected void tellAtemporalPhysicsSentences()
        {
            //
            // 7.7.1 - The current state of the World
            // The agent knows that the starting square contains no pit
            tell(new ComplexSentence(Connective.NOT, newSymbol(PIT, start.getX(), start.getY())));
            // and no wumpus.
            tell(new ComplexSentence(Connective.NOT, newSymbol(WUMPUS, start.getX(), start.getY())));

            // Atemporal rules about breeze and stench
            // For each square, the agent knows that the square is breezy
            // if and only if a neighboring square has a pit; and a square
            // is smelly if and only if a neighboring square has a wumpus.
            for (int y = 1; y <= caveYDimension; y++)
            {
                for (int x = 1; x <= caveXDimension; x++)
                {

                    IQueue<PropositionSymbol> pitsIn = Factory.CreateQueue<PropositionSymbol>();
                    IQueue<PropositionSymbol> wumpsIn = Factory.CreateQueue<PropositionSymbol>();

                    if (x > 1)
                    { // West room exists
                        pitsIn.Add(newSymbol(PIT, x - 1, y));
                        wumpsIn.Add(newSymbol(WUMPUS, x - 1, y));
                    }
                    if (y < caveYDimension)
                    { // North room exists
                        pitsIn.Add(newSymbol(PIT, x, y + 1));
                        wumpsIn.Add(newSymbol(WUMPUS, x, y + 1));
                    }
                    if (x < caveXDimension)
                    { // East room exists
                        pitsIn.Add(newSymbol(PIT, x + 1, y));
                        wumpsIn.Add(newSymbol(WUMPUS, x + 1, y));
                    }
                    if (y > 1)
                    { // South room exists
                        pitsIn.Add(newSymbol(PIT, x, y - 1));
                        wumpsIn.Add(newSymbol(WUMPUS, x, y - 1));
                    }

                    tell(new ComplexSentence
                            (newSymbol(BREEZE, x, y), Connective.BICONDITIONAL, Sentence.newDisjunction(pitsIn.ToArray())));
                    tell(new ComplexSentence
                            (newSymbol(STENCH, x, y), Connective.BICONDITIONAL, Sentence.newDisjunction(wumpsIn.ToArray())));
                }
            }

            // The agent also knows there is exactly one wumpus. This is represented
            // in two parts. First, we have to say that there is at least one wumpus
            IQueue<PropositionSymbol> wumpsAtLeast = Factory.CreateQueue<PropositionSymbol>();
            for (int x = 1; x <= caveXDimension; x++)
                for (int y = 1; y <= caveYDimension; y++)
                    wumpsAtLeast.Add(newSymbol(WUMPUS, x, y));

            tell(Sentence.newDisjunction(wumpsAtLeast.ToArray()));

            // Then, we have to say that there is at most one wumpus.
            // For each pair of locations, we add a sentence saying
            // that at least one of them must be wumpus-free.
            int numRooms = (caveXDimension * caveYDimension);
            for (int i = 0; i < numRooms;++i)
            {
                for (int j = i + 1; j < numRooms; j++)
                {
                    tell(new ComplexSentence(Connective.OR,
                            new ComplexSentence
                                    (Connective.NOT, newSymbol(WUMPUS, (i / caveXDimension) + 1, (i % caveYDimension) + 1)),
                            new ComplexSentence
                                    (Connective.NOT, newSymbol(WUMPUS, (j / caveXDimension) + 1, (j % caveYDimension) + 1))));
                }
            }
        }

        /**
         * TELL the KB the temporal "physics" sentences for time t.
         * As in this version, the agent does not communicate its current position
         * to the knowledge base, general navigation axioms are needed, which
         * entail the current position. Therefore, navigation sentences are always
         * added, independent of the value of {@link #disableNavSentences}.
         *
         * @param t current time step.
         */
        public void tellTemporalPhysicsSentences(int t)
        {
            if (t == 0)
            {
                // temporal rules at time zero
                tell(newSymbol(LOCATION, 0, start.getX(), start.getY()));
                tell(newSymbol(start.getOrientation().getSymbol(), 0));
                tell(newSymbol(HAVE_ARROW, 0));
                tell(newSymbol(WUMPUS_ALIVE, 0));
                // Optimization: Make questions about unvisited locations faster
                tell(newSymbol(LOCATION_VISITED, start.getX(), start.getY()));
            }

            // We can connect stench and breeze percepts directly
            // to the properties of the squares where they are experienced
            // through the location fluent as follows. For any time step t
            // and any square [x,y], we assert
            for (int x = 1; x <= caveXDimension; x++)
            {
                for (int y = 1; y <= caveYDimension; y++)
                {
                    tell(new ComplexSentence(
                            newSymbol(LOCATION, t, x, y),
                            Connective.IMPLICATION,
                            new ComplexSentence(newSymbol(PERCEPT_BREEZE, t), Connective.BICONDITIONAL, newSymbol(BREEZE, x, y))));

                    tell(new ComplexSentence(
                            newSymbol(LOCATION, t, x, y),
                            Connective.IMPLICATION,
                            new ComplexSentence(newSymbol(PERCEPT_STENCH, t), Connective.BICONDITIONAL, newSymbol(STENCH, x, y))));
                }
            }

            tellCommonTemporalPhysicsSentences(t);
            for (int x = 1; x <= caveXDimension; x++)
            {
                for (int y = 1; y <= caveYDimension; y++)
                {
                    tellSuccessorStateLocationAxiom(t, x, y);
                    // Optimization to make questions about unvisited locations faster
                    tell(new ComplexSentence(
                            newSymbol(LOCATION, t + 1, x, y),
                            Connective.IMPLICATION,
                            newSymbol(LOCATION_VISITED, x, y)));
                }
                tellSuccessorStateOrientationAxioms(t);
            }
        }

        /**
         * TELL the KB the temporal "physics" sentences for time t.
         * This version profits from the agent's knowledge about its current position.
         * Verbosity of the created sentences depends on the value of {@link #disableNavSentences}.
         *
         * @param t current time step.
         */
        public void tellTemporalPhysicsSentences(int t, AgentPosition agentPosition)
        {
            if (t == 0)
            {
                // temporal rules at time zero
                tell(newSymbol(HAVE_ARROW, 0));
                tell(newSymbol(WUMPUS_ALIVE, 0));
            }
            tell(newSymbol(LOCATION, t, agentPosition.getX(), agentPosition.getY()));
            tell(newSymbol(agentPosition.getOrientation().getSymbol(), t));
            // Optimization to make questions about unvisited locations faster
            tell(newSymbol(LOCATION_VISITED, agentPosition.getX(), agentPosition.getY()));

            // We can connect stench and breeze percepts directly
            // to the properties of the squares where they are experienced
            // through the location fluent as follows. For any time step t
            // and any square [x,y], we assert
            tell(new ComplexSentence(
                    newSymbol(LOCATION, t, agentPosition.getX(), agentPosition.getY()),
                    Connective.IMPLICATION,
                    new ComplexSentence(newSymbol(PERCEPT_BREEZE, t), Connective.BICONDITIONAL,
                            newSymbol(BREEZE, agentPosition.getX(), agentPosition.getY()))));

            tell(new ComplexSentence(
                    newSymbol(LOCATION, t, agentPosition.getX(), agentPosition.getY()),
                    Connective.IMPLICATION,
                    new ComplexSentence(newSymbol(PERCEPT_STENCH, t), Connective.BICONDITIONAL,
                            newSymbol(STENCH, agentPosition.getX(), agentPosition.getY()))));

            tellCommonTemporalPhysicsSentences(t);
            if (!_disableNavSentences)
            {
                tellSuccessorStateLocationAxiom(t, agentPosition.getX(), agentPosition.getY());
                tellSuccessorStateOrientationAxioms(t);
            }
        }

        private void tellCommonTemporalPhysicsSentences(int t)
        {
            for (int x = 1; x <= caveXDimension; x++)
            {
                for (int y = 1; y <= caveYDimension; y++)
                {
                    // The most important question for the agent is whether
                    // a square is OK to move into, that is, the square contains
                    // no pit nor live wumpus.
                    tell(new ComplexSentence(
                            newSymbol(OK_TO_MOVE_INTO, t, x, y),
                            Connective.BICONDITIONAL,
                            // Optimization idea: Do not create OK sentences. Instead, ASK the following sentence
                            new ComplexSentence(
                                    new ComplexSentence(Connective.NOT, newSymbol(PIT, x, y)),
                                    Connective.AND,
                                    new ComplexSentence(Connective.NOT,
                                            new ComplexSentence(
                                                    newSymbol(WUMPUS, x, y),
                                                    Connective.AND,
                                                    newSymbol(WUMPUS_ALIVE, t))))));
                }
            }

            // Rule about the arrow
            tell(new ComplexSentence(
                    newSymbol(HAVE_ARROW, t + 1),
                    Connective.BICONDITIONAL,
                    new ComplexSentence(
                            newSymbol(HAVE_ARROW, t),
                            Connective.AND,
                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_SHOOT, t)))));

            // Rule about wumpus (dead or alive)
            tell(new ComplexSentence(
                    newSymbol(WUMPUS_ALIVE, t + 1),
                    Connective.BICONDITIONAL,
                    new ComplexSentence(
                            newSymbol(WUMPUS_ALIVE, t),
                            Connective.AND,
                            new ComplexSentence(Connective.NOT, newSymbol(PERCEPT_SCREAM, t + 1)))));

        }

        private void tellSuccessorStateLocationAxiom(int t, int x, int y)
        {
            // Successor state axiom for square [x, y]
            // Rules about current location
            IQueue<Sentence> locDisjuncts = Factory.CreateQueue<Sentence>();
            locDisjuncts.Add(new ComplexSentence(
                    newSymbol(LOCATION, t, x, y),
                    Connective.AND,
                    new ComplexSentence(
                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_FORWARD, t)),
                            Connective.OR,
                            newSymbol(PERCEPT_BUMP, t + 1))));
            if (x > 1)
            { // West room is possible
                locDisjuncts.Add(new ComplexSentence(
                        newSymbol(LOCATION, t, x - 1, y),
                        Connective.AND,
                        new ComplexSentence(
                                newSymbol(FACING_EAST, t),
                                Connective.AND,
                                newSymbol(ACTION_FORWARD, t))));
            }
            if (y < caveYDimension)
            { // North room is possible
                locDisjuncts.Add(new ComplexSentence(
                        newSymbol(LOCATION, t, x, y + 1),
                        Connective.AND,
                        new ComplexSentence(
                                newSymbol(FACING_SOUTH, t),
                                Connective.AND,
                                newSymbol(ACTION_FORWARD, t))));
            }
            if (x < caveXDimension)
            { // East room is possible
                locDisjuncts.Add(new ComplexSentence(
                        newSymbol(LOCATION, t, x + 1, y),
                        Connective.AND,
                        new ComplexSentence(
                                newSymbol(FACING_WEST, t),
                                Connective.AND,
                                newSymbol(ACTION_FORWARD, t))));
            }
            if (y > 1)
            { // South room is possible
                locDisjuncts.Add(new ComplexSentence(
                        newSymbol(LOCATION, t, x, y - 1),
                        Connective.AND,
                        new ComplexSentence(
                                newSymbol(FACING_NORTH, t),
                                Connective.AND,
                                newSymbol(ACTION_FORWARD, t))));
            }

            tell(new ComplexSentence(
                    newSymbol(LOCATION, t + 1, x, y),
                    Connective.BICONDITIONAL,
                    Sentence.newDisjunction(locDisjuncts)));
        }

        private void tellSuccessorStateOrientationAxioms(int t)
        {
            //
            // Successor state axioms (independent of location)
            // Rules about current orientation
            // Facing North
            tell(new ComplexSentence(
                    newSymbol(FACING_NORTH, t + 1),
                    Connective.BICONDITIONAL,
                    Sentence.newDisjunction(
                            new ComplexSentence(newSymbol(FACING_WEST, t), Connective.AND, newSymbol(ACTION_TURN_RIGHT, t)),
                            new ComplexSentence(newSymbol(FACING_EAST, t), Connective.AND, newSymbol(ACTION_TURN_LEFT, t)),
                            new ComplexSentence(newSymbol(FACING_NORTH, t),
                                    Connective.AND,
                                    new ComplexSentence(
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_LEFT, t)),
                                            Connective.AND,
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_RIGHT, t))))
                    )));
            // Facing South
            tell(new ComplexSentence(
                    newSymbol(FACING_SOUTH, t + 1),
                    Connective.BICONDITIONAL,
                    Sentence.newDisjunction(
                            new ComplexSentence(newSymbol(FACING_WEST, t), Connective.AND, newSymbol(ACTION_TURN_LEFT, t)),
                            new ComplexSentence(newSymbol(FACING_EAST, t), Connective.AND, newSymbol(ACTION_TURN_RIGHT, t)),
                            new ComplexSentence(newSymbol(FACING_SOUTH, t),
                                    Connective.AND,
                                    new ComplexSentence(
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_LEFT, t)),
                                            Connective.AND,
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_RIGHT, t))))
                    )));
            // Facing East
            tell(new ComplexSentence(
                    newSymbol(FACING_EAST, t + 1),
                    Connective.BICONDITIONAL,
                    Sentence.newDisjunction(
                            new ComplexSentence(newSymbol(FACING_NORTH, t), Connective.AND, newSymbol(ACTION_TURN_RIGHT, t)),
                            new ComplexSentence(newSymbol(FACING_SOUTH, t), Connective.AND, newSymbol(ACTION_TURN_LEFT, t)),
                            new ComplexSentence(newSymbol(FACING_EAST, t),
                                    Connective.AND,
                                    new ComplexSentence(
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_LEFT, t)),
                                            Connective.AND,
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_RIGHT, t))))
                    )));
            // Facing West
            tell(new ComplexSentence(
                    newSymbol(FACING_WEST, t + 1),
                    Connective.BICONDITIONAL,
                    Sentence.newDisjunction(
                            new ComplexSentence(newSymbol(FACING_NORTH, t), Connective.AND, newSymbol(ACTION_TURN_LEFT, t)),
                            new ComplexSentence(newSymbol(FACING_SOUTH, t), Connective.AND, newSymbol(ACTION_TURN_RIGHT, t)),
                            new ComplexSentence(newSymbol(FACING_WEST, t),
                                    Connective.AND,
                                    new ComplexSentence(
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_LEFT, t)),
                                            Connective.AND,
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_RIGHT, t))))
                    )));
        }


        public override string ToString()
        {
            IQueue<Sentence> sentences = getSentences();
            if (sentences.Size() == 0)
            {
                return "";
            }
            else
            {
                bool first = true;
                StringBuilder sb = new StringBuilder();
                foreach (Sentence s in sentences)
                {
                    if (!first)
                    {
                        sb.Append("\n");
                    }
                    sb.Append(s.ToString());
                    first = false;
                }
                return sb.ToString();
            }
        }

        public PropositionSymbol newSymbol(string prefix, int timeStep)
        {
            return new PropositionSymbol(prefix + "_" + timeStep);
        }

        public PropositionSymbol newSymbol(string prefix, int x, int y)
        {
            return new PropositionSymbol(prefix + "_" + x + "_" + y);
        }

        public PropositionSymbol newSymbol(string prefix, int timeStep, int x, int y)
        {
            return newSymbol(newSymbol(prefix, timeStep).ToString(), x, y);
        }

        public Metrics getMetrics()
        {
            Metrics result = new Metrics();
            result.set("kb.size", size());
            result.set("kb.sym.size", getSymbols().Size());
            result.set("kb.cnf.size", asCNF().Size());
            result.set("reasoning.time[s]", reasoningTime / 1000);
            return result;
        }
    }
}
