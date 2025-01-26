using Assets.Scripts.DataStructures;
using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarMind : AbstractPathMind
{
    class Node
    {
        public CellInfo Info;
        public Node Parent;
        public int GCost; // Cost from start to this node
        public int HCost; // Heuristic cost (distance to goal)
        public int FCost => GCost + HCost; // Total cost

        public Node(CellInfo info, Node parent, int gCost, int hCost)
        {
            Info = info;
            Parent = parent;
            GCost = gCost;
            HCost = hCost;
        }
    }

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        if (goals == null || goals.Length == 0) return Locomotion.MoveDirection.None;

        // Select the closest valid goal
        CellInfo goal = goals.FirstOrDefault(g => g.Walkable);
        if (goal == null) return Locomotion.MoveDirection.None;

        // Find path using A* algorithm
        List<CellInfo> path = FindPath(boardInfo, currentPos, goal);
        if (path == null || path.Count < 2) return Locomotion.MoveDirection.None;

        // Determine direction from current position to the next step
        CellInfo nextStep = path[1];
        if (nextStep.RowId < currentPos.RowId) return Locomotion.MoveDirection.Down;
        if (nextStep.RowId > currentPos.RowId) return Locomotion.MoveDirection.Up;
        if (nextStep.ColumnId < currentPos.ColumnId) return Locomotion.MoveDirection.Left;

        return Locomotion.MoveDirection.Right;
    }

    private List<CellInfo> FindPath(BoardInfo boardInfo, CellInfo start, CellInfo goal)
    {
        List<Node> openList = new List<Node>();
        HashSet<CellInfo> closedList = new HashSet<CellInfo>();

        Node startNode = new Node(start, null, 0, CalculateHCost(start, goal));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // Get the node with the lowest F cost
            Node currentNode = openList.OrderBy(node => node.FCost).ThenBy(node => node.HCost).First();
            openList.Remove(currentNode);
            closedList.Add(currentNode.Info);

            // Check if we've reached the goal
            if (currentNode.Info == goal)
                return ReconstructPath(currentNode);

            // Process neighbors
            foreach (CellInfo neighbor in currentNode.Info.WalkableNeighbours(boardInfo))
            {
                if (closedList.Contains(neighbor)) continue;

                int tentativeGCost = currentNode.GCost + 1; // Assuming uniform cost for moving to adjacent tiles
                Node neighborNode = openList.FirstOrDefault(node => node.Info == neighbor);

                if (neighborNode == null)
                {
                    // Add new node to open list
                    neighborNode = new Node(neighbor, currentNode, tentativeGCost, CalculateHCost(neighbor, goal));
                    openList.Add(neighborNode);
                }
                else if (tentativeGCost < neighborNode.GCost)
                {
                    // Update G cost and parent for an existing node
                    neighborNode.GCost = tentativeGCost;
                    neighborNode.Parent = currentNode;
                }
            }
        }

        // No path found
        return null;
    }

    private List<CellInfo> ReconstructPath(Node endNode)
    {
        List<CellInfo> path = new List<CellInfo>();
        Node currentNode = endNode;

        while (currentNode != null)
        {
            path.Add(currentNode.Info);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

    private int CalculateHCost(CellInfo currentCell, CellInfo goal)
    {
        // Manhattan distance heuristic
        return Mathf.Abs(currentCell.RowId - goal.RowId) + Mathf.Abs(currentCell.ColumnId - goal.ColumnId);
    }
}
