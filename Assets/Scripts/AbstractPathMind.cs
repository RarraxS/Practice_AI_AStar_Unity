using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class AbstractPathMind: MonoBehaviour
    {
        protected CharacterBehaviour character;

        //--  The concrete implementation will determine the next direction of movement of the character.
        public abstract Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals);


        //--  create a behavior for the character.
        public void SetCharacter(CharacterBehaviour characterBehaviour)
        {
            this.character = characterBehaviour;
        }
    }
}
