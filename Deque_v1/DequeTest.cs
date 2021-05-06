using System;
using System.Collections.Generic;
using System.Text;


public static class DequeTest
{
	public static IList<T> GetReverseView<T>(Deque<T> d)
	{
		return d.GetReversed();
	}
}

