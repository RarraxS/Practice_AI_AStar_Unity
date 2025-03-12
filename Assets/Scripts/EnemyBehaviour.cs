using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Locomotion))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyBehaviour : CharacterBehaviour
    {

        //-- CurrentPosition returns the enemy's current position as a CellInfo object.
        public CellInfo CurrentPosition()
        {
            return BoardManager.boardInfo.CellInfos[(int)transform.position.x, (int)transform.position.y];
        }


        void Awake()
        {

            PathController = GetComponentInChildren<AbstractPathMind>();//-- Get the pathfinding controller from a child object.
            PathController.SetCharacter(this);//--  Set the current enemy (this) to the PathController.
            LocomotionController = GetComponent<Locomotion>();//-- Get the Locomotion controller attached to this GameObject.
            LocomotionController.SetCharacter(this);//--  Set the current enemy to the Locomotion controller.



        }

        void Update()
        {
            //-- If BoardManager is not assigned, exit the method.
            if (BoardManager == null) return;
            //-- Check if the enemy needs to move.
            if (LocomotionController.MoveNeed)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                LocomotionController.SetNewDirection(PathController.GetNextMove(BoardManager.boardInfo,
                    LocomotionController.CurrentEndPosition(), null));

                stopwatch.Stop();
                UnityEngine.Debug.Log($"RAE: Tiempo de ejecución del get next move en el enemy behaviour: {stopwatch.ElapsedMilliseconds} milisegundos");
            }
        }




    }
}