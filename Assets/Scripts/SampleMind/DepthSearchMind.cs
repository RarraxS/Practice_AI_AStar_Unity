using Assets.Scripts.DataStructures;
using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;

public class DepthSearchMind : AbstractPathMind
{

    // En la primera iteración, "saca" el camino, y en la segunda y las venideras, lo sigue.
    class Node
    {
        public CellInfo info;
        public Node parent;

        public Node(CellInfo _info, Node _parent)
        {
            this.info = _info;
            this.parent = _parent;
        }
    }

    bool foundGoal = false;
    List<Node> VisitedNodes = new List<Node>();
    int countNodes = 1;

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {

        CellInfo goal = goals[0];
        Node startingPoint = new Node(currentPos, null);
        CellInfo finishingPoint = null;
        foundGoal = false;

        if (!goal.Walkable)
            return Locomotion.MoveDirection.None;
        if (VisitedNodes.Count == 0)
        {
            finishingPoint = DFS(boardInfo, startingPoint, goal);
        }
        else
            finishingPoint = VisitedNodes[countNodes].info;
        countNodes++;


        if (finishingPoint.RowId < startingPoint.info.RowId)
            return Locomotion.MoveDirection.Down;
        else if (finishingPoint.RowId > startingPoint.info.RowId)
            return Locomotion.MoveDirection.Up;
        else if (finishingPoint.ColumnId < startingPoint.info.ColumnId)
            return Locomotion.MoveDirection.Left;

        return Locomotion.MoveDirection.Right;
    }

    private CellInfo DFS(BoardInfo boardInfo, Node initNode, CellInfo goal)
    {
        VisitedNodes.Add(initNode);

        CellInfo[] neighbours = initNode.info.WalkableNeighbours(boardInfo);

        foreach (CellInfo neighbor in neighbours)
        {
            if (neighbor == goal)
            {
                Node goalN = new Node(neighbor, initNode);
                VisitedNodes.Add(goalN);
                foundGoal = true;
                break;
            }
            else if (foundGoal) break;
            else if (neighbor != null && !VisitedNodes.Any(node => node.info.CellId == neighbor.CellId))
            {
                Node nextNode = new Node(neighbor, initNode);
                DFS(boardInfo, nextNode, goal);
            }
        }
        if (VisitedNodes.Count > 2)
            return VisitedNodes[1].info;
        else return null;
    }
}