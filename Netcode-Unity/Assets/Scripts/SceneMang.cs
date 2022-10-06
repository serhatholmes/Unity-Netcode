using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMang : MonoBehaviour
{
    
    void Start()
    {
        
    }

    public void startGame(){

        SceneManager.LoadScene("Gameplay");
    }

    public void backMenu(){
        SceneManager.LoadScene("Start");
    }
    

}
