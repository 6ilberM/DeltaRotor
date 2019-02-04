using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isTraversable;
    public Vector2 WorldPos;

    public int gridX, gridY;

    public int gCost;
    public int hCost;

    public Node Parent;
    
}