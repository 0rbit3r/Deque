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
            Deque<int>.ReversedDeque<int> reversed = (Deque<int>.ReversedDeque<int>)deque.GetReversed();

            for (int i = 99; i >= 0; i--)
            {
                reversed.Add(i);
            }

            reversed.RemoveFirst();
            reversed.RemoveLast();

            for (int i = 0; i < 198; i++)
            {
                Assert.AreEqual(deque[i], i + 1);
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
            foreach (int i in reversed)
            {
                Assert.AreEqual(i, j--);
            }


        }

        [TestMethod]
        public void ModifyDuringReversedForeach_Test()
        {
            Deque<int> deque = new Deque<int>();

            int cycles = 5;


            for (int i = 0; i < cycles; i++)
            {
                deque.Add(i);
            }

            IList<int> reversed = deque.GetReversed();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                foreach (int i in reversed)
                {
                    reversed.Add(5);
                }
            });
        }

        [TestMethod]
        public void InsertRemove_Test()
        {
            IDeque<int> reversed = (IDeque<int>)new Deque<int>().GetReversed();

            int cycles = 1_000_000;

            for (int i = 0; i < cycles * 10; i++)
            {
                reversed.Prepend(i);
            }

            for (int i = 0; i < cycles * 9; i++)
            {
                reversed.RemoveAt(reversed.Count - 1);
            }

            for (int i = 0; i < cycles; i++)
            {
                Assert.AreEqual(cycles * 10 - 1 - i, reversed[i]);
            }
        }

        [TestMethod]
        public void Insert()
        {
            IDeque<int> deque = (IDeque<int>)new Deque<int>().GetReversed();
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

        public void Exhaustive_1()
        {
            IDeque<int> deque = (IDeque<int>)new Deque<int>().GetReversed();
            int cycles = 1_000_000;

            for (int i = 0; i < cycles; i++)
            {
                deque.Add(i + cycles);
            }

            Assert.AreEqual(-1, deque.IndexOf(-42));
            Assert.AreEqual(cycles, deque.Count);

            for (int i = cycles - 1; i >= 0; i--)
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

            for (int i = 0; i < 60; i++)
            {
                Assert.AreEqual(i + cycles * 2 - 60, deque[i]);
            }
        }

        [TestMethod]

        public void Exhaustive_2()
        {

            int cycles = 100;
            IDeque<int> deque = (IDeque<int>)new Deque<int>().GetReversed();

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
            IDeque<int> deque = (IDeque<int>)new Deque<int>().GetReversed();
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

            IDeque<int> deque = (IDeque<int>)new Deque<int>().GetReversed();

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
