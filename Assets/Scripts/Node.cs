using Assets.Scripts.DataStructures;

public class Node
{
    public float g;
    public float h;
    public float f;
    public CellInfo cell;
    public Node parent;
    public Node(float _g, float _h, CellInfo _cell, Node _parent)
    {
        this.g = _g;
        this.h = _h;
        this.f = this.g + this.h;
        this.cell = _cell;
        this.parent = _parent;
    }

    public Node(CellInfo _cell, Node _parent)
    {
        this.cell = _cell;
        this.parent = _parent;
    }
}