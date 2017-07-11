using System.Collections.Generic;

namespace tvn.cosine.ai.nlp.parsing.grammers
{
    public interface ProbabilisticGrammar
    { 
       bool addRules(IList<Rule> ruleList);
       
       bool addRule(Rule r);
       
       bool validRule(Rule r);
       
       bool validateRuleProbabilities(IList<Rule> ruleList);
       
       void updateVarsAndTerminals(Rule r);
       
       void updateVarsAndTerminals();
    }

}
