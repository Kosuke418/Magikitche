using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MainGameManager.P1Score = 100;
        MainGameManager.P2Score = 100;
        MainGameManager.GameCount = 0;
        MainGameManager.IngredNum1=0;
        MainGameManager.IngredNum2=0;
        MainGameManager.GameProgress=0;
        MainGameManager.PrecedNum=0;
        MainGameManager.Player1Ingred = new int[10];
        MainGameManager.Player2Ingred = new int[10];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Main");
        }
    }
}