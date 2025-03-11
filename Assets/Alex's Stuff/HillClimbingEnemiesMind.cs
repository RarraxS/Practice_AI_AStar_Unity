using Assets.Scripts.DataStructures;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HillClimbingEnemiesMind : AbstractPathMind
{
    private static int Heuristic(CellInfo a, CellInfo b)
    {
        return Mathf.Abs(a.ColumnId - b.ColumnId) + Mathf.Abs(a.RowId - b.RowId);
    }

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        Debug.Log($"Current Position: {currentPos}");
        if (goals == null || goals.Length == 0)
        {
            Debug.Log("No goals available. Returning None.");
            return Locomotion.MoveDirection.None;
        }

        // Obtener posiciones de los enemigos
        List<CellInfo> enemyPositions = boardInfo.Enemies.Select(enemy => enemy.CurrentPosition()).ToList();
        Debug.Log($"Enemy Positions: {string.Join(", ", enemyPositions)}");

        CellInfo goal = enemyPositions.Count > 0
            ? enemyPositions.OrderBy(enemyPos => Heuristic(currentPos, enemyPos)).First() // Enemigo más cercano
            : boardInfo.Exit; // Si no hay enemigos, ir a la salida

        Debug.Log($"Selected Goal: {goal}");

        CellInfo bestMove = currentPos;
        int bestScore = Heuristic(currentPos, goal);
        Debug.Log($"Current Position Heuristic Score: {bestScore}");

        // Obtener vecinos
        var neighbors = currentPos.WalkableNeighbours(boardInfo);
        Debug.Log($"Neighbors Count: {neighbors.Count()}");

        // Explorar vecinos y seleccionar el mejor basado en la heurística
        foreach (CellInfo neighbor in neighbors)
        {
            if (neighbor == null)
            {
                Debug.Log("Skipping null neighbor");
                continue; // Evitar valores nulos
            }

            int score = Heuristic(neighbor, goal);
            Debug.Log($"Evaluating Neighbor {neighbor} with Score {score}");

            if (score < bestScore) // Si el vecino está más cerca del objetivo, actualizar
            {
                bestScore = score;
                bestMove = neighbor;
                Debug.Log($"New Best Move: {bestMove} with Score {bestScore}");
            }
        }

        Locomotion.MoveDirection direction = GetMoveDirection(currentPos, bestMove);
        Debug.Log($"Moving {direction} from {currentPos} to {bestMove}");
        return direction;
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
}

