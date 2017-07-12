using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 9.1, page
     * 328. 
     *  
     * 
     * <pre>
     * function UNIFY(x, y, theta) returns a substitution to make x and y identical
     *   inputs: x, a variable, constant, list, or compound
     *           y, a variable, constant, list, or compound
     *           theta, the substitution built up so far (optional, defaults to empty)
     *           
     *   if theta = failure then return failure
     *   else if x = y the return theta
     *   else if VARIABLE?(x) then return UNIVY-VAR(x, y, theta)
     *   else if VARIABLE?(y) then return UNIFY-VAR(y, x, theta)
     *   else if COMPOUND?(x) and COMPOUND?(y) then
     *       return UNIFY(x.ARGS, y.ARGS, UNIFY(x.OP, y.OP, theta))
     *   else if LIST?(x) and LIST?(y) then
     *       return UNIFY(x.REST, y.REST, UNIFY(x.FIRST, y.FIRST, theta))
     *   else return failure
     *   
     * ---------------------------------------------------------------------------------------------------
     * 
     * function UNIFY-VAR(var, x, theta) returns a substitution
     *            
     *   if {var/val} E theta then return UNIFY(val, x, theta)
     *   else if {x/val} E theta then return UNIFY(var, val, theta)
     *   else if OCCUR-CHECK?(var, x) then return failure
     *   else return add {var/x} to theta
     * </pre>
     * 
     * Figure 9.1 The unification algorithm. The algorithm works by comparing the
     * structures of the inputs, elements by element. The substitution theta that is
     * the argument to UNIFY is built up along the way and is used to make sure that
     * later comparisons are consistent with bindings that were established earlier.
     * In a compound expression, such as F(A, B), the OP field picks out the
     * function symbol F and the ARGS field picks out the argument list (A, B).
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     * @author Mike Stampone
     * 
     */
    public class Unifier
    {
        //
        private static SubstVisitor _substVisitor = new SubstVisitor();

        public Unifier()
        { }

        /**
         * Returns a IDictionary<Variable, Term> representing the substitution (i.e. a set
         * of variable/term pairs) or null which is used to indicate a failure to
         * unify.
         * 
         * @param x
         *            a variable, constant, list, or compound
         * @param y
         *            a variable, constant, list, or compound
         * 
         * @return a IDictionary<Variable, Term> representing the substitution (i.e. a set
         *         of variable/term pairs) or null which is used to indicate a
         *         failure to unify.
         */
        public IDictionary<Variable, Term> unify(FOLNode x, FOLNode y)
        {
            return unify(x, y, new Dictionary<Variable, Term>());
        }

        /**
         * Returns a IDictionary<Variable, Term> representing the substitution (i.e. a set
         * of variable/term pairs) or null which is used to indicate a failure to
         * unify.
         * 
         * @param x
         *            a variable, constant, list, or compound
         * @param y
         *            a variable, constant, list, or compound
         * @param theta
         *            the substitution built up so far
         * 
         * @return a IDictionary<Variable, Term> representing the substitution (i.e. a set
         *         of variable/term pairs) or null which is used to indicate a
         *         failure to unify.
         */
        public IDictionary<Variable, Term> unify(FOLNode x, FOLNode y, IDictionary<Variable, Term> theta)
        {
            // if theta = failure then return failure
            if (theta == null)
            {
                return null;
            }
            else if (x.Equals(y))
            {
                // else if x = y then return theta
                return theta;
            }
            else if (x is Variable)
            {
                // else if VARIABLE?(x) then return UNIVY-VAR(x, y, theta)
                return unifyVar((Variable)x, y, theta);
            }
            else if (y is Variable)
            {
                // else if VARIABLE?(y) then return UNIFY-VAR(y, x, theta)
                return unifyVar((Variable)y, x, theta);
            }
            else if (isCompound(x) && isCompound(y))
            {
                // else if COMPOUND?(x) and COMPOUND?(y) then
                // return UNIFY(x.ARGS, y.ARGS, UNIFY(x.OP, y.OP, theta))
                return unify(args(x), args(y), unifyOps(op(x), op(y), theta));
            }
            else
            {
                // else return failure
                return null;
            }
        }

        /**
         * Returns a IDictionary<Variable, Term> representing the substitution (i.e. a set
         * of variable/term pairs) or null which is used to indicate a failure to
         * unify.
         * 
         * @param x
         *            a variable, constant, list, or compound
         * @param y
         *            a variable, constant, list, or compound
         * @param theta
         *            the substitution built up so far
         * 
         * @return a IDictionary<Variable, Term> representing the substitution (i.e. a set
         *         of variable/term pairs) or null which is used to indicate a
         *         failure to unify.
         */
        // else if LIST?(x) and LIST?(y) then
        // return UNIFY(x.REST, y.REST, UNIFY(x.FIRST, y.FIRST, theta))
        public IDictionary<Variable, Term> unify(IList<FOLNode> x, IList<FOLNode> y, IDictionary<Variable, Term> theta)
        {
            if (theta == null)
            {
                return null;
            }
            else if (x.Count != y.Count)
            {
                return null;
            }
            else if (x.Count == 0 && y.Count == 0)
            {
                return theta;
            }
            else if (x.Count == 1 && y.Count == 1)
            {
                return unify(x[0], y[0], theta);
            }
            else
            {
                return unify(x.Skip(1).ToList(), y.Skip(1).ToList(), unify(x[0], y[0], theta));
            }
        }

        //
        // PROTECTED METHODS
        //

        // Note: You can subclass and override this method in order
        // to re-implement the OCCUR-CHECK?() to always
        // return false if you want that to be the default
        // behavior, as is the case with Prolog.
        // Note: Implementation is based on unify-bug.pdf document by Peter Norvig:
        // http://norvig.com/unify-bug.pdf
        protected bool occurCheck(IDictionary<Variable, Term> theta, Variable var, FOLNode x)
        {
            // ((equal var x) t)
            if (var.Equals(x))
            {
                return true;
                // ((bound? x subst)
            }
            else if (theta.ContainsKey(x as Variable))
            {
                // (occurs-in? var (lookup x subst) subst))
                return occurCheck(theta, var, theta[x as Variable]);
                // ((consp x) (or (occurs-in? var (first x) subst) (occurs-in? var
                // (rest x) subst)))
            }
            else if (x is Function)
            {
                // (or (occurs-in? var (first x) subst) (occurs-in? var (rest x)
                // subst)))
                Function fx = (Function)x;
                foreach (Term fxt in fx.getArgs())
                {
                    if (occurCheck(theta, var, fxt))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //
        // PRIVATE METHODS
        //

        /**
         * <code>
         * function UNIFY-VAR(var, x, theta) returns a substitution
         *   inputs: var, a variable
         *       x, any expression
         *       theta, the substitution built up so far
         * </code>
         */
        private IDictionary<Variable, Term> unifyVar(Variable var, FOLNode x, IDictionary<Variable, Term> theta)
        {

            if (!(x is Term))
            {
                return null;
            }
            else if (theta.Keys.Contains(var))
            {
                // if {var/val} E theta then return UNIFY(val, x, theta)
                return unify(theta[var], x, theta);
            }
            else if (theta.Keys.Contains(x))
            {
                // else if {x/val} E theta then return UNIFY(var, val, theta)
                return unify(var, theta[x as Variable], theta);
            }
            else if (occurCheck(theta, var, x))
            {
                // else if OCCUR-CHECK?(var, x) then return failure
                return null;
            }
            else
            {
                // else return add {var/x} to theta
                cascadeSubstitution(theta, var, (Term)x);
                return theta;
            }
        }

        private IDictionary<Variable, Term> unifyOps(string x, string y, IDictionary<Variable, Term> theta)
        {
            if (theta == null)
            {
                return null;
            }
            else if (x.Equals(y))
            {
                return theta;
            }
            else
            {
                return null;
            }
        }

        private IList<FOLNode> args(FOLNode x)
        {
            return x.getArgs();
        }

        private string op(FOLNode x)
        {
            return x.getSymbolicName();
        }

        private bool isCompound(FOLNode x)
        {
            return x.isCompound();
        }

        // See:
        // http://logic.stanford.edu/classes/cs157/2008/miscellaneous/faq.html#jump165
        // for need for this.
        private IDictionary<Variable, Term> cascadeSubstitution(IDictionary<Variable, Term> theta, Variable var, Term x)
        {
            theta.Add(var, x);
            foreach (Variable v in theta.Keys)
            {
                theta.Add(v, _substVisitor.subst(theta, theta[v]));
            }
            // Ensure Function Terms are correctly updates by passing over them
            // again. Fix for testBadCascadeSubstitution_LCL418_1()
            foreach (Variable v in theta.Keys)
            {
                Term t = theta[v];
                if (t is Function)
                {
                    theta.Add(v, _substVisitor.subst(theta, t));
                }
            }
            return theta;
        }
    }
}
