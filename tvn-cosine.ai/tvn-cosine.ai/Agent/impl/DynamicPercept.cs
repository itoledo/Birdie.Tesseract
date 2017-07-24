﻿using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.common;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.agent.impl
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public class DynamicPercept : ObjectWithDynamicAttributes, IPercept, IHashable, IEquatable
    {
        public DynamicPercept()
        { }

        public override string describeType()
        {
            return "Percept";
        }

        /**
         * Constructs a DynamicPercept with one attribute
         * 
         * @param key1
         *            the attribute key
         * @param value1
         *            the attribute value
         */
        public DynamicPercept(object key1, object value1)
        {
            setAttribute(key1, value1);
        }

        /**
         * Constructs a DynamicPercept with two attributes
         * 
         * @param key1
         *            the first attribute key
         * @param value1
         *            the first attribute value
         * @param key2
         *            the second attribute key
         * @param value2
         *            the second attribute value
         */
        public DynamicPercept(object key1, object value1, object key2, object value2)
        {
            setAttribute(key1, value1);
            setAttribute(key2, value2);
        }

        /**
         * Constructs a DynamicPercept with an array of attributes
         * 
         * @param keys
         *            the array of attribute keys
         * @param values
         *            the array of attribute values
         */
        public DynamicPercept(object[] keys, object[] values)
        {
            if (keys.Length != values.Length)
            {
                throw new ArgumentOutOfRangeException("keys.Length != values.Length", null);
            }

            for (int i = 0; i < keys.Length;++i)
            {
                setAttribute(keys[i], values[i]);
            }
        }

        public override bool Equals(object o)
        {
            if (null == (o as DynamicPercept)) return false;
            return ToString().Equals(o.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
