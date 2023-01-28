using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

//ai snake
public class Snake : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right };

    private Vector3[] vector = new Vector3[] {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right
    };

    public Vector3 prev_pos;

    public Direction myDirection;
    public float intervalSpeed = 0.2f;
    private float nextTime = 0f;
    private List<GameObject> tail = new List<GameObject>();
    private bool reward = false;
    private bool dead = false;
    int points;

    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public GameObject tailPrefab;

    [SerializeField] private KeyCode upkey;
    [SerializeField] private KeyCode downkey;
    [SerializeField] private KeyCode leftkey;
    [SerializeField] private KeyCode rightkey;

    public static Reward _reward;

    public AStar route;

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        _reward = (Reward)FindObjectOfType(typeof(Reward));
        //set initial position and Direction at random
        myDirection = Direction.Right;
        //transform.position = new Vector2(Random.Range(-6, 6), Random.Range(-4, 4)); //what could be inmproved here?
        route.seeker = this.transform;
        route.target = _reward.transform;

    }
    void OnDrawGizmos() {
      Gizmos.color = Color.yellow;
      Vector3 direction = GetDirection(myDirection);
      Gizmos.DrawRay(transform.position, direction);
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
          Snake otherSnake = other.gameObject.GetComponent(typeof(Snake)) as Snake;
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
        else if (other.tag.Equals("Tail"))
        {
            dead = true;
            text.text = "DEAD: " + points;
            Debug.Log("GAME OVER");
            gameObject.SetActive(false);
        }
    }

    private Vector3 GetDirection(Direction _direction)
    {
        return vector[(int)_direction];
    }

    void FollowRoute(List<Node> path){
      if(path.Count > 0){
        Node goTo = path.First();

        //direction berekenen
        if(transform.position.x == goTo.worldPosition.x){
          if(transform.position.y != goTo.worldPosition.y){
            if(transform.position.y > goTo.worldPosition.y){
              myDirection = Direction.Down;
            }
            else{
              myDirection = Direction.Up;
            }
          }
        }
        else{
          if(transform.position.x > goTo.worldPosition.x){
            myDirection = Direction.Left;
          }
          else{
            myDirection = Direction.Right;
          }
        }

        prev_pos = transform.position;
        transform.position = goTo.worldPosition;
      }
    }

    // Update is called once per frame
    void Update()
    {
        _reward = (Reward)FindObjectOfType(typeof(Reward));
        if (Time.timeSinceLevelLoad >= nextTime && !dead)
        {
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
            FollowRoute(route.path);
            //transform.position = new Vector3(transform.position.x + xpos, transform.position.y + ypos, 0);
            nextTime += intervalSpeed;
        }
    }
}
