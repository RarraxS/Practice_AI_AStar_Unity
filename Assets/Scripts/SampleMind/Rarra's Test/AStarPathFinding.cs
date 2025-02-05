using Assets.Scripts.DataStructures;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour
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

    private void CalculateCosts()
    {
        BoardInfo bi = new();


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

        goal = bi.Exit;//Acceder al objeto

        for (int i = 0; i < bi.NumRows; i++)
        {
            for(int j = 0; j < bi.NumColumns; j++)
            {
                //node[i][j].h;
            }
        }
        
        //unsearchedNodes
    }
}
