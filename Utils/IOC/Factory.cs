using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utils.IOC
{
    /// <summary>
    /// Class that implements a simplified Inversion Of Control (IOC) pattern and performs dependency injection
    /// on behalf of the caller. 
    /// 
    /// Simplified in that there are much more complete projects that you could use through NuGet or other locations. 
    /// 
    /// Given that, this is not going to be as complete as a commmercial library, but for certain circumstances would
    /// suit the need of the developer. 
    /// </summary>
    public class Factory
    {
        /// <summary>
        /// Internal list of types registered with the factory. 
        /// </summary>
        private Dictionary<Type, Registered> TypeMap { get; set; }

        public Factory()
        {
            this.TypeMap = new Dictionary<Type, Registered>();
        }

        /// <summary>
        /// Registers a type with the factory
        /// </summary>
        /// <param name="instance">Any class instance to be restisterd</param>
        public void RegisterType(object instance)
        {
            Type toadd = instance.GetType();
            Registered reg = new Registered(toadd.GetConstructors()) { Self = toadd, Instance = instance };
            this.TypeMap.Add(toadd, reg);

            foreach (Type t in toadd.GetInterfaces())
            {
                if (this.TypeMap.ContainsKey(t))
                {
                    this.TypeMap[t].Derived.Add(toadd);
                }
                else
                {
                    Registered interfaceType = new Registered(null) { Self = t, Derived = new List<Type>() { toadd } };
                    this.TypeMap.Add(t, interfaceType);
                }
            }
        }

        /// <summary>
        /// Retrieves a list of ojects whose type matches the input type. 
        /// </summary>
        /// <param name="inputType">Type of object to get</param>
        /// <returns>A list of registered types maching the input type. </returns>
        public List<object> GetInstance(Type inputType)
        {
            List<object> return_value = new List<object>();

            if (this.TypeMap.ContainsKey(inputType))
            {
                if (this.TypeMap[inputType].Derived.Count > 0)
                {
                    foreach (Type t in this.TypeMap[inputType].Derived)
                    {
                        Registered derivedType = this.TypeMap[t];
                        return_value.Add(derivedType.Instance);
                    }
                }
                else
                {
                    Registered actualTypeToCreate = this.TypeMap[inputType];
                    return_value.Add(actualTypeToCreate.Instance);
                }
            }
            return return_value;
        }

        /// <summary>
        /// Creates an instance of an object by finding it by type. If there is something registered with 
        /// this type, that is returned, otherwise 
        /// 
        /// 1.  Get any generic tempalte parameters used on the class definition. Any expected classes 
        ///     must exist in the factory already. 
        /// 2.  Take the constructor with the most parameters (some may implement an empty param constructor
        ///     to meet other needs) and collect the name/type of the parameters. 
        /// 3.  Create a list of objects using the full list of parameters.
        ///         - If type is an actual registered type, use that value
        ///         - If type is not found, look through the additional parameters for it by name. 
        /// 4. Try and call the constructor with the paramers listed above. 
        /// 5. If succesful, add the type to the registered types of the factory. 
        /// 
        /// NOTE: The function takes the constructor with the most parameters. It does not validate that the number
        /// of collected parameters actually matches the constructor signature so if the wrong number of parameters (less)
        /// are provided it WILL throw an exception. 
        /// 
        /// </summary>
        /// <param name="tType">Type of the object to create</param>
        /// <param name="additionalParams">A dictionary of additional constructor parameters that are things that are not
        ///     already in the registered list of classes in the factory. :
        ///         Key = string match of the actual parameter name expected by the class
        ///         Value = Whatever acceptable value that matches the constructor.
        /// </param>
        /// <returns>An instance of the object of Type tType complete with dependency injection (if called for)</returns>
        public object CreateInstance(Type tType, Dictionary<string, object> additionalParams = null)
        {
            object return_value = null;
            List<object> found_instances = this.GetInstance(tType);

            if (found_instances.Count > 0)
            {
                return_value = found_instances[0];
            }

            if (return_value == null)
            {
                // Will have to create it
                Type constructorType = tType;

                List<Type> genericTypes = new List<Type>();

                // If it's a generic class we need to get the generic types so we can 
                // construct an instance of the generic class, otherwise we woudl not
                // be able to actually create an instance of it. 
                if (tType.IsGenericType)
                {
                    foreach (Type gen in tType.GetGenericArguments())
                    {
                        if (gen.IsGenericParameter)
                        {
                            foreach (Type constraints in gen.GetGenericParameterConstraints())
                            {
                                List<object> con = this.GetInstance(constraints);
                                if (con.Count > 0)
                                {
                                    List<object> obj = this.GetInstance(constraints);
                                    if (obj.Count > 0)
                                    {
                                        genericTypes.Add(obj[0].GetType());
                                    }
                                }
                            }

                        }
                    }

                    constructorType = tType.MakeGenericType(genericTypes.ToArray());
                }

                // Get each constructor from the constructor type and get the one with the 
                // most parameters. Certainly could adopt this, but it's not meant to be a commercial
                // library. 
                int paramCount = 0;
                ConstructorInfo usableInfo = null;
                ParameterInfo[] constructorParams = null;
                foreach (ConstructorInfo cInfo in constructorType.GetConstructors())
                {
                    ParameterInfo[] parameters = cInfo.GetParameters();
                    if ((usableInfo == null) || (paramCount < parameters.Length))
                    {
                        usableInfo = cInfo;
                        paramCount = parameters.Length;
                        constructorParams = parameters;
                    }
                }

                // If no parameters are needed, just call the constructor, otherwise build up the list of 
                // parameters from registered classes and passed in arguments and call that constructor
                // with the parameters list. 
                if (paramCount == 0)
                {
                    return_value = Activator.CreateInstance(constructorType);
                }
                else
                {
                    List<object> parameters = new List<object>();
                    foreach (ParameterInfo pInfo in constructorParams)
                    {
                        object obj_param = null;
                        if (additionalParams != null && additionalParams.ContainsKey(pInfo.Name))
                        {
                            obj_param = additionalParams[pInfo.Name];
                        }
                        else
                        {
                            List<object> paramSearch = this.GetInstance(pInfo.ParameterType);

                            if (paramSearch.Count > 0)
                            {
                                obj_param = paramSearch[0];
                            }

                            if (obj_param == null)
                            {
                                throw new Exception($"Missing parameter for constructor - {pInfo.Name}");
                            }
                        }

                        parameters.Add(obj_param);
                    }

                    return_value = Activator.CreateInstance(constructorType, parameters.ToArray());

                }

                // If the creation was a generic then we want to keep this copy as well for later reference
                if( tType.IsGenericType)
                {
                    Registered reg = new Registered(tType.GetConstructors()) { Self = tType, Instance = return_value };
                    this.TypeMap.Add(tType, reg);
                }

                // We didn't know about this before, so keep it
                this.RegisterType(return_value);
            }

            return return_value;
        }
    }
}
