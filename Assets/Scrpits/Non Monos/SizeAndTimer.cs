using System;

public class SizeAndTimer
{
	public int size;
	public float timer;
	public SizeAndTimer ()
	{
	}
	public SizeAndTimer (int size, float timer)
	{
		this.size = size;
		this.timer=timer;
	}

	public override bool Equals (object obj)
	{
		SizeAndTimer a = (SizeAndTimer)obj;
		return ((a.timer == timer) & (a.size == size));
	}

	public override int GetHashCode ()
	{
		return (int)timer + size;
	}
}

