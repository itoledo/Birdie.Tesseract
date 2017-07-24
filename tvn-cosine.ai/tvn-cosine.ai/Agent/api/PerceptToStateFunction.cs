namespace tvn.cosine.ai.agent.api
{
    public delegate STATE PerceptToStateFunction<PERCEPT, STATE>(PERCEPT percept) where PERCEPT : IPercept;
}
