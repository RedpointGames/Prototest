﻿namespace Prototest.Library
{
    public class PrototestInequalityFailureException : PrototestFailureException
    {
        /// <summary>
        /// The first object that was compared.
        /// </summary>
        public object Object1 { get; set; }

        /// <summary>
        /// The second object that was compared.
        /// </summary>
        public object Object2 { get; set; }

        public PrototestInequalityFailureException(object object1, object object2)
            : base("Value " + PrototestValueFormatter.Format(object1) + " is equal to " + PrototestValueFormatter.Format(object2))
        {
            Object1 = object1;
            Object2 = object2;
        }
    }
}
