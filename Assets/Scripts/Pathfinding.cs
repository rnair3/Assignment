using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    private const int VH_COST = 10;
    private const int DIAGONAL_COST = 14;

    public Transform leader;
    WorldDecomposer w;
    RaycastHit hit;
    float turnSpeed;
    [SerializeField] Rigidbody rb;
    bool move;

    void Awake()
    {
        w = GetComponent<WorldDecomposer>();
        turnSpeed = 2f;
        move = false;
    }

    void Update()
    {
        List<Node> path = null ;
        if (Input.GetMouseButton(0))
        {
            //To set up the variable that will give the position of the mouse through the camera
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                move = true;
            }
        }
        if (move)
        {
            path = GeneratePath(leader.position, hit.point);
            if (path != null)
            {
                Vector3 previous = leader.position;
                Vector3 towards;

                foreach (Node n in path)
                {
                    //Calculate the distance between the node and the leader
                    towards = n.worldPosition - leader.position;
                    
                    //rotating the leader
                    leader.rotation = Quaternion.Lerp(leader.rotation, Quaternion.LookRotation(towards), turnSpeed * Time.deltaTime);
                    //leader.position = n.worldPosition;
                    if(Vector3.Distance(n.worldPosition, leader.position) > 2f)
                    {
                        towards.Normalize();
                        towards *= 5f;
                        rb.velocity = towards;
                    }
                    else
                    {
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        move = false;
                    }

                }
                
            }
        }
        else
        {
            //rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;
        }
        
                
    }

    //A* algorithm
    private List<Node> GeneratePath(Vector3 startPos, Vector3 targetPos)
    {
        //to convert the vector into node
        Node startNode = w.CreateNode(startPos);
        Node goalNode = w.CreateNode(targetPos);

        //openlist -- nodes where our leader can move to 
        List<Node> openList = new List<Node>();

        //closedlist -- nodes already visited
        HashSet<Node> closedList = new HashSet<Node>();
        openList.Add(startNode);

        //while all the nodes in openlist are not visited
        while (openList.Count > 0)
        {
            Node current = openList[0];

            //finding the minimum f(n) node
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fn < current.fn || openList[i].fn == current.fn)
                {
                    if (openList[i].hn < current.hn)
                        current = openList[i];
                }
            }

            
            //if goal state reached retrace the path
            if (current == goalNode)
            {
                List<Node> path = new List<Node>();
                Node currentNode = goalNode;

                while (currentNode != startNode)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.parent;
                }
                path.Reverse();

                w.path = path;
                return path;
            }

            //looping throught all the neighboring nodes to find the best possible move
            foreach (Node neighbour in w.GetNeighbours(current))
            {
                //if there is no obstacle and the node has been visited
                if (!neighbour.walk || closedList.Contains(neighbour))
                {
                    continue;
                }

                int cost = current.gn + CalcGandH(current, neighbour);
                //finding the least f(n) value
                if (cost < neighbour.gn || !openList.Contains(neighbour))
                {
                    neighbour.gn = cost;
                    neighbour.hn = CalcGandH(neighbour, goalNode);
                    neighbour.parent = current;

                    if (!openList.Contains(neighbour))
                        openList.Add(neighbour);
                }
            }
            //removing the node from openlist to closedlist
            openList.Remove(current);
            closedList.Add(current);
        }
        return null;
    }

    //method to calculate the cost to move to a node
    int CalcGandH(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.x - nodeB.x);
        int dstY = Mathf.Abs(nodeA.y - nodeB.y);

        if (dstX > dstY)
            return DIAGONAL_COST * dstY + VH_COST * (dstX - dstY);
        return DIAGONAL_COST * dstX + VH_COST * (dstY - dstX);
    }
}