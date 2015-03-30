using UnityEngine;
using System.Collections;


public sealed class BasicRandom
{
	public static readonly BasicRandom Instance = new BasicRandom();

	private int seed;

	private BasicRandom()
	{
		seed = System.Environment.TickCount;
	}

	public int Next(int maxValue)
	{
		seed = (int)(1386674153 * (uint)seed + 265620371);
		return (int)(((long)maxValue * (uint)seed) >> 32);
	}
}