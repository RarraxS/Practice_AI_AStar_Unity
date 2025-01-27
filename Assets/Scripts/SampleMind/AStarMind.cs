using Assets.Scripts.DataStructures;
using Assets.Scripts;
using System.Collections.Generic;

public class AStarMind : AbstractPathMind
{
    public class Node
    {
        public int g;
        public int h;
        public int f;
        public CellInfo cell;
        public Node parent;
    }
    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        throw new System.NotImplementedException();
    }

    public void AStar(Node initNode, CellInfo goal)
    {
        List<Node> openList = new List<Node> { initNode };
        List<Node> closedList = new List<Node>();

        initNode.g = 0;
        initNode.h = Heuristic(initNode.cell, goal);

    }

    public int Heuristic(CellInfo current,CellInfo goal)
    {
        return 0;
    }
}
