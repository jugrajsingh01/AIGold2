using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadGame(int i){
      SceneManager.LoadScene(i);
    }

    public void LoadMenu(){
      SceneManager.LoadScene(0);
    }

    public void LoadAIs(){
      SceneManager.LoadScene(1);
    }

    public void LoadPlayers(){
      SceneManager.LoadScene(2);
    }

    public void LoadPlayerAI(){
      SceneManager.LoadScene(3);
    }
}
