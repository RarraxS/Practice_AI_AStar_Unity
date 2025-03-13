using Assets.Scripts.DataStructures;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HillClimbingFinished : AbstractPathMind
{
    private static int Heuristic(CellInfo a, CellInfo b)
    {
        return Mathf.Abs(a.ColumnId - b.ColumnId) + Mathf.Abs(a.RowId - b.RowId);
    }

    Sequencer sequencer = new Sequencer();
    bool hasLookedForAGoal = false;
    int count = 0;

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        Debug.Log($"(RAE) Current Position: {currentPos}");
        if (goals == null || goals.Length == 0 || (sequencer.List.Count > 0 && sequencer.List[sequencer.List.Count - 1].CellId != boardInfo.Exit.CellId))
        {
            Debug.Log("(RAE) No goals available. Returning None.");
            return Locomotion.MoveDirection.None;
        }

        // Obtener posiciones de los enemigos
        List<CellInfo> enemyPositions = boardInfo.Enemies.Select(enemy => enemy.CurrentPosition()).ToList();
        Debug.Log($"(RAE) Enemy Positions: {string.Join(", ", enemyPositions)}");

        CellInfo goal = null;
        CellInfo bestMove = null;

        if (enemyPositions.Count > 0)
        {
            goal = enemyPositions.OrderBy(enemyPos => Heuristic(currentPos, enemyPos)).First();
            bestMove = HillClimbing(currentPos, goal, boardInfo);
        }
        else
        {
            if (!hasLookedForAGoal)
            {
                goal = boardInfo.Exit;
                sequencer.List = BreadthFirstSearch(currentPos, boardInfo, goal);
                hasLookedForAGoal = true;
            }
            bestMove = sequencer.List[count];
            count++;
        }

        Debug.Log($"(RAE) Selected Goal: {goal}");



        Locomotion.MoveDirection direction = GetMoveDirection(currentPos, bestMove);
        Debug.Log($"(RAE) Moving {direction} from {currentPos} to {bestMove}");
        return direction;
    }

    private CellInfo HillClimbing(CellInfo current, CellInfo end, BoardInfo info)
    {
        CellInfo bestMove = current;
        int bestScore = Heuristic(current, end);
        Debug.Log($"(RAE) Current Position Heuristic Score: {bestScore}");

        // Obtener vecinos
        var neighbors = current.WalkableNeighbours(info);
        Debug.Log($"(RAE) Neighbors Count: {neighbors.Count()}");

        // Explorar vecinos y seleccionar el mejor basado en la heurística
        foreach (CellInfo neighbor in neighbors)
        {
            if (neighbor == null)
            {
                Debug.Log("(RAE) Skipping null neighbor");
                continue; // Evitar valores nulos
            }

            int score = Heuristic(neighbor, end);
            Debug.Log($"(RAE) Evaluating Neighbor {neighbor} with Score {score}");

            if (score < bestScore) // Si el vecino está más cerca del objetivo, actualizar
            {
                bestScore = score;
                bestMove = neighbor;
                Debug.Log($"(RAE) New Best Move: {bestMove} with Score {bestScore}");
            }
        }

        return bestMove;
    }


    private Locomotion.MoveDirection GetMoveDirection(CellInfo start, CellInfo next)
    {
        UnityEngine.Debug.Log(next.CellId);
        if (next.ColumnId < start.ColumnId) return Locomotion.MoveDirection.Left;
        if (next.ColumnId > start.ColumnId) return Locomotion.MoveDirection.Right;
        if (next.RowId < start.RowId) return Locomotion.MoveDirection.Down;
        if (next.RowId > start.RowId) return Locomotion.MoveDirection.Up;
        return Locomotion.MoveDirection.None;
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
            sequencer.visitedNodes++;
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
