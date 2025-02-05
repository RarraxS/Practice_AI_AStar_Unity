using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BreadhtFirstSearchMind : AbstractPathMind
{
    public class Node
    {
        public CellInfo info;
        public Node parent;
        public Node(CellInfo _info, Node _parent)
        {
            this.info = _info;
            this.parent = _parent;
        }
    }

    List<CellInfo> Path = new List<CellInfo> ();
    bool foundGoal=false;
    int countNodes=0;
    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        Path = BreadthFirstSearch(currentPos, boardInfo, goals[0]);
        CellInfo finishingPoint = new CellInfo(1, 0);
        foundGoal = false;

        if (!goals[0].Walkable || !currentPos.Walkable || Path.Count == 1)
        {
            UnityEngine.Debug.Log("Meta en obstáculo");
            return Locomotion.MoveDirection.None;
        }
        else
        {
            finishingPoint = Path[countNodes];
            countNodes++;
        }

        if (finishingPoint.RowId < currentPos.RowId) return Locomotion.MoveDirection.Down;
        else if (finishingPoint.RowId > currentPos.RowId) return Locomotion.MoveDirection.Up;
        else if (finishingPoint.ColumnId < currentPos.ColumnId) return Locomotion.MoveDirection.Left;

        return Locomotion.MoveDirection.Right;
    }

    public List<CellInfo> BreadthFirstSearch(CellInfo start, BoardInfo board, CellInfo goal)
    {
        Node priorNode = null;
        // List to store the visited nodes
        List<CellInfo> visited = new List<CellInfo>();

        // Queue to store the nodes to be visited
        Queue<CellInfo> queue = new Queue<CellInfo>();

        // Add the starting node to the queue
        queue.Enqueue(start);

        // Loop until the queue is empty
        while (queue.Count > 0)
        {
            // Dequeue a node from the queue
            Node node = new Node(queue.Dequeue(), priorNode);
            priorNode = node;
            // If the node has not been visited
            if (!visited.Any(example => example.CellId == node.info.CellId))
            {
                // Mark the node as visited
                visited.Add(node.info);

                if (node.info.CellId == goal.CellId)
                {
                    priorNode = node;
                    break;
                }

                // Enqueue the neighbors of the node
                foreach (var neighbor in node.info.WalkableNeighbours(board))
                {
                    if(neighbor!=null)
                        queue.Enqueue(neighbor);
                }
            }
        }

        // Return the list of visited nodes
        return Reconstruct(priorNode);
    }

    public List<CellInfo> Reconstruct(Node current)
    {
        var path = new List<CellInfo>();
        while (current != null)
        {
            path.Add(current.info);
            current = current.parent;
        }
        path.Reverse();
        return path;
    }
}
