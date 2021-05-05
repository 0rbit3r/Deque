using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Deque<T> : IDeque<T>
{

    private readonly int blockSize;

    private int count;

    private int startIndex;

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
        return new DequeEnum<T>(map, startIndex, Count);
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
    }

    public void RemoveFirst(T item)
    {
        startIndex++;
    }

    public void RemoveLast(T item)
    {
        count--;
    }

    public void Reverse()
    {
        throw new NotImplementedException();
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

class DequeEnum<T> : IEnumerator<T>
{
    DataBlock<T>[] map;
    int startIndex;
    int count;

    int currentIndex, currentMapIdx, currentBlockIdx;
    public DequeEnum(DataBlock<T>[] map, int startIndex, int count)
    {
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
        currentIndex++;
        if (currentBlockIdx >= DataBlock<T>.size - 1)
        {
            currentBlockIdx = 0;
            currentMapIdx++;
        }
        else
        {
            currentBlockIdx++;
        }

        if (currentIndex >= count)
        {
            return false;
        }

        return true;
    }

    public void Reset()
    {
        currentIndex = -1;
        currentMapIdx = (startIndex - 1) / DataBlock<T>.size;
        currentBlockIdx = (startIndex - 1) % DataBlock<T>.size;
    }
}

