using UnityEngine;
using System.Collections.Generic;

public class Grid3D : MonoBehaviour {

	public bool displayGridGizmos;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake() {
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

	void CreateGrid() {
		grid = new Node[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				grid[x,y] = new Node(walkable,worldPoint, x,y);
			}
		}
	}

	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		// for (int x = -1; x <= 1; x++) {
		// 	for (int y = -1; y <= 1; y++) {
		// 		if (x == 0 && y == 0)
		// 			continue;

		// 		int checkX = node.gridX + x;
		// 		int checkY = node.gridY + y;

		// 		if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
		// 			neighbours.Add(grid[checkX,checkY]);
		// 		}
		// 	}
		// }	

		for(int x = -1; x <= 1; x += 2)
		{
			int checkX = node.gridX + x;

			if(checkX >= 0 && checkX < gridSizeX && node.gridY >= 0 && node.gridY < gridSizeY) 
			{
				neighbours.Add(grid[checkX,node.gridY]);
			}	
		}

		for(int y = -1; y <= 1; y += 2)
		{
			int checkY = node.gridY + y;

			if(node.gridX >= 0 && node.gridX < gridSizeX && checkY >= 0 && checkY < gridSizeY) 
			{
				neighbours.Add(grid[node.gridX,checkY]);
			}	
		}

		for(int y = -1; y <= 1; y += 2)
		{
			for(int x = -1; x <= 1; x += 2)
			{
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) 
				{
					if(grid[node.gridX,checkY].walkable && grid[checkX,node.gridY].walkable)
					{
						neighbours.Add(grid[checkX,checkY]);
					}
				}	
			}	
		}

		return neighbours;
	}

	public Node GetClosestNeighbor(Vector3 worldPosition) //<------------- COLOCAR ISSO NO GRID
	{
		Node node = NodeFromWorldPoint(worldPosition);
		List<Node> neighbours = GetNeighbours(node);

		float nodeDistance = 100;
		Node closestNeighbor = node;

		foreach(Node neighbor in neighbours)
		{
			// Vector3Int nodePos = new Vector3Int(neighbor.gridX, neighbor.gridY);
			Vector3 neighborWorldPosition = grid[neighbor.gridX, neighbor.gridY].worldPosition;
			//CellToWorld(nodePos);

			if(neighbor.walkable && Vector3.Distance(worldPosition, neighborWorldPosition) < nodeDistance)
			{
				nodeDistance = Vector3.Distance(worldPosition, neighborWorldPosition);
				closestNeighbor = neighbor;
			}
		}

		return closestNeighbor;
	}
	
    // public Vector3Int WorldToCell(Vector3 worldPosition)
    // {
    //     float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
	// 	float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
	// 	percentX = Mathf.Clamp01(percentX);
	// 	percentY = Mathf.Clamp01(percentY);

	// 	int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
	// 	int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
	// 	return new Vector3Int(x,y,0);
    // }

    // public Vector3 CellToWorld(Vector3Int cellPosition)
    // {
	// 	return grid[cellPosition.x, cellPosition.y].worldPosition;
    // }

	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);

		x = Mathf.Clamp(x ,0, gridSizeX);
		y = Mathf.Clamp(y ,0, gridSizeY);

		return grid[x,y];
	}

	public bool ValidatePosition(Vector3 position)
	{
		float percentX = (position.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (position.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int nodeX = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int nodeY = Mathf.RoundToInt((gridSizeY-1) * percentY);

		bool validation = true;

		for(int x = -1; x <= 1; x++) 
		{
			for(int y = -1; y <= 1; y++) 
			{		
				int checkX = nodeX + x;
				int checkY = nodeY + y;

				if (checkX < 0 || checkX > gridSizeX || checkY < 0 || checkY > gridSizeY) {
					validation = false;
				}
			}
		}

		return validation;
	}
	
	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
		if (grid != null && displayGridGizmos) {
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable)?new Color(1,1,1,0.05f):new Color(1,0,0,0.05f);
				if(n.gridX == 0 && n.gridY == 0)
				{
					Gizmos.color = Color.green;
				}
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
			}
		}
	}
}