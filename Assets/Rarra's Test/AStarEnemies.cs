using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;

public class AStarEnemies : AbstractPathMind
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private static int Heuristic(CellInfo a, CellInfo b)
    {
        return Heuristic(a.ColumnId, a.RowId, b.ColumnId, b.RowId);
    }

    private static int Heuristic(int x1, int y1, int x2, int y2)
    {
        return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
    }

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        if (goals == null || goals.Length == 0) { return Locomotion.MoveDirection.None; }

        // itera el boardInfo Enemies y mapealos a CurrentPosition


        List<CellInfo> enemyPositions = boardInfo.Enemies.Select(enemy => enemy.CurrentPosition()).ToList();
        CellInfo goal = null;

        if (enemyPositions.Count > 0)
        {
            // seleccionando el enemigo mas cercano
            goal = enemyPositions.OrderBy(enemyPos => Heuristic(currentPos, enemyPos)).First();
        }
        else
        {
            // selecciono el objetivo mas cercano
            goal = goals.OrderBy(itemGoal => Heuristic(currentPos, itemGoal)).First();
        }

        // Setup del A*
        var openNodes = new List<Node>();
        var closedNodes = new HashSet<CellInfo>();
        var startNode = new Node(currentPos, null);

        openNodes.Add(startNode);

        // mientras haya algo que visitar
        while (openNodes.Count > 0)
        {
            // ordeno los open nodes por F
            openNodes.Sort((a, b) => a.f.CompareTo(b.f)); // Ordena el openNodes por F de menor a mayor
            Node currentNode = openNodes[0];
            openNodes.RemoveAt(0);

            // agregar a la lista de nodos visitados
            closedNodes.Add(currentNode.cell);

            // he llegado al objetivo
            if (currentNode.cell == goal)
            {
                while (currentNode.parent != null && currentNode.parent.parent != null)
                {
                    currentNode = currentNode.parent;
                }

                return GetMoveDirection(currentPos, currentNode.cell);
            }

            // sino estoy en el objetivo aun, tengo que calcular a traves de los vecinos
            foreach (CellInfo neighbor in currentNode.cell.WalkableNeighbours(boardInfo))
            {
                // saltarme la iteracion de bucle, 
                // si el vecino es nulo (se sale del tablero) o si ya he visitado ese vecino previamente...
                if (neighbor == null || closedNodes.Contains(neighbor)) continue;

                float gCost = currentNode.g + neighbor.WalkCost;
                float hCost = Heuristic(neighbor, goal);

                Node neighborNode = new Node(gCost, hCost, neighbor, currentNode);

                if (openNodes.Find(n => n.cell == neighbor && n.g <= gCost) != null) continue;

                openNodes.Add(neighborNode);
            }
        }

        return Locomotion.MoveDirection.None;
    }

    private Locomotion.MoveDirection GetMoveDirection(CellInfo start, CellInfo next)
    {
        if (next.ColumnId < start.ColumnId) return Locomotion.MoveDirection.Left;
        if (next.ColumnId > start.ColumnId) return Locomotion.MoveDirection.Right;
        if (next.RowId < start.RowId) return Locomotion.MoveDirection.Down;
        if (next.RowId > start.RowId) return Locomotion.MoveDirection.Up;
        return Locomotion.MoveDirection.None;
    }
}
