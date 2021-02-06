using IOCFactory.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOCFactory.Classes
{
    class Example3
    {
        public String Name { get; private set; }
        public IExampleInterface ExampleInterface { get; private set; }

        public Example3(IExampleInterface iface, string name)
        {
            this.Name = name;
            this.ExampleInterface = iface;
        }

        public void DoSomething()
        {
            Console.WriteLine($"Example3 -> {this.Name} with IExampleInterface {this.ExampleInterface.Name}");
        }
    }
}
