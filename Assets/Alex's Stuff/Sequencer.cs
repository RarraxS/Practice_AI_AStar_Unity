using System.Collections.Generic;
using Assets.Scripts.DataStructures;

public class Sequencer
{
    public int count;
    public List<CellInfo> List;
    public int visitedNodes;

    public Sequencer()
    {
        count = 0;
        visitedNodes = 0;
        List = new List<CellInfo>();
    }
}
