using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class AmplitudeSearchMind : AbstractPathMind
{
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
    int countNodes = 2;

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        Node startingPoint = new Node(currentPos, null);
        CellInfo finishingPoint = null;
        foundGoal = false;

        if (!goals[0].Walkable)
            return Locomotion.MoveDirection.None;

        if (VisitedNodes.Count == 0)
            finishingPoint = AS(boardInfo, startingPoint, goals[0]);
        else
        {
            finishingPoint = VisitedNodes[countNodes].info;
            countNodes++;
        }

        if (finishingPoint.RowId < startingPoint.info.RowId) return Locomotion.MoveDirection.Down;
        else if (finishingPoint.RowId > startingPoint.info.RowId) return Locomotion.MoveDirection.Up;
        else if (finishingPoint.ColumnId < startingPoint.info.ColumnId) return Locomotion.MoveDirection.Left;

        return Locomotion.MoveDirection.Right;
    }

    private CellInfo AS(BoardInfo boardInfo, Node initNode, CellInfo goal)
    {
        return initNode.info;
    }
}
