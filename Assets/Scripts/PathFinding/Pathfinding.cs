using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;

public class Pathfinding : MonoBehaviour {
    
	public GameObject teste;

	PathRequestManager requestManager;
	Grid3D grid;

	// Vector3[] debugWaypoints;
	// public bool debugBool;
	// Tilemap collisionTileMap;
	// Tilemap areaTileMap;

	// public int gridArea;
	// public Vector2 xAxisLimit;
    // public Vector2 yAxisLimit;
	
	void Awake() {
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid3D>();
		// collisionTileMap = transform.Find("CollisionTileMap").GetComponent<Tilemap>();
		// areaTileMap = transform.Find("AreaTileMap").GetComponent<Tilemap>();
	}
	
	
	public void StartFindPath(Vector3 startPos, Vector3 targetPos, GameObject enemy) 
	{
		StartCoroutine(FindPath(startPos,targetPos, enemy));
	}
	
	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, GameObject enemy) 
	{
		// debugBool = false;
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;
		
		Node startNode = grid.NodeFromWorldPoint(startPos);
		// if(this.gameObject.name == "Boss1Arena")
		// {
		// 	Debug.Log("startNode.worldPosition: " + startNode.worldPosition + " - real one is: " + startPos + ". startNode.x: " + startNode.gridX + ", startNode.y: " + startNode.gridY);
		// }
		Node targetNode = grid.NodeFromWorldPoint(targetPos);
		// if(this.gameObject.name == "Boss1Arena")
		// {
		// 	Debug.Log("targetNode.worldPosition: " + targetNode.worldPosition + " - real one is: " + targetPos + ". targetNode.x: " + targetNode.gridX + ", targetNode.y: " + targetNode.gridY);
		// }
		
		Node helpStartNode = null;
		Node helpTargetNode = null;
		
		if(!startNode.walkable)
		{
			Debug.Log("!startNode.walkable");
			helpStartNode = startNode;
			startNode = grid.GetClosestNeighbor(startPos);
			startNode.parent = helpStartNode;
		}
		if(!targetNode.walkable)
		{
			Debug.Log("!targetNode.walkable");
			helpTargetNode = targetNode;
			targetNode = grid.GetClosestNeighbor(targetPos);
		}

		if(startNode.walkable && targetNode.walkable) 
		{
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
			//List<Vector2Int> openSetPositions = new List<Vector2Int>(grid.MaxSize);
			//HashSet<Node> closedSet = new HashSet<Node>();
			HashSet<Node> closedSet = new HashSet<Node>();
			//List<Vector2Int> closedSetPositions = new List<Vector2Int>(grid.MaxSize);
			openSet.Add(startNode);
			//openSetPositions.Add(startNode.GetPosition());
			
			while(openSet.Count > 0 ) 
			{
				Node currentNode = openSet.RemoveFirst();
				//openSetPositions.RemoveAt(0);
				closedSet.Add(currentNode);
				//closedSetPositions.Add(currentNode.GetPosition());
				
				if(currentNode.gridX == targetNode.gridX && currentNode.gridY == targetNode.gridY) 
				{
					targetNode.parent = currentNode.parent;
					pathSuccess = true;
					break;
				}
				
				if(grid.GetNeighbours(currentNode).Count < 1)
				{
					pathSuccess = false;
					break;
				}
				
				foreach(Node neighbour in grid.GetNeighbours(currentNode)) 
				{	
					// Debug.Log("currentNode = " + currentNode.gridX + ", " + currentNode.gridY + ", neighbour = " + neighbour.gridX + ", " + neighbour.gridY);
					//yield return new WaitForSeconds(0.01f);

					if(!neighbour.walkable || closedSet.Contains(neighbour)
					//closedSetPositions.Contains(neighbour.GetPosition())
					) 
					{
						continue;
					}
					
					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

					// Debug.Log("currentNode = " + currentNode.gridX + ", " + currentNode.gridY);
					// yield return new WaitForSeconds(0.1f);
					if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)
					//!openSetPositions.Contains(neighbour.GetPosition())
					) 
					{
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;
						
						if(!openSet.Contains(neighbour)
						//!openSetPositions.Contains(neighbour.GetPosition())
						)
						{
							// Debug.Log(enemy.name);
							
							openSet.Add(neighbour);
							//openSetPositions.Add(startNode.GetPosition());
							//Debug.Log("openSet.Size: " + grid.MaxSize + ", openSet.Count: " + openSet.Count);
						}	
					}
				}

				//Debug.Log("openSet.Count: " + openSet.Count);

				// if(openSet.Count > 100)
				// {
				// 	Debug.Log("toma");
				// 	pathSuccess = false;
				// 	foreach(Node node in openSet.GetItems())
				// 	{
				// 		GameObject insObj = Instantiate(teste, node.worldPosition, Quaternion.identity);
				// 		insObj.GetComponent<AutoDelete>().nodeXY = new Vector2Int(node.gridX, node.gridY);
				// 	}
				// 	break;
				// }

				// foreach(Node node in openSet.GetItems())
				// {
				// 	//Instantiate(teste, node.worldPosition, teste.transform.rotation);
				// 	//insObj.GetComponent<AutoDelete>().nodeXY = new Vector2Int(node.gridX, node.gridY);
				// }

				//yield return new WaitForSeconds(1);
			}
		}
		yield return null;
		if(pathSuccess) 
		{
			if(helpStartNode != null)
			{
				if(helpTargetNode != null)
				{
					waypoints = RetracePath(helpStartNode,helpTargetNode);
				}
				else
				{
					waypoints = RetracePath(helpStartNode,targetNode);
				}
			}
			else
			{
				waypoints = RetracePath(startNode,targetNode);
			}
		}
		// debugWaypoints = new Vector3[0];
		// debugWaypoints = waypoints;
		// debugBool = true;
		requestManager.FinishedProcessingPath(waypoints,pathSuccess);
	}

	// void OnDrawGizmosSelected()
    // {
	// 	if(debugBool)
	// 	{
	// 		foreach(Vector3 tile in debugWaypoints)
	// 		{
	// 			Gizmos.color = new Color(0,1,1,1f);
	// 			Gizmos.DrawCube(tile, Vector3.one * 0.25f);
	// 		}
	// 	}	
    // }

	// public bool CheckIfContain(Node[] array, Vector2Int position)
	// {
	// 	foreach(Node node in array)
	// 	{
	// 		if(node.GetPosition() == position)
	// 		{
	// 			return true;
	// 		}
	// 	}
	// 	return false;
	// }

	// public Node NodeFromWorldPoint(Vector3 worldPosition) //<------------
    // {
    //     Vector3Int nodePosition = grid.WorldToCell(worldPosition);

	// 	if(!areaTileMap.HasTile(nodePosition))
	// 	{
	// 		nodePosition.x = (int)Mathf.Clamp(nodePosition.x ,xAxisLimit.x, xAxisLimit.y);
	// 		nodePosition.y = (int)Mathf.Clamp(nodePosition.y ,yAxisLimit.x, yAxisLimit.y);
	// 	}

	// 	Node node = new Node(!collisionTileMap.HasTile(nodePosition), grid.CellToWorld(nodePosition), nodePosition.x, nodePosition.y);

	// 	return node;
    // }

    // public Node NodeFromVector3(Vector3Int nodePos) //<------------
    // {
    //     Vector3 worldPosition = grid.CellToWorld(nodePos);

	// 	Node node = new Node(!collisionTileMap.HasTile(nodePos), worldPosition, nodePos.x, nodePos.y);

	// 	return node;
    // }

	// public List<Node> GetNodeNeighbours(Node node) 
	// {
	// 	List<Node> neighbours = new List<Node>();

	// 	for(int x = -1; x <= 1; x++) 
	// 	{
	// 		for(int y = -1; y <= 1; y++) 
	// 		{
	// 			if(x == 0 && y == 0)
	// 			{
	// 				continue;
	// 			}
					
	// 			int checkX = node.gridX + x;
	// 			int checkY = node.gridY + y;

	// 			if(areaTileMap.HasTile(new Vector3Int(checkX, checkY, 0)))
	// 			{
	// 				neighbours.Add(NodeFromVector3(new Vector3Int(checkX, checkY, 0)));
	// 			}
	// 		}
	// 	}

	// 	//Debug.Log("Node (" + node.gridX + ", " + node.gridY + ") tem " + neighbours.Count + " vizinhos");
	// 	return neighbours;
	// }
	
	Vector3[] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;

		// List<Vector3> waypoints = new List<Vector3>();

		// foreach(Node node in path)
		// {
		// 	waypoints.Add(node.worldPosition);
		// }

		// Vector3[] testWaypoints = waypoints.ToArray();

		// Array.Reverse(testWaypoints);
		// return testWaypoints;
	}
	
	Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;
		
		for (int i = 1; i < path.Count; i ++) {
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
			if (directionNew != directionOld) {
				waypoints.Add(path[i-1].worldPosition);
				// if(this.gameObject.name == "Boss1Arena")
				// {
				// 	Debug.Log("final path: " + path[i-1].worldPosition + ". x: " + path[i-1].gridX + ", y: " + path[i-1].gridY);
				// }
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}
	
	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		
		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
}