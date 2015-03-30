using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Cell
{
	public bool[] Walls;
	public bool Visited;
	public bool Blocked;
}

public enum Direction
{
	North, South, West, East, Unknown
}

public struct Point
{
	public int X, Y;
	public Point(int X, int Y) { this.X = X; this.Y = Y; }
}

public class Dungeon
{
	public int Width, Height;
	private readonly Cell[,] cells;

	public Dungeon(int width, int height)
	{
		Width = width;
		Height = height;
		cells = new Cell[width, height];
		for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++)
			{
				bool[] walls = new bool[4];
				walls[0] = walls[1] = walls[2] = walls[3] = true;
				cells[x, y].Walls = walls;
			}
	}

	public Cell this[Point point]
	{
		get { return cells[point.X, point.Y]; }
	}

	public Cell this[int x, int y]
	{
		get { return cells[x, y]; }
	}

	public void SetBlocked(int x, int y)
	{
		cells[x, y].Blocked = true;
	}

	public void SetVisited(Point currentLocation, bool value)
	{
		cells[currentLocation.X, currentLocation.Y].Visited = value;
	}

	private Point? NextCell(Point location, Direction direction)
	{
		switch (direction)
		{
			case Direction.North:
				if (location.Y > 0) return new Point(location.X, location.Y - 1);
				break;
			case Direction.West:
				if (location.X > 0) return new Point(location.X - 1, location.Y);
				break;
			case Direction.South:
				if (location.Y < Height - 1) return new Point(location.X, location.Y + 1);
				break;
			case Direction.East:
				if (location.X < Width - 1) return new Point(location.X + 1, location.Y);
				break;
		}
		return null;
	}

	public bool CellInDirectionIsVisited(Point location, Direction direction)
	{
		Point? target = NextCell(location, direction);
		if (!target.HasValue) return false;
		return this[target.Value].Visited;
	}

	public bool CellInDirectionIsBlocked(Point location, Direction direction, bool allowAdjacent)
	{
		Point? target = NextCell(location, direction);
		if (!target.HasValue) return true;

		Cell cell = this[target.Value];
		if (cell.Blocked || cell.Visited) return true;

		if (!allowAdjacent)
		{
			for (int i = 0; i < 4; i++)
			{
				if ((i ^ (int)direction) != 1)
				{
					Point? target2 = NextCell(target.Value, (Direction)i);
					if (target2.HasValue)
					{
						if (this[target2.Value].Visited || i == (int)direction
							&& (CellInDirectionIsVisited(target2.Value, (Direction)(i ^ 2))
							|| CellInDirectionIsVisited(target2.Value, (Direction)(i ^ 3))))
						{
							cell.Blocked = true;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public Point DestroyWall(Point location, Direction direction)
	{
		Point target = NextCell(location, direction).Value;
		this[location].Walls[(int)direction] =
			this[target].Walls[((int)direction) ^ 1] = false;
		return target;
	}

	public Point CreateWall(Point location, Direction direction)
	{
		Point target = NextCell(location, direction).Value;
		this[location].Walls[(int)direction] =
			this[target].Walls[((int)direction) ^ 1] = true;
		return target;
	}

	public IEnumerable<Point> ConnectCorridorsEnumerator
	{
		get
		{
			for (int x = 2; x < Width - 2; x++)
				for (int y = 2; y < Height - 2; y++)
					if (!this[x, y].Visited)
					{
						bool c1 = this[x, y + 1].Visited;
						bool c2 = this[x, y - 1].Visited;
						if (c1 == c2)
						{
							bool c3 = this[x + 1, y].Visited;
							if (c2 != c3)
							{
								bool c4 = this[x - 1, y].Visited;
								if (c3 == c4)
								{
									Cell c5, c6;
									if (c1)
									{
										c5 = this[x + 2, y]; c6 = this[x - 2, y];
									}
									else
									{
										c5 = this[x, y + 2]; c6 = this[x, y - 2];
									}
									if (!c5.Visited && !c6.Visited)
										yield return new Point(x, y);
								}
							}
						}
					}
		}
	}
}