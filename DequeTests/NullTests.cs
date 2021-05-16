using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DequeTests
{
    [TestClass]
    public class NullTests
    {
        [TestMethod]
        public void ForeachWithNulls_Test()
        {
            IDeque<string> deque = (IDeque<string>)new Deque<string>();

            int cycles = 100;

            for (int i = 0; i < cycles; i++)
            {
                deque.Add(i.ToString());
            }

            deque[42] = null;

            int j = 0;
            foreach (string s in deque)
            {
                if(s == null)
                {
                    Assert.AreEqual(42, deque.IndexOf(s));
                }
                else
                {
                    Assert.AreEqual(j, deque.IndexOf(s));
                }
                j++;
            }
        }

        [TestMethod]
        public void ReversedForeachWithNulls_Test()
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
                    Assert.AreEqual(42, deque.IndexOf(s));
                }
                else
                {
                    Assert.AreEqual(j, deque.IndexOf(s));
                }
                j++;
            }
        }

        [TestMethod]
        public void IndexOfNull_Test()
        {
            Deque<string> d = new Deque<string>();

            int cycles = 100;

            for (int i = 0; i < cycles; i++)
            {
                d.Add(i.ToString());
            }

            d[42] = null;

            Assert.AreEqual(42, d.IndexOf(null));
        }


    }
}
