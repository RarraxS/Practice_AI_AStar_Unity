using Assets.Scripts.DataStructures;
using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

public class DepthSearchMind : AbstractPathMind
{
    class Node
    {
        public CellInfo info;
        public Node parent;

        public Node(CellInfo _info, Node _parent)
        {
            this.info = _info;
            this.parent = _parent;
        }
    }

    bool foundGoal = false;
    List<Node> VisitedNodes = new List<Node>();
    Node priorNode = null;

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {

        CellInfo goal = goals[0];
        Node startingPoint = new Node(currentPos, null);

        VisitedNodes.Clear();
        foundGoal = false;

        if (!goal.Walkable)
            return Locomotion.MoveDirection.None;

        CellInfo finishingPoint = DFS(boardInfo, startingPoint, goal);
        if (finishingPoint.CellId == "6,14")
        {
            UnityEngine.Debug.Log("Prior:" + priorNode.info.CellId);
            UnityEngine.Debug.Log("Post: " + finishingPoint.CellId);
        }
        if (finishingPoint.RowId < startingPoint.info.RowId)
            return Locomotion.MoveDirection.Down;
        else if (finishingPoint.RowId > startingPoint.info.RowId)
            return Locomotion.MoveDirection.Up;
        else if (finishingPoint.ColumnId < startingPoint.info.ColumnId)
            return Locomotion.MoveDirection.Left;

        return Locomotion.MoveDirection.Right;
    }

    private CellInfo DFS(BoardInfo boardInfo, Node initNode, CellInfo goal)
    {
        //Marco U como descubierto.
        VisitedNodes.Add(initNode);

        //Por cada vértce v adyacente a U.
        CellInfo[] neighbours = initNode.info.WalkableNeighbours(boardInfo);
        Node[] adjacentNodes = new Node[neighbours.Length]; ;
        int i = 0;
        foreach (CellInfo neighbour in neighbours){
            if (neighbours[i] != null){
                adjacentNodes[i] = new Node(neighbours[i], initNode);
            }
            i++;
        }
        foreach (Node adjNode in adjacentNodes){
            if (adjNode != null && adjNode.info == goal){
                VisitedNodes.Add(adjNode);
                foundGoal = true;
            }
            else if (foundGoal){
                VisitedNodes.Add(initNode);
                break;
            }
                  //(!VisitedNodes.Contains(adjNode)
            else if (adjNode != null){
                if ((priorNode == null) || (!VisitedNodes.Any(node => node.info.CellId == adjNode.info.CellId) && (adjNode.info.CellId != priorNode.info.CellId))){
                    
                    priorNode = initNode;
                    DFS(boardInfo, adjNode, goal);
                }
            }
        }
        if (VisitedNodes.Count > 2)
            return VisitedNodes[VisitedNodes.Count - 2].info;
        else return null;
    }
}