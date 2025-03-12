    using Assets.Scripts.DataStructures;
    using Assets.Scripts;
    using System.Collections.Generic;
    using System;
    using System.Linq;
using System.IO;

    public class AStarMindMKIII : AbstractPathMind
    {
        Sequencer sequencer = new Sequencer();
        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
        if (sequencer.count == 0)
        {
            AStar(boardInfo, new Node(0, Heuristic(currentPos, boardInfo.Exit), currentPos, null), boardInfo.Exit);
            UnityEngine.Debug.Log("(RAE) Visited nodes: " + sequencer.visitedNodes);
        }

        if (!boardInfo.Exit.Walkable || !currentPos.Walkable || sequencer.List[sequencer.List.Count - 1].CellId != boardInfo.Exit.CellId)
        {
            if (sequencer.count == 0)
            {
                UnityEngine.Debug.Log("(RAE) No hay camino disponible o meta no accesible.");
                sequencer.count = 1;
            }
            return Locomotion.MoveDirection.None;
        }

        CellInfo finishingPoint = sequencer.List[sequencer.count];
        sequencer.count++;

        if (finishingPoint.RowId < currentPos.RowId) return Locomotion.MoveDirection.Down;
        else if (finishingPoint.RowId > currentPos.RowId) return Locomotion.MoveDirection.Up;
        else if (finishingPoint.ColumnId < currentPos.ColumnId) return Locomotion.MoveDirection.Left;

        return Locomotion.MoveDirection.Right;
    }

    public void AStar(BoardInfo board, Node initNode, CellInfo goal)
    {
        List<Node> openList = new List<Node> { initNode };
        HashSet<string> closedSet = new HashSet<string>();
        Node actualNode = null;

        while (openList.Count > 0)
        {
            // Get node with the smallest F value
            actualNode = openList.OrderBy(n => n.f).First();
            openList.Remove(actualNode);
            closedSet.Add(actualNode.cell.CellId);
            sequencer.visitedNodes++;

            // Check if goal is reached
            if (actualNode.cell.CellId == goal.CellId)
            {
                sequencer.List = ReconstructPath(actualNode); // Reconstruct path
                return;
            }

            // Process neighbors
            foreach (var neighbor in actualNode.cell.WalkableNeighbours(board))
            {
                if (neighbor == null || closedSet.Contains(neighbor.CellId)) continue;

                Node adjacentNode = new Node( actualNode.g + Distance(neighbor, actualNode.cell), Heuristic(neighbor, goal), neighbor, actualNode );

                // If a better path is already in openList, skip
                if (openList.Any(n => n.cell.CellId == neighbor.CellId && n.f <= adjacentNode.f))
                    continue;

                openList.Add(adjacentNode);
            }
        }

        sequencer.List.Clear(); // Clear path if no goal is found
    }

    public int Heuristic(CellInfo current, CellInfo goal)
        { return (int)(MathF.Abs(goal.RowId - current.RowId) + MathF.Abs(goal.ColumnId - current.ColumnId)); }
        public int Distance(CellInfo current, CellInfo start)
        { return (int)(MathF.Abs(start.RowId - current.RowId) + MathF.Abs(start.ColumnId - current.ColumnId)); }

        public List<CellInfo> ReconstructPath(Node current)
        {
            var path = new List<CellInfo>();
            while (current.parent != null)
            {
                path.Add(current.cell);
                current = current.parent;
            }
            path.Reverse();
            return path;
        }
    }
