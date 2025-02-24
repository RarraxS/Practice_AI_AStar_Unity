using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AStarPathFinding : AbstractPathMind
{
    //CellInfo 
    //List pasados
    //List por buscar
    private List<Node> unsearchedNodes;

    //Matrix of the board

    //Clase de costos ¿y posiciones?



    void Start()
    {
        // Fill the lists
    }

    void Update()
    {
        
    }

    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        CalculateCosts(boardInfo);

        if (!goals[0].Walkable || !currentPos.Walkable)
        {
            Debug.Log("No es posible llegar a la meta");
            return Locomotion.MoveDirection.None;
        }
        else
        {

        }

        //if (finishingPoint.RowId < currentPos.RowId) return Locomotion.MoveDirection.Down;
        //else if (finishingPoint.RowId > currentPos.RowId) return Locomotion.MoveDirection.Up;
        //else if (finishingPoint.ColumnId < currentPos.ColumnId) return Locomotion.MoveDirection.Left;

        return Locomotion.MoveDirection.Right;
    }

    private void CalculateCosts(BoardInfo _boardInfo)
    {
        // Loader->GameManager->Exit;

        //CellInfo ci = _boardInfo.Exit();

        //BoardInfo _boardInfo = new BoardInfo(1, 1, _boardManager);


        /*
         BOARD MANAGER TIENE UNA REFERENCIA A BOARD INFO, LLAMARLO DESDE AQUI
         
         EL CÓDIGO DEL BOARD MANAGER EN CUASTION

         public void SetupScene(int seed, bool forPlanner, int enemyCount)
        {
            this.boardInfo = new BoardInfo(columns, rows, this);
            this.boardInfo.SetupBoard(seed, forPlanner, this.wallCount, this.leverCount, enemyCount);
        }
         */


        CellInfo goal;

        goal = _boardInfo.Exit;//Acceder al objeto

        for (int i = 0; i < _boardInfo.NumRows; i++)
        {
            for (int j = 0; j < _boardInfo.NumColumns; j++)
            {
                //node[i][j].h;
            }
        }

        //unsearchedNodes
    }
}
