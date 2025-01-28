using Assets.Scripts.DataStructures;
using Assets.Scripts;
using System.Collections.Generic;
using System;
using System.Linq;

public class AStarMind : AbstractPathMind
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
            this.f = this.g+this.h;
            this.cell = _cell;
            this.parent = _parent;
        }
    }

    List<Node> closedList = new List<Node>();
    int count = 0;
    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        if(count == 0)
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
            CellInfo nextPosition = closedList[count].cell;
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
        Node actualNode = initNode;
        while(actualNode.cell.CellId != goal.CellId && openList.Count != 0)
        {
            actualNode = openList.OrderBy(n => n.f).First();

            openList.Remove(actualNode);
            closedList.Add(actualNode);
            CellInfo[] neighbours = actualNode.cell.WalkableNeighbours(board);
            openList.Clear();
            for(int count=0; count<neighbours.Length; count++)
            {
                Node adjacentNode = null;

                if(neighbours[count] != null)
                {

                
                    adjacentNode = new Node(Distance(neighbours[count], initNode.cell), Heuristic(neighbours[count], goal), neighbours[count], actualNode);

                    if (closedList.Any(node => node.cell.CellId == adjacentNode.cell.CellId))
                    {
                        continue;
                    }
                    if (!openList.Contains(adjacentNode))
                    {
                        openList.Add(adjacentNode);
                    }
                    if (adjacentNode.cell.CellId == goal.CellId)
                    {
                        closedList.Add(adjacentNode);
                        break;
                    }
                }
            }
        }
    }

    public int Heuristic(CellInfo current,CellInfo goal)
    {
        return (int)(MathF.Abs(goal.RowId - current.RowId) + MathF.Abs(goal.ColumnId - current.ColumnId));
    }
    public int Distance(CellInfo current,CellInfo start)
    {
        return (int)(MathF.Abs(start.RowId - current.RowId) + MathF.Abs(start.ColumnId - current.ColumnId));
    }
}
