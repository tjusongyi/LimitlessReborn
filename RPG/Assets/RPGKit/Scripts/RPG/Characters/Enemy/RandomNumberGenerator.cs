using UnityEngine;
using System.Collections;

public sealed class RandomNumberGenerator
{
	public static readonly RandomNumberGenerator Instance = new RandomNumberGenerator();

	private int seed;

	private RandomNumberGenerator()
	{
		seed = System.Environment.TickCount;
	}

	public int Next(int maxValue)
	{
		seed = (int)(1386674153 * (uint)seed + 265620371);
		return (int)(((long)maxValue * (uint)seed) >> 32);
	}
}