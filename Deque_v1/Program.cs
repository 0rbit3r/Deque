using System;
using System.Collections.Generic;
using System.Text;

namespace Deque_v1
{
    class Program
    {
        public static void Main()
        {

            for (int cycles = 10; cycles < 100; cycles += 71)
            {

                Deque<int> deque = new Deque<int>();

                //int cycles = 5;


                for (int i = 0; i < cycles; i++)
                {
                    deque.Add(i);
                }

                for (int i = 0; i < cycles / 2; i++)
                {
                    deque.RemoveFirst();
                }

                for (int i = cycles / 2; i < cycles; i++)
                {
                    deque[i] = 1;
                }
            }
        }
    }
}
