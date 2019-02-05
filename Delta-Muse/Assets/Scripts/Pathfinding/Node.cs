using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool isTraversable;
    public Vector2 m_world;

    public int gridX, gridY;

    public int gCost, hCost;

    public Node Parent;

    ///Init
    public Node(bool _Traversable, Vector2 _WorldPos, int _GridX, int _GridY)
    {
        isTraversable = _Traversable;
        m_world = _WorldPos;
        gridX = _GridX;
        gridY = _GridY;
    }


    public int fCost
    {

        get
        { return gCost + hCost; }
    }
    public int heapIndex
    {
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int Compare = fCost.CompareTo(nodeToCompare.fCost);
        if (Compare == 0)
        {
            Compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -Compare;
    }

}