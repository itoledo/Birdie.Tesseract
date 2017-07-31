using tvn.cosine.collections.api;

namespace tvn.cosine.ai.nlp.parsing.grammars
{
    public interface ProbabilisticGrammar
    { 
        bool addRules(ICollection<Rule> ruleList); 
        bool addRule(Rule r); 
        bool validRule(Rule r); 
        bool validateRuleProbabilities(ICollection<Rule> ruleList); 
        void updateVarsAndTerminals(Rule r); 
        void updateVarsAndTerminals();
    }
}
