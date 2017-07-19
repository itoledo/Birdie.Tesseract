using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.nlp.parsing.grammars
{
    public interface ProbabilisticGrammar
    { 
        bool addRules(IQueue<Rule> ruleList); 
        bool addRule(Rule r); 
        bool validRule(Rule r); 
        bool validateRuleProbabilities(IQueue<Rule> ruleList); 
        void updateVarsAndTerminals(Rule r); 
        void updateVarsAndTerminals();
    }
}
