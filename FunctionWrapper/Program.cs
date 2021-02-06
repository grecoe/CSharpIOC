using System;
using FunctionWrapper.Classes;
using FunctionWrapper.Interfaces;
using Utils.ClassMapper;

namespace FunctionWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            IBareIt nc = new NakedClass();
            nc.DoSomething();

            Console.WriteLine("\nNow encapsulated it and call it again, same output with logging around it with entry/exit.");

            IBareIt nnc = new NotNakedClass(nc as NakedClass, new ConsoleScribe());
            nnc.DoSomething();
        }
    }
}
