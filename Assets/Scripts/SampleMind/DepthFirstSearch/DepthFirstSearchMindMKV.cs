using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;
using System.Linq;

public class DepthFirstSearchMindMKV : AbstractPathMind
{
    class Node
    {
        public CellInfo info;
        public Node parent;
        public bool visited;
        public Node(CellInfo _info, Node _parent, bool _visited)
        {
            this.info = _info;
            this.parent = _parent;
            this.visited = _visited;
        }
    }

    bool foundGoal = false;
    List<Node> PathNodes = new List<Node>();
    int countNodes = 2;

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        CellInfo finishingPoint = null;
        foundGoal = false;

        if (!goals[0].Walkable ||!currentPos.Walkable)
        {
            UnityEngine.Debug.Log("Imposible llegar a meta");
            return Locomotion.MoveDirection.None;
        }

        if (PathNodes.Count == 0)
            finishingPoint = DFS(boardInfo, new Node(currentPos, null, false), goals[0]);
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

    private CellInfo DFS(BoardInfo boardInfo, Node currentNode, CellInfo goal)
    {
        PathNodes.Add(currentNode);
        currentNode.visited = true;

        CellInfo[] neighbours = currentNode.info.WalkableNeighbours(boardInfo);

        foreach (CellInfo neighbor in neighbours)
        {
            if(neighbor != null && (!PathNodes.Any(node => node.visited)))
            {
                DFS(boardInfo, new Node(neighbor, currentNode, false), goal);
                PathNodes.Add(currentNode);
            }
        }
                
           return PathNodes[1].info;
    }
}
