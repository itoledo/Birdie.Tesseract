﻿using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.logic.fol.domain
{
    public class FOLDomain
    {
        private ISet<string> constants, functions, predicates;
        private int skolemConstantIndexical = 0;
        private int skolemFunctionIndexical = 0;
        private int answerLiteralIndexical = 0;
        private IQueue<FOLDomainListener> listeners = Factory.CreateQueue<FOLDomainListener>();

        public FOLDomain()
        {
            this.constants = Factory.CreateSet<string>();
            this.functions = Factory.CreateSet<string>();
            this.predicates = Factory.CreateSet<string>();
        }

        public FOLDomain(FOLDomain toCopy)
            : this(toCopy.getConstants(), toCopy.getFunctions(), toCopy.getPredicates())
        {  }

        public FOLDomain(ISet<string> constants, ISet<string> functions, ISet<string> predicates)
        {
            this.constants = Factory.CreateSet<string>(constants);
            this.functions = Factory.CreateSet<string>(functions);
            this.predicates = Factory.CreateSet<string>(predicates);
        }

        public ISet<string> getConstants()
        {
            return constants;
        }

        public ISet<string> getFunctions()
        {
            return functions;
        }

        public ISet<string> getPredicates()
        {
            return predicates;
        }

        public void addConstant(string constant)
        {
            constants.Add(constant);
        }

        public string addSkolemConstant()
        {

            string sc = null;
            do
            {
                sc = "SC" + (skolemConstantIndexical++);
            } while (constants.Contains(sc) 
                    || functions.Contains(sc)
                    || predicates.Contains(sc));

            addConstant(sc);
            notifyFOLDomainListeners(new FOLDomainSkolemConstantAddedEvent(this, sc));

            return sc;
        }

        public void addFunction(string function)
        {
            functions.Add(function);
        }

        public string addSkolemFunction()
        {
            string sf = null;
            do
            {
                sf = "SF" + (skolemFunctionIndexical++);
            } while (constants.contains(sf) || functions.contains(sf)
                    || predicates.contains(sf));

            addFunction(sf);
            notifyFOLDomainListeners(new FOLDomainSkolemFunctionAddedEvent(this, sf));

            return sf;
        }

        public void addPredicate(string predicate)
        {
            predicates.Add(predicate);
        }

        public string addAnswerLiteral()
        {
            string al = null;
            do
            {
                al = "Answer" + (answerLiteralIndexical++);
            } while (constants.contains(al) || functions.contains(al)
                    || predicates.contains(al));

            addPredicate(al);
            notifyFOLDomainListeners(new FOLDomainAnswerLiteralAddedEvent(this, al));

            return al;
        }

        public void addFOLDomainListener(FOLDomainListener listener)
        {
            synchronized(listeners) {
                if (!listeners.contains(listener))
                {
                    listeners.Add(listener);
                }
            }
        }

        public void removeFOLDomainListener(FOLDomainListener listener)
        {
            synchronized(listeners) {
                listeners.Remove(listener);
            }
        }

        //
        // PRIVATE METHODS
        //
        private void notifyFOLDomainListeners(FOLDomainEvent event) {
            synchronized(listeners) {
                for (FOLDomainListener l : listeners)
                {

                event.notifyListener(l);
        }
    }
}
}
}
