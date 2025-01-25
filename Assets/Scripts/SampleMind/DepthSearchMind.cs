using Assets.Scripts.DataStructures;
using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;

public class DepthSearchMind : AbstractPathMind
{

    // En la primera iteración, "saca" el camino, y en la segunda y las venideras, lo sigue.
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
    int countNodes = 1;

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {

        CellInfo goal = goals[0];
        Node startingPoint = new Node(currentPos, null);
        CellInfo finishingPoint = null;
        foundGoal = false;

        if (!goal.Walkable)
            return Locomotion.MoveDirection.None;
        if (VisitedNodes.Count == 0)
        {
            finishingPoint = DFS(boardInfo, startingPoint, goal);
        }
        else
            finishingPoint = VisitedNodes[countNodes].info;
        countNodes++;


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
        int count = 0, noNullsCount = 0;
        foreach (CellInfo neighbour in neighbours){
            if (neighbours[count] != null){
                adjacentNodes[count] = new Node(neighbours[count], initNode);
                noNullsCount++;
            }
            count++;
        }
        foreach (Node adjNode in adjacentNodes){

            if(adjNode != null)
            {
                if (adjNode.info == goal)
                {
                    VisitedNodes.Add(adjNode);
                    foundGoal = true;
                    break;
                }
                else if(foundGoal)
                {
                    break;
                }

                //(!VisitedNodes.Contains(adjNode)
                else if (!VisitedNodes.Any(node => node.info.CellId == adjNode.info.CellId) || noNullsCount == 1 )
                {       
                    DFS(boardInfo, adjNode, goal);
                }
            }
        }
        if (VisitedNodes.Count > 2)
            return VisitedNodes[1].info;
        else return null;
    }
}