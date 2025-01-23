using Assets.Scripts.DataStructures;
using Assets.Scripts;
using System.Collections.Generic;

public class DepthSearchMind : AbstractPathMind
{
    class Node
    {
        public CellInfo info;
        public Node parent;

        public Node (CellInfo _info, Node _parent)
        {
            this.info = _info;
            this.parent = _parent;
        }
    }

    List<Node> VisitedNodes = new List<Node>();
    List<Node> DiscoveredNodes = new List<Node>();

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        CellInfo goal = goals[0];
        Node startingPoint = new Node(currentPos, null);

        CellInfo finishingPoint = DFS(boardInfo, startingPoint, goal);

        if (finishingPoint.RowId < startingPoint.info.RowId)
            return Locomotion.MoveDirection.Left;
        else if (finishingPoint.RowId > startingPoint.info.RowId)
            return Locomotion.MoveDirection.Right;
        else if (finishingPoint.ColumnId < startingPoint.info.ColumnId)
            return Locomotion.MoveDirection.Down;

        return Locomotion.MoveDirection.Up;
    }

    private CellInfo DFS(BoardInfo boardInfo, Node initNode, CellInfo goal)
    {
        Node nextNode = null;

        //Marco U como descubierto.
        VisitedNodes.Add(initNode);
        //Por cada vértce v adyacente a U.
        CellInfo[] neighbours = initNode.info.WalkableNeighbours(boardInfo);

        foreach (CellInfo neighbour in neighbours) 
        {
        
        }
        //if(v no fue visitado)
        
            // padre[v] = u;
            
            // DFS(g,v);

    }
}
