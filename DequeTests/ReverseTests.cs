using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DequeTests
{
    [TestClass]
    public class ReverseTests
    {
        [TestMethod]

        public void Add_Remove_Test()
        {
            Deque<int> deque = new Deque<int>();

            for (int i = 100; i < 200; i++)
            {
                deque.Add(i);
            }
            ReversedDeque<int> reversed = (ReversedDeque<int>)deque.GetReversed();

            for (int i = 99; i >=0 ; i--)
            {
                reversed.Add(i);
            }

            reversed.RemoveFirst();
            reversed.RemoveLast();

            for (int i = 0; i < 198; i++)
            {
                Assert.AreEqual(deque[i], i+1);
            }
        }

        [TestMethod]
        public void Foreach_Test()
        {
            Deque<int> deque = new Deque<int>();

            int cycles = 200;


            for (int i = 0; i < cycles; i++)
            {
                deque.Add(i);
            }

            IList<int> reversed = deque.GetReversed();

            int j = 199;
            foreach(int i in reversed)
            {
                Assert.AreEqual(i, j--);
            }


        }

    }
}
