using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AStarPathFinding : AbstractPathMind
{
    private List<Node> _nodes;
    //CellInfo 
    //List pasados
    //List por buscar
    private List<Node> unsearchedNodes;

    //Matrix of the board

    //Clase de costos ¿y posiciones?



    void Start()
    {
        _nodes = new List<Node>();
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

        CellInfo cellInfo = _boardInfo.Exit;

        int _totalCost = 0;

        CellInfo _initialPosition = new CellInfo(0, 0);
        Node _initialNode = new Node(_totalCost, _initialPosition.WalkCost, _initialPosition, null);


        // g = coste desde el inicio hasta ese momento
        // h = coste de moverse desde el nodo anterior hasta el nuevo nodo
        // f = h + g



        CellInfo _actualCell = new CellInfo(0, 0);
        Node _actualNode = new Node(_totalCost, 0, _actualCell, null);


        int _actualColumn = 0, _actualRow = 0;

        Debug.Log("\nColumna actual: " + _actualColumn + "\nFila actual: " + _actualRow);






        

        // Todo esto tiene que ir en una funcion que se repite hasta que se encuentra la meta

        CellInfo _upperCell = new CellInfo(_actualColumn + 1, _actualRow);
        Node _upperNode = new Node(_totalCost, _upperCell.WalkCost, _upperCell, _actualNode);

        CellInfo _lowerCell = new CellInfo(_actualColumn - 1, _actualRow);
        Node _lowerNode = new Node(_totalCost, _lowerCell.WalkCost, _lowerCell, _actualNode);

        CellInfo _rightCell = new CellInfo(_actualColumn, _actualRow + 1);
        Node _rightNode = new Node(_totalCost, _rightCell.WalkCost, _rightCell, _actualNode);

        CellInfo _leftCell = new CellInfo(_actualColumn, _actualRow - 1);
        Node _leftNode = new Node(_totalCost, _leftCell.WalkCost, _leftCell, _actualNode);

        // Move
        if (true)
        {
            //Locomotion.
        }








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
