using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Snake1 : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right, None };
    public Direction myDirection;
    private float intervalSpeed = 0.2f;
    private float nextTime = 0f;
    private List<GameObject> tail = new List<GameObject>();
    private bool reward = false;
    private bool dead = false;
    int points;
    public GameManager gameManager;

    public Vector3 prev_pos;

    [SerializeField] private KeyCode upkey;
    [SerializeField] private KeyCode downkey;
    [SerializeField] private KeyCode leftkey;
    [SerializeField] private KeyCode rightkey;

    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public GameObject tailPrefab;

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        //set initial position and Direction at random
        myDirection = Direction.None;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Reward(Clone)") && !dead)
        {
            reward = true;
            Debug.Log("Found a Reward");
            GetComponent<AudioSource>().Play();
            points++;
            text.text = "Points: " + points;
        }
        else if (other.gameObject.name.Contains("Snake"))
        {
          //berekenen welke snake af is op basis van de directie en de vorige positie, dmv de som
          Snake1 otherSnake = other.gameObject.GetComponent(typeof(Snake1)) as Snake1;
          int sum_x = (int)myDirection + (int)(otherSnake.prev_pos.x - prev_pos.x);
          int sum_y = (int)myDirection + (int)(otherSnake.prev_pos.y - prev_pos.y);

          // otherSnake.transform.position = otherSnake.prev_pos;
          // transform.position = prev_pos;
          //
          // Debug.Log("MY pos ; His Pos " + prev_pos + " " + otherSnake.prev_pos);
          // Debug.Log("INT x , y " + sum_x + " " + sum_y);
          // Debug.Break();

          if(sum_x == 5 || sum_x == 2 || sum_x == 0){
            Debug.Log("Dead x , y " + sum_x + " " + sum_y);
            dead = true;
            text.text = "DEAD: " + points;
            Debug.Log("GAME OVER");
            gameObject.SetActive(false);
          }
          else if(sum_y == 1 || sum_y == 2 || sum_y == 0){
            Debug.Log("Dead x , y " + sum_x + " " + sum_y);
            dead = true;
            text.text = "DEAD: " + points;
            Debug.Log("GAME OVER");
            gameObject.SetActive(false);
          }
          else{
            //continue;
            Debug.Log("Survived x , y " + sum_x + " " + sum_y);
          }
        }
        else if (other.gameObject.name.Contains("TailPart"))
        {
            dead = true;
            Debug.Log("GAME OVER");
            text.text = "DEAD: " + points;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(upkey))
            myDirection = Direction.Up;
        else if (Input.GetKeyDown(downkey))
            myDirection = Direction.Down;
        else if (Input.GetKeyDown(leftkey))
            myDirection = Direction.Left;
        else if (Input.GetKeyDown(rightkey))
            myDirection = Direction.Right;

        if (Time.timeSinceLevelLoad >= nextTime && !dead)
        {
            int xpos = 0;
            float ypos = 0;
            switch (myDirection)
            {
                case Direction.Up:
                    ypos++;
                    break;
                case Direction.Down:
                    ypos--;
                    break;
                case Direction.Right:
                    xpos++;
                    break;
                case Direction.Left:
                    xpos--;
                    break;
            }


            List<GameObject> tailtmp = new List<GameObject>();
            tailtmp.Add((GameObject)Instantiate(tailPrefab));
            tailtmp[0].transform.position = transform.position;
            if (reward)
            {
                tailtmp.AddRange(tail.GetRange(0, tail.Count));
                reward = false;
            }
            else
            {
                if (tail.Count > 0)
                {
                    tailtmp.AddRange(tail.GetRange(0, tail.Count - 1)); //cut last item
                    Destroy(tail[tail.Count - 1]);
                }
                else
                {
                    Destroy(tailtmp[0]);
                    tailtmp.Clear();
                }
            }
            tail = tailtmp;
            transform.position = new Vector3(transform.position.x + xpos, transform.position.y + ypos, 0);
            prev_pos = transform.position;
            nextTime += intervalSpeed;
        }
    }
}
