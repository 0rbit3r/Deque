using System;
using System.Collections.Generic;
using System.Text;

namespace Deque_v1
{
    class Program
    {
        public static void Main()
        {
            List<int> l = new List<int>();

            l.Add(1);
            l.Add(2);

            foreach(int i in l)
            {
                l.Add(3);
            }
        }
    }
}
