using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Linq;

public class DepthFirstSearchMindMKV : AbstractPathMind
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
    List<Node> PathNodes = new List<Node>();
    int countNodes = 1;

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        DFS(boardInfo, new Node(currentPos, null), goals[0]);
        CellInfo finishingPoint = new CellInfo(1, 0);
        foundGoal = false;

        if (!goals[0].Walkable || !currentPos.Walkable || PathNodes.Count == 1)
        {
            UnityEngine.Debug.Log("Meta en obstáculo");
            return Locomotion.MoveDirection.None;
        }
        else
        {
            finishingPoint = PathNodes[countNodes].info;
            countNodes++;
        }

        if (finishingPoint.RowId < currentPos.RowId) return Locomotion.MoveDirection.Down;
        else if (finishingPoint.RowId > currentPos.RowId) return Locomotion.MoveDirection.Up;
        else if (finishingPoint.ColumnId < currentPos.ColumnId) return Locomotion.MoveDirection.Left;

        return Locomotion.MoveDirection.Right;
    }

    private void DFS(BoardInfo boardInfo, Node currentNode, CellInfo goal)
    {
        PathNodes.Add(currentNode);

        int prohibitedAdjacentNodes = 0;

        CellInfo[] neighbours = currentNode.info.WalkableNeighbours(boardInfo);

        foreach (CellInfo neighbor in neighbours)
        {
            if (neighbor == goal)
            {
                PathNodes.Add(new Node(neighbor, currentNode));
                foundGoal = true;
                break;
            }
        }
        foreach (CellInfo neighbor in neighbours)
        {
            if (foundGoal) break;

            if ((neighbor != null) && (!PathNodes.Any(node => node.info.CellId == neighbor.CellId)))
                DFS(boardInfo, new Node(neighbor, currentNode), goal);

            else prohibitedAdjacentNodes++;

            if (prohibitedAdjacentNodes == 4 && currentNode.parent != null)
                DFS(boardInfo, new Node(currentNode.parent.info, currentNode.parent.parent), goal);
        }
    }
}
