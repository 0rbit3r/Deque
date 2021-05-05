using Deque_v1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DequeTests
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void Add_Test()
        {
            Deque<int> deque = new Deque<int>();

            for (int i = 0; i < 200; i++)
            {
                deque.Add(i);
            }

            for (int i = 0; i < 200; i++)
            {
                Assert.AreEqual(deque[i], i);
            }


        }

        [TestMethod]
        public void Add_Remove_Test()
        {
            Deque<int> deque = new Deque<int>();

            int cycles = 200;

            for (int i = 0; i < cycles; i++)
            {
                deque.Add(i);
            }

            for (int i = 0; i < cycles / 2; i++)
            {
                deque.Remove(i);
            }

            for (int i = 0; i < deque.Count; i++)
            {
                Assert.AreEqual(deque[i], i + cycles / 2);
            }
        }

        [TestMethod]
        public void Add_Insert_Test()
        {
            Deque<int> deque = new Deque<int>();
            for (int i = 0; i < 20; i++)
            {
                deque.Add(i);
            }
            for (int i = 1; i < 10; i++)
            {
                deque.Insert(0, -i);
            }

            for (int i = 0; i < deque.Count; i++)
            {
                Assert.AreEqual(deque[i], i - 9);
            }


        }

        [TestMethod]
        public void CopyTo_Test()
        {
            Deque<int> deque = new Deque<int>();

            int cycles = 200;

            for (int i = 0; i < cycles; i++)
            {
                deque.Add(i);
            }

            int[] arr = new int[cycles + 10];

            deque.CopyTo(arr, 10);

            for (int i = 0; i < cycles; i++)
            {
                Assert.AreEqual(arr[i + 10], i);
            }
        }

    }
}