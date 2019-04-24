using UnityEngine;
using System.Collections;

public class Node
{

    public bool walk;
    public Vector3 worldPosition;
    public int x;
    public int y;

    public int gn;
    public int hn;
    public Node parent;

    public Node(bool _walkable, Vector3 _worldPos, int _x, int _y)
    {
        walk = _walkable;
        worldPosition = _worldPos;
        x = _x;
        y = _y;
    }

    public int fn
    {
        get
        {
            return gn + hn;
        }
    }
}