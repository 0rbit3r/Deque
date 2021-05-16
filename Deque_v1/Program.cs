using System;
using System.Collections.Generic;
using System.Text;

namespace Deque_v1
{
    class Program
    {
        public static void Main()
        {
            ReversedForeachWithNulls_Test();
        }

        public static void ReversedForeachWithNulls_Test()
        {
            IDeque<string> deque = (IDeque<string>)new Deque<string>().GetReversed();

            int cycles = 100;

            for (int i = 0; i < cycles; i++)
            {
                deque.Add(i.ToString());
            }

            deque[42] = null;

            int j = 0;
            foreach (string s in deque)
            {
                if (s == null)
                {
                    Console.WriteLine($"Expected: 42\tReal: {deque.IndexOf(s)}");
                }
                else
                {
                    Console.WriteLine($"Expected: {j}\tReal: {deque.IndexOf(s)}");
                }
                j++;
            }
        }
    }
}
