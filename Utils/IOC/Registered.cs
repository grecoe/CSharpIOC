using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Utils.IOC
{
    /// <summary>
    /// Class used internally to Factory to track registered types
    /// </summary>
    class Registered
    {
        /// <summary>
        /// The type of the object that is registered
        /// </summary>
        public Type Self { get; set; }
        /// <summary>
        /// Constructor information for the object
        /// </summary>
        public ConstructorInfo[] Constructors { get; private set; }
        /// <summary>
        /// An instance of the object
        /// </summary>
        public object Instance { get; set; }
        /// <summary>
        /// A list of derived interfaces for the object
        /// </summary>
        public List<Type> Derived { get; set; }

        public Registered(ConstructorInfo[] constructors)
        {
            this.Derived = new List<Type>();
            this.Constructors = constructors;
        }
    }
}
