using tvn.cosine.ai.agent.api;

namespace tvn.cosine.ai.agent
{
    public delegate STATE PerceptToStateFunction<PERCEPT, STATE>(PERCEPT percept) where PERCEPT : IPercept;
}
