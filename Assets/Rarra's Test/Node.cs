using Assets.Scripts.DataStructures;

public class Node
{
    public int g;
    public int h;
    public int f;
    public CellInfo cell;
    public Node parent;
    public Node(int _g, int _h, CellInfo _cell, Node _parent)
    {
        this.g = _g;
        this.h = _h;
        this.f = this.g + this.h;
        this.cell = _cell;
        this.parent = _parent;
    }
}