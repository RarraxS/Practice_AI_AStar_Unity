using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.Analytics;
using Assets.Scripts.DataStructures;
using System.Linq;
using Assets.Scripts;

public class HorizonSearch : AbstractPathMind
{
    [SerializeField]
    int searchDepth = 3;
    int numOfMoves = 0;
    List<CellInfo> movements = null;

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
        List<CellInfo> enemyPositions = boardInfo.Enemies.Select(enemy => enemy.CurrentPosition()).ToList();
        if(numOfMoves == 0 ) 
            movements = HorizonS(currentPos, enemyPositions[0], boardInfo, searchDepth);

        if(numOfMoves < searchDepth-1)
        {
            numOfMoves++;
            return GetMoveDirection(currentPos, movements[numOfMoves-1]);
        }
        else
        {
            numOfMoves=0;
            return GetMoveDirection(currentPos, movements.Last());
        }
    }

    private static int Heuristic(CellInfo a, CellInfo b)
    {
        return Mathf.Abs(a.ColumnId - b.ColumnId) + Mathf.Abs(a.RowId - b.RowId);
    }

    List<CellInfo> HorizonS (CellInfo start,  CellInfo end, BoardInfo info, int horizon)
    {
        Queue<Node> frontier = new Queue<Node>();
        List<CellInfo> explored = new List<CellInfo>();

        frontier.Enqueue(new Node(start,null));

        int depth = 0;

        Node actualState = frontier.FirstOrDefault();
        do
        {
            actualState = frontier.OrderBy(node => Heuristic(node.cell, end)).FirstOrDefault();

            if (actualState.cell.CellId == end.CellId)
            {
                UnityEngine.Debug.Log("Estado encontrado");
                break;
            }

            if (depth >= horizon)
            {   
                UnityEngine.Debug.Log("Cota de profundidad alcanzada");
                break;
            }

            if(explored.Count<1 || explored.Any(n => n.CellId != actualState.cell.CellId))
            {
                explored.Add(actualState.cell);

                foreach(var neigbour in actualState.cell.WalkableNeighbours(info))
                {
                    if(neigbour != null && explored.Any(n => n.CellId != neigbour.CellId))
                        frontier.Enqueue(new Node (neigbour, actualState));
                }
                depth++;
                frontier.Dequeue();
            }
        } while (frontier.Count > 0);

        return ReconstructPath(actualState);
    }

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
    private Locomotion.MoveDirection GetMoveDirection(CellInfo start, CellInfo next)
    {
        UnityEngine.Debug.Log(next.CellId);
        if (next.ColumnId < start.ColumnId) return Locomotion.MoveDirection.Left;
        if (next.ColumnId > start.ColumnId) return Locomotion.MoveDirection.Right;
        if (next.RowId < start.RowId) return Locomotion.MoveDirection.Down;
        if (next.RowId > start.RowId) return Locomotion.MoveDirection.Up;
        return Locomotion.MoveDirection.None;
    }


    //    Función HorizonSearch(inicio, objetivo, boardinfo, horizonte_maximo):
    //    Inicializar frontera como una cola vacía
    //    Inicializar explorados como un conjunto vacío
    //    Agregar el estado inicial a la frontera

    //    Mientras la frontera no esté vacía:
    //        estado_actual = extraer de la frontera el primer elemento
    //        Si estado_actual es igual a objetivo:
    //            devolver "Objetivo encontrado"

    //        Si la profundidad de estado_actual es mayor que horizonte_maximo:
    //            continuar con el siguiente ciclo(ignorar este estado)


    //        Si estado_actual no está en explorados:
    //            Agregar estado_actual a explorados
    //            Generar los sucesores de estado_actual

    //            Para cada sucesor:
    //                Si no está en explorados:
    //                    Agregar sucesor a la frontera
    //    Fin Mientras

    //    devolver "Objetivo no encontrado dentro del horizonte"
    //Fin Función

    //   Inicio y Objetivo: El algoritmo comienza con el estado inicial y tiene como objetivo encontrar un estado objetivo.
    //   Frontera: Usa una cola para manejar los estados que aún no se han explorado.La frontera es donde se almacenan los estados por explorar.
    //   Explorados: Un conjunto que mantiene los estados que ya se han explorado para evitar ciclos.
    //   Búsqueda por Horizonte: Limita la exploración de estados hasta un cierto nivel o "horizonte máximo" para evitar la exploración excesiva en espacios muy grandes.
    //   Generación de Sucesores: A partir del estado actual, genera los posibles sucesores (estados alcanzables) y los agrega a la frontera si no han sido explorados previamente.
}
