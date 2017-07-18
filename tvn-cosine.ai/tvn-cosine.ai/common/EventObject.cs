using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.common
{
    public class EventObject
    {
        /**
         * The object on which the Event initially occurred.
         */
        protected object source;

        /**
         * Constructs a prototypical Event.
         *
         * @param    source    The object on which the Event initially occurred.
         * @exception  IllegalArgumentException  if source is null.
         */
        public EventObject(object source)
        {
            if (source == null)
                throw new IllegalArgumentException("null source");

            this.source = source;
        }

        /**
         * The object on which the Event initially occurred.
         *
         * @return   The object on which the Event initially occurred.
         */
        public object getSource()
        {
            return source;
        }

        /**
         * Returns a String representation of this EventObject.
         *
         * @return  A a String representation of this EventObject.
         */
        public override string ToString()
        {
            return GetType().Name + "[source=" + source + "]";
        }
    } 
}
