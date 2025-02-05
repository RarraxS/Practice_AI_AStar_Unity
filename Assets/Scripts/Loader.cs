using UnityEngine;

namespace Assets.Scripts
{
    public class Loader : MonoBehaviour
    {
        public GameObject gameManager;          //GameManager prefab to instantiate.
        public bool Planner=false; //-- A flag to determine if the game should be loaded in planner mode
        public int seed = 2016; //-- seed value used to initialize random number generation
        public int numEnemies = 0; //-- Number of enemies to be spawned at the start of the game.
        void Awake()
        {
            //Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
            if (GameManager.instance == null)
            {
                //Instantiate gameManager prefab
                var obj = Instantiate(gameManager) as GameObject;

                //-- Get the GameManager component attached to the instantiated GameObject.
                obj.name = "GameManager";

                //-- Set the values for Planner mode, seed, and number of enemies in the GameManager.
                var manager =obj.GetComponent<GameManager>();
                manager.ForPlanner = Planner;
                manager.seed = seed;
                manager.numEnemies = numEnemies;
                manager.InitGame();
            }


        }
    }
}