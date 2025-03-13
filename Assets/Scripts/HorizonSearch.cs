using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Assets.Scripts.DataStructures;
using System.Linq;
using Assets.Scripts;

public class HorizonSearch : AbstractPathMind
{
    [SerializeField]
    int searchDepth = 5;  // Maximum search depth for Horizon Search
    int numOfMoves = 0;
    List<CellInfo> movements = null;
    Sequencer sequencer = new Sequencer();
    bool hasLookedForAGoal = false;
    int exploredNodes = 0;
    bool hasShownExploredNodes= false;

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        List<CellInfo> enemyPositions = boardInfo.Enemies.Select(enemy => enemy.CurrentPosition()).ToList();
        Debug.Log($"(RAE) Enemy Positions: {string.Join(", ", enemyPositions)}");
        CellInfo goal = null;

        if(enemyPositions.Count == 1 && !hasShownExploredNodes)
        {
            Debug.Log($"(RAE) Explored nodes until enemy 1: {exploredNodes}");
            hasShownExploredNodes = true;
        }

        if(!boardInfo.Exit.Walkable || !currentPos.Walkable)
        {
            Debug.Log($"(RAE) Invalid Path.");
            return Locomotion.MoveDirection.None;
        }
        else if (enemyPositions.Count > 0)
        {
            goal = enemyPositions.OrderBy(enemyPos => Heuristic(currentPos, enemyPos)).First();
            Debug.Log($"(RAE) Selected Goal: {goal.CellId}");
            if (numOfMoves == 0)
                movements = HorizonS(currentPos, goal, boardInfo, searchDepth);

            // Move towards the goal using the pre-calculated path
            if (numOfMoves < movements.Count)
            {
                numOfMoves++;
                return GetMoveDirection(currentPos, movements[numOfMoves - 1]);
            }
            else
            {
                numOfMoves = 0;
                return GetMoveDirection(currentPos, movements.Last());
            }
        }
        else
        {
            // If no enemies, find the exit
            if (!hasLookedForAGoal)
            {
                goal = boardInfo.Exit;
                Debug.Log($"(RAE) Selected Goal: {goal.CellId}");
                sequencer.List = BreadthFirstSearch(currentPos, boardInfo, goal);
                hasLookedForAGoal = true;
                numOfMoves = 0;
            }

            numOfMoves++;
            return GetMoveDirection(currentPos, sequencer.List[numOfMoves - 1]);
        }
    }

    private static int Heuristic(CellInfo a, CellInfo b)
    {
        // Manhattan distance heuristic
        return Mathf.Abs(a.ColumnId - b.ColumnId) + Mathf.Abs(a.RowId - b.RowId);
    }

    List<CellInfo> HorizonS(CellInfo start, CellInfo end, BoardInfo info, int horizon)
    {
        Queue<Node> frontier = new Queue<Node>();
        HashSet<CellInfo> explored = new HashSet<CellInfo>();  // Use HashSet for faster lookups

        frontier.Enqueue(new Node(start, null));

        int depth = 0;
        Node actualState = null;

        while (frontier.Count > 0)
        {
            exploredNodes++;

            actualState = frontier.OrderBy(node => Heuristic(node.cell, end)).FirstOrDefault();

            if (actualState == null)
                break;

            // If goal is found or depth limit is reached, stop
            if (actualState.cell.CellId == end.CellId || depth >= horizon)
            {
                UnityEngine.Debug.Log(depth >= horizon ? "Cota de profundidad alcanzada" : "Estado encontrado");
                break;
            }

            explored.Add(actualState.cell);

            // Explore neighbors
            foreach (var neighbor in actualState.cell.WalkableNeighbours(info))
            {
                if (neighbor != null && !explored.Contains(neighbor))
                {
                    frontier.Enqueue(new Node(neighbor, actualState));
                }
            }

            depth++;
        }

        return ReconstructPath(actualState);
    }

    public List<CellInfo> BreadthFirstSearch(CellInfo start, BoardInfo board, CellInfo goal)
    {
        HashSet<CellInfo> visited = new HashSet<CellInfo>();  // Use HashSet for faster lookups
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(new Node(start, null));

        Node goalNode = null;

        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();

            if (visited.Contains(node.cell))
                continue;

            visited.Add(node.cell);

            if (node.cell.CellId == goal.CellId)
            {
                goalNode = node;
                break;
            }

            foreach (var neighbor in node.cell.WalkableNeighbours(board))
            {
                if (neighbor != null && !visited.Contains(neighbor))
                {
                    queue.Enqueue(new Node(neighbor, node));
                }
            }
        }

        return goalNode != null ? ReconstructPath(goalNode) : new List<CellInfo>();
    }

    // Reconstruct path from goal to start
    public List<CellInfo> ReconstructPath(Node current)
    {
        var path = new List<CellInfo>();
        while (current != null)
        {
            path.Add(current.cell);
            current = current.parent;
        }
        path.Reverse();
        return path;
    }

    private Locomotion.MoveDirection GetMoveDirection(CellInfo start, CellInfo next)
    {
        Debug.Log($"Moving from {start.CellId} to {next.CellId}");

        UnityEngine.Debug.Log(next.CellId);
        if (next.ColumnId < start.ColumnId) return Locomotion.MoveDirection.Left;
        if (next.ColumnId > start.ColumnId) return Locomotion.MoveDirection.Right;
        if (next.RowId < start.RowId) return Locomotion.MoveDirection.Down;
        if (next.RowId > start.RowId) return Locomotion.MoveDirection.Up;
        return Locomotion.MoveDirection.None;
    }
}
