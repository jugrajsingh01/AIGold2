using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject myPrefab;
    public GameManager gameManager;
    System.Random rand;
    // Start is called before the first frame update

    void Awake()
    {
    Instantiate(myPrefab, new Vector3(Random.Range(-10, 10), Random.Range(-6, 6), 0), Quaternion.identity);
    }

    void Start(){

    }
}
