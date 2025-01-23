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
    List<Node> PathNodes = new List<Node>();

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

    private CellInfo DFS(BoardInfo boardInfo, Node cell, CellInfo goal)
    {
        Node nextNode = null;

        //Marco U como descubierto.
        DiscoveredNodes.Add(cell);

        //Por cada vértce v adyacente a U.
        CellInfo[] neighbours = cell.info.WalkableNeighbours(boardInfo);

        //if(v no fue visitado)
        for (int i = 0;i < neighbours.Length; i++)
        {
            // padre[v] = u;
            nextNode = new Node(neighbours[i],cell);


            if (nextNode.info != null) 
            // DFS(g,v);
                DFS(boardInfo, nextNode, goal);
        }

        return nextNode.info;
    }
}
