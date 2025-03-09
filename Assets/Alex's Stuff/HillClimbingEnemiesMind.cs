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
        if (goals == null || goals.Length == 0) { return Locomotion.MoveDirection.None; }

        // Obtener posiciones de los enemigos
        List<CellInfo> enemyPositions = boardInfo.Enemies.Select(enemy => enemy.CurrentPosition()).ToList();
        CellInfo goal = enemyPositions.Count > 0
            ? enemyPositions.OrderBy(enemyPos => Heuristic(currentPos, enemyPos)).First() // Enemigo más cercano
            : boardInfo.Exit; // Si no hay enemigos, ir a la salida

        CellInfo bestMove = currentPos;
        int bestScore = Heuristic(currentPos, goal);
        // Explorar vecinos y seleccionar el mejor basado en la heurística
        foreach (CellInfo neighbor in currentPos.WalkableNeighbours(boardInfo))
        {
            if (neighbor == null)
            {
                continue; // Evitar valores nulos
            }

            int score = Heuristic(neighbor, goal);
            if (score < bestScore) // Si el vecino está más cerca del objetivo, actualizar
            {
                bestScore = score;
                bestMove = neighbor;
            }
        }

        return GetMoveDirection(currentPos, bestMove);
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

