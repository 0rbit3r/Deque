using System;
using System.Collections.Generic;
using System.Text;

namespace Deque_v1
{
    class Program
    {
        public static void Main()
        {
            int cycles = 100;
            IDeque<int> deque = (IDeque<int>)new Deque<int>().GetReversed();

            for (int i = 0; i < cycles; i++)
            {
                deque.Insert(0, i);
            }


            Console.ReadLine();
        }
    }
}
