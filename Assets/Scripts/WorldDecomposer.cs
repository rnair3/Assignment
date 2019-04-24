using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldDecomposer : MonoBehaviour
{

    public LayerMask unwalkableMask;            
    public Vector2 worldSize;
    public float nodeSize;
    Node[,] world;

    float nodeDiameter;
    int rows, cols;

    public List<Node> path;

    void Awake()
    {
        nodeDiameter = nodeSize * 2;
        rows = Mathf.RoundToInt(worldSize.x / nodeDiameter);
        cols = Mathf.RoundToInt(worldSize.y / nodeDiameter);
        DecomposeWorld();
    }

    //dividing the world into nodes
    void DecomposeWorld()
    {
        world = new Node[rows, cols];
        Vector3 worldBottomLeft = transform.position - Vector3.right * worldSize.x / 2 - Vector3.forward * worldSize.y / 2;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (row * nodeDiameter + nodeSize) + Vector3.forward * (col * nodeDiameter + nodeSize);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeSize/2, unwalkableMask));
                world[row, col] = new Node(walkable, worldPoint, row, col);
            }
        }
    }

    //finding the neighbouring nodes
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.x + x;
                int checkY = node.y + y;

                if (checkX >= 0 && checkX < rows && checkY >= 0 && checkY < cols)
                {
                    neighbours.Add(world[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    //conevrting vector3 to nodes
    public Node CreateNode(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + worldSize.x / 2) / worldSize.x;
        float percentY = (worldPosition.z + worldSize.y / 2) / worldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((rows - 1) * percentX);
        int y = Mathf.RoundToInt((cols - 1) * percentY);
        return world[x, y];
    }



    // used to draw the path found using A* after pausing the scene
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, 1, worldSize.y));

        if (world != null)
        {
            foreach (Node n in world)
            {
                Gizmos.color = (n.walk) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}