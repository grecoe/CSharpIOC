using System;
using System.Collections.Generic;
using System.Text;
using FunctionWrapper.Interfaces;

namespace FunctionWrapper.Classes
{
    class NakedClass : IBareIt
    {
        public NakedClass()
        {

        }

        public void DoSomething()
        {
            Console.WriteLine("NakedClass Doing Somethign");
            System.Threading.Thread.Sleep(100);
        }
    }
}
