using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

interface IDeque<T> : IList<T>
{
    public void Append(T item);

    public void Prepend(T item);

    public void RemoveFirst();

    public void RemoveLast();

    public IList<T> GetReversed();
}
