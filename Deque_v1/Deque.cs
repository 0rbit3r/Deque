using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Deque<T> : IDeque<T>
{

    private readonly int blockSize;

    private int count;

    private int startIndex;

    private int _version = 0;

    private DataBlock<T>[] map;

    public Deque()
    {
        blockSize = DataBlock<T>.size;

        Clear();
    }

    private void IncIndexes(ref int map, ref int block)
    {
        if (block >= blockSize - 1)
        {
            block = 0;
            map++;
        }
        else
        {
            block++;
        }
    }

    private void DoubleCapacity()
    {
        DataBlock<T>[] newMap = new DataBlock<T>[map.Length * 2];

        for (int i = 0; i < map.Length / 2; i++)
        {
            newMap[i] = new DataBlock<T>();
            newMap[i + map.Length / 2 * 3] = new DataBlock<T>();
        }

        for (int i = 0; i < map.Length; i++)
        {
            newMap[i + map.Length / 2] = map[i];
        }

        startIndex += (map.Length / 2) * blockSize;
        map = newMap;
    }

    /// <summary>
    /// Returns element at desired index. Can be used with less than zero or more than count indexes provided, caller knows what they are doing.
    /// </summary>
    /// <param name="index">Indexed from first element as viewed from the outside</param>
    /// <returns></returns>
    private T getElementAt(int index)
    {
        return map[(startIndex + index) / blockSize][(startIndex + index) % blockSize];
    }

    /// <summary>
    /// Replaces element at desired index. Can be used with less than zero or more than count indexes provided, caller knows what they are doing.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="element"></param>
    private void setElementAt(int index, T element)
    {
        map[(startIndex + index) / blockSize][(startIndex + index) % blockSize] = element;
    }

    public T this[int index]
    {
        get
        {
            if (index >= count)
            {
                throw new IndexOutOfRangeException();
            }
            return getElementAt(index);

        }
        set
        {
            if (index >= count)
            {
                throw new IndexOutOfRangeException();
            }
            setElementAt(index, value);
        }
    }

    public int Count => count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
        if (startIndex + count >= map.Length * blockSize || startIndex == 0)
        {
            DoubleCapacity();
        }

        setElementAt(count, item);
        count++;
        _version++;
    }

    public void Clear()
    {
        count = 0;
        startIndex = blockSize * 2;

        map = new DataBlock<T>[4];

        for (int i = 0; i < map.Length; i++)
        {
            map[i] = new DataBlock<T>();
        }
        _version++;
    }

    public bool Contains(T item)
    {
        return this.Any((t) => t.Equals(item));
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
        {
            throw new ArgumentNullException();
        }
        if (arrayIndex < 0)
        {
            throw new ArgumentOutOfRangeException();
        }
        if (arrayIndex + Count > array.Length)
        {
            throw new ArgumentException();
        }

        int i = arrayIndex;
        foreach (T item in this)
        {
            array[i++] = item;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new DequeEnum<T>(this);
    }

    public int IndexOf(T item)
    {
        int i = 0;
        foreach (T element in this)
        {
            if (element.Equals(item))
                return i;
            i++;
        }
        return -1;
    }

    public void Insert(int index, T item)
    {
        if (index == 0)
        {
            Prepend(item);
            return;
        }

        if (startIndex + Count >= map.Length * blockSize)
        {
            DoubleCapacity();
        }

        int mapIdx = (startIndex + index) / blockSize,
            blockIdx = (startIndex + index) % blockSize;

        T itemSwapper1 = map[mapIdx][blockIdx];
        T itemSwapper2;
        map[mapIdx][blockIdx] = item;

        for (int i = index; i < Count + 1; i++)
        {
            IncIndexes(ref mapIdx, ref blockIdx);
            itemSwapper2 = map[mapIdx][blockIdx];
            map[mapIdx][blockIdx] = itemSwapper1;
            itemSwapper1 = itemSwapper2;
        }

        count++;
        _version++;
    }

    public bool Remove(T item)
    {
        int mapIdx = (startIndex) / blockSize,
            blockIdx = (startIndex) % blockSize;

        int i = 0;
        for (; !map[mapIdx][blockIdx].Equals(item) && i < Count; i++)
        {
            IncIndexes(ref mapIdx, ref blockIdx);
        }

        if (map[mapIdx][blockIdx].Equals(item))
        {
            RemoveAt(i);
            return true;
        }

        return false;
    }

    public void RemoveAt(int index)
    {
        int mapIdx = (startIndex + index) / blockSize,
            blockIdx = (startIndex + index) % blockSize;
        int oldMap, oldBlock;

        for (int i = index; i < Count - 1; i++)
        {
            oldBlock = blockIdx;
            oldMap = mapIdx;
            IncIndexes(ref mapIdx, ref blockIdx);
            map[oldMap][oldBlock] = map[mapIdx][blockIdx];
        }

        count--;
        _version++;
        CleanMap();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Append(T item)
    {
        Add(item);
    }

    public void Prepend(T item)
    {
        if (startIndex + count >= map.Length * blockSize || startIndex == 0)
        {
            DoubleCapacity();
        }

        setElementAt(-1, item);
        count++;
        startIndex--;
        _version++;
    }

    public void RemoveFirst()
    {
        CleanMap();

        startIndex++;
        count--;
        _version++;
    }

    public void RemoveLast()
    {
        CleanMap();
        count--;
        _version++;
    }

    public IList<T> GetReversed()
    {
        return new ReversedDeque<T>(this, map, startIndex);
    }

    private void CleanMap()
    {
        return;
        if(startIndex >= map.Length * blockSize / 2 || startIndex + Count <= map.Length * blockSize / 2)
        {
            int mapStartIdx = startIndex / blockSize;
            int populatedMaps = (startIndex + count) / blockSize - mapStartIdx + 1;
            int indexOffset = startIndex % blockSize;

            int newMapSize = populatedMaps < map.Length / 8 ? map.Length / 2 : map.Length;
            DataBlock<T>[] newMap = new DataBlock<T>[newMapSize];

            int valuesLowBound = newMapSize / 2 - Math.Max(1, populatedMaps / 2);
            int valuesHighBound = newMapSize / 2 + populatedMaps / 2;

            for (int i = 0; i < valuesLowBound; i++)
            {
                newMap[i] = new DataBlock<T>();
            }

            
            for (int i = valuesLowBound, j = 0; i < valuesHighBound; i++, j++)
            {
                newMap[i] = map[mapStartIdx + j];
            }

            for (int i = valuesHighBound; i < newMap.Length; i++)
            { 
                newMap[i] = new DataBlock<T>();
            }

            map = newMap;
            startIndex = valuesLowBound * blockSize + indexOffset;
        }
    }

    public class DequeEnum<U> : IEnumerator<U>
    {
        Deque<U> deque;

        int version;

        int currentIndex, currentMapIdx, currentBlockIdx;
        internal DequeEnum(Deque<U> deque)
        {

            this.deque = deque;
            version = deque._version;

            Reset();
        }
        public U Current => deque.map[currentMapIdx][currentBlockIdx];

        object IEnumerator.Current => Current;

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            if (deque._version != version)
            {
                throw new InvalidOperationException();
            }
            currentIndex++;

            if (currentIndex >= deque.Count)
            {
                return false;
            }

            deque.IncIndexes(ref currentMapIdx, ref currentBlockIdx);

            return true;
        }

        public void Reset()
        {
            currentIndex = -1;
            currentMapIdx = (deque.startIndex - 1) / DataBlock<T>.size;
            currentBlockIdx = (deque.startIndex - 1) % DataBlock<T>.size;
        }
    }

    public class ReversedDeque<T> : IDeque<T>
    {
        private DataBlock<T>[] map;

        public T this[int index] { get => original[Count - index - 1]; set { original[Count - index - 1] = value; } }

        public int Count => original.Count;

        public bool IsReadOnly => false;

        private Deque<T> original;

        internal ReversedDeque(Deque<T> original, DataBlock<T>[] map, int startIndex)
        {
            this.original = original;
            this.map = map;
        }


        public void Add(T item)
        {
            original.Prepend(item);

        }

        public void Append(T item)
        {
            original.Prepend(item);
        }

        public void Clear()
        {
            original.Clear();
        }

        public bool Contains(T item)
        {
            return original.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (arrayIndex + Count > array.Length)
            {
                throw new ArgumentException();
            }

            int i = arrayIndex;
            foreach (T item in this)
            {
                array[i++] = item;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ReversedDequeEnum<T>(this, map, original.startIndex, Count);
        }

        public int IndexOf(T item) //Will not return first one!
        {
            return Count - original.IndexOf(item) - 1;
        }

        public void Insert(int index, T item)
        {
            original.Insert(Count - index - 1, item);
        }

        public void Prepend(T item)
        {
            original.Append(item);
        }

        public bool Remove(T item)
        {
            return original.Remove(item);
        }

        public void RemoveAt(int index)
        {
            original.RemoveAt(Count - index - 1);
        }

        public void RemoveFirst()
        {
            original.RemoveLast();
        }

        public void RemoveLast()
        {
            original.RemoveFirst();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IList<T> GetReversed()
        {
            return original;
        }

        class ReversedDequeEnum<T> : IEnumerator<T>
        {
            ReversedDeque<T> reversedDeque;
            DataBlock<T>[] map;
            int startIndex;
            int count;
            int version;

            int currentIndex, currentMapIdx, currentBlockIdx;
            public ReversedDequeEnum(ReversedDeque<T> reversed, DataBlock<T>[] map, int startIndex, int count)
            {
                reversedDeque = reversed;

                version = reversedDeque.original._version;

                this.map = map;
                this.startIndex = startIndex;
                this.count = count;

                Reset();
            }
            public T Current => map[currentMapIdx][currentBlockIdx];

            object IEnumerator.Current => Current;

            public void Dispose()
            {

            }

            public bool MoveNext()
            {
                if (reversedDeque.original._version != version)
                {
                    throw new InvalidOperationException();
                }
                currentIndex--;
                if (currentBlockIdx <= 0)
                {
                    currentBlockIdx = DataBlock<T>.size - 1;
                    currentMapIdx--;
                }
                else
                {
                    currentBlockIdx--;
                }

                if (currentIndex < 0)
                {
                    return false;
                }

                return true;
            }

            public void Reset()
            {
                currentIndex = count;
                currentMapIdx = (startIndex + count) / DataBlock<T>.size;
                currentBlockIdx = (startIndex + count) % DataBlock<T>.size;
            }
        }

    }


}

internal class DataBlock<U>
{
    public static readonly int size = 20;

    private U[] dataArray;

    public DataBlock()
    {
        dataArray = new U[size];
    }

    public U this[int index]
    {
        get => dataArray[index];
        set { dataArray[index] = value; }
    }
}
