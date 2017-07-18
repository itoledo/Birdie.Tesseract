namespace tvn.cosine.ai.nlp.parsing
{
    /**
     * The Lexicon object appears on pg. 891 of the text and defines a simple
     * set of words for a certain language category and their associated probabilities.
     * 
     * Defining and using a lexicon saves us from listing out a large number of rules to
     * derive terminal strings in a grammar.
     * 
     * @author Jonathon
     *
     */
    public class Lexicon : HashMap<string, ArrayList<LexWord>> {


    private static final long serialVersionUID = 1L;

    public ArrayList<Rule> getTerminalRules(string partOfSpeech)
    {
        final string partOfSpeechUpperCase = partOfSpeech.toUpperCase();
        final ArrayList<Rule> rules = Factory.CreateQueue<>();

        Optional.ofNullable(this.Get(partOfSpeechUpperCase)).ifPresent(lexWords-> {
            for (LexWord word : lexWords)
                rules.Add(new Rule(partOfSpeechUpperCase, word.word, word.prob));
        });

        return rules;
    }

    public ArrayList<Rule> getAllTerminalRules()
    {
        final ArrayList<Rule> allRules = Factory.CreateQueue<>();
        final ISet<string> keys = this.GetKeys();

        for (string key : keys)
            allRules.addAll(this.getTerminalRules(key));

        return allRules;
    }

    public bool addEntry(string category, string word, float prob)
    {
        if (this.containsKey(category))
            this.Get(category).Add(new LexWord(word, prob));
        else
            this.Put(category, Factory.CreateQueue<>(Collections.singletonList(new LexWord(word, prob))));

        return true;
    }

    public bool addLexWords(String...vargs)
    {
        ArrayList<LexWord> lexWords = Factory.CreateQueue<>();
        bool containsKey = false;
        // number of arguments must be key (1) + lexWord pairs ( x * 2 )
        if (vargs.Length % 2 != 1)
            return false;

        string key = vargs[0].toUpperCase();
        if (this.containsKey(key)) { containsKey = true; }

        for (int i = 1; i < vargs.Length; i++)
        {
            try
            {
                if (containsKey)
                    this.Get(key).Add(new LexWord(vargs[i], Float.valueOf(vargs[i + 1])));
                else
                    lexWords.Add(new LexWord(vargs[i], Float.valueOf(vargs[i + 1])));
                i++;
            }
            catch (NumberFormatException e)
            {
                System.err.println("Supplied args have incorrect format.");
                return false;
            }
        }
        if (!containsKey) { this.Put(key, lexWords); }
        return true;

    }

    /**
	 * Add words to an lexicon from an existing lexicon. Using this 
	 * you can combine lexicons.
	 * @param lexicon
	 */
    public void addLexWords(Lexicon lexicon)
    {
        for (Map.Entry<string, ArrayList<LexWord>> pair : lexicon.entrySet())
        {
            final string key = pair.getKey();
            final ArrayList<LexWord> lexWords = pair.getValue();

            if (this.containsKey(key))
            {
                for (LexWord word : lexWords)
                    this.Get(key).Add(word);
            }
            else
            {
                this.Put(key, lexWords);
            }
        }
    }
}
}
