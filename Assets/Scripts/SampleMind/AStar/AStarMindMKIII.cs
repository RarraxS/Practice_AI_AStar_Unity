    using Assets.Scripts.DataStructures;
    using Assets.Scripts;
    using System.Collections.Generic;
    using System;
    using System.Linq;

    public class AStarMindMKIII : AbstractPathMind
    {
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

        List<Node> pathList = new List<Node>();
        int count = 0;
        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
            if (count == 0)
            {
                AStar(boardInfo, new Node(0, Heuristic(currentPos, goals[0]), currentPos, null), goals[0]);
                count++;
            }


            if (!goals[0].Walkable || !currentPos.Walkable)
            {
                UnityEngine.Debug.Log("Meta imposible de alcanzar");
                return Locomotion.MoveDirection.None;
            }
            else
            {
                CellInfo nextPosition = pathList[count].cell;
                count++;
                UnityEngine.Debug.Log(nextPosition.CellId);
                if (nextPosition.RowId < currentPos.RowId) return Locomotion.MoveDirection.Down;
                else if (nextPosition.RowId > currentPos.RowId) return Locomotion.MoveDirection.Up;
                else if (nextPosition.ColumnId < currentPos.ColumnId) return Locomotion.MoveDirection.Left;
            }

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

            // Check if goal is reached
            if (actualNode.cell.CellId == goal.CellId)
            {
                pathList = ReconstructPath(actualNode); // Reconstruct path
                UnityEngine.Debug.Log("Path found!");
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

        UnityEngine.Debug.Log("Goal is unreachable.");
        pathList.Clear(); // Clear path if no goal is found
    }

    public int Heuristic(CellInfo current, CellInfo goal)
        { return (int)(MathF.Abs(goal.RowId - current.RowId) + MathF.Abs(goal.ColumnId - current.ColumnId)); }
        public int Distance(CellInfo current, CellInfo start)
        { return (int)(MathF.Abs(start.RowId - current.RowId) + MathF.Abs(start.ColumnId - current.ColumnId)); }

        public List<Node> ReconstructPath(Node current)
        {
            var path = new List<Node>();
            while (current != null)
            {
                path.Add(current);
                current = current.parent;
            }
            path.Reverse();
            return path;
        }
    }
