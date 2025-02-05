using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts
{
    public class ItemLogic : MonoBehaviour
    {

        public string Tag {get { return PlaceableItem!=null?PlaceableItem.Tag:""; } } //-- The Tag property returns the tag of
                                                                                      //the placeable item if it exists, otherwise returns an empty string.
        
        public Sprite ActiveSprite; //-- The ActiveSprite represents the sprite
                                    //that will be displayed when the item is activated.

        public PlaceableItem.ItemType Type; //-- Type of the item defined by PlaceableItem.

        public PlaceableItem PlaceableItem { get; set; }//-- Reference to the PlaceableItem that holds the logic and properties of the item.

        //-- The RequieredTags property generates a string listing the tags of the required preconditions to activate the item.
        public string RequieredTags
        {
            get
            {
                //-- Start string.
                var str = "Reqs: ";

                return PlaceableItem.Preconditions.Aggregate(str,(curr, next) => curr + ", " + next.Tag);

            }
        }

        public void Start()
        {
            Debug.Log(Tag+ ">"+ RequieredTags);
            
        }

        void OnTriggerEnter2D(Collider2D collider2D)

        {
            //-- If the collider is not the player, exit the method.
            if (!collider2D.gameObject.CompareTag("Player")) return;

            //--  Check if any precondition of the item is not activated.
            if (PlaceableItem.Preconditions.Any(o => !o.Activated))
            {
                return;
            }

            //-- Change the sprite of the item to the active sprite.
            var render = GetComponent<SpriteRenderer>();
            render.sprite = ActiveSprite;
            PlaceableItem.Activated = true;

            //-- If the item is a goal and there are no active enemies, quit the game.
            if (this.Type == PlaceableItem.ItemType.Goal && GameManager.instance.ActiveEnemies.Count == 0)
                QuitGame();

            //--if the item is an enemy, remove it from the active enemy list and destroy it. 
            if (this.Type == PlaceableItem.ItemType.Enemy)
            {
                GameManager.instance.ActiveEnemies.Remove(gameObject);
                GameObject.Destroy(gameObject, 0.5f);
            }
                
        }

        public void QuitGame()
        {
            // save any game data here
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }

        //--  OnGUI is used to display the item's tag and required tags on the screen in the game view.
        void OnGUI()
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            GUI.Label(new Rect(pos.x-25,Screen.height-pos.y,70,20), Tag );
            GUI.Label(new Rect(pos.x - 25, Screen.height - pos.y - 20, 370, 20), RequieredTags);
        }
    }
}
