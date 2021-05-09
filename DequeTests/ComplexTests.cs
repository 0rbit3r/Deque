using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DequeTests
{
    [TestClass]
    public class ComplexTests
    {
        [TestMethod]

        public void Exhaustive_1()
        {
            Deque<int> deque = new Deque<int>();
            int cycles = 1_000_000;

            for (int i = 0; i < cycles; i++)
            {
                deque.Add(i + cycles);
            }

            Assert.AreEqual(-1, deque.IndexOf(-42));
            Assert.AreEqual(cycles, deque.Count);

            for (int i = cycles - 1; i >= 0 ; i--)
            {
                deque.Insert(0, i);
            }

            for (int i = 0; i < cycles * 2; i++)
            {
                Assert.AreEqual(i, deque[i]);
            }

            deque.Insert(cycles / 2, 42);
            Assert.AreEqual(42, deque[cycles / 2]);

            Assert.AreEqual(cycles * 2 + 1, deque.Count);

            deque.RemoveAt(cycles / 2);

            for (int i = 0; i < cycles * 2 - 60; i++)
            {
                deque.RemoveAt(0);
            }

            Assert.AreEqual(deque.Count, 60);

            for(int i = 0; i < 60; i++)
            {
                Assert.AreEqual(i + cycles * 2 - 60, deque[i]);
            }
        }

        [TestMethod]

        public void Exhaustive_2()
        {

            int cycles = 10_000_000;
            Deque<int> deque = new Deque<int>();

            for (int i = 0; i < cycles; i++)
            {
                deque.Insert(0, i);
            }

            for (int i = 0; i < cycles; i++)
            {
                Assert.AreEqual(cycles - 1 - i, deque[i]);
            }

            for (int i = 0; i < cycles; i++)
            {
                deque[i] = i;
            }

            for (int i = 0; i < cycles; i++)
            {
                Assert.AreEqual(i, deque[i]);
            }

            Assert.AreEqual(cycles, deque.Count);

            for (int i = 0; i < cycles; i++)
            {
                deque.RemoveAt(deque.IndexOf(i));
            }

            Assert.AreEqual(0, deque.Count);
        }

        [TestMethod]
        public void Exhaustive_3()
        {
            Deque<int> deque = new Deque<int>();
            int cycles = 100;
            for (int i = 0; i < cycles; i += 2)
            {
                deque.Add(i);
            }
            for (int i = 1; i < cycles - 1; i += 2)
            {
                deque.Insert(i, i);
            }

            deque.Add(cycles - 1);

            Assert.AreEqual(cycles, deque.Count);

            for (int i = 0; i < cycles; i++)
            {
                Assert.AreEqual(i, deque[i]);
            }
        }

        [TestMethod]
        public void Exhaustive_4()
        {

            Deque<int> deque = new Deque<int>();

            int cycles = 1_000_000;

            for (int i = 0; i < cycles * 10; i++)
            {
                deque.Prepend(i);
            }

            for (int i = 0; i < cycles * 9; i++)
            {
                deque.RemoveAt(deque.Count - 1);
            }

            for (int i = 0; i < cycles; i++)
            {
                Assert.AreEqual(cycles * 10 - 1 - i, deque[i]);
            }
            
        }
    }
}
