using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts.SampleMind
{
    public class RandomMind : AbstractPathMind {

        //--  Overrides the GetNextMove method from the AbstractPathMind class.
        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
            //-- Generate a random value between 0 and 3.

            var val = Random.Range(0, 4);
            if (val == 0 && currentPos.RowId<14) return Locomotion.MoveDirection.Up;
            if (val == 1 && currentPos.RowId > 0) return Locomotion.MoveDirection.Down;
            if (val == 2 && currentPos.ColumnId > 0) return Locomotion.MoveDirection.Left;
            if(currentPos.ColumnId < 14) return Locomotion.MoveDirection.Right;
            return Locomotion.MoveDirection.None;
        }
    }
}
