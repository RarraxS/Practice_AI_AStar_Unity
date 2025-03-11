using System;
using System.Diagnostics;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Locomotion))]
    public class CharacterBehaviour: MonoBehaviour
    {
        
        protected Locomotion LocomotionController; //--  It is used to handle the movement of the character.
        protected AbstractPathMind PathController; //--  It is responsible for calculating and providing the next movement of the character.
        public BoardManager BoardManager { get; set; }//-- it gives accesss to the board. 
        protected CellInfo currentTarget; //-- It is the cell that the character is aiming to move to.

        void Awake()
        {

            PathController = GetComponentInChildren<AbstractPathMind>();//-- Get the pathfinding controller (AbstractPathMind) from a child object.
            PathController.SetCharacter(this);//--  Set the current character to the pathfinding controller.
            LocomotionController = GetComponent<Locomotion>();//-- // Get the Locomotion controller attached to this GameObject.
            LocomotionController.SetCharacter(this);//--  Set the current character to the locomotion controller.



        }

        void Update()
        {

            if (BoardManager == null) return;//-- If the BoardManager is null, exit the update method.

            if (LocomotionController.MoveNeed)//--  Check if the character needs to move according to the LocomotionController.
            {

                var boardClone = (BoardInfo)BoardManager.boardInfo.Clone();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                LocomotionController.SetNewDirection(PathController.GetNextMove(boardClone,LocomotionController.CurrentEndPosition(),new [] {this.currentTarget}));
                stopwatch.Stop();
                UnityEngine.Debug.Log($"Tiempo de ejecución del get next move en el character behaviour: {stopwatch.ElapsedMilliseconds} milisegundos");
            }
        }

       

        public void SetCurrentTarget(CellInfo newTargetCell) //-- Method to set the current target cell where the character needs to go.
        {
            this.currentTarget = newTargetCell;
        }
    }
}

