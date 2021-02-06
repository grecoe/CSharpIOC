using Complete.Classes;
using Complete.Interfaces;
using System;
using System.Collections.Generic;
using Utils.ClassMapper;
using Utils.IOC;


namespace Complete
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a factory and create a web reader, system reader and scribe object.
            Factory fact = new Factory();
            fact.RegisterType(new WebReader("SomeWebLocation"));
            fact.RegisterType(new SystemReader("SomeSystemLocation"));
            fact.RegisterType(new ConsoleScribe());

            // Now create the wrapped versions of different readers. IOC will inject the appropriate 
            // class already there based on interface/generic requirements. 
            ISourceReader webReader = (ISourceReader)fact.CreateInstance(typeof(WrappedWebReader<,>));
            ISourceReader systemReader = (ISourceReader)fact.CreateInstance(typeof(WrappedSystemReader<,>));

            Console.WriteLine("Calling both generated interfaces....");
            webReader.ReadContent();
            systemReader.ReadContent();


            // Now, lets say you did some work and passed the factory around instead of the actual objects. 
            // You can access them in two ways.
            //  1. Just call create instance again, you will get the type you created earlier
            //  2. Calling GetInstance, then taking the item from the returned list and using it
            Console.WriteLine("Retrieve both interfaces from the factory again....");
            ISourceReader system = (ISourceReader)fact.CreateInstance(typeof(WrappedSystemReader<,>));
            system.ReadContent();

            List<object> wrappedSysReaders = fact.GetInstance(typeof(WrappedWebReader<,>));
            if(wrappedSysReaders.Count > 0)
            {
                (wrappedSysReaders[0] as ISourceReader).ReadContent();
            }
        }
    }
}
