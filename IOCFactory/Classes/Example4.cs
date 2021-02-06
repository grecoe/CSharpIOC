using System;
using System.Collections.Generic;
using System.Text;
using IOCFactory.Interfaces;

namespace IOCFactory.Classes
{
    class Example4<TE1, TE2> : IGenericClassWrapper
        where TE1 : Example1
        where TE2 : Example2
    {

        public Example4(TE1 e1, TE2 e2)
        {
            Console.WriteLine($"Example 4 created with ({e1.Name}) and ({e2.Name})");
        }
    }
}
