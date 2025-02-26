using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public class BreadhtFirstSearchMind : AbstractPathMind
{
    List<CellInfo> Path = new List<CellInfo>();
    int countNodes = 0;

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        if (countNodes == 0)
        {
            Path = BreadthFirstSearch(currentPos, boardInfo, boardInfo.Exit);
        }

        if(!goals[0].Walkable || !currentPos.Walkable || Path[Path.Count-1].CellId != boardInfo.Exit.CellId)
        {
            if(countNodes == 0)
                UnityEngine.Debug.Log("No hay camino disponible o meta no accesible.");

            countNodes = 1;

            return Locomotion.MoveDirection.None;
        }

        CellInfo finishingPoint = Path[countNodes];
        countNodes++;

        if (finishingPoint.RowId < currentPos.RowId) return Locomotion.MoveDirection.Down;
        else if (finishingPoint.RowId > currentPos.RowId) return Locomotion.MoveDirection.Up;
        else if (finishingPoint.ColumnId < currentPos.ColumnId) return Locomotion.MoveDirection.Left;

        return Locomotion.MoveDirection.Right;
    }

    public List<CellInfo> BreadthFirstSearch(CellInfo start, BoardInfo board, CellInfo goal)
    {
        Node priorNode = null;
        List<CellInfo> visited = new List<CellInfo>();
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(new Node(start, null));

        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();
            priorNode = node;

            // Verificar si la celda ya fue visitada
            if (!visited.Any(example => example.CellId == node.cell.CellId))
            {
                visited.Add(node.cell);

                if (node.cell.CellId == goal.CellId)
                {
                    priorNode = node;
                    break;
                }

                // Encolar los vecinos que son válidos
                foreach (var neighbor in node.cell.WalkableNeighbours(board))
                {
                    if (neighbor != null && !visited.Any(n => n.CellId == neighbor.CellId))
                    {
                        queue.Enqueue(new Node(neighbor, node));
                    }
                }
            }
        }

        return Reconstruct(priorNode);
    }

    public List<CellInfo> Reconstruct(Node current)
    {
        List<CellInfo> path = new List<CellInfo>();

        while (current.parent != null)
        {
            path.Add(current.cell);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }
}

