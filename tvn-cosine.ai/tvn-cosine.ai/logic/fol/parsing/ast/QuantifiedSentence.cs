namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    public class QuantifiedSentence : Sentence
    {

    private string quantifier;
    private IQueue<Variable> variables = Factory.CreateQueue<Variable>();
    private Sentence quantified;
    private IQueue<FOLNode> args = Factory.CreateQueue<FOLNode>();
    private string stringRep = null;
    private int hashCode = 0;

    public QuantifiedSentence(string quantifier, IQueue<Variable> variables,
            Sentence quantified)
    {
        this.quantifier = quantifier;
        this.variables.addAll(variables);
        this.quantified = quantified;
        this.args.addAll(variables);
        this.args.Add(quantified);
    }

    public string getQuantifier()
    {
        return quantifier;
    }

    public IQueue<Variable> getVariables()
    {
        return Factory.CreateReadOnlyQueue<>(variables);
    }

    public Sentence getQuantified()
    {
        return quantified;
    }

    //
    // START-Sentence
    public string getSymbolicName()
    {
        return getQuantifier();
    }

    public bool isCompound()
    {
        return true;
    }

    public IQueue<FOLNode> getArgs()
    {
        return Factory.CreateReadOnlyQueue<>(args);
    }

    public object accept(FOLVisitor v, object arg)
    {
        return v.visitQuantifiedSentence(this, arg);
    }

    public QuantifiedSentence copy()
    {
        IQueue<Variable> copyVars = Factory.CreateQueue<Variable>();
        for (Variable v : variables)
        {
            copyVars.Add(v.copy());
        }
        return new QuantifiedSentence(quantifier, copyVars, quantified.copy());
    }

    // END-Sentence
    //

     
    public override bool Equals(object o)
    {

        if (this == o)
        {
            return true;
        }
        if ((o == null) || (this.GetType() != o.GetType()))
        {
            return false;
        }
        QuantifiedSentence cs = (QuantifiedSentence)o;
        return cs.quantifier.Equals(quantifier)
                && cs.variables.Equals(variables)
                && cs.quantified.Equals(quantified);
    }

     
    public override int GetHashCode()
    {
        if (0 == hashCode)
        {
            hashCode = 17;
            hashCode = 37 * hashCode + quantifier.GetHashCode();
            for (Variable v : variables)
            {
                hashCode = 37 * hashCode + v.GetHashCode();
            }
            hashCode = hashCode * 37 + quantified.GetHashCode();
        }
        return hashCode;
    }

     
    public override string ToString()
    {
        if (null == stringRep)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(quantifier);
            sb.Append(" ");
            for (Variable v : variables)
            {
                sb.Append(v.ToString());
                sb.Append(" ");
            }
            sb.Append(quantified.ToString());
            stringRep = sb.ToString();
        }
        return stringRep;
    }
}
}
