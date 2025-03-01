using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;

public class AStarPathFindingV2 : AbstractPathMind
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
        if (goals.Length== 0) { return Locomotion.MoveDirection.None; }

        CellInfo goal = goals[0]; // Cojo el primer objetivo

        // Setup del A*
        var openNodes = new List<Node>();
        var closedNodes = new HashSet<(int, int)>();
        var startNode = new Node(currentPos, null);

        openNodes.Add(startNode);

        var directions = 
    }
}
