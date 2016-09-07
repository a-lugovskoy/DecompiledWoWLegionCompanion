using System;
using System.Collections.Generic;
using System.IO;

public class ThreadSafeStack : IDisposable, MemoryStreamStack
{
	private Stack<MemoryStream> stack = new Stack<MemoryStream>();

	public MemoryStream Pop()
	{
		Stack<MemoryStream> stack = this.stack;
		MemoryStream result;
		lock (stack)
		{
			if (this.stack.get_Count() == 0)
			{
				result = new MemoryStream();
			}
			else
			{
				result = this.stack.Pop();
			}
		}
		return result;
	}

	public void Push(MemoryStream stream)
	{
		Stack<MemoryStream> stack = this.stack;
		lock (stack)
		{
			this.stack.Push(stream);
		}
	}

	public void Dispose()
	{
		Stack<MemoryStream> stack = this.stack;
		lock (stack)
		{
			this.stack.Clear();
		}
	}
}
