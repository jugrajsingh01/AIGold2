using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{

	private float intervalSpeed = 0.2f;
	private float nextTime = 0f;
	public Transform seeker, target;
	public GameManager grid;
  public List<Node> path;
  public int player;
	public GameObject pathPrefab;
  // enum Direction { Up, Down, Left, Right };
  // public Direction Direction;

	void Awake() {
		grid = (GameManager)FindObjectOfType(typeof(GameManager));
    path = new List<Node>();
	}

	void Update() {
		if (Time.time >= nextTime)
		{
			FindPath(seeker.position, target.position);
			nextTime += intervalSpeed;
		}
	}

	void FindPath(Vector3 startPos, Vector3 targetPos) {
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i ++) {
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) {
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode) {
				RetracePath(startNode,targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(node)) {
				if (!neighbour.walkable || closedSet.Contains(neighbour)) {
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode) {
		List<Node> _path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			//Instantiate(pathPrefab, currentNode.worldPosition, Quaternion.identity);
			_path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		_path.Reverse();

    path = _path;

    if(player == 1){
      grid.path = path;
    }
    else{
      grid.path_two = path;
    }


	}

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
}
