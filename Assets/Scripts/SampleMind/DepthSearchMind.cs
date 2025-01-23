using Assets.Scripts.DataStructures;
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DepthSearchMind : AbstractPathMind
{
    class Node
    {
        public CellInfo info;
        public Node parent;

        public Node (CellInfo _info, Node _parent)
        {
            this.info = _info;
            this.parent = _parent;
        }
    }

    List<CellInfo> VisitedNodes = new List<CellInfo>();
    List<CellInfo> DiscoveredNodes = new List<CellInfo>();
    List<Node> WayNodes = new List<Node>();

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
       
        CellInfo goal = goals[0];
        Node startingPoint = new Node(currentPos, null);

        if (!goal.Walkable)
            return Locomotion.MoveDirection.None;

            CellInfo finishingPoint = DFS(boardInfo, startingPoint, goal);

        if (finishingPoint.RowId < startingPoint.info.RowId)
            return Locomotion.MoveDirection.Left;
        else if (finishingPoint.RowId > startingPoint.info.RowId)
            return Locomotion.MoveDirection.Right;
        else if (finishingPoint.ColumnId < startingPoint.info.ColumnId)
            return Locomotion.MoveDirection.Down;

        return Locomotion.MoveDirection.Up;
    }

    private CellInfo DFS(BoardInfo boardInfo, Node initNode, CellInfo goal)
    {
        Node nextNode = null;
        //Marco U como descubierto.
        DiscoveredNodes.Add(initNode.info);

        //Por cada vértce v adyacente a U.
        CellInfo[] neighbours = initNode.info.WalkableNeighbours(boardInfo);

        foreach (CellInfo neighbour in neighbours) 
        {
            if(neighbour == goal || WayNodes.Count>0)
            {
                Debug.Log("He encontrado la meta");

                WayNodes.Add(initNode);
                break;
            }

            //if(v no fue visitado)
            else if(!VisitedNodes.Contains(neighbour) && neighbour!=null && neighbour.ItemInCell == null)
            {
                // padre[v] = u;
                nextNode = new Node(neighbour, initNode);
                VisitedNodes.Add(initNode.info);
                DiscoveredNodes.Remove(initNode.info);

                // DFS(g,v);
                DFS(boardInfo, nextNode, goal);
            }
        }
        if(WayNodes.Count>2) 
            return WayNodes[WayNodes.Count-2].info;
        else return null;
    }
}
