using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
	public float MoveSpeed = 250;
	
	[HideInInspector]
	public bool UseController = true;
	public float GridSize = 2;
	public enum DirectionType { AnyDirection, Rectangular, Diagonal };
	public DirectionType Movement = DirectionType.AnyDirection;
	public LayerMask Layers = ~0;
	public int MaxNodes = 5000;
	public int MaxHitTestPerFrame = 100;
	public float WaitBeforeRetry = 3;
	public float MaxHitTestDistance = 40;

	bool debug = false;

	Vector3 StartPos, TargetPos;
	public bool Moving = false;
	bool Finding;
	public LinkedList<Vector3> Path = new LinkedList<Vector3>();
	float pathPos, pathLineLen, pathPrevDist;
	Vector3 pathPrevPos, pathNextPos;
	float BodyWidth = 0.9f;
	float BodyHeight = 1;
	float PathYoffset;
	CharacterController characterController;
	CollisionFlags flags;
	
	void Start()
	{
		if (collider != null)
		{
			//get collider width and height
			Vector3 extents = transform.InverseTransformDirection(collider.bounds.extents);
			BodyWidth = new Vector2(extents.x, extents.z).magnitude / 1.5f;
			BodyHeight = extents.y;
			if (BodyWidth > BodyHeight)
			{
				BodyWidth *= 0.8f;
				PathYoffset = BodyWidth - BodyHeight;
				if (PathYoffset > 0) BodyHeight = BodyWidth;
			}
			BodyHeight += 0.1f;
			PathYoffset += 0.1f;
			if (debug) Debug.Log(name + " PathFinder: BodyHeight=" + BodyHeight + ", BodyWidth=" + BodyWidth);
		}

		characterController = GetComponent<CharacterController>();
	}

	public void Continue()
	{
		Moving = true;
		if (!Finding)
		{
			Finding = true;
			StartCoroutine(Find());
		}
	}

	public void GoTo(Vector3 position)
	{
		TargetPos = position;
		Continue();
	}

	public void Stop()
	{
		Moving = false;
	}

	void OnDrawGizmos()
	{
		if (debug && Moving)
		{
			//draw path
			Vector3 p = pathPrevPos;
			foreach (Vector3 item in Path)
			{
				if (p != Vector3.zero)
				{
					Vector3 start = p, end = item;
					Vector2 v2 = new Vector2(end.z - start.z, start.x - end.x);
					v2.Normalize();
					v2 *= BodyWidth;
					Vector3 v3 = new Vector3(v2.x, 0, v2.y);
					Debug.DrawLine(start - v3, end - v3, Color.yellow);
					Debug.DrawLine(start + v3, end + v3, Color.yellow);
					Debug.DrawLine(start, end, Color.red);
				}
				p = item;
			}
			Vector3 v = new Vector3(TargetPos.z - StartPos.z, 0, StartPos.x - TargetPos.x);
			v.Normalize();
			v *= BodyWidth;
			Vector3 c = collider.bounds.center;
			Debug.DrawLine(c + v, c - v, Color.magenta);
		}
	}

	bool NextPathNode()
	{
		Path.RemoveFirst();
		if (Path.Count == 0)
		{
			if (!Finding) //don't finish if finding path to new target position
			{
				Moving = false;
				SendMessage("PathEnd", SendMessageOptions.DontRequireReceiver);
				if (debug) Debug.Log("PathFinder: finish");
			}
			return true;
		}
		pathPrevPos = pathNextPos;
		pathNextPos = Path.First.Value;
		pathLineLen = Vector3.Distance(pathNextPos, pathPrevPos);
		pathPrevDist = 1e10f;
		return false;
	}

	void Update()
	{
		if (!Moving || Path.Count == 0) return;
		//move CharacterController
		Vector3 direction, next;
		float dist;
		for (; ; )
		{
			next = new Vector3(pathNextPos.x, transform.position.y, pathNextPos.z);
			direction = next - transform.position;
			dist = direction.magnitude;
			if (dist > 5 || dist < pathPrevDist) break;
			if (NextPathNode()) return;
		}
		transform.LookAt(next);
		pathPrevDist = dist;
		direction.Normalize();
		
		if (flags != CollisionFlags.CollidedBelow)
		{
			if (Terrain.activeTerrain != null)
			{
				Vector3 position = characterController.transform.position;
				position.y = Terrain.activeTerrain.SampleHeight(characterController.transform.position) + 0.75f;
				characterController.transform.position = position;
			}
			else
			{
				flags = characterController.Move(Vector3.down * 10 * Time.deltaTime);	
			}
		}
		else
		{
			flags = characterController.Move(direction * 4.5f * Time.deltaTime);
            Debug.Log(direction);
		}
	}

	Vector3 GetCurrentPosition()
	{
		Vector3 currentPos = (collider ? collider.bounds.center : transform.position);
		currentPos.y += PathYoffset;
		return currentPos;
	}


	class PathNode
	{
		public float Eval, Dist;
		public Vector3 Pos;
		public PathNode Prev;
	}

	float Grid, GridDiag;
	int DirectionCount;
	int HitTestCount, NextHitTestCount;
	IDictionary<Vector3, PathNode> Visited = new Dictionary<Vector3, PathNode>();
	IList<PathNode> Active = new List<PathNode>();

	void AddActive(PathNode node)
	{
		Active.Add(node);
		float x = node.Eval;
		int i, j;
		for (j = Active.Count - 1; j > 0; j = i)
		{
			i = (j - 1) >> 1;
			if (Active[i].Eval <= x) break;
			Active[j] = Active[i];
			j = i;
		}
		Active[j] = node;
	}

	PathNode MinActive()
	{
		PathNode result = Active[0];
		int lastIndex = Active.Count - 1;
		if (lastIndex == 0)
		{
			Active.Clear();
		}
		else
		{
			PathNode node = Active[lastIndex];
			float x = node.Eval;
			Active.RemoveAt(lastIndex);
			lastIndex--;
			int i = 1, j = 0;
			while (i <= lastIndex)
			{
				if (i < lastIndex && Active[i].Eval > Active[i + 1].Eval) i++;
				if (x <= Active[i].Eval) break;
				Active[j] = Active[i];
				j = i;
				i = 2 * i + 1;
			}
			Active[j] = node;
		}
		return result;
	}

	IEnumerator Find()
	{
	start:
		NextHitTestCount = MaxHitTestPerFrame;

		while (Path.Count == 0 || Vector3.Distance(TargetPos, Path.Last.Value) > GridSize)
		{
			HitTestCount = 0;
			StartPos = GetCurrentPosition();
			//if (debug) Debug.Log(name + " PathFinder: StartPos=" + StartPos + ", TargetPos=" + TargetPos);

			LinkedList<Vector3> path = new LinkedList<Vector3>();
			Grid = GridSize;
			GridDiag = Grid * 1.414214f;
			DirectionCount = Movement == DirectionType.Rectangular ? 4 : 8;

			//create start node
			PathNode node = new PathNode
			{
				Pos = StartPos,
				Dist = 0,
				Eval = Eval(StartPos, 0),
			};
			Visited.Add(StartPos, node);
			Active.Add(node);

			for (; ; )
			{
				if (Active.Count == 0 || Visited.Count > MaxNodes)
				{
					//path not found
					if (debug) Debug.Log(name + " PathFinder: path not found" + ", visited nodes=" + Visited.Count + ", hittest=" + HitTestCount);
					Clear();
					//wait and then try again
					yield return new WaitForSeconds(WaitBeforeRetry);
					goto start;
				}
				//get Active node which has minimal Eval
				node = MinActive();
				//check distance from target
				if (Vector3.Distance(node.Pos, TargetPos) < GridDiag + BodyWidth) break;

				//yield
				if (HitTestCount > NextHitTestCount)
				{
					NextHitTestCount += MaxHitTestPerFrame;
					//wait 1 frame
					yield return null;
					if (!Moving)
					{
						if (debug) Debug.Log(name + " PathFinder: stop");
						Clear();
						Finding = false;
						yield break;
					}
				}

				//test adjacent nodes
				for (int i = 0; i < DirectionCount; i++)
				{
					//next node position
					Vector3 nextPos = node.Pos;
					switch (i)
					{
						case 0:
							nextPos.x += Grid; break;
						case 1:
							nextPos.x -= Grid; break;
						case 2:
							nextPos.z += Grid; break;
						case 3:
							nextPos.z -= Grid; break;
						case 4:
							nextPos.x += Grid; nextPos.z += Grid; break;
						case 5:
							nextPos.x -= Grid; nextPos.z += Grid; break;
						case 6:
							nextPos.x += Grid; nextPos.z -= Grid; break;
						case 7:
							nextPos.x -= Grid; nextPos.z -= Grid; break;
					}
					//check if we can go to next node
					float newY;
					if (HitTest(node.Pos, nextPos, out newY))
					{
						nextPos.y = newY;
						//evaluate next node
						float lineLength = i >= 4 ? GridDiag : Grid;
						float dist = node.Dist + lineLength;
						float eval = Eval(nextPos, dist);
						PathNode nextNode;
						if (Visited.TryGetValue(nextPos, out nextNode))
						{
							//already visited, only correct evaluation
							if (nextNode.Dist > dist)
							{
								nextNode.Dist = dist;
								nextNode.Eval = eval;
								nextNode.Prev = node;
							}
						}
						else
						{
							//not yet visited, create a new node
							nextNode = new PathNode()
							{
								Dist = dist,
								Pos = nextPos,
								Eval = eval,
								Prev = node,
							};
							Visited.Add(nextPos, nextNode);
							AddActive(nextNode);
						}
					}
				}
			}
			//create path
			float resultDist = node.Dist;
			if (node.Pos != TargetPos) path.AddFirst(TargetPos);
			for (; node != null; node = node.Prev)
			{
				path.AddFirst(node.Pos);
			}
			int visitedCount = Visited.Count;
			Clear();

			int removed = 0;
			if (Movement == DirectionType.AnyDirection && path.Count > 2)
			{
				//smooth path by deleting some nodes
				LinkedListNode<Vector3> item1, item2, item3;
				item1 = path.First;
				item2 = item1.Next;
				item3 = item2.Next;
				for (; item3 != null; item2 = item3, item3 = item3.Next)
				{
					float newY;
					if (item2.Value - item1.Value == item3.Value - item2.Value
						|| Vector3.Distance(item1.Value, item3.Value) < MaxHitTestDistance
						&& HitTest(item1.Value, item3.Value, out newY))
					{
						path.Remove(item2);
						removed++;
					}
					else
					{
						item1 = item2;
					}

					//yield
					if (HitTestCount > NextHitTestCount)
					{
						NextHitTestCount += MaxHitTestPerFrame;
						yield return null;
					}
				}
			}
			if (debug) Debug.Log(name + " PathFinder: length=" + (int)resultDist + ", nodes=" + path.Count
				 + "-" + removed + ", visited=" + visitedCount + ", hittest=" + HitTestCount);
			//set result
			Path = path;
			pathPos = 1;
			pathNextPos = StartPos;
			pathLineLen = pathPrevDist = 1e10f;
			NextHitTestCount -= HitTestCount;
		}
		Finding = false;
	}

	void Clear()
	{
		Visited.Clear();
		Active.Clear();
	}

	float Eval(Vector3 pos, float dist)
	{
		return Vector3.Distance(TargetPos, pos) * 1.5f + dist;
	}

	bool HitTest(Vector3 start, Vector3 end, out float y)
	{
		HitTestCount++;

		//terrain height
		y = end.y;
		RaycastHit hit;
		if (Physics.Linecast(end, new Vector3(end.x, end.y - 10, end.z), out hit, Layers)
			&& hit.collider != collider)
		{
			y = hit.point.y + BodyHeight;
			if (y > end.y) end.y = y;
		}

		if (Vector3.Distance(transform.position, start) > 4 * BodyWidth)
			return CheckCapsule(start, end);
		else
			return CheckSphereCast(start, end);
	}

	bool CheckCapsule(Vector3 start, Vector3 end)
	{
		return !Physics.CheckCapsule(start, end, BodyWidth, Layers);
	}

	bool CheckSphereCast(Vector3 start, Vector3 end)
	{
		Vector3 direction = end - start;
		RaycastHit[] hits = Physics.SphereCastAll(start, BodyWidth, direction, direction.magnitude, Layers);
		if (hits.Length > 0)
		{
			foreach (RaycastHit hit in hits)
			{
				//ignore own collider
				if (hit.collider != collider) return false;
			}
		}
		return true;
	}
}
