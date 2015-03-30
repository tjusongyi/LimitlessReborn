using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator
{
	public static Dungeon Generate(int width, int height, int startX, int startY, bool allowAdjacent, int directionParameter, int sparsenessParameter, int cycleParameter)
	{
		if (!allowAdjacent) sparsenessParameter /= 3;

		Dungeon map = new Dungeon(width, height);
		for (int x = 0; x < width; x++)
		{
			map.SetBlocked(x, 0);
			map.SetBlocked(x, height - 1);
		}
		for (int y = 0; y < height; y++)
		{
			map.SetBlocked(0, y);
			map.SetBlocked(width - 1, y);
		}

		CreateMaze(map, startX, startY, allowAdjacent, directionParameter);
		ConnectCorridors(map, allowAdjacent, cycleParameter);
		RemoveCorridors(map, startX, startY, sparsenessParameter);
		return map;
	}

	public static void CreateMaze(Dungeon map, int startX, int startY, bool allowAdjacent, int directionParameter)
	{
		Point current = new Point(startX, startY);
		Direction previousDirection = Direction.North;
		Direction direction;
		List<Point> visited = new List<Point>();
		int index = 0;

		for (; ; )
		{
			map.SetVisited(current, true);
			visited.Add(current);

			if (directionParameter < BasicRandom.Instance.Next(100)
				&& !map.CellInDirectionIsBlocked(current, previousDirection, allowAdjacent))
			{
				direction = previousDirection;
			}
			else
			{
				int mask = 0;
				for (; ; )
				{
					do
					{
						direction = (Direction)BasicRandom.Instance.Next(4);
					} while ((mask & (1 << (int)direction)) != 0);

					if (!map.CellInDirectionIsBlocked(current, direction, allowAdjacent)) break;
					mask |= (1 << (int)direction);
					if (mask == 15)
					{
						visited[index] = visited[visited.Count - 1];
						visited.RemoveAt(visited.Count - 1);
						if (visited.Count == 0) return;
						index = BasicRandom.Instance.Next(visited.Count);
						current = visited[index];
						mask = 0;
					}
				}
			}
			previousDirection = direction;
			current = map.DestroyWall(current, direction);
		}
	}

	public static void ConnectCorridors(Dungeon map, bool allowAdjacent, int cycleParameter)
	{
		if (allowAdjacent)
		{
			for (int count = (map.Width * map.Height * cycleParameter) / 1000; count > 0; count--)
			{
				map.DestroyWall(new Point(BasicRandom.Instance.Next(map.Width - 4) + 2,
					BasicRandom.Instance.Next(map.Height - 4) + 2),
					(Direction)BasicRandom.Instance.Next(4));
			}
		}
		else
		{
			foreach (Point point in map.ConnectCorridorsEnumerator)
			{
				if (BasicRandom.Instance.Next(300) < cycleParameter)
				{
					if (map.CellInDirectionIsVisited(point, Direction.East))
					{
						map.DestroyWall(point, Direction.East);
						map.DestroyWall(point, Direction.West);
					}
					else
					{
						map.DestroyWall(point, Direction.North);
						map.DestroyWall(point, Direction.South);
					}
					map.SetVisited(point, true);
				}
			}
		}
	}

	public static void RemoveCorridors(Dungeon map, int startX, int startY, int sparsenessParameter)
	{
		int count = (map.Width * map.Height * sparsenessParameter) / 100;
		bool removed;
		do
		{
			removed = false;
			for (int x = 0; x < map.Width; x++)
				for (int y = 0; y < map.Height; y++)
				{
					bool[] walls = map[x, y].Walls;
					if ((walls[0] ? 1 : 0) + (walls[1] ? 1 : 0) + (walls[2] ? 1 : 0) + (walls[3] ? 1 : 0) == 3
							&& (x != startX || y != startY))
					{
						if (count-- <= 0) return;
						Point point = new Point(x, y);
						map.SetVisited(point, false);
						for (int j = 0; j < 4; j++)
							if (!walls[j])
							{
								map.CreateWall(point, (Direction)j);
								break;
							}
						removed = true;
					}
				}
		} while (removed);
	}
}
