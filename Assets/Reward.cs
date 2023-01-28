using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    public GameManager gameManager;
    System.Random rand;
    // Start is called before the first frame update
    void Start()
    {
      rand = new System.Random();
      gameManager = (GameManager)FindObjectOfType(typeof(GameManager));

    }

    //not sure if its more optimised, but this way the reward wont spawn inside the snake(s bodies)
    //so its more optimal in the way it spawns
    public void reposition(){
      bool walkable = false;
      while(!walkable){
        Node node = gameManager.grid[rand.Next(gameManager.grid.GetLength(0)), rand.Next(gameManager.grid.GetLength(1))];

        walkable = !(Physics2D.OverlapCircle(node.worldPosition,1f,gameManager.unwalkableMask));

        if(walkable){
          transform.position = node.worldPosition;
        }
      }
    }

    private void OnTriggerEnter2D(Collider2D other){
      reposition();
    }



    // Update is called once per frame
    void Update()
    {

    }
}
