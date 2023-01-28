using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  //[SerializeField]
  //public Rect gameField;

    public LayerMask unwalkableMask;
  	[SerializeField] public Vector2 gridWorldSize;
  	public float nodeRadius;
  	public Node[,] grid;
    public List<Node> path;
    public List<Node> path_two;
    public List<Node> combined_path;
    private float nextTime = 0f;
    private float intervalSpeed = 0.2f;
    public List<Collider2D> results;
    public int player;

  	float nodeDiameter;
  	int gridSizeX, gridSizeY;

  	void Awake() {
      combined_path = new List<Node>();
  		nodeDiameter = nodeRadius*2;
  		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
  		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
  		CreateGrid();
  	}

    void Update(){
        CreateGrid();
    }

  	public void CreateGrid() {
      Node[,] _grid = new Node[gridSizeX,gridSizeY];
  		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;

  		for (int x = 0; x < gridSizeX; x ++) {
  			for (int y = 0; y < gridSizeY; y ++) {
  				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
  				bool walkable = !(Physics2D.OverlapCircle(worldPoint,nodeRadius,unwalkableMask));


          //This was me tryinng to get the path of the other snake, in order to make them interfere with eachother lesser,
          //for some reason this helped increasing the avg score and game time, im not sure why though since most times the games ended when the
          // snakes were close to each other, and im only avoiding the middle of their paths
          //and the first 5 and last 5 nodes are being ignored
          // i guess the algorithm now gets away from the other snake in the beginning to find a shorter path while ignoring the middle

          //this required me to create 2 seperate grids and calculate 2 seperate paths, which means that its slower however
          //it can be more optimal in the way that it can create longer games with higher scores

          //Try running it with the code enabled and disabled

          //I tried my best

          // if(walkable){
          //   List<Node> temp;
          //   if(player == 1){
          //     temp = path_two;
          //   }
          //   else{
          //     temp = path;
          //   }
          //   if(temp != null){
          //     if(temp.Skip(5).SkipLast(5).Any(n => n.worldPosition == worldPoint)){
          //       Debug.Log(temp.FirstOrDefault().worldPosition);
          //       walkable = false;
          //     }
          //   }
          // }

  				_grid[x,y] = new Node(walkable,worldPoint,x,y);
  			}
  		}
      grid = _grid;
  	}

  	public List<Node> GetNeighbours(Node node) {
  		List<Node> neighbours = new List<Node>();

  		for (int x = -1; x <= 1; x++) {
  			for (int y = -1; y <= 1; y++) {
  				if (x == 0 && y == 0)
  					continue;
          if(x == 0 || y == 0){
    				int checkX = node.gridX + x;
    				int checkY = node.gridY + y;

    				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
    					neighbours.Add(grid[checkX,checkY]);
    				}
          }
  			}
  		}
  		return neighbours;
  	}


  	public Node NodeFromWorldPoint(Vector3 worldPosition) {
  		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
  		float percentY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
  		percentX = Mathf.Clamp01(percentX);
  		percentY = Mathf.Clamp01(percentY);

  		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
  		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
  		return grid[x,y];
  	}


  	void OnDrawGizmos() {
      combined_path = new List<Node>();
      if (path != null && path_two != null){
        combined_path = path.Concat(path_two).ToList();
      }
  		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,gridWorldSize.y,1));

  		if (grid != null) {
  			foreach (Node n in grid) {
  				Gizmos.color = (n.walkable)?Color.white:Color.red;
  				if (combined_path != null)
            combined_path.Reverse();

  					if (combined_path.Any(node => node.worldPosition == n.worldPosition)){
  						Gizmos.color = Color.black;
              Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
            }
          Gizmos.color = Color.yellow;
          Gizmos.DrawSphere(n.worldPosition, 0.1f);
  			}
  		}
  	}
}
