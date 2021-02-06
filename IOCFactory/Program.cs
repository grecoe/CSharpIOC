using System;
using System.Collections.Generic;
using IOCFactory.Classes;
using IOCFactory.Interfaces;
using Utils.IOC;

namespace IOCFactory
{
    /// <summary>
    /// This program shows how to use the Utils.IOC classes to using Inversion Of Control 
    /// to do dependency injection to generated classes. 
    /// 
    /// This is very similar to other commercial libraries, but clearly isn't going to be as resilient as
    /// those libraries are. 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Start by creating a factory and putting an instance Example1 and Example2 in it.
             * 
             * In practice, these would be the classes that are injected into further classes created
             * but we can see how the factory works with these. 
             */
            Factory iocFactory = new Factory();
            iocFactory.RegisterType(new Example1());
            iocFactory.RegisterType(new Example2());


            /*
             * Get an instance of the Example1 class. Because there may be several (for interfaces specifically)
             * we get a list and ensure we have it.
             * 
             * In this case there will be exactly 1
             */ 
            List<object> obj = iocFactory.GetInstance(typeof(Example1));
            Console.WriteLine($"Recieved {obj.Count} instances of Example1");
            Console.WriteLine($"Of type {obj[0].GetType()} \n");

            /*
             * Get instances of the deriving classes. 
             * 
             * In this case there will be exactly 2 because Example1 and Example2 both derive from the 
             * interface IExampleInterface.
             */
            List<object> interfaces = iocFactory.GetInstance(typeof(IExampleInterface));
            Console.WriteLine($"Recieved {interfaces.Count} instances of IExampleInterface, they are:");
            foreach(object iface in interfaces)
            {
                Console.WriteLine($"\t {((IExampleInterface)iface).Name}");
            }

            /*
             * Now create a more complext type. Example3 requires an instance of IExampleInterface as 
             * a construction parameter. 
             * 
             * Simply asking the factory to create one will get one from  the internal list (first one) 
             * and inject it into our class.
             * 
             * WARNING: Because you just get the first version, if you have multiple objects deriving from the 
             * same interface but you need specific ones you may run into issues. Can certainly be worked around
             * but just be aware. 
             * 
             * We can further identify the other construction parameters in a dictionary if they are not
             * a class/interface already created with RegisterType.
             */
            Console.WriteLine("\nExample of true IOC");
            Example3 e3 = (Example3) iocFactory.CreateInstance(
                typeof(Example3), 
                new Dictionary<string, object>() { 
                    { "name" , "IOC Example" } 
                });
            e3.DoSomething();

            /*
             * If we ask for Example3 again, we will get the same instance as above because it was added to 
             * the internal list of objects it's tracking. 
             * 
             * In this case we don't even need to include the additional construcotr parameters.
             */
            e3 = (Example3)iocFactory.CreateInstance(typeof(Example3));
            e3.DoSomething();


            /*
             * For the last example, Example4, this is a template class expecting 2 generic parameters
             * in the constructor.
             * 
             * NOTE: For template classes this library will only work if the class takes an instance in the construcotr
             * parameters (I think). 
             * 
             * Since we have both expected types (Example1 and Example2) as the definied types we won't hit a conflict
             * mentioned above as if we had used IExampleInterface. It will inject the two created classes into the 
             * generic class for you and return an instance of Exaple4 all set up.
             * 
             * To keep the code simple, Example4 is just going to write to the console during construction to say it's
             * being created and with which parameters. 
             */
            Console.WriteLine("\nExample of IOC with generic template class");
            IGenericClassWrapper e4 = (IGenericClassWrapper)iocFactory.CreateInstance(typeof(Example4<,>));
            Console.WriteLine($"Object is {e4.GetType()}");
        }
    }
}
