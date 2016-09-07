using System;
using System.Collections.Generic;
using System.IO;

public class ThreadUnsafeStack : IDisposable, MemoryStreamStack
{
	private Stack<MemoryStream> stack = new Stack<MemoryStream>();

	public MemoryStream Pop()
	{
		if (this.stack.get_Count() == 0)
		{
			return new MemoryStream();
		}
		return this.stack.Pop();
	}

	public void Push(MemoryStream stream)
	{
		this.stack.Push(stream);
	}

	public void Dispose()
	{
		this.stack.Clear();
	}
}
